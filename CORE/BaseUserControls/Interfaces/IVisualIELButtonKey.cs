using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IEL.CORE.BaseUserControls.Interfaces
{
    public interface IVisualIELButtonKey : IVisualIELButton
    {
        /// <summary>
        /// Активность видимости символа действия активации кнопки
        /// </summary>
        public bool CharKeyboardActivate { get; protected set; }

        /// <summary>
        /// Клавиша отвечающая за активацию кнопки
        /// </summary>
        public Key? CharKeyKeyboard { get; protected set; }
    }
}
