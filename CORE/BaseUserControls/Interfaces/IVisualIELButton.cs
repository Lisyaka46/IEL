using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IEL.CORE.BaseUserControls.Interfaces
{
    public interface IVisualIELButton
    {
        /// <summary>
        /// Скругление границ
        /// </summary>
        public CornerRadius CornerRadius { get; protected set; }

        /// <summary>
        /// Толщина границ
        /// </summary>
        public Thickness BorderThickness { get; protected set; }

        /// <summary>
        /// Смещение контента в объекте
        /// </summary>
        public Thickness PaddingContent { get; protected set; }
    }
}
