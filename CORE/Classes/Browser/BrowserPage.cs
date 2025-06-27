using IEL.Interfaces.Core;
using System.Windows.Controls;
using static IEL.Interfaces.Core.IBrowserPage;

namespace IEL.CORE.Classes.Browser
{
    public class BrowserPage(Page ElementPage, string title, string? description) : IBrowserPage
    {
        /// <summary>
        /// Объект страницы
        /// </summary>
        public Page PageContent { get; } = ElementPage;

        /// <summary>
        /// Название фкладки браузера страниц
        /// </summary>
        public string Title { get; set; } = title;

        /// <summary>
        /// Описание вкладки браузера страниц
        /// </summary>
        public string Description { get; set; } = description ?? string.Empty;

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

        public BrowserEvent? EventDisposed { get; set; }
        /// <summary>
        /// Событие отключение или закрытия или удаления страницы из браузера
        /// </summary>
        public event BrowserEvent? Disposed
        {
            add
            {
                EventDisposed += value;
            }
            remove
            {
                EventDisposed -= value;
            }
        }

        /// <summary>
        /// Очистка ресурсов под событие очистки
        /// </summary>
        public void Dispose()
        {
            EventDisposed?.Invoke(this);
            GC.SuppressFinalize(this);
        }
    }
}
