using System.Windows;
using System.Windows.Controls;

namespace IEL.CORE.Classes
{
    public struct PanelActionSettingVisual(FrameworkElement Element, PagePanelAction SettingPage, Size size, int ZIndex = 2)
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
        /// Активная страница управления
        /// </summary>
        public PagePanelAction ActiveSource { get; internal set; } = SettingPage;

        private readonly PagePanelAction _DefaultSourcePage = SettingPage;
        /// <summary>
        /// Стартовая страница управления
        /// </summary>
        public PagePanelAction DefaultSource
        {
            get
            {
                ActiveSource = _DefaultSourcePage;
                return _DefaultSourcePage;
            }
        }
    }
}
