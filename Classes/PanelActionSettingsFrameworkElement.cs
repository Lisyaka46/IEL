using System.Windows;

namespace IEL.Interfaces.Core
{
    public struct PanelActionSettingsFrameworkElement(FrameworkElement Element, IPageKey DefaultPage, Size size, int ZIndex = 2)
    {
        /// <summary>
        /// Элемент интерфейса в границах которого будет находится панель действий
        /// </summary>
        public FrameworkElement ElementInPanel { get; } = Element;

        /// <summary>
        /// Z индекс читаемый для объекта
        /// </summary>
        public int Z { get; } = ZIndex;

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
