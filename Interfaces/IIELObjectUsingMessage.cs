using LibraryIEL.CORE.Themes.Palettes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace LibraryIEL.Interfaces
{
    /// <summary>
    /// Интерфейс элемента, который поддерживает отображение сообщения
    /// </summary>
    public interface IIELObjectUsingMessage
    {
        /// <summary>
        /// Отображаемое сообщение
        /// </summary>
        public string Message { get; internal protected set; }

        /// <summary>
        /// Данные спектра использования цветов
        /// </summary>
        public PaletteData Palette { get; set; }

        /// <summary>
        /// Узнать позицию и размер объекта
        /// </summary>
        /// <param name="RelativeTo">Относительно какого элемента расчитывается позиция</param>
        internal Rect GetRectFrom(UIElement RelativeTo);
    }
}
