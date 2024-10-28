using System.Windows.Media;

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
        internal delegate void ColorDefaultChangeEventHandler(Color Value);

        /// <summary>
        /// Событие изменения цвета обычного состояния
        /// </summary>
        internal event ColorDefaultChangeEventHandler? ColorDefaultChange;

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
                if (IsEnabled) ColorDefaultChange?.Invoke(StateSpectrum.Default, value ? _Used : _Default);
            }
        }

        #region Default
        private Color _Default;
        /// <summary>
        /// Цвет отключённого состояния состояния
        /// </summary>
        public Color Default
        {
            get => IsEnabled ? (UsedState ? _Used : _Default) : _NotEnabled;
            set
            {
                _Default = value;
                if (IsEnabled) ColorDefaultChange?.Invoke(value);
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
            get => IsEnabled ? _Select : _NotEnabled;
            set
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
            get => IsEnabled ? (UsedState ? _Default : _Used) : _NotEnabled;
            set
            {
                _Used = value;
            }
        }
        #endregion

        public BrushSettingQ()
        {
            IsEnabled = true;
            _Default = Colors.Black;
            Select = Colors.Black;
            Used = Colors.Black;
            NotEnabled = Colors.Black;
        }

        internal BrushSettingQ(Color Default, Color Select, Color Used, Color NotEnabled)
        {
            IsEnabled = true;
            _UsedState = false;
            _Default = Default;
            this.Select = Select;
            this.Used = Used;
            this.NotEnabled = NotEnabled;
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
