using System.Windows.Input;

namespace IEL.Interfaces.Front
{
    internal interface IIELButtonKey
    {
        /// <summary>
        /// Активность видимости символа действия активации
        /// </summary>
        public bool CharKeyboardActivate { get; protected set; }

        /// <summary>
        /// Клавиша отвечающая за активацию
        /// </summary>
        public Key CharKeyKeyboard { get; }

        /// <summary>
        /// Воспроизвести анимацию мерцания
        /// </summary>
        protected void BlinkAnimation();

        /// <summary>
        /// Воспроизвести анимацию выделения
        /// </summary>
        protected void UnfocusAnimation();
    }
}
