using IEL.Classes.Browser;
using IEL.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEL.Interfaces.Front
{
    internal interface IIELInley : IIELButton
    {
        /// <summary>
        /// Данные об странице заголовка
        /// </summary>
        public BrowserPage? Page { get; }

        /// <summary>
        /// Объект страницы
        /// </summary>
        protected object Content { get; }
    }
}
