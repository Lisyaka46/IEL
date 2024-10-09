using IEL.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEL.Interfaces.Front
{
    public interface IIELInley : IIELButtonDefault
    {
        /// <summary>
        /// Страница заголовка
        /// </summary>
        public IPageDefault Page { get; }
    }
}
