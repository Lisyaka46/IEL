using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEL.CORE.Enums
{
    /// <summary>
    /// Перечисление вариаций позиционирования
    /// </summary>
    public enum OrientationBorderPosition
    {
        /// <summary>
        /// Слева снизу
        /// </summary>
        LeftDown = 0,

        /// <summary>
        /// Слева сверху
        /// </summary>
        LeftUp = 1,

        /// <summary>
        /// Справа сверху
        /// </summary>
        RightUp = 2,

        /// <summary>
        /// Справа снизу
        /// </summary>
        RightDown = 3,

        /// <summary>
        /// Автоматическое определение оптимальной позиции элемента сообщения
        /// </summary>
        Auto = 4,
    }
}
