using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Point = System.Windows.Point;

namespace IEL.CORE.Animation
{
    /// <summary>
    /// Класс менеджера анимаций
    /// </summary>
    public class AnimationManager
    {
        /// <summary>
        /// Массив объектов анимаций
        /// </summary>
        private List<AnimationTypeBase> ListAnimations;

        /// <summary>
        /// Словарь всех объектов управляемых анимаций по типам самих анимаций
        /// </summary>
        private Dictionary<Type, AnimationTypeBase> DictionaryAnimationTypes;

        /// <summary>
        /// Словарь всех объектов управляемых анимаций по типам анимируемых значений
        /// </summary>
        private Dictionary<Type, AnimationTypeBase> DictionaryAnimationValueTypes;

        /// <summary>
        /// Инициализировать объект менеджера анимаций
        /// </summary>
        public AnimationManager()
        {
            ListAnimations =
            [
                new ThicknessAnimationType<ThicknessAnimation>(new(new Thickness(0), TimeSpan.FromMilliseconds(300d))
                {
                    DecelerationRatio = 0.6d,
                    EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut },
                    From = null
                }),
                new DoubleAnimationType<DoubleAnimation>(new(0, TimeSpan.FromMilliseconds(250d))
                {
                    DecelerationRatio = 0.2d,
                    EasingFunction = new QuinticEase() { EasingMode = EasingMode.EaseOut },
                    From = null
                }),
                new ColorAnimationType<ColorAnimation>(new(Colors.Black, TimeSpan.FromMilliseconds(250d))
                {
                    DecelerationRatio = 0.2d,
                    EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseOut },
                    From = null
                }),
                new PointAnimationType<PointAnimation>(new(new Point(0, 0), TimeSpan.FromMilliseconds(250d))
                {
                    DecelerationRatio = 0.2d,
                    EasingFunction = new QuinticEase() { EasingMode = EasingMode.EaseOut },
                    From = null
                }),
                new RectAnimationType<RectAnimation>(new(new Rect(), TimeSpan.FromMilliseconds(250d))
                {
                    DecelerationRatio = 0.8d,
                    EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut },
                    From = null
                }),
            ];
            DictionaryAnimationTypes = new()
            {
                [typeof(ThicknessAnimation)] = ListAnimations[0],
                [typeof(DoubleAnimation)] = ListAnimations[1],
                [typeof(ColorAnimation)] = ListAnimations[2],
                [typeof(PointAnimation)] = ListAnimations[3],
                [typeof(RectAnimation)] = ListAnimations[4],
            };
            DictionaryAnimationValueTypes = new()
            {
                [typeof(Thickness)] = ListAnimations[0],
                [typeof(double)] = ListAnimations[1],
                [typeof(Color)] = ListAnimations[2],
                [typeof(Point)] = ListAnimations[3],
                [typeof(Rect)] = ListAnimations[4],
            };

        }

        /// <summary>
        /// Анимировать учитывая условность наличия менеджера анимаций
        /// </summary>
        /// <param name="SourceManager">Текущий менеджер анимаций</param>
        /// <param name="Element">Анимируемый элемент</param>
        /// <param name="PropertySet">Анимироваемое свойство</param>
        /// <param name="ValueFrom">Начальное значение</param>
        /// <param name="ValueTo">Конечное значение</param>
        /// <param name="Duration">Длительность анимации</param>
        public static void AnimateTakingZeroFromTo<T>(in AnimationManager? SourceManager, in T Element,
            DependencyProperty PropertySet, object? ValueFrom, object? ValueTo, TimeSpan Duration) where T : IAnimatable
        {
            if (ValueTo == null) return;
            else if (SourceManager == null)
            {
                if (Element is Animatable AnimatableSetter) AnimatableSetter.SetValue(PropertySet, ValueTo);
                else if (Element is DependencyObject DependencyObjectSetter) DependencyObjectSetter.SetValue(PropertySet, ValueTo);
            }
            else
            {
                AnimationTypeBase SourceTimeLine = SourceManager.DictionaryAnimationValueTypes[PropertySet.PropertyType];
                SourceTimeLine.From = ValueFrom;
                SourceTimeLine.To = ValueTo;
                SourceTimeLine.AnimateEffect(Element, PropertySet, Duration);
            }
        }

        /// <summary>
        /// Анимировать учитывая условность наличия менеджера анимаций
        /// </summary>
        /// <param name="SourceManager">Текущий менеджер анимаций</param>
        /// <param name="Element">Анимируемый элемент</param>
        /// <param name="PropertySet">Анимироваемое свойство</param>
        /// <param name="ValueTo">Конечное значение</param>
        /// <param name="Duration">Длительность анимации</param>
        public static void AnimateTakingZeroTo<T>(in AnimationManager? SourceManager, in T Element,
            DependencyProperty PropertySet, object ValueTo, TimeSpan Duration) where T : IAnimatable =>
                AnimateTakingZeroFromTo(in SourceManager, in Element, PropertySet, null, ValueTo, Duration);

        /// <summary>
        /// Получить объект анимации для конкретного типа
        /// </summary>
        /// <typeparam name="T">Опорный тип анимации</typeparam>
        public T GetCloneAnimationElementFromType<T>() where T : AnimationTimeline // T => ThicknessAnimation
        {
            if (DictionaryAnimationTypes.TryGetValue(typeof(T), out var anim))
                return (T)anim.SourceTimeLine.Clone();
            throw new NotImplementedException("Входной тип является не предусматриваемым в поддержке через анимацию OPL");
        }
    }
}
