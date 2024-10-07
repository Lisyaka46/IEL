using IEL.Classes;
using System.Windows;

namespace IEL.Interfaces.Front
{
    public interface IIELButton : IIELObject
    {
        /// <summary>
        /// Перечисление состояний отображения кнопки
        /// </summary>
        public enum StateButton
        {
            /// <summary>
            /// Обычное отображение кнопки
            /// </summary>
            Default = 0,

            /// <summary>
            /// Отображение кнопки с левосторонней стрелкой
            /// </summary>
            LeftArrow = 1,

            /// <summary>
            /// Отображение кнопки с правосторонней стрелкой
            /// </summary>
            RightArrow = 2,
        }

        /// <summary>
        /// Обект настройки поведения анимации цвета
        /// </summary>
        public IELSettingAnimate SettingAnimate { get; }

        /// <summary>
        /// Количество миллисекунд для анимации
        /// </summary>
        public int AnimationMillisecond { get; set; }

        /// <summary>
        /// Скругление границ
        /// </summary>
        public CornerRadius CornerRadius { get; set; }

        /// <summary>
        /// Толщина границ
        /// </summary>
        public Thickness BorderThicknessBlock { get; set; }

        /// <summary>
        /// Длительность задержки в миллисекундах
        /// </summary>
        public double IntervalHover { get; set; }

        /// <summary>
        /// Состояние отображения кнопки
        /// </summary>
        public StateButton StateVisualizationButton { get; }

        /// <summary>
        /// Событие задержки курсора на элементе
        /// </summary>
        public abstract event EventHandler? MouseHover;
    }
}
