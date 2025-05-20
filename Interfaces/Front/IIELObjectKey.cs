using IEL.Interfaces.Front;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IEL.Interfaces.Front
{
    public interface IIELObjectKey : IIELObject
    {
        /// <summary>
        /// Активность видимости символа действия активации
        /// </summary>
        public bool CharKeyboardActivate { get; protected set; }

        /// <summary>
        /// Клавиша отвечающая за активацию
        /// </summary>
        public Key? CharKeyKeyboard { get; }

        /// <summary>
        /// Объект события активации кнопки левым щелчком мыши
        /// </summary>
        public IIELObject.Activate? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации кнопки правым щелчком мыши
        /// </summary>
        public IIELObject.Activate? OnActivateMouseRight { get; set; }
    }
}
