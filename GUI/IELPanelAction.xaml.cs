using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using IEL.Interfaces.Front;
using IEL.CORE.Enums;
using IEL.CORE.Classes;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELPanelAction.xaml
    /// </summary>
    public partial class IELPanelAction : UserControl, IIELObject
    {
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

        /// <summary>
        /// История используемых страниц
        /// </summary>
        private readonly List<PanelActionSettingVisual> HistoryBufferPages = [];

        /// <summary>
        /// Имя активного объекта страницы
        /// </summary>
        public string ActualNameFrameElement => PanelActionActivate ? ActiveSettingVisual.ElementInPanel?.Name ?? string.Empty : string.Empty;

        /// <summary>
        /// Актуальный статус активности режима клавиатуры в активной странице
        /// </summary>
        internal bool ActualKeyboardMode
        {
            get => ActiveSettingVisual.ActiveSource.IsKeyboardMode;
            set => ActiveSettingVisual.ActiveSource.IsKeyboardMode = value;
        }

        /// <summary>
        /// Объект настроек панели для активного объекта реализации
        /// </summary>
        private PanelActionSettingVisual ActiveSettingVisual;

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
                    AnimateSizePanelAction(new(ActiveSettingVisual.SizedPanel.Width + 16, ActiveSettingVisual.SizedPanel.Height + 16));
                }
                else
                {
                    if (MainPageController.ActualPage == null) return;
                    if (ActualKeyboardMode && !SelectButtonKeyboardMode)
                    {
                        SelectButtonKeyboardMode = true;
                        ActiveSettingVisual.ActiveSource.ActivateElementKey<IIELButtonKey>(e.Key, ActionButton.BlinkActivate,
                            ActivateRightClickKeyboardMode ? OrientationActivate.RightButton : OrientationActivate.LeftButton);
                    }
                }
            };
            KeyUp += (sender, e) =>
            {
                if (!PanelActionActivate && !BlockWhileEvent) return;
                else BlockWhileEvent = false;
                if (e.Key == KeyActivateKeyboardMode)
                {
                    ActualKeyboardMode = !ActualKeyboardMode;
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
                        ActiveSettingVisual.ActiveSource.ActivateElementKey<IIELButtonKey>(e.Key, ActionButton.ActionActivate,
                            ActivateRightClickKeyboardMode ? OrientationActivate.RightButton : OrientationActivate.LeftButton);
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
        /// Метод использования панели действий независимо на её состояния
        /// </summary>
        /// <param name="SettingVisual">Объект настроек для взаимодействия с панелью действий</param>
        public void UsingPanelAction(PanelActionSettingVisual SettingVisual)
        {
            if (!PanelActionActivate) OpenPanelAction(SettingVisual);
            else
            {
                if (!ActiveSettingVisual.ElementInPanel.Equals(SettingVisual.ElementInPanel))
                {
                    if (SettingVisual.ElementInPanel.ActualWidth < SettingVisual.SizedPanel.Width)
                        SettingVisual.SizedPanel = new(SettingVisual.ElementInPanel.ActualWidth, SettingVisual.SizedPanel.Height);
                    if (SettingVisual.ElementInPanel.ActualHeight < SettingVisual.SizedPanel.Height)
                        SettingVisual.SizedPanel = new(SettingVisual.SizedPanel.Width, SettingVisual.ElementInPanel.ActualHeight);
                    AddBufferElementPageAction(ActiveSettingVisual);
                    ThicknessAnimate.Duration = TimeSpan.FromMilliseconds(360d);
                    PanelActionSettingVisual? SearchSettingVisual = BufferSearchSettingVisual(SettingVisual);
                    PagePanelAction ActivatePage = SearchSettingVisual.HasValue ? SearchSettingVisual.Value.ActiveSource : SettingVisual.DefaultSource;
                    NextPage(ActivatePage);
                    ThicknessAnimate.Duration = TimeSpan.FromMilliseconds(300d);
                    AnimateSizePanelAction(SettingVisual.SizedPanel);
                    ActiveSettingVisual = SearchSettingVisual ?? SettingVisual;
                }
                AnimationMovePanelAction(PositionAnimActionPanel.Cursor, SettingVisual.SizedPanel, SettingVisual.ElementInPanel);
            }
        }

        /// <summary>
        /// Метод открытия панели действий
        /// </summary>
        /// <param name="SettingVisual">Объект настроек для открытия панели действий</param>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private void OpenPanelAction(PanelActionSettingVisual SettingVisual)
        {
            if (PanelActionActivate) return;
            Focus();
            PanelActionSettingVisual SearchSettingVisual = BufferSearchSettingVisual(SettingVisual) ?? SettingVisual;
            SearchSettingVisual.ActiveSource.IsKeyboardMode = false;
            NextPage(SearchSettingVisual);
            DoubleAnimateObj.To = 1d;
            BeginAnimation(OpacityProperty, DoubleAnimateObj);
            AnimationMovePanelAction(PositionAnimActionPanel.Cursor, SettingVisual.SizedPanel, SettingVisual.ElementInPanel);
            AnimateSizePanelAction(SettingVisual.SizedPanel);
            ActiveSettingVisual = SearchSettingVisual;
            PanelActionActivate = true;
            Canvas.SetZIndex(this, ActiveSettingVisual.Z);
        }

        /// <summary>
        /// Метод закрытия панели действий
        /// </summary>
        /// <param name="PositionAnim">Состояние анимирования позиции</param>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void ClosePanelAction(PositionAnimActionPanel PositionAnim = PositionAnimActionPanel.Cursor)
        {
            if (!PanelActionActivate) return;

            DoubleAnimateObj.To = 0d;
            if (ActivateRightClickKeyboardMode) ActivateRightClickKeyboardMode = false;
            ActiveSettingVisual.ActiveSource.IsKeyboardMode = false;
            BeginAnimation(OpacityProperty, DoubleAnimateObj);
            AnimationMovePanelAction(PositionAnim, new Size(0, 0), ActiveSettingVisual.ElementInPanel);
            AnimateSizePanelAction(new(0, 0));
            AddBufferElementPageAction(ActiveSettingVisual);
            
            PanelActionActivate = false;
            string NamePanel = ActiveSettingVisual.ElementInPanel.Name;
            ClearInformation();
            EventClosingPanelAction?.Invoke(NamePanel);
        }

        /// <summary>
        /// Перенаправить страницу панели и переместиться в другой элемент
        /// </summary>
        /// <param name="SettingVisual">Настройки для переключения между объектами</param>
        /// <exception cref="Exception">Исключение при отключённом состоянии панели действий</exception>
        public void NextPage(PanelActionSettingVisual SettingVisual)
        {
            //if (!PanelActionActivate)
            //{
            //    OpenPanelAction(SettingVisual);
            //    return;
            //}
            double X = Mouse.GetPosition((IInputElement)VisualParent).X;
            AnimationMovePanelAction(PositionAnimActionPanel.Cursor, SettingVisual.SizedPanel, SettingVisual.ElementInPanel);
            //NextPage(SettingPage, X >= Margin.Left);
            if (PanelActionActivate)
            {
                SettingVisual.ActiveSource.IsKeyboardMode = ActualKeyboardMode;
                ActualKeyboardMode = false;
            }
            ActiveSettingVisual = SettingVisual;
            MainPageController.NextPage(ActiveSettingVisual.ActiveSource.ObjectPage, X >= Margin.Left);
        }

        /// <summary>
        /// Перенаправить страницу панели
        /// </summary>
        /// <param name="SettingPage">Новая страница панели</param>
        /// <param name="RightAlign">Правая ориентация движения</param>
        public void NextPage([NotNull()] PagePanelAction NextPagePanelAction, bool RightAlign = true)
        {
            if (!PanelActionActivate) return;
            NextPagePanelAction.IsKeyboardMode = ActualKeyboardMode;
            ActiveSettingVisual.ActiveSource = NextPagePanelAction;
            MainPageController.NextPage(NextPagePanelAction.ObjectPage, RightAlign);
        }

        /// <summary>
        /// Поиск настроек страниц сохранённых в буфере
        /// </summary>
        /// <param name="SettingVisual">Настройка визуализации страниц</param>
        /// <returns>Возможно найденный объект настроек</returns>
        private PanelActionSettingVisual? BufferSearchSettingVisual(PanelActionSettingVisual SettingVisual)
        {
            FrameworkElement[] FrameElements = [.. HistoryBufferPages.Select((i) => i.ElementInPanel)];
            int index = Array.IndexOf(FrameElements, SettingVisual.ElementInPanel);
            return index == -1 ? null : HistoryBufferPages[index];
        }

        /// <summary>
        /// Метод добавления объекта в буфер
        /// </summary>
        /// <param name="SettingsElement">Объект настроек для добавления в буфер</param>
        private void AddBufferElementPageAction(PanelActionSettingVisual SettingsElement)
        {
            FrameworkElement[] FrameElements = [.. HistoryBufferPages.Select((i) => i.ElementInPanel)];
            int index = Array.IndexOf(FrameElements, SettingsElement.ElementInPanel);
            if (index == -1 || HistoryBufferPages.Count == 0)
                HistoryBufferPages.Add(SettingsElement);
            else if (!HistoryBufferPages[index].ActiveSource.Equals(SettingsElement.ActiveSource))
                HistoryBufferPages[index] = SettingsElement;
        }

        /// <summary>
        /// Метод очистки информации при закрытой панели действий
        /// </summary>
        private void ClearInformation()
        {
            ActiveSettingVisual = default;
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
                    Canvas.SetZIndex(this, -ActiveSettingVisual.Z);
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
            if (StylePositionToAnimate == PositionAnimActionPanel.Cursor)
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
