using IEL.CORE.Enums;
using IEL.Interfaces.Front;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IEL.CORE.BaseUserControls
{
    /// <summary>
    /// БАЗОВЫЙ КЛАСС для отображения кнопки IEL
    /// </summary>
    public class IELButton : IELObject, IIELButton
    {
        /// <summary>
        /// Делегат события активации
        /// </summary>
        public delegate void ActivateHandler(object Source, MouseButtonEventArgs eventArgs, bool KeyActivate = false);

        /// <summary>
        /// Скругление границ
        /// </summary>
        public CornerRadius CornerRadius { get; set; }

        /// <summary>
        /// Толщина границ
        /// </summary>
        public Thickness BorderThicknessBlock { get; set; }
        //{
        //    get => BorderThickness;
        //    set => BorderThickness = value;
        //}

        /// <summary>
        /// Смещение контента в объекте
        /// </summary>
        public Thickness PaddingContent { get; set; }
        //{
        //    get => Padding;
        //    set => Padding = value;
        //}

        private new event MouseButtonEventHandler? MouseLeftButtonDown;
        private new event MouseButtonEventHandler? MouseLeftButtonUp;
        /// <summary>
        /// Объект события активации левым щелчком мыши
        /// </summary>
        public ActivateHandler? OnActivateMouseLeft { get; set; }

        private new event MouseButtonEventHandler? MouseRightButtonDown;
        private new event MouseButtonEventHandler? MouseRightButtonUp;
        /// <summary>
        /// Объект события активации правым щелчком мыши
        /// </summary>
        public ActivateHandler? OnActivateMouseRight { get; set; }

        /// <summary>
        /// Инициализировать <b>БАЗОВОЕ ПРЕДСТАВЛЕНИЕ</b> кнопки IEL
        /// </summary>
        public IELButton()
        {
            //InitializeComponent();
        }

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
