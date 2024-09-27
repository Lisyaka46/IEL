using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IEL.Interfaces.Front
{
    public interface IIELButton : IIELObject
    {
        #region Default
        /// <summary>
        /// Цвет границы
        /// </summary>
        public Color DefaultBorderBrush { get; set; }

        /// <summary>
        /// Цвет фона
        /// </summary>
        public Color DefaultBackground { get; set; }
        #endregion


        #region Select
        /// <summary>
        /// Выделенный цвет границы
        /// </summary>
        public Color SelectBorderBrush { get; set; }

        /// <summary>
        /// Выделенный цвет фона
        /// </summary>
        public Color SelectBackground { get; set; }
        #endregion


        #region Clicked
        /// <summary>
        /// Нажатый цвет границы
        /// </summary>
        public Color ClickedBorderBrush { get; set; }

        /// <summary>
        /// Нажатый цвет фона
        /// </summary>
        public Color ClickedBackground { get; set; }
        #endregion


        #region NotEnabled
        /// <summary>
        /// Выключенный цвет границы
        /// </summary>
        public Color NotEnabledBorderBrush { get; set; }

        /// <summary>
        /// Выключенный цвет фона
        /// </summary>
        public Color NotEnabledBackground { get; set; }
        #endregion


        #region AnimationMillisecond
        /// <summary>
        /// Количество миллисекунд для анимации
        /// </summary>
        public int AnimationMillisecond { get; set; }
        #endregion

        /// <summary>
        /// Скругление границ
        /// </summary>
        public CornerRadius CornerRadius { get; set; }

        /// <summary>
        /// Толщина границ
        /// </summary>
        public Thickness BorderThicknessBlock { get; set; }

        /// <summary>
        /// Длительность задержки в миллисекундах
        /// </summary>
        public double IntervalHover { get; set; }

        /// <summary>
        /// Событие задержки курсора на элементе
        /// </summary>
        public abstract event EventHandler? MouseHover;
    }
}
