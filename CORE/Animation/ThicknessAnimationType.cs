using System.Windows;
using System.Windows.Media.Animation;

namespace IEL.CORE.Animation
{
    internal class ThicknessAnimationType<T> : AnimationTypeBase where T : ThicknessAnimation
    {
        /// <summary>
        /// Объект анимации Thickness
        /// </summary>
        public T SourceAnimation { get; protected set; }

        internal ThicknessAnimationType(T SourceAnimation) : base(SourceAnimation)
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
        public void AnimateEffect(IAnimatable Element, DependencyProperty Property, Thickness From, Thickness To, TimeSpan Duration)
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
        public void AnimateEffect(IAnimatable Element, DependencyProperty Property, Thickness To, TimeSpan Duration)
        {
            SourceAnimation.Duration = Duration;
            SourceAnimation.From = (Thickness?)(Element as DependencyObject)?.GetValue(Property) ??
                throw new Exception("Не удалось распознать стартовую точку анимации");
            SourceAnimation.To = To;
            Element.BeginAnimation(Property, SourceAnimation, HandoffBehavior.SnapshotAndReplace);
        }
    }
}
