using LibraryIEL.CORE.Animation.Interpolator;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Point = System.Windows.Point;

namespace LibraryIEL.CORE.Animation
{
    /// <summary>
    /// Класс менеджера анимаций
    /// </summary>
    public static class ManagerAnimation
    {
        /// <summary>
        /// Словарь функций преобразования значения, относительно прогресса анимации
        /// </summary>
        private static readonly Dictionary<Type, InterpolatorBase> InterpolatesValue = [];

        /// <summary>
        /// Объект вычисления прогресса анимирования
        /// </summary>
        public static IEasingFunction? EasingFunction { get; set; } = null;

        /// <summary>
        /// Состояние использования анимирования через менеджер анимаций.<br/>
        /// При отключении, отключает сам менеджер анимаций, из за чего всё анимирование сводится к установке сразу конечного значения
        /// </summary>
        public static bool IsEnabled { get; set; } = false;

        /// <summary>
        /// Последний использованный преобразователь значений для анимаций
        /// </summary>
        private static InterpolatorBase? EndInterpolator = null;

        /// <summary>
        /// Статический коструктор менеджера анимаций
        /// </summary>
        static ManagerAnimation()
        {
            InterpolatesValue.Add(typeof(double), new DoubleInterpolator());
            InterpolatesValue.Add(typeof(Color), new ColorInterpolator());
            InterpolatesValue.Add(typeof(Thickness), new ThicknessInterpolator());
            InterpolatesValue.Add(typeof(CornerRadius), new CornerRadiusInterpolator());
            InterpolatesValue.Add(typeof(Rect), new RectInterpolator());
        }

        /// <summary>
        /// Анимировать учитывая начальное и конечное значение
        /// </summary>
        /// <param name="Element">Анимируемый элемент</param>
        /// <param name="PropertySet">Анимироваемое свойство</param>
        /// <param name="ValueFrom">Начальное значение</param>
        /// <param name="ValueTo">Конечное значение</param>
        /// <param name="Duration">Длительность анимации</param>
        public static void AnimateTakingZeroFromTo<T1, T2>(in T1 Element,
            DependencyProperty PropertySet, T2? ValueFrom, T2 ValueTo, TimeSpan Duration)
            where T1 : IAnimatable where T2 : struct
        {
            if (!IsEnabled)
            {
                if (Element is Animatable AnimatableSetter) AnimatableSetter.SetValue(PropertySet, ValueTo);
                else if (Element is DependencyObject DependencyObjectSetter) DependencyObjectSetter.SetValue(PropertySet, ValueTo);
                return;
            }
            else if (!InterpolatesValue.ContainsKey(typeof(T2)))
                throw new InvalidOperationException(
                    $"Невозможно произвести анимирование свойства \"{typeof(T2).Name}\", " +
                    "так как нет преобразователя значения для этого типа");
            else if (EndInterpolator == null || EndInterpolator.InterpolatorType != typeof(T2))
                EndInterpolator = InterpolatesValue[typeof(T2)];
            EndInterpolator.From = ValueFrom ?? null;
            EndInterpolator.To = ValueTo;
            EndInterpolator.EasingFunction = EasingFunction;
            EndInterpolator.Duration = Duration;
            Element.BeginAnimation(PropertySet, EndInterpolator, HandoffBehavior.Compose);
        }

        /// <summary>
        /// Анимировать учитывая только конечное значение
        /// </summary>
        /// <param name="Element">Анимируемый элемент</param>
        /// <param name="PropertySet">Анимироваемое свойство</param>
        /// <param name="ValueTo">Конечное значение</param>
        /// <param name="Duration">Длительность анимации</param>
        public static void AnimateTakingZeroTo<T1, T2>(in T1 Element,
            DependencyProperty PropertySet, T2 ValueTo, TimeSpan Duration) where T1 : IAnimatable where T2 : struct =>
                AnimateTakingZeroFromTo(in Element, PropertySet, new Nullable<T2>(), ValueTo, Duration);
    }
}
