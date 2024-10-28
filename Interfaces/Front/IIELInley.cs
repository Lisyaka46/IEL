using IEL.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEL.Interfaces.Front
{
    internal interface IIELInley : IIELButtonDefault
    {
        /// <summary>
        /// Данные об странице заголовка
        /// </summary>
        public IPageDefault? Page { get; }

        /// <summary>
        /// Объект страницы
        /// </summary>
        protected object Content { get; }
    }
}
