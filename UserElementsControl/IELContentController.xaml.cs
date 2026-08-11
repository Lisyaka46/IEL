using IEL.CORE.Animation;
using IEL.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace IEL.UserElementsControl
{
    /// <summary>
    /// Контроллер управления отображения контента
    /// </summary>
    /// <remarks>
    /// Отображает контент подключая его из <see cref="UserControl"/><br/>
    /// Поддерживает управление анимированием переключения контента<br/>
    /// </remarks>
    public partial class IELContentController : UserControl, IIELAnimate
    {
        #region Properties

        #region LeftSwitchMargin
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty LeftSwitchMarginProperty =
            DependencyProperty.Register(nameof(LeftSwitchMargin), typeof(Thickness), typeof(IELContentController),
                new(new Thickness(-20, 0, 20, 0)));

        ///// <summary>
        ///// Обработчик события изменения значения свойства <see cref="LeftSwitchMarginProperty"/>
        ///// </summary>
        //private static void LeftSwitchMarginChangedHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        //{
        //    IELContentController Source = (IELContentController)Element;
        //    Thickness SourceNewValue = (Thickness)e.NewValue;
        //    Source.LeftSwitchMargin = SourceNewValue;
        //}

        /// <summary>
        /// Смещение контента при переключении его влево
        /// </summary>
        /// <remarks>
        /// Отражает позицию, к которой стремится прошлый контейнер при переключении контента влево<br/>
        /// </remarks>
        public Thickness LeftSwitchMargin
        {
            get => (Thickness)GetValue(LeftSwitchMarginProperty);
            set => SetValue(LeftSwitchMarginProperty, value);
        }
        #endregion

        #region RightSwitchMargin
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty RightSwitchMarginProperty =
            DependencyProperty.Register(nameof(RightSwitchMargin), typeof(Thickness), typeof(IELContentController),
                new(new Thickness(20, 0, -20, 0)));

        ///// <summary>
        ///// Обработчик события изменения значения свойства <see cref="RightSwitchMarginProperty"/>
        ///// </summary>
        //private static void RightSwitchMarginChangedHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        //{
        //    IELContentController Source = (IELContentController)Element;
        //    Thickness SourceNewValue = (Thickness)e.NewValue;
        //    Source.RightSwitchMargin = SourceNewValue;
        //}

        /// <summary>
        /// Смещение контента при переключении его вправо
        /// </summary>
        /// <remarks>
        /// Отражает позицию, к которой стремится прошлый контейнер при переключении контента вправо<br/>
        /// </remarks>
        public Thickness RightSwitchMargin
        {
            get => (Thickness)GetValue(RightSwitchMarginProperty);
            set => SetValue(RightSwitchMarginProperty, value);
        }
        #endregion

        #region HorizontalAlignment
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static new readonly DependencyProperty HorizontalAlignmentProperty =
            DependencyProperty.Register(nameof(HorizontalAlignment), typeof(HorizontalAlignment), typeof(IELContentController),
                new(HorizontalAlignment.Stretch, HorizontalAlignmentChangedHandler));

        /// <summary>
        /// Обработчик события изменения значения свойства <see cref="HorizontalAlignmentProperty"/>
        /// </summary>
        private static void HorizontalAlignmentChangedHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            IELContentController Source = (IELContentController)Element;
            HorizontalAlignment SourceNewValue = (HorizontalAlignment)e.NewValue;
            Source.ContainerLeft.HorizontalAlignment = SourceNewValue;
            Source.ContainerRight.HorizontalAlignment = SourceNewValue;
        }

        /// <summary>
        /// Горизонтальная ориентация контента в контейнерах <see cref="ContainerLeft"/> и <see cref="ContainerRight"/>
        /// </summary>
        public new HorizontalAlignment HorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(HorizontalAlignmentProperty);
            set => SetValue(HorizontalAlignmentProperty, value);
        }
        #endregion

        #region VerticalAlignment
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static new readonly DependencyProperty VerticalAlignmentProperty =
            DependencyProperty.Register(nameof(VerticalAlignment), typeof(VerticalAlignment), typeof(IELContentController),
                new(VerticalAlignment.Stretch, VerticalAlignmentChangedHandler));

        /// <summary>
        /// Обработчик события изменения значения свойства <see cref="VerticalAlignmentProperty"/>
        /// </summary>
        private static void VerticalAlignmentChangedHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            IELContentController Source = (IELContentController)Element;
            VerticalAlignment SourceNewValue = (VerticalAlignment)e.NewValue;
            Source.ContainerLeft.VerticalAlignment = SourceNewValue;
            Source.ContainerRight.VerticalAlignment = SourceNewValue;
        }

        /// <summary>
        /// Вертикальная ориентация контента в контейнерах <see cref="ContainerLeft"/> и <see cref="ContainerRight"/>
        /// </summary>
        public new VerticalAlignment VerticalAlignment
        {
            get => (VerticalAlignment)GetValue(VerticalAlignmentProperty);
            set => SetValue(VerticalAlignmentProperty, value);
        }
        #endregion

        #region SwichTime
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty SwichTimeProperty =
            DependencyProperty.Register(nameof(SwichTime), typeof(TimeSpan), typeof(IELContentController),
                new(TimeSpan.FromMilliseconds(100d)));

        ///// <summary>
        ///// Обработчик события изменения значения свойства <see cref="SwichTimeProperty"/>
        ///// </summary>
        //private static void SwichTimeChangedHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        //{
        //    IELContentController Source = (IELContentController)Element;
        //    TimeSpan SourceNewValue = (TimeSpan)e.NewValue;
        //    Source.SwichTime = SourceNewValue;
        //}

        /// <summary>
        /// Время переключения контента
        /// </summary>
        public TimeSpan SwichTime
        {
            get => (TimeSpan)GetValue(SwichTimeProperty);
            set => SetValue(SwichTimeProperty, value);
        }
        #endregion

        #endregion

        /// <summary>
        /// Объект менеджера анимаций настроек OPL
        /// </summary>
        public virtual AnimationManager? ManagerAnimation { get; set; }

        /// <summary>
        /// Индекс смены контента
        /// </summary>
        private int VerschachteIndex = 0;

        /// <summary>
        /// Объект управляемого текущего контейнера
        /// </summary>
        private ContentControl ActualContainer => VerschachteIndex % 2 == 0 ? ContainerLeft : ContainerRight;

        /// <summary>
        /// Объект управляемого прошлого контейнера
        /// </summary>
        private ContentControl BackContainer => (VerschachteIndex % 2 == 1) ? ContainerLeft : ContainerRight;

        /// <summary>
        /// Объект актуальной страницы
        /// </summary>
        public ContentControl? ActualContent => IsActiveVisualContent ? (ContentControl)ActualContainer.Content : null;

        /// <summary>
        /// Текущее состояние отображения визуального контента
        /// </summary>
        public bool IsActiveVisualContent { get; private set; } = false;

        /// <summary>
        /// Инициализировать объект интерфейса. Контроллер страничных объектов
        /// </summary>
        public IELContentController()
        {
            InitializeComponent();
            //ContainerLeft.Navigated += (sender, e) =>
            //{
            //    if (e.Content == null) return;
            //    ContainerLeft.UpdateLayout();
            //    if (ActualContainer.Equals(ContainerLeft))
            //        SizeChanged?.Invoke(new(ContainerLeft.ActualWidth, ContainerLeft.ActualHeight));
            //};
            //ContainerRight.Navigated += (sender, e) =>
            //{
            //    if (e.Content == null) return;
            //    ContainerRight.UpdateLayout();
            //    if (ActualContainer.Equals(ContainerRight))
            //        SizeChanged?.Invoke(new(ContainerRight.ActualWidth, ContainerRight.ActualHeight));
            //};
            //ContainerLeft.NavigationService.Navigating += (sender, e) =>
            //{
            //    if (e.NavigationMode == NavigationMode.Back)
            //    {
            //        e.Cancel = true;
            //    }
            //};
            //ContainerRight.NavigationService.Navigating += (sender, e) =>
            //{
            //    if (e.NavigationMode == NavigationMode.Back)
            //    {
            //        e.Cancel = true;
            //    }
            //};
        }

        /// <summary>
        /// Перенаправить отображение контенгта на новый элемент <see cref="ContentControl"/>
        /// </summary>
        /// <param name="Content">Новый контент</param>
        /// <param name="RightAlign">Правая ориентация движения</param>
        public void NextElement(ContentControl Content, bool RightAlign = true)
        {
            IsActiveVisualContent = true;
            VerschachteIndex = (VerschachteIndex + 1) % 2;
            ContentControl
                SourceBackContainer = BackContainer,
                SourceActualContainer = ActualContainer;

            SourceBackContainer.IsEnabled = false;

            SourceActualContainer.IsEnabled = true;

            ActualContainer.Content = Content;

            AnimationManager.AnimateTakingZeroTo(ManagerAnimation, BackContainer, MarginProperty,
                !RightAlign ? RightSwitchMargin : LeftSwitchMargin, SwichTime);
            AnimationManager.AnimateTakingZeroFromTo(ManagerAnimation, ActualContainer, MarginProperty,
                RightAlign ? RightSwitchMargin : LeftSwitchMargin, new Thickness(0d), SwichTime);

            AnimationManager.AnimateTakingZeroTo(ManagerAnimation, BackContainer, OpacityProperty,
                0d, SwichTime);
            AnimationManager.AnimateTakingZeroTo(ManagerAnimation, ActualContainer, OpacityProperty,
                1d, SwichTime);
        }

        /// <summary>
        /// Закрыть отображаемый контент
        /// </summary>
        public void CloseElement()
        {
            IsActiveVisualContent = false;
            ActualContainer.IsEnabled = false;
            AnimationManager.AnimateTakingZeroTo(ManagerAnimation, ActualContainer, OpacityProperty,
                0d, SwichTime);
        }
    }
}
