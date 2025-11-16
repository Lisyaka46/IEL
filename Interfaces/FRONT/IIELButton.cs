using IEL.CORE.BaseUserControls;
using IEL.CORE.Classes.ObjectSettings;
using IEL.CORE.Enums;
using System.Windows;
using System.Windows.Input;

namespace IEL.Interfaces.Front
{
    /// <summary>
    /// Интерфейс реализации всех кнопочных элементов
    /// </summary>
    internal interface IIELButton
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
    }
}
