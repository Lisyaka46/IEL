using IEL.CORE.Classes.ObjectSettings;
using System.Windows;
using System.Windows.Input;

namespace IEL.Interfaces.Front
{
    public interface IIELButton : IIELObject
    {
        /// <summary>
        /// Делегат события активации
        /// </summary>
        public delegate void Activate(object Source, MouseButtonEventArgs eventArgs, bool KeyActivate = false);

        ///// <summary>
        ///// Настройки объекта
        ///// </summary>
        //public IELObjectSetting IELSettingObject { get; }

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
        /// Объект события активации кнопки левым щелчком мыши
        /// </summary>
        public Activate? OnActivateMouseLeft { get; protected set; }

        /// <summary>
        /// Объект события активации кнопки правым щелчком мыши
        /// </summary>
        public Activate? OnActivateMouseRight { get; protected set; }
    }
}
