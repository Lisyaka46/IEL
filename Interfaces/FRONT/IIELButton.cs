using IEL.Classes;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IEL.Interfaces.Front
{
    public interface IIELButton : IIELObject, IIELControl, IIELHover
    {
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
    }
}
