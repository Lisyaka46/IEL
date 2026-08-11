using System.Reflection;
using System.Windows;
using System.Windows.Media.Animation;

namespace IEL.CORE.Animation
{
    /// <summary>
    /// Базовый класс анимационного элемента программы
    /// </summary>
    internal abstract class AnimationTypeBase(AnimationTimeline timeline)
    {
        /// <summary>
        /// Объект анимирования свойства
        /// </summary>
        public virtual AnimationTimeline SourceTimeLine { get; } = timeline;

        /// <summary>
        /// Информация о свойстве From
        /// </summary>
        private readonly PropertyInfo PropertyFrom =
            timeline.GetType().GetProperty("From") ?? throw new Exception("Отсутствие ожидаемого свойства From");

        object? _From;
        /// <summary>
        /// Стартовая точка анимации
        /// </summary>
        public object? From
        {
            get => _From;
            set
            {
                if (value != null && value.GetType() != SourceTimeLine.TargetPropertyType)
                    throw new InvalidOperationException("Невозможно присвоить значение несоответствующее анимироваемому типу");
                _From = value;
            }
        }

        /// <summary>
        /// Информация о свойстве To
        /// </summary>
        private readonly PropertyInfo PropertyTo =
            timeline.GetType().GetProperty("To") ?? throw new Exception("Отсутствие ожидаемого свойства To");

        object? _To;
        /// <summary>
        /// Стартовая точка анимации
        /// </summary>
        public object? To
        {
            get => _To;
            set
            {
                if (value != null && value.GetType() != SourceTimeLine.TargetPropertyType)
                    throw new InvalidOperationException("Невозможно присвоить значение несоответствующее анимироваемому типу");
                _To = value;
            }
        }

        /// <summary>
        /// Анимировать эффект объекта по общему объекту анимирования
        /// </summary>
        /// <param name="Element">Объект анимации</param>
        /// <param name="Property">Анимируемое свойство</param>
        /// <param name="Duration">Количество миллисекунд для анимации</param>
        public virtual void AnimateEffect(IAnimatable Element, DependencyProperty Property, TimeSpan Duration)
        {
            SourceTimeLine.Duration = Duration;
            SetAnimateTimeLinePropertyFromTo();
            Element.BeginAnimation(Property, SourceTimeLine, HandoffBehavior.SnapshotAndReplace);
        }

        /// <summary>
        /// Контролируемо присвоить к процессу анимации значения начала и конца
        /// </summary>
        internal void SetAnimateTimeLinePropertyFromTo()
        {
            PropertyTo.SetValue(SourceTimeLine, To);
            PropertyFrom.SetValue(SourceTimeLine, From);
        }
    }
}
