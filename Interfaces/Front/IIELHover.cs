using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEL.Interfaces.Front
{
    public interface IIELHover
    {
        /// <summary>
        /// Длительность задержки в миллисекундах
        /// </summary>
        public double IntervalHover { get; set; }

        /// <summary>
        /// Событие задержки курсора на элементе
        /// </summary>
        public abstract event EventHandler? MouseHover;
    }
}
