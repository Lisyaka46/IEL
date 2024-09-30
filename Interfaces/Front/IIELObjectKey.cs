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
        protected bool CharKeyboardActivate { get; set; }

        /// <summary>
        /// Клавиша отвечающая за активацию
        /// </summary>
        public Key? CharKeyKeyboard { get; }
    }
}
