using IEL.CORE.Animation;
using IEL.UserElementsControl.Base;
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

        #region BorderBrush
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty BorderBrushProperty =
            DependencyProperty.Register("BorderBrush", typeof(SolidColorBrush), typeof(IELVisualLoading),
                new(new SolidColorBrush(Colors.Black),
                    (sender, e) =>
                    {
                        ((IELVisualLoading)sender).ElementLoading.Stroke = (SolidColorBrush)e.NewValue;
                    }));

        /// <summary>
        /// Цвет барьера отображения загрузки
        /// </summary>
        public new SolidColorBrush BorderBrush
        {
            get => (SolidColorBrush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }
        #endregion

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

        #endregion

        /// <summary>
        /// Инициализировать объект отображения загрузки
        /// </summary>
        public IELVisualLoading()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Начать отображение загрузки
        /// </summary>
        public void OpenLoading()
        {
            AnimationManager.AnimateTakingZeroTo(ManagerAnimation, this, IELVisualLoading.OpacityProperty,
                1d, TimeSpan.FromMilliseconds(600d));
            if (ManagerAnimation != null)
                ElementLoading.BeginAnimation(Ellipse.StrokeDashOffsetProperty, Animation);

        }

        /// <summary>
        /// Закончить отображение загрузки
        /// </summary>
        public void CloseLoading()
        {
            AnimationManager.AnimateTakingZeroTo(ManagerAnimation, this, IELVisualLoading.OpacityProperty,
                0d, TimeSpan.FromMilliseconds(600d));
            ElementLoading.BeginAnimation(Ellipse.StrokeDashOffsetProperty, null);
            AnimationManager.AnimateTakingZeroTo(ManagerAnimation, ElementLoading, Ellipse.StrokeDashOffsetProperty,
                0d, TimeSpan.FromMilliseconds(570d));
        }
    }
}
