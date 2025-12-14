using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace IEL.UserElementsControl
{
    /// <summary>
    /// Логика взаимодействия для IELPageController.xaml
    /// </summary>
    public partial class IELPageController : UserControl
    {

        /// <summary>
        /// Объект анимации для управления размерами панели действий
        /// </summary>
        private static readonly DoubleAnimation DoubleAnimate = new(0, TimeSpan.FromMilliseconds(300d))
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
        /// Индекс смены окна страницы
        /// </summary>
        private int PanelVerschachtelung = 0;

        /// <summary>
        /// Объект актуального окна страницы
        /// </summary>
        internal Frame ActualFrame => PanelVerschachtelung % 2 == 0 ? ref FrameActionPanelLeft : ref FrameActionPanelRight;

        /// <summary>
        /// Объект предыдущего окна страницы
        /// </summary>
        internal Frame BackFrame => !(PanelVerschachtelung % 2 == 0) ? ref FrameActionPanelLeft : ref FrameActionPanelRight;

        /// <summary>
        /// Объект актуальной страницы
        /// </summary>
        internal Page? ActualPage => ActualFrame.Content as Page;

        /// <summary>
        /// Объект предыдущей страницы
        /// </summary>
        internal Page? BackPage => BackFrame.Content as Page;

        /// <summary>
        /// Левая анимация переключателя
        /// </summary>
        /// <remarks>
        /// Используется позиция настройки при левом переключении страниц
        /// </remarks>
        public Thickness LeftAnimateSwitch { get; set; }

        /// <summary>
        /// Правая анимация переключателя
        /// </summary>
        /// <remarks>
        /// Используется позиция настройки при правом переключении страниц
        /// </remarks>
        public Thickness RightAnimateSwitch { get; set; }

        /// <summary>
        /// Горизонтальная ориентация содержимого
        /// </summary>
        public new HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return base.HorizontalAlignment;
            }
            set
            {
                base.HorizontalAlignment = value;
                FrameActionPanelLeft.HorizontalAlignment = value;
                FrameActionPanelRight.HorizontalAlignment = value;
            }
        }

        /// <summary>
        /// Вертикальная ориентация содержимого
        /// </summary>
        public new VerticalAlignment VerticalAlignment
        {
            get
            {
                return base.VerticalAlignment;
            }
            set
            {
                base.VerticalAlignment = value;
                FrameActionPanelLeft.VerticalAlignment = value;
                FrameActionPanelRight.VerticalAlignment = value;
            }
        }

        /// <summary>
        /// Делегат события изменения актуального размера
        /// </summary>
        /// <param name="NewSize"></param>
        public delegate void SizeChangedEventHandler(Size NewSize);

        /// <summary>
        /// Событие изменения актуального размера
        /// </summary>
        public new event SizeChangedEventHandler? SizeChanged;

        /// <summary>
        /// Инициализировать объект интерфейса. Контроллер страничных объектов
        /// </summary>
        public IELPageController()
        {
            InitializeComponent();
            LeftAnimateSwitch = new(0);
            RightAnimateSwitch = new(0);
            FrameActionPanelLeft.Navigated += (sender, e) =>
            {
                if (e.Content == null) return;
                FrameActionPanelLeft.UpdateLayout();
                if (ActualFrame.Equals(FrameActionPanelLeft))
                    SizeChanged?.Invoke(new(FrameActionPanelLeft.ActualWidth, FrameActionPanelLeft.ActualHeight));
            };
            FrameActionPanelRight.Navigated += (sender, e) =>
            {
                if (e.Content == null) return;
                FrameActionPanelRight.UpdateLayout();
                if (ActualFrame.Equals(FrameActionPanelRight))
                    SizeChanged?.Invoke(new(FrameActionPanelRight.ActualWidth, FrameActionPanelRight.ActualHeight));
            };
        }

        /// <summary>
        /// Перенаправить страницу
        /// </summary>
        /// <param name="Content">Новая страница</param>
        /// <param name="RightAlign">Правая ориентация движения</param>
        public void NextPage([NotNull()] Page Content, bool RightAlign = true)
        {
            DoubleAnimation animation_double = DoubleAnimate.Clone();
            ThicknessAnimation animation_thickness = ThicknessAnimate.Clone();
            PanelVerschachtelung = (PanelVerschachtelung + 1) % 2;

            ActualFrame.Opacity = 0d;
            Canvas.SetZIndex(BackFrame, 0);
            Canvas.SetZIndex(ActualFrame, 1);
            BackFrame.NavigationService.Navigate(null);
            BackFrame.IsEnabled = false;
            ActualFrame.IsEnabled = true;
            ActualFrame.BeginAnimation(MarginProperty, null);
            ActualFrame.Margin = !RightAlign ? LeftAnimateSwitch : RightAnimateSwitch;
            Content.HorizontalAlignment = HorizontalAlignment;
            Content.VerticalAlignment = VerticalAlignment;
            ActualFrame.Navigate(Content);
            ActualFrame.UpdateLayout();

            animation_thickness.To = !RightAlign ? RightAnimateSwitch : LeftAnimateSwitch;
            BackFrame.BeginAnimation(MarginProperty, animation_thickness);
            animation_thickness.To = new(0);
            ActualFrame.BeginAnimation(MarginProperty, animation_thickness);

            animation_double.To = 0d;
            BackFrame.BeginAnimation(OpacityProperty, animation_double);
            animation_double.To = 1d;
            ActualFrame.BeginAnimation(OpacityProperty, animation_double);
        }

        /// <summary>
        /// Переместить текущую страницу к определённой точке
        /// </summary>
        /// <param name="MoveTo">Точка перемещения</param>
        /// <param name="Millisecond">Время анимации в миллисекундах</param>
        public void MoveActualPage(Thickness MoveTo, uint Millisecond = 100u)
        {
            ThicknessAnimation animation_thickness = ThicknessAnimate.Clone();
            animation_thickness.To = MoveTo;
            animation_thickness.Duration = TimeSpan.FromMilliseconds(Millisecond);
            ActualFrame.BeginAnimation(MarginProperty, animation_thickness);
        }

        /// <summary>
        /// Закрыть элемент фрейма
        /// </summary>
        public void ClosePage()
        {
            BackFrame.Navigate(null);
            BackFrame.Source = null;
            ActualFrame.Navigate(null);
            ActualFrame.Source = null;
        }
    }
}
