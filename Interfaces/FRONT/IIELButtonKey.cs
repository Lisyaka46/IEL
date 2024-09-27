using System.Windows.Input;

namespace IEL.Interfaces.Front
{
    public interface IIELButtonKey : IIELButton
    {
        /// <summary>
        /// Активность видимости символа действия активации кнопки
        /// </summary>
        public bool CharKeyKeyboardActivate { get; set; }

        /// <summary>
        /// Клавиша отвечающая за активацию кнопки
        /// </summary>
        public Key? CharKeyKeyboard { get; set; }

        /// <summary>
        /// Объект события активации кнопки левым щелчком мыши
        /// </summary>
        public Activate? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации кнопки правым щелчком мыши
        /// </summary>
        public Activate? OnActivateMouseRight { get; set; }

        /// <summary>
        /// Делегат события активации
        /// </summary>
        /// <param name="KeyboardActivate">Активировался ли объект с помощью клавиатуры</param>
        public delegate void Activate(bool KeyboardActivate);

        /// <summary>
        /// Анимация мерцания
        /// </summary>
        [MTAThread()]
        public void BlinkAnimation();
    }
}
