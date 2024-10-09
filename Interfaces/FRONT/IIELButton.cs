using IEL.Classes;
using System.Windows;
using System.Windows.Media;

namespace IEL.Interfaces.Front
{
    public interface IIELButton : IIELObject, IIELControl
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
