using IEL.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static IEL.Interfaces.Core.IBrowserPage;

namespace IEL.CORE.Classes.Browser
{
    public class BrowserPage(Page ElementPage) : IBrowserPage
    {
        /// <summary>
        /// Имя страницы
        /// </summary>
        public string PageName => PageContent.Name;

        /// <summary>
        /// Объект страницы
        /// </summary>
        public Page PageContent { get; } = ElementPage;

        public BrowserEvent? EventUnfocusPage { get; set; }
        /// <summary>
        /// Событие получения выключения отображения
        /// </summary>
        public event BrowserEvent? UnfocusPage
        {
            add
            {
                EventUnfocusPage += value;
            }
            remove
            {
                EventUnfocusPage -= value;
            }
        }


        public BrowserEvent? EventFocusPage { get; set; }
        /// <summary>
        /// Событие получения отображения страницы
        /// </summary>
        public event BrowserEvent? FocusPage
        {
            add
            {
                EventUnfocusPage += value;
            }
            remove
            {
                EventUnfocusPage -= value;
            }
        }
    }
}
