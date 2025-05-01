using IEL.Interfaces.Core;
using System.Windows.Media;
using static IEL.Interfaces.Core.IQData;

namespace IEL.Classes
{
    /// <summary>
    /// Класс настройки поведения цвета при разных состояниях объекта
    /// </summary>
    public class BrushSettingQ
    {
        /// <summary>
        /// Стили создания цветов
        /// </summary>
        public enum CreateStyle
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
        /// Делегат события изменения обычного цвета
        /// </summary>
        /// <param name="Value">Новое значение цвета</param>
        internal delegate void ColorDefaultChangeEventHandler(StateSpectrum Spectrum, Color Value);

        /// <summary>
        /// Событие изменения цвета обычного состояния
        /// </summary>
        internal event ColorDefaultChangeEventHandler? ColorDefaultChange;

        /// <summary>
        /// Массив данных цвета
        /// </summary>
        internal QData ColorData { get; set; } = new();

        /// <summary>
        /// Изменить напрямую стиль отображения объекта
        /// </summary>
        /// <param name="Spectrum">Стиль придаваемый использоваемому объекту</param>
        public void InvokeObjectUsedStateColor(StateSpectrum Spectrum)
        {
            if (ColorDefaultChange == null) return;
            Color Value = Spectrum switch
            {
                StateSpectrum.Default => Default,
                StateSpectrum.Select => Select,
                StateSpectrum.Used => Used,
                StateSpectrum.NotEnabled => NotEnabled,
                _ => Default,
            };
            ColorDefaultChange?.Invoke(Spectrum, Value);
        }

        /// <summary>
        /// Состояние активности элемента
        /// </summary>
        /// <remarks>
        /// При отключённой активности будут доступны только спектры цвета <b>Default</b> или <b>NotEnabled</b>
        /// <code></code>
        /// При попытке вызова отключённого спектра будет выводится спектр <b>Default</b>
        /// </remarks>
        internal bool IsEnabled { get; set; }

        private bool _UsedState;
        /// <summary>
        /// Состояние навигации использования
        /// </summary>
        /// <remarks>
        /// При включённом состоянии цвет обычного состояния становится использованным, а использованный обычным
        /// <code></code>
        /// <b>Default <![CDATA[<]]>=<![CDATA[>]]> Used</b>
        /// </remarks>
        public bool UsedState
        {
            get => _UsedState;
            set
            {
                _UsedState = value;
                if (IsEnabled) 
                    ColorDefaultChange?.Invoke(StateSpectrum.Default, ColorData.GetIndexingColor(value ? StateSpectrum.Used : StateSpectrum.Default));
            }
        }

        #region Default
        /// <summary>
        /// Цвет отключённого состояния состояния
        /// </summary>
        public Color Default
        {
            get => ColorData.GetIndexingColor(IsEnabled ? (UsedState ? StateSpectrum.Used : StateSpectrum.Default) : StateSpectrum.NotEnabled);
            set
            {
                ColorData.SetIndexingColor(StateSpectrum.Default, value);
                if (IsEnabled) ColorDefaultChange?.Invoke(StateSpectrum.Default, value);
            }
        }
        #endregion

        #region NotEnabled
        /// <summary>
        /// Цвет отключённого состояния состояния
        /// </summary>
        public Color NotEnabled
        {
            get => ColorData.GetIndexingColor(StateSpectrum.NotEnabled);
            set
            {
                ColorData.SetIndexingColor(StateSpectrum.NotEnabled, value);
                ColorDefaultChange?.Invoke(StateSpectrum.NotEnabled, value);
            }
        }
        #endregion

        #region Select
        /// <summary>
        /// Цвет выделенного состояния
        /// </summary>
        public Color Select
        {
            get => ColorData.GetIndexingColor(IsEnabled ? StateSpectrum.Select : StateSpectrum.NotEnabled);
            set
            {
                ColorData.SetIndexingColor(StateSpectrum.Select, value);
            }
        }
        #endregion

        #region Used
        /// <summary>
        /// Цвет нажатого или использованого состояния
        /// </summary>
        public Color Used
        {
            get => ColorData.GetIndexingColor(IsEnabled ? (UsedState ? StateSpectrum.Default : StateSpectrum.Used) : StateSpectrum.NotEnabled);
            set
            {
                ColorData.SetIndexingColor(StateSpectrum.Used, value);
            }
        }
        #endregion

        public BrushSettingQ()
        {
            IsEnabled = true;
            Default = Colors.Black;
            Select = Colors.Black;
            Used = Colors.Black;
            NotEnabled = Colors.Black;
        }

        public BrushSettingQ(byte[,] ByteColorData)
        {
            IsEnabled = true;
            _UsedState = false;
            ColorData = new(ByteColorData);
        }

        internal BrushSettingQ(CreateStyle Style)
        {
            IsEnabled = true;
            _UsedState = false;
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
