using System.Diagnostics;
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
    public partial class IELPanelAction : UserControl, IIELObject
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

        private readonly List<(IPageKey, string)> BufferPages = [];

        /// <summary>
        /// Имя активного объекта страницы
        /// </summary>
        public string ActualNameFrameElement => PanelActionActivate ? ActiveObject.ElementInPanel?.Name ?? string.Empty : string.Empty;

        /// <summary>
        /// Имя активной страницы
        /// </summary>
        public string ActualNamePage => PanelActionActivate ? ((IPage)MainPageController.ActualPage).PageName ?? string.Empty : string.Empty;

        /// <summary>
        /// Актуальный статус активности режима клавиатуры в активной странице
        /// </summary>
        internal bool ActualKeyboardMode => ((IPageKey)MainPageController.ActualPage).KeyboardMode;

        /// <summary>
        /// Объект настроек панели для активного объекта реализации
        /// </summary>
        private PanelActionSettingsFrameworkElement ActiveObject;

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
            MainPageController.LeftAnimateSwitch = new(-20, -20, 40, -3);
            MainPageController.RightAnimateSwitch = new(40, -10, -20, -3);
            KeyDown += (sender, e) =>
            {
                if (!PanelActionActivate && BlockWhileEvent) return;
                else BlockWhileEvent = true;
                if (e.Key == KeyKeyboardModeActivateRightClick)
                {
                    if (ActualKeyboardMode && !ActivateRightClickKeyboardMode)
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
                    if (MainPageController.ActualPage == null) return;
                    if (ActualKeyboardMode && !SelectButtonKeyboardMode)
                    {
                        SelectButtonKeyboardMode = true;
                        ((IPageKey)MainPageController.ActualPage).ActivateElementKey<IIELButtonKey>((Page)MainPageController.ActualFrame.Content, e.Key, IPageKey.ActionButton.BlinkActivate,
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
                    ((IPageKey)MainPageController.ActualPage).KeyboardMode = !ActualKeyboardMode;
                    if (!ActualKeyboardMode && ActivateRightClickKeyboardMode) AnimTextBlockRightClick(false);
                }
                else if (e.Key == KeyKeyboardModeActivateRightClick && ActualKeyboardMode && ActivateRightClickKeyboardMode)
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
                    if (MainPageController.ActualPage == null) return;
                    if (ActualKeyboardMode && SelectButtonKeyboardMode)
                    {
                        SelectButtonKeyboardMode = false;
                        ((IPageKey)MainPageController.ActualPage).ActivateElementKey<IIELButtonKey>((Page)MainPageController.ActualFrame.Content, e.Key, IPageKey.ActionButton.ActionActivate,
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
        public void UsingPanelAction(PanelActionSettingsFrameworkElement Settings)
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
        /// Перемещение панели действий к курсору учитывая активные настройки
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void MovingPanelAction()
        {
            if (!PanelActionActivate) throw new InvalidOperationException("Невозможно переместить объект в отключённом состоянии...");
        }

        /// <summary>
        /// Метод открытия панели действий
        /// </summary>
        /// <param name="Settings">Объект настроек для открытия панели действий</param>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private void OpenPanelAction(PanelActionSettingsFrameworkElement Settings)
        {
            if (PanelActionActivate) return;
            Focus();
            MainPageController.ActualFrame.Navigate((Page)(BufferSearchDefaultPage(Settings.ElementInPanel.Name) ?? Settings.DefaultPageInPanel));
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
            ((IPageKey)MainPageController.ActualPage).KeyboardMode = false;
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
        /// Перенаправить страницу панели и переместиться в другой элемент
        /// </summary>
        /// <remarks>
        /// <b>Страница по умолчанию в настройках не учитывается</b>
        /// </remarks>
        /// <param name="Settings">Настройки для переключения между объектами</param>
        /// <param name="Content">Новая страница панели</param>
        /// <param name="RightAlign">Правая ориентация движения</param>
        /// <exception cref="Exception">Исключение при отключённом состоянии панели действий</exception>
        public void NextPage(PanelActionSettingsFrameworkElement Settings, [NotNull()] IPageKey Content)
        {
            if (!PanelActionActivate) throw new Exception("При отключённом состоянии нельзя переключаться между страницами");
            double X = Mouse.GetPosition((IInputElement)VisualParent).X;
            AnimationMovePanelAction(PositionAnimActionPanel.Default, Settings.SizedPanel, Settings.ElementInPanel);
            NextPage(Content, X >= Margin.Left);    
        }

        /// <summary>
        /// Перенаправить страницу панели
        /// </summary>
        /// <param name="Content">Новая страница панели</param>
        /// <param name="RightAlign">Правая ориентация движения</param>
        public void NextPage([NotNull()] IPageKey Content, bool RightAlign = true)
        {
            if (!PanelActionActivate) return;

            Content.KeyboardMode = ((IPageKey)MainPageController.ActualPage).KeyboardMode;
            ((IPageKey)MainPageController.ActualPage).KeyboardMode = false;
            MainPageController.NextPage((Page)Content, RightAlign);
            /*ActualFrame.Opacity = 0d;
            Canvas.SetZIndex(BackFrame, 0);
            Canvas.SetZIndex(ActualFrame, 1);
            BackFrame.IsEnabled = false;
            ActualFrame.IsEnabled = true;
            ActualFrame.BeginAnimation(MarginProperty, null);
            ActualFrame.Margin = !RightAlign ? MainPageController.LeftAnimateSwitch : MainPageController.RightAnimateSwitch;
            ActualFrame.Navigate(Content);

            DoubleAnimateObj.To = 0d;
            BackFrame.BeginAnimation(OpacityProperty, DoubleAnimateObj);
            DoubleAnimateObj.To = 1;
            ActualFrame.BeginAnimation(OpacityProperty, DoubleAnimateObj);

            ThicknessAnimate.To = !RightAlign ? MainPageController.RightAnimateSwitch : MainPageController.LeftAnimateSwitch;
            BackFrame.BeginAnimation(MarginProperty, ThicknessAnimate);
            ThicknessAnimate.To = new(0);
            ActualFrame.BeginAnimation(MarginProperty, ThicknessAnimate);*/
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
        private void AddBufferElementPageAction(PanelActionSettingsFrameworkElement SettingsElement)
        {
            if (!((IPageKey)MainPageController.ActualFrame.Content).PageName.Equals(SettingsElement.DefaultPageInPanel.PageName))
                BufferPages.Add(((IPageKey)MainPageController.ActualFrame.Content, SettingsElement.ElementInPanel.Name));
        }

        /// <summary>
        /// Метод очистки информации при закрытой панели действий
        /// </summary>
        private void ClearInformation()
        {
            ActiveObject = default;
            MainPageController.BackFrame.Navigate(null);
            MainPageController.ActualFrame.Navigate(null);
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
