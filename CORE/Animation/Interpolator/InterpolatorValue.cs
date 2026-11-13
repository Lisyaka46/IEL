using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace LibraryIEL.CORE.Animation.Interpolator
{
    /// <summary>
    /// Абстрактный базовый преобразователь значения
    /// </summary>
    internal abstract class InterpolatorBase : AnimationTimeline
    {
        /// <summary>
        /// Тип преобразовываемого значения
        /// </summary>
        public abstract Type InterpolatorType { get; }

        /// <summary>
        /// Начальное значение анимации
        /// </summary>
        public abstract object? From { get; set; }

        /// <summary>
        /// Конечное значение анимации
        /// </summary>
        public abstract object? To { get; set; }

        /// <summary>
        /// Объект вычисления прогресса анимирования
        /// </summary>
        public IEasingFunction? EasingFunction { get; set; }
    }

    /// <summary>
    /// Абстрактный преобразователь типизированного значения
    /// </summary>
    /// <typeparam name="T">Тип преобразовываемого значения</typeparam>
    internal abstract class InterpolatorValue<T> : InterpolatorBase where T : struct
    {
        /// <summary>
        /// Преобразовываемый тип значения
        /// </summary>
        public override Type InterpolatorType => typeof(T);

        /// <summary>
        /// Начальное значение анимации
        /// </summary>
        public T? FromTyped { get; set; } = null;
        public override object? From
        {
            get => FromTyped;
            set => FromTyped = (T?)value;
        }

        /// <summary>
        /// Конечное значение анимации
        /// </summary>
        public T ToTyped { get; set; } = default;
        public override object? To
        {
            get => ToTyped;
            set => ToTyped = (T)value!;
        }

        /// <summary>
        /// Динамическое свойство отображающее значение прогресса анимирования
        /// </summary>
        protected double ProgressAnimate = 0d;

        /// <summary>
        /// Преобразование значения относительно анимирования свойства
        /// </summary>
        public abstract T InterpolateTyped(T From, T To);

        /// <summary>
        /// Анимируемый тип менеджером анимаций
        /// </summary>
        /// <remarks>
        /// Это свойство является изменяемым и отражает лишь последний анимированный тип
        /// </remarks>
        public override Type TargetPropertyType => typeof(T);

        /// <summary>
        /// Расчитать текущее значение анимирования
        /// </summary>
        public override object GetCurrentValue(object DefaultStartValue, object DefaultEndValue, AnimationClock SourceAnimationClock)
        {
            if (!SourceAnimationClock.CurrentProgress.HasValue)
                return From ?? DefaultStartValue;
            ProgressAnimate = EasingFunction?.Ease(SourceAnimationClock.CurrentProgress.Value) ?? SourceAnimationClock.CurrentProgress.Value;

            return InterpolateTyped(FromTyped ?? (T)DefaultStartValue, ToTyped);
        }

        /// <summary>
        /// Получить замороженный объект преобразователя значений
        /// </summary>
        protected override Freezable CreateInstanceCore() => (Freezable)MemberwiseClone();
    }
}
