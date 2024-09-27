using System.Windows;

namespace IEL.Interfaces.Core
{
    public struct SettingsPanelActionFrameworkElement(FrameworkElement Element, IPageKey DefaultPage, Size size)
    {
        /// <summary>
        /// Элемент интерфейса в границах которого будет находится панель действий
        /// </summary>
        public FrameworkElement ElementInPanel { get; } = Element;

        /// <summary>
        /// Размер панели действий находясь в объекте
        /// </summary>
        public Size SizedPanel { get; set; } = size;

        /// <summary>
        /// Стартовая страница панели
        /// </summary>
        public IPageKey DefaultPageInPanel { get; set; } = DefaultPage;
    }
}
