using System.Windows;
using System.Windows.Media.Animation;

namespace IEL.CORE.Animation
{
    internal class RectAnimationType<T> : AnimationTypeBase where T : RectAnimation
    {
        /// <summary>
        /// Объект анимации Rect
        /// </summary>
        public T SourceAnimation { get; protected set; }

        internal RectAnimationType(T SourceAnimation) : base(SourceAnimation)
        {
            this.SourceAnimation = SourceAnimation;
        }

        /// <summary>
        /// Анимировать эффект объекта
        /// </summary>
        /// <param name="Element">Объект анимации</param>
        /// <param name="Property">Анимируемое свойство</param>
        /// <param name="From">Значение от которого начинается анимация</param>
        /// <param name="To">Значение к которому стремится анимация</param>
        /// <param name="Duration">Количество миллисекунд для анимации</param>
        public void AnimateEffect(IAnimatable Element, DependencyProperty Property, Rect From, Rect To, TimeSpan Duration)
        {
            SourceAnimation.Duration = Duration;
            SourceAnimation.From = From;
            SourceAnimation.To = To;
            Element.BeginAnimation(Property, SourceAnimation, HandoffBehavior.SnapshotAndReplace);
        }

        /// <summary>
        /// Анимировать эффект объекта
        /// </summary>
        /// <param name="Element">Объект анимации</param>
        /// <param name="Property">Анимируемое свойство</param>
        /// <param name="To">Значение к которому стремится анимация</param>
        /// <param name="Duration">Количество миллисекунд для анимации</param>
        public void AnimateEffect(IAnimatable Element, DependencyProperty Property, Rect To, TimeSpan Duration)
        {
            SourceAnimation.Duration = Duration;
            SourceAnimation.From = (Rect?)(Element as DependencyObject)?.GetValue(Property) ??
                throw new Exception("Не удалось распознать стартовую точку анимации");
            SourceAnimation.To = To;
            Element.BeginAnimation(Property, SourceAnimation, HandoffBehavior.SnapshotAndReplace);
        }
    }
}
