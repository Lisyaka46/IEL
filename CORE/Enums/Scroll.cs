using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEL.CORE.Enums
{
    /// <summary>
    /// Перечисление вариаций позиционирования горизонтального скроллбара
    /// </summary>
    public enum HorizontalScrollAlignment
    {
        /// <summary>
        /// Позиционирование сверху
        /// </summary>
        Up = 0,

        /// <summary>
        /// Позиционирование снизу
        /// </summary>
        Down = 1,
    }

    /// <summary>
    /// Перечисление вариаций позиционирования вертикального скроллбара
    /// </summary>
    public enum VerticalScrollAlignment
    {
        /// <summary>
        /// Позиционирование слева
        /// </summary>
        Left = 0,

        /// <summary>
        /// Позиционирование справа
        /// </summary>
        Right = 1,
    }

    /// <summary>
    /// Перечисление состояний реализаций прокрутки контекста
    /// </summary>
    public enum ScrollOrientation
    {
        /// <summary>
        /// Горизонтальная ориентация прокрутки контента
        /// </summary>
        Horizontal = 0,

        /// <summary>
        /// Вертикальная ориентация прокрутки контента
        /// </summary>
        Vertical = 1,
    }
}
