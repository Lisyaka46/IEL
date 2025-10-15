namespace IEL.Interfaces.Core
{
    /// <summary>
    /// Интерфейс элемента браузера страницы IEL
    /// </summary>
    public interface IBrowserPage : IDisposable
    {
        /// <summary>
        /// Делегат обычного события реализуемое браузером
        /// </summary>
        /// <param name="Source">Элемент который вызвал данное событие</param>
        public delegate void BrowserEventHandler(IBrowserPage Source);

        /// <summary>
        /// Событие отключения фокуса на элемент страницы браузера
        /// </summary>
        public event BrowserEventHandler? UnfocusPage;

        /// <summary>
        /// Событие добавления фокуса на элемент страницы браузера
        /// </summary>
        public event BrowserEventHandler? FocusPage;
    }
}
