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
        /// Перечисление всех видов спектров
        /// </summary>
        internal enum SpectrumElement
        {
            /// <summary>
            /// Спектр обычного цвета
            /// </summary>
            Default = 0,

            /// <summary>
            /// Спектр выделенного цвета
            /// </summary>
            Select = 1,

            /// <summary>
            /// Спектр использованного цвета
            /// </summary>
            Used = 2,

            /// <summary>
            /// Спектр отключённого цвета
            /// </summary>
            NotEnabled = 3,
        }

        /// <summary>
        /// Делегат события изменения спектра цвета
        /// </summary>
        internal delegate void ChangeColorEventHandler(SpectrumElement Element, Color Value);

        /// <summary>
        /// Событие изменения спектра
        /// </summary>
        internal event ChangeColorEventHandler? SpectrumChange;

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
                SpectrumChange?.Invoke(SpectrumElement.Default, value);
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
                SpectrumChange?.Invoke(SpectrumElement.NotEnabled, value);
            }
        }
        #endregion

        #region Select
        private Color _Select;
        /// <summary>
        /// Цвет выделенного состояния
        /// </summary>
        public Color Select
        {
            get => _Select;
            set
            {
                _Select = value;
                SpectrumChange?.Invoke(SpectrumElement.Select, value);
            }
        }
        #endregion

        #region Used
        private Color _Used;
        /// <summary>
        /// Цвет нажатого или использованого состояния
        /// </summary>
        public Color Used
        {
            get => _Used;
            set
            {
                _Used = value;
                SpectrumChange?.Invoke(SpectrumElement.Used, value);
            }
        }
        #endregion

        public BrushSettingDNSU()
        {
            Select = Colors.Black;
            Used = Colors.Black;
            NotEnabled = Colors.Black;
            Default = Colors.Black;
        }

        public BrushSettingDNSU(Color Default, Color Select, Color Used, Color NotEnabled)
        {
            this.Select = Select;
            this.Used = Used;
            this.NotEnabled = NotEnabled;
            this.Default = Default;
        }
    }
}
