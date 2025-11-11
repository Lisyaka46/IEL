using IEL.CORE.Classes.ObjectSettings;
using IEL.CORE.Enums;
using System.Windows;
using System.Windows.Input;

namespace IEL.Interfaces.Front
{
    /// <summary>
    /// Интерфейс реализации всех кнопочных элементов
    /// </summary>
    public interface IIELButton : IIELObject
    {
        /// <summary>
        /// Делегат события активации
        /// </summary>
        public delegate void ActivateHandler(object Source, MouseButtonEventArgs eventArgs, bool KeyActivate = false);

        /// <summary>
        /// Скругление границ
        /// </summary>
        public CornerRadius CornerRadius { get; protected set; }

        /// <summary>
        /// Толщина границ
        /// </summary>
        public Thickness BorderThicknessBlock { get; protected set; }

        /// <summary>
        /// Смещение контента в объекте
        /// </summary>
        public Thickness PaddingContent { get; protected set; }

        /// <summary>
        /// Объект события нажатия на левую кнопку мыши
        /// </summary>
        public ActivateHandler? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события нажатия на правую кнопку мыши
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
