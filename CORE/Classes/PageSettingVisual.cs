using System.Windows;
using System.Windows.Controls;

namespace IEL.CORE.Classes
{
    /// <summary>
    /// Инициализратор структуры настройки визуализации страницы в панели действий
    /// </summary>
    /// <param name="Element">Элемент который является зависимым для панели действий</param>
    /// <param name="SourcePage">Страница отображаемая в панели действий</param>
    /// <param name="size">Размер панели действий для страниц данной настройки</param>
    /// <param name="ZIndex">Индекс верхней позиции для корректного отображения страницы</param>
    public struct PageSettingVisual(FrameworkElement Element, Page SourcePage, Size size, int ZIndex = 2)
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
        /// Cтраница управления
        /// </summary>
        public Page PageSource { get; set; } = SourcePage;
    }
}
