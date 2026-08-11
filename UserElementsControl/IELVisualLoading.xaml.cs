using IEL.CORE.Animation;
using IEL.UserElementsControl.Base;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace IEL.UserElementsControl
{
    /// <summary>
    /// Логика взаимодействия для OPLVisualLoading.xaml
    /// </summary>
    public partial class IELVisualLoading : IELContainerBase
    {
        /// <summary>
        /// Объект менеджера анимаций настроек OPL
        /// </summary>
        public override AnimationManager? ManagerAnimation
        {
            get => base.ManagerAnimation;
            set
            {
                if (value == null) Animation = null;
                else
                {
                    Animation = new()
                    {
                        From = 0d,
                        To = -7.44d,
                        EasingFunction = null,
                        Duration = TimeSpan.FromMilliseconds(800d),
                        FillBehavior = FillBehavior.HoldEnd,
                        RepeatBehavior = RepeatBehavior.Forever,
                    };
                }
                base.ManagerAnimation = value;
            }
        }

        /// <summary>
        /// Объект анимации управляемый отображением циклом загрузки
        /// </summary>
        private DoubleAnimation? Animation;

        #region Properties

        #region Opacity
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty OpacityProperty =
            DependencyProperty.Register("Opacity", typeof(double), typeof(IELVisualLoading),
                new(1d,
                    (sender, e) =>
                    {
                        ((IELVisualLoading)sender).ElementLoading.Opacity = (double)e.NewValue;
                    }));

        /// <summary>
        /// Прозрачность отображения загрузки
        /// </summary>
        public new double Opacity
        {
            get => (double)GetValue(OpacityProperty);
            set => SetValue(OpacityProperty, value);
        }
        #endregion

        #region IsLoading
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(IELVisualLoading),
                new(false, IsLoadingHandler));

        /// <summary>
        /// Установить элементу свойство
        /// </summary>
        private static void IsLoadingHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            IELVisualLoading Source = (IELVisualLoading)Element;
            bool SourceNewValue = (bool)e.NewValue;
            if (SourceNewValue)
                Source.OpenLoading();
            else Source.CloseLoading();
        }

        /// <summary>
        /// Состояние отображения загрузки
        /// </summary>
        [Description("Состояние отображения загрузки")]
        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set
            {
                if (value == IsLoading) return;
                SetValue(IsLoadingProperty, value);
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// Инициализировать объект отображения загрузки
        /// </summary>
        public IELVisualLoading()
        {
            InitializeComponent();
            ElementLoading.Opacity = 0d;
            ElementLoading.Stroke = SourceBorderBrush.SourceBrush;
        }

        /// <summary>
        /// Начать отображение загрузки
        /// </summary>
        private void OpenLoading()
        {
            AnimationManager.AnimateTakingZeroTo(ManagerAnimation, ElementLoading, Ellipse.OpacityProperty,
                1d, TimeSpan.FromMilliseconds(600d));
            if (ManagerAnimation != null)
                ElementLoading.BeginAnimation(Ellipse.StrokeDashOffsetProperty, Animation);
        }

        /// <summary>
        /// Закончить отображение загрузки
        /// </summary>
        private void CloseLoading()
        {
            AnimationManager.AnimateTakingZeroTo(ManagerAnimation, ElementLoading, Ellipse.OpacityProperty,
                0d, TimeSpan.FromMilliseconds(600d));
            ElementLoading.BeginAnimation(Ellipse.StrokeDashOffsetProperty, null);
            AnimationManager.AnimateTakingZeroTo(ManagerAnimation, ElementLoading, Ellipse.StrokeDashOffsetProperty,
                0d, TimeSpan.FromMilliseconds(570d));
        }
    }
}
