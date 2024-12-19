using System.Windows.Input;
using System.Windows.Media;

namespace IEL.Interfaces.Front
{
    public interface IIELButtonKey : IIELButton, IIELObjectKey
    {
        /// <summary>
        /// Делегат события активации
        /// </summary>
        /// <param name="KeyboardActivate">Активировался ли объект с помощью клавиатуры</param>
        public delegate void Activate(bool KeyboardActivate);
        
        /// <summary>
        /// Объект события активации кнопки левым щелчком мыши
        /// </summary>
        public Activate? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации кнопки правым щелчком мыши
        /// </summary>
        public Activate? OnActivateMouseRight { get; set; }

        /// <summary>
        /// Анимация мерцания
        /// </summary>
        [MTAThread()]
        public void BlinkAnimation();
    }
}
