using IEL.CORE.Classes;
using IEL.CORE.Enums;
using IEL.UserElementsControl.Base;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace IEL.UserElementsControl
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
                if (ActualVisualPage != null)
                    UpdateVisualKeyboardMode(ActualVisualPage, value);
                _KeyboardModeInActualPage = value;
            }
        }

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
        /// Предыдущая точка привязки курсора
        /// </summary>
        private OrientationPositionCursor OldOrientationPositionCursor = OrientationPositionCursor.Empty;

        /// <summary>
        /// Актуальный размер который отображается в объекте
        /// </summary>
        private Size ActualVisualSize = new(0, 0);

        /// <summary>
        /// Объект отображаемой страницы
        /// </summary>
        public Page? ActualVisualPage { get; private set; } = null;

        /// <summary>
        /// Текущий объект в которой отображается панель действий
        /// </summary>
        public FrameworkElement? ActualVisualElement { get; private set; } = null;

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
                    UpdateSizeFromAnimate(new(MainPageController.ActualWidth + 16, MainPageController.ActualHeight + 16));
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
            Base_BorderContainer.LostFocus += (sender, e) =>
            {
                if (PanelActionActivate)
                    ClosePanelAction(PositionAnimActionPanel.CenterObject);
            };

            Width = 0;
            Height = 0;

            //MainPageController.SizeChanged += UpdateSizeFromAnimate;
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
        /// Метод реализуемый условное использование панели действий
        /// </summary>
        /// <remarks>
        /// Использование подразумевает собой открытие панели при её не активном состоянии<br/>
        /// Далее при её активном состоянии и использовании в том же объекте <paramref name="ElementVisual"/>, реализуется перемещение<br/>
        /// При её активном состоянии и фокусе на другой объект <paramref name="ElementVisual"/> 
        /// реализовывает перемещение и изменение страницы <paramref name="PageVisual"/><br/>
        /// <br/>
        /// <b>Не реализует переключение на другую страницу внутри того же объекта</b>
        /// </remarks>
        /// <param name="ElementVisual">Элемент в котором отображается панель действий</param>
        /// <param name="PageVisual">Страница которая будет визуализироваться в панели действий</param>
        /// <param name="PositionAnimate">Вид расчёта позиционирования панели при её перемещении</param>
        /// <param name="Orientation">Ориентация привязки к объекту</param>
        /// <param name="DependencePointOnSize">Ограничивать позиционирование только внутри объекта</param>
        public void UsingPanelAction(FrameworkElement ElementVisual, Page PageVisual,
            PositionAnimActionPanel PositionAnimate = PositionAnimActionPanel.Cursor,
            OrientationPositionCursor Orientation = OrientationPositionCursor.Empty,
            bool DependencePointOnSize = true)
        {
            if (!PanelActionActivate) OpenPanelAction(ElementVisual, PageVisual, PositionAnimate, Orientation, DependencePointOnSize);
            else
            {
                if (ActualVisualElement?.Equals(ElementVisual) ?? false)
                    AnimationMovePanelAction(PositionAnimate, Orientation, DependencePointOnSize);
                else
                    MoveNextObjectPage(ElementVisual, PageVisual, PositionAnimate, Orientation, DependencePointOnSize);
            }
        }

        /// <summary>
        /// Метод открытия панели действий
        /// </summary>
        /// <param name="ElementVisual">Элемент в котором отображается панель действий</param>
        /// <param name="PageVisual">Страница которая будет визуализироваться в панели действий</param>
        /// <param name="Orientation">Ориентация привязки к объекту</param>
        /// <param name="PositionAnimate">Вид расчёта позиционирования панели при её перемещении</param>
        /// <param name="DependencePointOnSize">Ограничивать позиционирование только внутри объекта</param>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void OpenPanelAction(FrameworkElement ElementVisual, Page PageVisual,
            PositionAnimActionPanel PositionAnimate = PositionAnimActionPanel.Cursor,
            OrientationPositionCursor Orientation = OrientationPositionCursor.Empty,
            bool DependencePointOnSize = true)
        {
            if (PanelActionActivate)
                throw new Exception("Невозможно открыть панель действий если она уже открыта!");

            EventOpenPanelAction?.Invoke(this, EventArgs.Empty);
            Focus();
            KeyboardModeInActualPage = !IsKeyboardModeExit && ActiveKeyboardMode;
            DoubleAnimateObj.To = 1d;
            BeginAnimation(OpacityProperty, DoubleAnimateObj, HandoffBehavior.SnapshotAndReplace);
            PanelActionActivate = true;

            MoveNextObjectPage(ElementVisual, PageVisual, PositionAnimate, Orientation, DependencePointOnSize);
            Canvas.SetZIndex(this, 2);
        }

        /// <summary>
        /// Метод закрытия панели действий
        /// </summary>
        /// <param name="PositionAnimate">Вид расчёта позиционирования панели при её перемещении</param>
        /// <param name="PositionAnim">Состояние анимирования позиции</param>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void ClosePanelAction(
            PositionAnimActionPanel PositionAnim = PositionAnimActionPanel.Cursor,
            OrientationPositionCursor PositionAnimate = OrientationPositionCursor.Empty)
        {
            if (!PanelActionActivate || ActualVisualElement == null)
                throw new Exception("Невозможно закрыть панель действий если она уже закрыта!");

            DoubleAnimateObj.To = 0d;
            if (ActivateRightClickKeyboardMode) ActivateRightClickKeyboardMode = false;
            if (IsKeyboardModeExit)
            {
                ActiveKeyboardMode = false;
                KeyboardModeInActualPage = false;
            }
            BeginAnimation(OpacityProperty, DoubleAnimateObj, HandoffBehavior.SnapshotAndReplace);
            UpdateSizeFromAnimate(new Size(0, 0));
            AnimationMovePanelAction(PositionAnim, PositionAnimate, false);

            EventClosingPanelAction?.Invoke(ActualVisualElement.Name);
            ActualVisualElement = null;
            ActualVisualPage = null;
            PanelActionActivate = false;

            MainPageController.BackFrame.Navigate(null);
            MainPageController.ActualFrame.Navigate(null);
        }

        #region NextPage
        /// <summary>
        /// Перенаправить страницу панели и переместиться в другой элемент
        /// </summary>
        /// <param name="ElementVisual">Элемент в котором отображается панель действий</param>
        /// <param name="PageVisual">Страница которая будет визуализироваться в панели действий</param>
        /// <param name="Orientation">По какой ориентации перемещать панель действий</param>
        /// <param name="PositionAnimate">Вид расчёта позиционирования панели при её перемещении</param>
        /// <param name="DependencePointOnSize">Ограничивать позиционирование только внутри объекта</param>
        /// <param name="RightAlgin">Справой стороны открывать страницу. При нулевом значении задействует позицию курсора</param>
        public void MoveNextObjectPage(FrameworkElement ElementVisual, Page PageVisual,
            PositionAnimActionPanel PositionAnimate = PositionAnimActionPanel.Cursor,
            OrientationPositionCursor Orientation = OrientationPositionCursor.Empty,
            bool DependencePointOnSize = true,
            bool? RightAlgin = null)
        {
            ActualVisualElement = ElementVisual;

            //PositionChangeSizeAnimation = PositionAnimActionPanel.ActualPosition.
            EventMoveNewObjectInActivePanelAction?.Invoke(this, EventArgs.Empty);
            NextPageInObject(PageVisual, Orientation, DependencePointOnSize, RightAlgin ?? Mouse.GetPosition((IInputElement)VisualParent).X >= Margin.Left);

            AnimationMovePanelAction(PositionAnimate, Orientation, DependencePointOnSize);
        }

        /// <summary>
        /// Перенаправить страницу панели внутри объекта
        /// </summary>
        /// <param name="PageAction">Страница на которую переключается панель действий</param>
        /// <param name="PositionAnimate">Вид расчёта позиционирования панели при её перемещении</param>
        /// <param name="DependencePointOnSize">Ограничивать позиционирование только внутри объекта</param>
        /// <param name="RightAlgin">Справой стороны открывать страницу или нет</param>
        [NonEvent]
        public void NextPageInObject([NotNull] Page PageAction,
            OrientationPositionCursor PositionAnimate = OrientationPositionCursor.Empty,
            bool DependencePointOnSize = true,
            bool RightAlgin = true)
        {
            ActualVisualPage = PageAction;
            MainPageController.NextPage(PageAction, RightAlgin);
            UpdateVisualKeyboardMode(PageAction, ActiveKeyboardMode);

            UpdateSizeFromAnimate(new(PageAction.Width, PageAction.Height));
            if (ActualVisualElement != null)
                AnimationMovePanelAction(ActualVisualElement, (sbyte)PositionAnimate, new(Margin.Left, Margin.Top), DependencePointOnSize);
        }
        #endregion

        /// <summary>
        /// Метод аниммирования размера панели действий
        /// </summary>
        /// <param name="SizeSource">Константа размера</param>
        private void UpdateSizeFromAnimate(Size SizeSource)
        {
            DoubleAnimation animation = DoubleAnimateObj.Clone();
            animation.To = SizeSource.Width;
            BeginAnimation(WidthProperty, animation, HandoffBehavior.SnapshotAndReplace);
            animation.To = SizeSource.Height;
            if (SizeSource.Width == 0 && SizeSource.Height == 0)
            {
                void SetZOne(object? sender, EventArgs e)
                {
                    if (PanelActionActivate) return;
                    Canvas.SetZIndex(this, -2);
                    animation.Completed -= SetZOne;
                    BeginAnimation(WidthProperty, null);
                    BeginAnimation(HeightProperty, null);
                }
                animation.Completed += SetZOne;
            }
            ActualVisualSize = SizeSource;
            BeginAnimation(HeightProperty, animation, HandoffBehavior.SnapshotAndReplace);
        }

        /// <summary>
        /// Анимировать передвижение панели действий для текущего объекте
        /// </summary>
        /// <param name="StylePositionToAnimate">Вид вычисления позиции анимации</param>
        /// <param name="PositionAnimate">Вид расчёта позиционирования панели при её перемещении</param>
        /// <param name="DependencePointOnSize">Ограничивать позиционирование только внутри объекта</param>
        public void AnimationMovePanelAction(PositionAnimActionPanel StylePositionToAnimate, OrientationPositionCursor PositionAnimate,
            bool DependencePointOnSize = true)
        {
            if (!PanelActionActivate || ActualVisualElement == null)
                throw new Exception("Невозможно передвинуть панель действий при её закрытом состоянии!");

            Point PointAnimation = StylePositionToAnimate switch
            {
                PositionAnimActionPanel.Cursor => Mouse.GetPosition((IInputElement)VisualParent), // Позиция курсора
                PositionAnimActionPanel.CenterObject => new(Margin.Left + Width / 2, Margin.Top + Height / 2),
                _ => throw new Exception("Недопустимое состояние расчёта позиции перемещения панели действий!")
            };

            if (PositionAnimate == OrientationPositionCursor.Auto && PanelActionActivate)
            {
                bool Up = PointAnimation.Y <= ActualVisualSize.Height / 2;
                PositionAnimate = PointAnimation.X <= ActualVisualSize.Width / 2 ?
                    (Up ? OrientationPositionCursor.LeftUp : OrientationPositionCursor.LeftDown) :
                    (Up ? OrientationPositionCursor.RightUp : OrientationPositionCursor.RightDown);
            }

            OldOrientationPositionCursor = PositionAnimate;
            AnimationMovePanelAction(ActualVisualElement, (sbyte)PositionAnimate, PointAnimation, DependencePointOnSize);
        }

        /// <summary>
        /// Анимировать перемещение элемента панели
        /// </summary>
        /// <param name="Element">Объект относительно которого происходит перемещение</param>
        /// <param name="PositionAnimate">Вид расчёта позиционирования панели при её перемещении</param>
        /// <param name="PositionTo">Позиция перемещения</param>
        /// <param name="DependencePointOnSize">Состояние зависимости позиции объекта от размера элемента "<paramref name="Element"/>"</param>
        private void AnimationMovePanelAction(FrameworkElement Element, sbyte PositionAnimate, Point PositionTo,
            bool DependencePointOnSize = true)
        {
            ThicknessAnimation animation = ThicknessAnimate.Clone();

            // Смещение позиции области относительно внешнего элемента
            Point OffsetPosElement = Element.TransformToAncestor((Visual)VisualParent).Transform(new Point(0, 0));

            if (PositionAnimate == -01 && OldOrientationPositionCursor != OrientationPositionCursor.Empty)
                PositionAnimate = (sbyte)OldOrientationPositionCursor;
            if (PositionAnimate != -01)
            {
                #region (Left/Right)Position
                if (PositionAnimate is 10 or 11 or 12)
                {
                    if (PositionTo.X + ActualVisualSize.Width > Element.ActualWidth + OffsetPosElement.X && DependencePointOnSize)
                        PositionTo.X = Element.ActualWidth + OffsetPosElement.X - ActualVisualSize.Width - 1;
                }
                else if (PositionAnimate is 00 or 01 or 02)
                {
                    if (PositionTo.X - ActualVisualSize.Width < OffsetPosElement.X && DependencePointOnSize)
                        PositionTo.X = OffsetPosElement.X + 1;
                    else PositionTo.X -= ActualVisualSize.Width;
                }
                #endregion

                #region (Up/Center/Down)Position
                if (PositionAnimate is 02 or 12)
                {
                    if (PositionTo.Y + ActualVisualSize.Height > Element.ActualHeight + OffsetPosElement.Y && DependencePointOnSize)
                        PositionTo.Y = Element.ActualHeight + OffsetPosElement.Y - ActualVisualSize.Height - 1;
                }
                else if (PositionAnimate is 01 or 11)
                {
                    if (PositionTo.Y - ActualVisualSize.Height / 2 < OffsetPosElement.Y && DependencePointOnSize)
                        PositionTo.Y = OffsetPosElement.Y + 1;
                    else PositionTo.Y -= ActualVisualSize.Height / 2;
                }
                else if (PositionAnimate is 00 or 10)
                {
                    if (PositionTo.Y - ActualVisualSize.Height < OffsetPosElement.Y && DependencePointOnSize)
                        PositionTo.Y = OffsetPosElement.Y + 1;
                    else PositionTo.Y -= ActualVisualSize.Height;
                }
                #endregion
            }

            animation.To = new(PositionTo.X, PositionTo.Y, 0, 0);
            BeginAnimation(MarginProperty, animation, HandoffBehavior.SnapshotAndReplace);
        }
    }
}
