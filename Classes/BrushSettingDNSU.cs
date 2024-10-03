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
        /// Стили создания цветов
        /// </summary>
        internal enum CreateStyle
        {
            /// <summary>
            /// Стиль фонового цвета
            /// </summary>
            Background = 0,

            /// <summary>
            /// Стиль цвета границ
            /// </summary>
            BorderBrush = 1,

            /// <summary>
            /// Стиль цвета текста
            /// </summary>
            Foreground = 2,
        }

        /// <summary>
        /// Делегат события изменения спектра цвета
        /// </summary>
        internal delegate void ChangeSpectrumDefaultEventHandler(Color Value);

        /// <summary>
        /// Событие изменения спектра
        /// </summary>
        internal event ChangeSpectrumDefaultEventHandler? SpectrumDefaultChange;

        /// <summary>
        /// Состояние активности элемента
        /// </summary>
        /// <remarks>
        /// При отключённой активности будут доступны только спектры цвета <b>Default</b> или <b>NotEnabled</b>
        /// <code></code>
        /// При попытке вызова отключённого спектра будет выводится спектр <b>Default</b>
        /// </remarks>
        internal bool IsEnabled { get; set; }

        #region Default
        private Color _Default;
        /// <summary>
        /// Цвет обычного состояния
        /// </summary>
        public Color Default
        {
            get => _Default;
            internal set
            {
                _Default = value;
                SpectrumDefaultChange?.Invoke(value);
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
            internal set
            {
                _NotEnabled = value;
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
            get => IsEnabled ? _Select : _Default;
            internal set
            {
                _Select = value;
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
            get => IsEnabled ? _Used : _Default;
            internal set
            {
                _Used = value;
            }
        }
        #endregion

        internal BrushSettingDNSU()
        {
            Select = Colors.Black;
            Used = Colors.Black;
            NotEnabled = Colors.Black;
            Default = Colors.Black;
            IsEnabled = false;
        }

        internal BrushSettingDNSU(Color Default, Color Select, Color Used, Color NotEnabled, ChangeSpectrumDefaultEventHandler? changeSpectrum = null)
        {
            IsEnabled = true;
            SpectrumDefaultChange = changeSpectrum;
            this.Default = Default;
            this.Select = Select;
            this.Used = Used;
            this.NotEnabled = NotEnabled;
        }

        internal BrushSettingDNSU(CreateStyle Style, ChangeSpectrumDefaultEventHandler? changeSpectrum = null)
        {
            IsEnabled = true;
            SpectrumDefaultChange = changeSpectrum;
            switch (Style)
            {
                case CreateStyle.Background:
                    Default = Color.FromRgb(58, 143, 108);
                    Select = Color.FromRgb(59, 172, 109);
                    Used = Color.FromRgb(150, 198, 140);
                    NotEnabled = Color.FromRgb(197, 97, 104);
                    break;
                case CreateStyle.BorderBrush:
                    Default = Color.FromRgb(0, 0, 0);
                    Select = Color.FromRgb(26, 53, 30);
                    Used = Color.FromRgb(101, 82, 76);
                    NotEnabled = Color.FromRgb(152, 29, 54);
                    break;
                case CreateStyle.Foreground:
                    Default = Color.FromRgb(0, 0, 0);
                    Select = Color.FromRgb(28, 54, 24);
                    Used = Color.FromRgb(61, 98, 94);
                    NotEnabled = Color.FromRgb(148, 0, 46);
                    break;
            }
        }
    }
}
