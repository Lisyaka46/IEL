using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEL.Interfaces.Front
{
    public interface IIELStateVisualizationButton : IIELEventsVision
    {
        /// <summary>
        /// Перечисление состояний отображения
        /// </summary>
        public enum StateButton
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
        /// Состояние отображения
        /// </summary>
        public StateButton StateVisualizationButton { get; }
    }
}
