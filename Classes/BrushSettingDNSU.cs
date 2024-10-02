using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IEL.Classes
{
    public class BrushSettingDNSU
    {
        /// <summary>
        /// Перечисление элементов DNSU
        /// </summary>
        public enum ElementColorSpectrum
        {
            /// <summary>
            /// Обычный цвет
            /// </summary>
            Default = 0,

            /// <summary>
            /// Выделенный цвет
            /// </summary>
            Select = 1,

            /// <summary>
            /// Использованный цвет
            /// </summary>
            Used = 2,

            /// <summary>
            /// Отключённый цвет
            /// </summary>
            NotEnabled = 3,
        }

        /// <summary>
        /// Делегат события изменения спектра цвета
        /// </summary>
        /// <param name="Element">Элемент спектра</param>
        /// <param name="Value">Новое значение цвета</param>
        public delegate void ChangeSpectrumEventHandler(ElementColorSpectrum Element, Color Value);

        /// <summary>
        /// Событие изменения спектра
        /// </summary>
        public event ChangeSpectrumEventHandler? ChangeSpectrum;

        #region Default
        private Color _Default;
        /// <summary>
        /// Цвет обычного состояния
        /// </summary>
        public Color Default
        {
            get => _Default;
            set
            {
                _Default = value;
                ChangeSpectrum?.Invoke(ElementColorSpectrum.Default, value);
            }
        }
        #endregion

        #region NotEnabled
        private Color _NotEnabled;
        /// <summary>
        /// Цвет отключённого состояния состояния
        /// </summary>
        public Color NotEnabled
        {
            get => _NotEnabled;
            set
            {
                _NotEnabled = value;
                ChangeSpectrum?.Invoke(ElementColorSpectrum.NotEnabled, value);
            }
        }
        #endregion

        #region Select
        private Color _Select = Colors.Black;
        /// <summary>
        /// Цвет выделенного состояния
        /// </summary>
        public Color Select
        {
            get => _Select;
            set
            {
                _Select = value;
                ChangeSpectrum?.Invoke(ElementColorSpectrum.Select, value);
            }
        }
        #endregion

        #region Used
        private Color _Used = Colors.Black;
        /// <summary>
        /// Цвет нажатого или использованого состояния
        /// </summary>
        public Color Used
        {
            get => _Used;
            set
            {
                _Used = value;
                ChangeSpectrum?.Invoke(ElementColorSpectrum.Used, value);
            }
        }
        #endregion

        /// <summary>
        /// Время анимации
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Анимация цвета в спектре
        /// </summary>
        public readonly ColorAnimation ColorAnimate;

        public BrushSettingDNSU(TimeSpan duration, ChangeSpectrumEventHandler? ChangeSpectrum = null)
        {
            Duration = duration;
            ColorAnimate = new(Colors.Black, Duration);
            Select = Colors.Black;
            Used = Colors.Black;
            NotEnabled = Colors.Black;
            this.ChangeSpectrum = ChangeSpectrum;
            Default = Colors.Black;
        }

        public BrushSettingDNSU(Color Default, Color Select, Color Used, Color NotEnabled, TimeSpan duration, ChangeSpectrumEventHandler? ChangeSpectrum = null)
        {
            Duration = duration;
            ColorAnimate = new(Colors.Black, Duration);
            this.Select = Select;
            this.Used = Used;
            this.NotEnabled = NotEnabled;
            this.ChangeSpectrum = ChangeSpectrum;
            this.Default = Default;
        }
    }
}
