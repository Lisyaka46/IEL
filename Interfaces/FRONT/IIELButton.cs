using IEL.Classes;
using System.Windows;
using System.Windows.Media;

namespace IEL.Interfaces.Front
{
    public interface IIELButton : IIELObject, IIELControl
    {
        /// <summary>
        /// Перечисление состояний отображения
        /// </summary>
        public enum StateVisual
        {
            /// <summary>
            /// Обычное отображение
            /// </summary>
            Default = 0,

            /// <summary>
            /// Отображение с левосторонней стрелкой
            /// </summary>
            LeftArrow = 1,

            /// <summary>
            /// Отображение с правосторонней стрелкой
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
        /// Состояние отображения
        /// </summary>
        public StateVisual StateVisualization { get; }

        /// <summary>
        /// Событие задержки курсора на элементе
        /// </summary>
        public abstract event EventHandler? MouseHover;
    }
}
