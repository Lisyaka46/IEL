using IEL.CORE.BaseUserControls;
using IEL.CORE.Classes;
using IEL.CORE.Enums;

using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELPanelAction.xaml
    /// </summary>
    public partial class IELPanelAction : IELContainerBase
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
        /// Кнопка которая выделенна клавишей клавиатуры
        /// </summary>
        private IELButtonKeyBase? ButtonKeySelect;

        /// <summary>
        /// Состояние бокирующее повторное срабатывание зажатой клавиши
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
                if (keys[0] == value) return;
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
                if (keys[1] == value) return;
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
                if (keys[2] == value) return;
                keys[2] = value;
            }
        }

        /// <summary>
        /// Объект анимации для управления размерами панели действий
        /// </summary>
        private static readonly ColorAnimation ColorAnimate = new(Colors.Black, TimeSpan.FromMilliseconds(200d))
        {
            DecelerationRatio = 0.6d,
            EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
        };

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

        private bool _KeyboardModeInActualPage;
        /// <summary>
        /// Актуальный статус активности режима клавиатуры в активной странице
        /// </summary>
        internal bool KeyboardModeInActualPage
        {
            get => _KeyboardModeInActualPage;
            set
            {
                if (MainPageController.ActualPage != null) UpdateVisualKeyboardMode(MainPageController.ActualPage, value);
                _KeyboardModeInActualPage = value;
            }
        }

        /// <summary>
        /// Объект настроек панели для активного объекта реализации
        /// </summary>
        private PageSettingVisual ActiveSettingVisual;

        /// <summary>
        /// Делегат события закрытия панели действий
        /// </summary>
        /// <param name="FrameworkElementName">Имя активного объекта для палени действий</param>
        public delegate void ClosingPanelAction(string FrameworkElementName);

        /// <summary>
        /// Событие закрытия панели действий
        /// </summary>
        public event ClosingPanelAction? EventClosingPanelAction;

        /// <summary>
        /// Событие открытия панели действий
        /// </summary>
        public event EventHandler? EventOpenPanelAction;

        /// <summary>
        /// Событие перемещения панели действий
        /// </summary>
        public event EventHandler? EventMovePanelAction;

        /// <summary>
        /// Событие переключения панели действий в новый объект в активном режиме
        /// </summary>
        public event EventHandler? EventMoveNewObjectInActivePanelAction;

        /// <summary>
        /// Отключать режим клавиатуры при закрытии объекта
        /// </summary>
        public bool IsKeyboardModeExit;

        /// <summary>
        /// Сохранённое состояние активности режима клавиатуры
        /// </summary>
        private bool ActiveKeyboardMode;

        /// <summary>
        /// Инициализировать объект интерфейса. Панель действий
        /// </summary>
        public IELPanelAction()
        {
            InitializeComponent();
            #region Background
            #endregion

            #region BorderBrush
            #endregion

            #region Foreground
            #endregion

            IsKeyboardModeExit = true;
            ActiveKeyboardMode = false;
            keys = [Key.Z, Key.Oem3, Key.Escape];
            TextBlockRightButtonIndicatorKey.Opacity = 0d;
            MainPageController.LeftAnimateSwitch = new(-20, -20, 40, -3);
            MainPageController.RightAnimateSwitch = new(40, -10, -20, -3);
            TextBlockRightButtonIndicatorKey.Text = "RIGHT";
            KeyDown += (sender, e) =>
            {
                if (!PanelActionActivate && BlockWhileEvent) return;
                else BlockWhileEvent = true;
                if (e.Key == KeyKeyboardModeActivateRightClick)
                {
                    if (KeyboardModeInActualPage && !ActivateRightClickKeyboardMode)
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
                    if (KeyboardModeInActualPage && !SelectButtonKeyboardMode)
                    {
                        SelectButtonKeyboardMode = true;
                        ButtonKeySelect = SearchButton<IELButtonKeyBase>((Visual)MainPageController.ActualPage.Content, e.Key);
                        ButtonKeySelect?.BlinkAnimation();
                    }
                }
            };
            KeyUp += (sender, e) =>
            {
                if (!PanelActionActivate && !BlockWhileEvent) return;
                else BlockWhileEvent = false;
                if (e.Key == KeyActivateKeyboardMode)
                {
                    KeyboardModeInActualPage = !KeyboardModeInActualPage;
                    ActiveKeyboardMode = KeyboardModeInActualPage;
                    if (!KeyboardModeInActualPage && ActivateRightClickKeyboardMode) AnimTextBlockRightClick(false);
                }
                else if (e.Key == KeyKeyboardModeActivateRightClick && KeyboardModeInActualPage && ActivateRightClickKeyboardMode)
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
                    if (KeyboardModeInActualPage && SelectButtonKeyboardMode)
                    {
                        SelectButtonKeyboardMode = false;
                        if (ButtonKeySelect != null)
                        {
                            ((ActivateRightClickKeyboardMode ? ButtonKeySelect.OnActivateMouseRight : ButtonKeySelect.OnActivateMouseLeft) ??
                                throw new Exception("Недопустимое значение объекта")).Invoke(
                                    ButtonKeySelect, new(Mouse.PrimaryDevice, 0, ActivateRightClickKeyboardMode ? MouseButton.Right : MouseButton.Left), true);
                            ButtonKeySelect.UnfocusAnimation();
                        }
                    }
                }
            };
            Base_BorderContainer.LostFocus += (sender, e) => ClosePanelAction(PositionAnimActionPanel.CenterObject);
        }

        /// <summary>
        /// Обновить отображение режима клавиатуры
        /// </summary>
        /// <param name="SourceUpdate">Страница в которой обновляется состояние</param>
        /// <param name="Value">Значение на которое обновляется визуализация</param>
        private void UpdateVisualKeyboardMode(Page SourceUpdate, bool Value)
        {
            IELButtonKeyBase[]? Buttons = SearchButton<IELButtonKeyBase>((Visual)SourceUpdate.Content);
            if (Buttons != null)
            {
                foreach (IELButtonKeyBase Element in Buttons)
                {
                    Element.IsVisibleKeyActivate = Value;
                }
            }
        }

        /// <summary>
        /// Найти элемент поддерживающий клавишу в странице
        /// </summary>
        /// <typeparam name="T">Тип элемента поддерживающего подключение по клавише</typeparam>
        /// <param name="VisualObject">Ссылка на объект поиска</param>
        private static T[]? SearchButton<T>(Visual VisualObject) where T : UIElement
        {
            var Return = new List<T>(); 
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(VisualObject); i++)
            {
                Visual ChildVisualElement = (Visual)VisualTreeHelper.GetChild(VisualObject, i);
                try
                {
                    T ObjectButton = (T)ChildVisualElement;
                    Return.Add(ObjectButton);
                    //if (ObjectButton.CharKeyKeyboard == key) return ObjectButton; // && ObjectButton.IsEnabled
                }
                catch
                {
                    if (ChildVisualElement.GetType() == typeof(Grid))
                    {
                        T[]? values = SearchButton<T>((Grid)ChildVisualElement);
                        if (values == null) continue;
                        Return.AddRange(values);
                    }
                }
            }
            return Return.Count == 0 ? null : [.. Return];
        }

        /// <summary>
        /// Найти элемент поддерживающий клавишу в странице
        /// </summary>
        /// <typeparam name="T">Тип элемента поддерживающего подключение по клавише</typeparam>
        /// <param name="VisualObject">Ссылка на объект поиска</param>
        /// <param name="key">Ключ клавиши</param>
        private static T? SearchButton<T>(Visual VisualObject, Key key) where T : IELButtonKeyBase
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(VisualObject); i++)
            {
                Visual ChildVisualElement = (Visual)VisualTreeHelper.GetChild(VisualObject, i);
                try
                {
                    T ObjectButton = (T)ChildVisualElement;
                    if (ObjectButton.KeyActivateButton == key && ObjectButton.IsEnabled) return ObjectButton;
                }
                catch
                {
                    if (ChildVisualElement.GetType() == typeof(Grid))
                    {
                        T? value = SearchButton<T>((Grid)ChildVisualElement, key);
                        if (value == null) continue;
                        else return value;
                    }
                }
            }
            return default;
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
        /// <param name="Orientation">Ориентация привязки к объекту</param>
        public void UsingPanelAction(PageSettingVisual SettingVisual, OrientationPanelActionPosition Orientation)
        {
            if (!PanelActionActivate) OpenPanelAction(SettingVisual, Orientation);
            else
            {
                if (!ActiveSettingVisual.ElementInPanel.Equals(SettingVisual.ElementInPanel))
                {
                    if (SettingVisual.ElementInPanel.ActualWidth < SettingVisual.SizedPanel.Width)
                        SettingVisual.SizedPanel = new(SettingVisual.ElementInPanel.ActualWidth, SettingVisual.SizedPanel.Height);
                    if (SettingVisual.ElementInPanel.ActualHeight < SettingVisual.SizedPanel.Height)
                        SettingVisual.SizedPanel = new(SettingVisual.SizedPanel.Width, SettingVisual.ElementInPanel.ActualHeight);
                    ThicknessAnimate.Duration = TimeSpan.FromMilliseconds(360d);

                    MoveNextObjectPage(SettingVisual, Orientation);

                    ThicknessAnimate.Duration = TimeSpan.FromMilliseconds(300d);
                    AnimateSizePanelAction(SettingVisual.SizedPanel);
                    ActiveSettingVisual = SettingVisual;
                }
                EventMovePanelAction?.Invoke(this, EventArgs.Empty);
                AnimationMovePanelAction(PositionAnimActionPanel.Cursor, SettingVisual.SizedPanel, SettingVisual.ElementInPanel, Orientation);
            }
        }

        /// <summary>
        /// Метод открытия панели действий
        /// </summary>
        /// <param name="SettingVisual">Объект настроек для открытия панели действий</param>
        /// <param name="Orientation">Ориентация привязки к объекту</param>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private void OpenPanelAction(PageSettingVisual SettingVisual, OrientationPanelActionPosition Orientation)
        {
            if (PanelActionActivate) return;
            EventOpenPanelAction?.Invoke(this, EventArgs.Empty);
            Focus();
            KeyboardModeInActualPage = !IsKeyboardModeExit && ActiveKeyboardMode;
            MoveNextObjectPage(SettingVisual, Orientation);
            DoubleAnimateObj.To = 1d;
            BeginAnimation(OpacityProperty, DoubleAnimateObj);
            AnimationMovePanelAction(PositionAnimActionPanel.Cursor, SettingVisual.SizedPanel, SettingVisual.ElementInPanel, Orientation);
            AnimateSizePanelAction(SettingVisual.SizedPanel);
            ActiveSettingVisual = SettingVisual;
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
            if (IsKeyboardModeExit)
            {
                ActiveKeyboardMode = false;
                KeyboardModeInActualPage = false;
            }
            BeginAnimation(OpacityProperty, DoubleAnimateObj);
            AnimationMovePanelAction(PositionAnim, new Size(0, 0), ActiveSettingVisual.ElementInPanel, OrientationPanelActionPosition.LeftUp);
            AnimateSizePanelAction(new(0, 0));

            PanelActionActivate = false;
            string NamePanel = ActiveSettingVisual.ElementInPanel.Name;
            ClearInformation();
            EventClosingPanelAction?.Invoke(NamePanel);
        }

        #region NextPage
        /// <summary>
        /// Перенаправить страницу панели и переместиться в другой элемент
        /// </summary>
        /// <param name="SettingVisual">Настройки для переключения между объектами</param>
        /// <param name="Orientation">По какой ориентации перемещать панель действий</param>
        /// <param name="RightAlgin">Справой стороны открывать страницу. При нулевом значении задействует позицию курсора</param>
        public void MoveNextObjectPage([NotNull] PageSettingVisual SettingVisual, OrientationPanelActionPosition Orientation,
            bool? RightAlgin = null)
        {
            AnimationMovePanelAction(PositionAnimActionPanel.Cursor, SettingVisual.SizedPanel,
                SettingVisual.ElementInPanel, Orientation);
            ActiveSettingVisual = SettingVisual;
            EventMoveNewObjectInActivePanelAction?.Invoke(this, EventArgs.Empty);
            NextPageInObject(SettingVisual.PageSource, RightAlgin ?? Mouse.GetPosition((IInputElement)VisualParent).X >= Margin.Left);
        }

        /// <summary>
        /// Перенаправить страницу панели внутри объекта
        /// </summary>
        /// <param name="PageAction">Страница на которую переключается панель действий</param>
        /// <param name="RightAlgin">Справой стороны открывать страницу или нет</param>
        [NonEvent]
        public void NextPageInObject([NotNull] Page PageAction, bool RightAlgin = true)
        {
            MainPageController.NextPage(PageAction, RightAlgin);
            UpdateVisualKeyboardMode(PageAction, ActiveKeyboardMode);
        }
        #endregion

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
            BeginAnimation(HeightProperty, animation, HandoffBehavior.SnapshotAndReplace);
        }

        /// <summary>
        /// Анимировать передвижение панели действий константно
        /// </summary>
        /// <param name="StylePositionToAnimate">Вид вычисления позиции анимации</param>
        /// <param name="ActionPanelSize">Размер панели действий при взаимодействии</param>
        /// <param name="Element">Элемент в котором будет находиться панель</param>
        /// <param name="Orientation">Ориентация привязки к объекту</param>
        private void AnimationMovePanelAction(PositionAnimActionPanel StylePositionToAnimate, Size ActionPanelSize,
            FrameworkElement Element, OrientationPanelActionPosition Orientation)
        {
            ThicknessAnimation animation = ThicknessAnimate.Clone();
            if (StylePositionToAnimate == PositionAnimActionPanel.Cursor)
            {
                // Позиция курсора
                Point MousePoint = Mouse.GetPosition((IInputElement)VisualParent);

                // Смещение позиции области относительно внешнего элемента
                Point OffsetPosElement = Element.TransformToAncestor((Visual)VisualParent).Transform(new Point(0, 0));

                if (Orientation == OrientationPanelActionPosition.Auto)
                {
                    bool Up = MousePoint.Y <= Element.ActualHeight / 2;
                    Orientation = MousePoint.X <= Element.ActualWidth / 2 ?
                        (Up ? OrientationPanelActionPosition.LeftUp : OrientationPanelActionPosition.LeftDown) :
                        (Up ? OrientationPanelActionPosition.RightUp : OrientationPanelActionPosition.RightDown);
                }

                #region (Left/Right)Position
                if (Orientation.Code_lr == 0x01)
                {
                    if (MousePoint.X + ActionPanelSize.Width > Element.ActualWidth + OffsetPosElement.X)
                        MousePoint.X = Element.ActualWidth + OffsetPosElement.X - ActionPanelSize.Width - 1;
                }
                else if (Orientation.Code_lr == 0x00)
                {
                    if (MousePoint.X - ActionPanelSize.Width < OffsetPosElement.X)
                        MousePoint.X = OffsetPosElement.X + 1;
                    else MousePoint.X -= ActionPanelSize.Width;
                }
                #endregion

                #region (Up/Down/Center)Position
                if (Orientation.Code_ud == 0x02)
                {
                    if (MousePoint.Y + ActionPanelSize.Height > Element.ActualHeight + OffsetPosElement.Y)
                        MousePoint.Y = Element.ActualHeight + OffsetPosElement.Y - ActionPanelSize.Height - 1;
                }
                else if (Orientation.Code_ud == 0x00)
                {
                    if (MousePoint.Y - ActionPanelSize.Height < OffsetPosElement.Y)
                        MousePoint.Y = OffsetPosElement.Y + 1;
                    else MousePoint.Y -= ActionPanelSize.Height;
                }
                else if (Orientation.Code_ud == 0x01)
                {
                    if (MousePoint.Y - ActionPanelSize.Height / 2 < OffsetPosElement.Y)
                        MousePoint.Y = OffsetPosElement.Y + 1;
                    else MousePoint.Y -= ActionPanelSize.Height / 2;
                }
                #endregion

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
            BeginAnimation(MarginProperty, animation, HandoffBehavior.SnapshotAndReplace);
        }
    }
}
