using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IEL.Interfaces.Front
{
    public interface IIELButton : IIELObject
    {
        /// <summary>
        /// Делегат события активации
        /// </summary>
        public delegate void Activate(object Source, bool KeyActivate = false);

        /// <summary>
        /// Скругление границ
        /// </summary>
        public CornerRadius CornerRadius { get; set; }

        /// <summary>
        /// Толщина границ
        /// </summary>
        public Thickness BorderThicknessBlock { get; set; }

        /// <summary>
        /// Смещение контента в объекте
        /// </summary>
        public Thickness PaddingContent { get; set; }

        /// <summary>
        /// Объект события активации кнопки левым щелчком мыши
        /// </summary>
        public Activate? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации кнопки правым щелчком мыши
        /// </summary>
        public Activate? OnActivateMouseRight { get; set; }
    }
}
