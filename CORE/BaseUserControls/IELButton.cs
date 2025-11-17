using IEL.CORE.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace IEL.CORE.BaseUserControls
{
    /// <summary>
    /// БАЗОВЫЙ КЛАСС для отображения кнопки IEL
    /// </summary>
    public class IELButton : IELObject
    {
        /// <summary>
        /// Делегат события активации
        /// </summary>
        public delegate void ActivateHandler(object Source, MouseButtonEventArgs eventArgs, bool KeyActivate = false);

        /// <summary>
        /// Объект события активации левым щелчком мыши
        /// </summary>
        public ActivateHandler? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации правым щелчком мыши
        /// </summary>
        public ActivateHandler? OnActivateMouseRight { get; set; }

        /// <summary>
        /// Узнать тип доступных нажатий на элемент
        /// </summary>
        /// <returns>Объект пересичления возможных нажатий</returns>
        internal EventsMouse GetSourceEventMouse()
        {
            if (OnActivateMouseLeft != null)
            {
                if (OnActivateMouseRight != null) return EventsMouse.Full;
                return EventsMouse.Left;
            }
            else if (OnActivateMouseRight != null) return EventsMouse.Right;
            return EventsMouse.Not;
        }
    }
}
