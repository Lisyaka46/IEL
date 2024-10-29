using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using IEL.Interfaces.Core;
using IEL.Interfaces.Front;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELPanelAction.xaml
    /// </summary>
    public partial class IELPanelAction : UserControl
    {
        /// <summary>
        /// Перечисление вариаций вычисления позиций панели действий
        /// </summary>
        public enum PositionAnimActionPanel
        {
            /// <summary>
            /// Обычное вычисление по курсору
            /// </summary>
            Default = 0,

            /// <summary>
            /// Вычисление цента объекта
            /// </summary>
            CenterObject = 1,
        }

        /// <summary>
        /// Флаг состояния активности панели действий
        /// </summary>
        public bool PanelActionActivate { get; private set; } = false;

        /// <summary>
        /// Состояние правого нажатия в режиме клавиатуры
        /// </summary>
        private bool ActivateRightClickKeyboardMode = false;

        /// <summary>
        /// Состояние выделения кнопки через режим клавиатуры
        /// </summary>
        private bool SelectButtonKeyboardMode = false;

        /// <summary>
        /// Состояние бокирующее повторное 
        /// </summary>
        private bool BlockWhileEvent = false;

        /// <summary>
        /// Массив уникальных кодов клавиш для взаимодействия с элементом
        /// </summary>
        private readonly Key[] keys;

        /// <summary>
        /// Код клавиши активирующий режим клавиатуры в панели действий
        /// </summary>
        public Key KeyActivateKeyboardMode
        {
            get => keys[0];
            set
            {
                if (keys.Any((i) => i == value))
                    throw new InvalidOperationException($"Нельзя задавать одинаковые значения клавиши ({value})");
                keys[0] = value;
            }
        }

        /// <summary>
        /// Код клавиши активирующий правое нажатие в режиме клавиатуры в панели действий
        /// </summary>
        public Key KeyKeyboardModeActivateRightClick
        {
            get => keys[1];
            set
            {
                if (keys.Any((i) => i == value))
                    throw new InvalidOperationException($"Нельзя задавать одинаковые значения клавиши ({value})");
                keys[1] = value;
            }
        }

        /// <summary>
        /// Код клавиши закрывающий элемент
        /// </summary>
        public Key KeyCloseElement
        {
            get => keys[2];
            set
            {
                if (keys.Any((i) => i == value))
                    throw new InvalidOperationException($"Нельзя задавать одинаковые значения клавиши ({value})");
                keys[2] = value;
            }
        }

        /// <summary>
        /// Объект анимации для управления размерами панели действий
        /// </summary>
        private static readonly DoubleAnimation DoubleAnimateObj = new(0, TimeSpan.FromMilliseconds(300d))
        {
            DecelerationRatio = 0.6d,
            EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
        };

        /// <summary>
        /// Объект анимации для управления позицией
        /// </summary>
        private static readonly ThicknessAnimation ThicknessAnimate = new(new Thickness(0), TimeSpan.FromMilliseconds(300d))
        {
            DecelerationRatio = 0.6d,
            EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut }
        };

        readonly List<(IPageKey, string)> BufferPages = [];

        /// <summary>
        /// Имя объекта страницы
        /// </summary>
        public string NameFrameElement => ActiveObject.ElementInPanel?.Name ?? string.Empty;

        /// <summary>
        /// Объект актуальной страницы
        /// </summary>
        private IPageKey ActualPage => (IPageKey)ActualFrame.Content;

        /// <summary>
        /// Объект предыдущей страницы
        /// </summary>
        private IPageKey BackPage => (IPageKey)BackFrame.Content;

        /// <summary>
        /// Объект актуального окна страницы
        /// </summary>
        private Frame ActualFrame => PanelVerschachtelung % 2 == 0 ? ref FrameActionPanelLeft : ref FrameActionPanelRight;

        /// <summary>
        /// Объект предыдущего окна страницы
        /// </summary>
        private Frame BackFrame => !(PanelVerschachtelung % 2 == 0) ? ref FrameActionPanelLeft : ref FrameActionPanelRight;

        /// <summary>
        /// Объект настроек панели для активного объекта реализации
        /// </summary>
        private SettingsPanelActionFrameworkElement ActiveObject;

        /// <summary>
        /// Индекс смены окна страницы
        /// </summary>
        private int PanelVerschachtelung = 0;

        /// <summary>
        /// Делегат события закрытия панели действий
        /// </summary>
        /// <param name="FrameworkElementName">Имя активного объекта для палени действий</param>
        public delegate void ClosingPanelAction(string FrameworkElementName);

        /// <summary>
        /// Событие закрытия панели действий
        /// </summary>
        public event ClosingPanelAction? EventClosingPanelAction;

        public IELPanelAction()
        {
            InitializeComponent();
            keys = [Key.Z, Key.RightCtrl, Key.Escape];
            TextBlockRightButtonIndicatorKey.Opacity = 0d;
            KeyDown += (sender, e) =>
            {
                if (!PanelActionActivate && BlockWhileEvent) return;
                else BlockWhileEvent = true;
                if (e.Key == KeyKeyboardModeActivateRightClick)
                {
                    if (ActualPage.KeyboardMode && !ActivateRightClickKeyboardMode)
                    {
                        AnimTextBlockRightClick(true);
                        if (SelectButtonKeyboardMode) SelectButtonKeyboardMode = false;
                    }
                    else return;
                }
                else if (e.Key == KeyCloseElement)
                {
                    AnimateSizePanelAction(new(ActiveObject.SizedPanel.Width + 16, ActiveObject.SizedPanel.Height + 16));
                }
                else
                {
                    if (ActualPage.KeyboardMode && !SelectButtonKeyboardMode)
                    {
                        SelectButtonKeyboardMode = true;
                        ActualPage.ActivateElementKey<IIELButtonKey>((Page)ActualFrame.Content, e.Key, IPageKey.ActionButton.BlinkActivate,
                            ActivateRightClickKeyboardMode ? IPageKey.OrientationActivate.RightButton : IPageKey.OrientationActivate.LeftButton);
                    }
                }
            };
            KeyUp += (sender, e) =>
            {
                if (!PanelActionActivate && !BlockWhileEvent) return;
                else BlockWhileEvent = false;
                if (e.Key == KeyActivateKeyboardMode)
                {
                    ActualPage.KeyboardMode = !ActualPage.KeyboardMode;
                    if (!ActualPage.KeyboardMode && ActivateRightClickKeyboardMode) AnimTextBlockRightClick(false);
                }
                else if (e.Key == KeyKeyboardModeActivateRightClick && ActualPage.KeyboardMode && ActivateRightClickKeyboardMode)
                {
                    AnimTextBlockRightClick(false);
                    if (SelectButtonKeyboardMode) SelectButtonKeyboardMode = false;
                }
                else if (e.Key == KeyCloseElement)
                {
                    ClosePanelAction(PositionAnimActionPanel.CenterObject);
                }
                else
                {
                    if (ActualPage.KeyboardMode && SelectButtonKeyboardMode)
                    {
                        SelectButtonKeyboardMode = false;
                        ActualPage.ActivateElementKey<IIELButtonKey>((Page)ActualFrame.Content, e.Key, IPageKey.ActionButton.ActionActivate,
                            ActivateRightClickKeyboardMode ? IPageKey.OrientationActivate.RightButton : IPageKey.OrientationActivate.LeftButton);
                    }
                }
            };
            LostFocus += (sender, e) => ClosePanelAction(PositionAnimActionPanel.CenterObject);
        }

        /// <summary>
        /// Анимировать текст правого нажатия по кнопке в панели действий
        /// </summary>
        private void AnimTextBlockRightClick(bool StateParam)
        {
            ActivateRightClickKeyboardMode = StateParam;
            DoubleAnimateObj.To = ActivateRightClickKeyboardMode ? 1d : 0d;
            TextBlockRightButtonIndicatorKey.BeginAnimation(OpacityProperty, DoubleAnimateObj);
        }

        /// <summary>
        /// Метод использования панели действий независимо на её состояние
        /// </summary>
        /// <param name="Settings">Объект настроек для взаимодействия с панелью действий</param>
        public void UsingPanelAction(SettingsPanelActionFrameworkElement Settings)
        {
            if (!PanelActionActivate) OpenPanelAction(Settings);
            else
            {
                if (!ActiveObject.ElementInPanel.Name.Equals(Settings.ElementInPanel.Name))
                {
                    double X = Mouse.GetPosition((IInputElement)VisualParent).X;
                    if (Settings.ElementInPanel.ActualWidth < Settings.SizedPanel.Width)
                        Settings.SizedPanel = new(Settings.ElementInPanel.ActualWidth, Settings.SizedPanel.Height);
                    if (Settings.ElementInPanel.ActualHeight < Settings.SizedPanel.Height)
                        Settings.SizedPanel = new(Settings.SizedPanel.Width, Settings.ElementInPanel.ActualHeight);
                    AddBufferElementPageAction(ActiveObject);
                    ThicknessAnimate.Duration = TimeSpan.FromMilliseconds(360d);
                    NextPage(BufferSearchDefaultPage(Settings.ElementInPanel.Name) ?? Settings.DefaultPageInPanel, X >= Margin.Left);
                    ThicknessAnimate.Duration = TimeSpan.FromMilliseconds(300d);
                    AnimateSizePanelAction(Settings.SizedPanel);
                    ActiveObject = Settings;
                }
                AnimationMovePanelAction(PositionAnimActionPanel.Default, Settings.SizedPanel, Settings.ElementInPanel);
            }
        }

        /// <summary>
        /// Метод открытия панели действий
        /// </summary>
        /// <param name="Settings">Объект настроек для открытия панели действий</param>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private void OpenPanelAction(SettingsPanelActionFrameworkElement Settings)
        {
            if (PanelActionActivate) return;
            Focus();
            ActualFrame.Navigate(BufferSearchDefaultPage(Settings.ElementInPanel.Name) ?? Settings.DefaultPageInPanel);

            DoubleAnimateObj.To = 1d;
            BeginAnimation(OpacityProperty, DoubleAnimateObj);
            AnimationMovePanelAction(PositionAnimActionPanel.Default, Settings.SizedPanel, Settings.ElementInPanel);
            AnimateSizePanelAction(Settings.SizedPanel);
            ActiveObject = Settings;
            PanelActionActivate = true;
            Canvas.SetZIndex(this, ActiveObject.Z);
        }

        /// <summary>
        /// Метод закрытия панели действий
        /// </summary>
        /// <param name="PositionAnim">Состояние анимирования позиции</param>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void ClosePanelAction(PositionAnimActionPanel PositionAnim = PositionAnimActionPanel.Default)
        {
            if (!PanelActionActivate) return;

            DoubleAnimateObj.To = 0d;
            if (ActivateRightClickKeyboardMode) ActivateRightClickKeyboardMode = false;
            if (ActualPage.KeyboardMode) ActualPage.KeyboardMode = false;
            BeginAnimation(OpacityProperty, DoubleAnimateObj);
            AnimationMovePanelAction(PositionAnim, new Size(0, 0), ActiveObject.ElementInPanel);
            AnimateSizePanelAction(new(0, 0));
            AddBufferElementPageAction(ActiveObject);
            
            PanelActionActivate = false;
            string NamePanel = ActiveObject.ElementInPanel.Name;
            ClearInformation();
            EventClosingPanelAction?.Invoke(NamePanel);
        }

        /// <summary>
        /// Перенаправить страницу панели
        /// </summary>
        /// <param name="Content">Новая страница панели</param>
        /// <param name="RightAlign">Правая ориентация движения</param>
        public void NextPage([NotNull()] IPageKey Content, bool RightAlign = true)
        {
            if (!PanelActionActivate) return;
            PanelVerschachtelung = (PanelVerschachtelung + 1) % 2;

            ActualFrame.Opacity = 0d;
            Canvas.SetZIndex(BackFrame, 0);
            Canvas.SetZIndex(ActualFrame, 1);
            BackFrame.IsEnabled = false;
            ActualFrame.IsEnabled = true;
            ActualFrame.BeginAnimation(MarginProperty, null);
            ActualFrame.Margin = !RightAlign ? new(-20, -20, 40, -3) : new(40, -10, -20, -3);
            Content.KeyboardMode = BackPage.KeyboardMode;
            BackPage.KeyboardMode = false;
            ActualFrame.Navigate(Content);

            DoubleAnimateObj.To = 0d;
            BackFrame.BeginAnimation(OpacityProperty, DoubleAnimateObj);
            ThicknessAnimate.To = !RightAlign ? new(40, -20, -20, -3) : new(-20, -20, 40, -3);
            BackFrame.BeginAnimation(MarginProperty, ThicknessAnimate);

            DoubleAnimateObj.To = 1;
            ActualFrame.BeginAnimation(OpacityProperty, DoubleAnimateObj);
            ThicknessAnimate.To = new(0);
            ActualFrame.BeginAnimation(MarginProperty, ThicknessAnimate);
        }

        /// <summary>
        /// Поиск страницы сохранённой в буфере
        /// </summary>
        /// <param name="FrameworkElement_Name">Имя объекта в котором была сохранена страница</param>
        /// <returns>Возможно найденная страница</returns>
        private IPageKey? BufferSearchDefaultPage(string FrameworkElement_Name)
        {
            string[] BufferNames = [.. BufferPages.Select((i) => i.Item2)];
            if (BufferNames.Any((i) => i.Equals(FrameworkElement_Name)))
            {
                int Index = Array.IndexOf(BufferNames, FrameworkElement_Name);
                IPageKey Page = BufferPages[Index].Item1;
                BufferPages.RemoveAt(Index);
                return Page;
            }
            return null;
        }

        /// <summary>
        /// Метод добавления объекта в буфер
        /// </summary>
        /// <param name="SettingsElement">Объект настроек для добавления в буфер</param>
        private void AddBufferElementPageAction(SettingsPanelActionFrameworkElement SettingsElement)
        {
            if (!((IPageKey)ActualFrame.Content).PageName.Equals(SettingsElement.DefaultPageInPanel.PageName))
                BufferPages.Add(((IPageKey)ActualFrame.Content, SettingsElement.ElementInPanel.Name));
        }

        /// <summary>
        /// Метод очистки информации при закрытой панели действий
        /// </summary>
        private void ClearInformation()
        {
            ActiveObject = default;
            BackFrame.Navigate(null);
            ActualFrame.Navigate(null);
        }

        /// <summary>
        /// Метод аниммирования размера панели действий
        /// </summary>
        /// <param name="Sized">Ожидаемый размер панели действий</param>
        private void AnimateSizePanelAction(Size Sized)
        {
            DoubleAnimation animation = DoubleAnimateObj.Clone();
            animation.From = ActualWidth;
            animation.To = Sized.Width;
            BeginAnimation(WidthProperty, animation);
            animation.From = ActualHeight;
            animation.To = Sized.Height;
            if (Sized.Width == 0 && Sized.Height == 0)
            {
                animation.FillBehavior = FillBehavior.Stop;
                void SetZOne(object? sender, EventArgs e)
                {
                    if (PanelActionActivate) return;
                    Canvas.SetZIndex(this, -ActiveObject.Z);
                    animation.FillBehavior = FillBehavior.HoldEnd;
                    animation.Completed -= SetZOne;
                }
                animation.Completed += SetZOne;
            }
            BeginAnimation(HeightProperty, animation);
        }

        /// <summary>
        /// Анимировать передвижение панели действий константно
        /// </summary>
        /// <param name="StylePositionToAnimate">Вид вычисления позиции позиции анимации</param>
        /// <param name="ActionPanelSize">Размер панели действий при взаимодействии</param>
        /// <param name="Element">Элемент в котором будет находиться панель</param>
        private void AnimationMovePanelAction(PositionAnimActionPanel StylePositionToAnimate, Size ActionPanelSize, FrameworkElement Element)
        {
            ThicknessAnimation animation = ThicknessAnimate.Clone();
            if (StylePositionToAnimate == PositionAnimActionPanel.Default)
            {
                Point MousePoint = Mouse.GetPosition((IInputElement)VisualParent);
                Point OffsetPosElement = Element.TransformToAncestor((Visual)VisualParent).Transform(new Point(0, 0));
                if (MousePoint.X + ActionPanelSize.Width > Element.ActualWidth + OffsetPosElement.X)
                    MousePoint.X = Element.ActualWidth + OffsetPosElement.X - ActionPanelSize.Width - 1;
                if (MousePoint.Y + ActionPanelSize.Height > Element.ActualHeight + OffsetPosElement.Y)
                    MousePoint.Y = Element.ActualHeight + OffsetPosElement.Y - ActionPanelSize.Height - 1;
                animation.To = new Thickness(MousePoint.X, MousePoint.Y, 0, 0);
            }
            else if (StylePositionToAnimate == PositionAnimActionPanel.CenterObject)
            {
                animation.To =
                    new Thickness(
                        Margin.Left + Width / 2,
                        Margin.Top + Height / 2,
                        0, 0);
            }
            BeginAnimation(MarginProperty, animation);
        }
    }
}
