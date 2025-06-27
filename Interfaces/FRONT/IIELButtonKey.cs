using System.Windows.Input;

namespace IEL.Interfaces.Front
{
    internal interface IIELButtonKey : IIELButton
    {
        /// <summary>
        /// Активность видимости символа действия активации
        /// </summary>
        public bool CharKeyboardActivate { get; protected set; }

        /// <summary>
        /// Клавиша отвечающая за активацию
        /// </summary>
        public Key? CharKeyKeyboard { get; }

        //
        public void BlinkAnimation();

        //
        public void LeaveAnimation();
    }
}
