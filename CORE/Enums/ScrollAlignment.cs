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
}
