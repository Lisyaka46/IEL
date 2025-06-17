using IEL.Interfaces.Core;
using System.Windows.Media;
using static IEL.Interfaces.Core.IQData;
using IEL.CORE.Enums;

namespace IEL.CORE.Classes
{
    /// <summary>
    /// Класс настройки поведения цвета при разных состояниях объекта
    /// </summary>
    public class BrushSettingQ : ICloneable
    {
        /// <summary>
        /// Делегат события изменения обычного цвета
        /// </summary>
        /// <param name="Value">Новое значение цвета</param>
        internal delegate void ColorChangedEventHandler(StateSpectrum Spectrum, Color Value);

        /// <summary>
        /// Событие изменения цвета обычного состояния
        /// </summary>
        internal event ColorChangedEventHandler? ColorChanged;

        /// <summary>
        /// Массив данных цвета
        /// </summary>
        internal QData ColorData { get; private set; }

        /// <summary>
        /// Изменить напрямую стиль отображения объекта
        /// </summary>
        /// <param name="Spectrum">Стиль придаваемый использоваемому объекту</param>
        public void InvokeObjectUsedStateColor(StateSpectrum Spectrum)
        {
            if (ColorChanged == null) return;
            Color Value = Spectrum switch
            {
                StateSpectrum.Default => Default,
                StateSpectrum.Select => Select,
                StateSpectrum.Used => Used,
                StateSpectrum.NotEnabled => NotEnabled,
                _ => Default,
            };
            ColorChanged?.Invoke(Spectrum, Value);
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

        #region UsedState
        /// <summary>
        /// Состояние навигации использования
        /// </summary>
        /// <remarks>
        /// При включённом состоянии цвет обычного состояния становится использованным, а использованный обычным
        /// <code></code>
        /// <b>Default <![CDATA[<]]>=<![CDATA[>]]> Used</b>
        /// </remarks>
        private bool UsedState;

        /// <summary>
        /// Узнать состояние использования
        /// </summary>
        /// <returns>Текущее состояние использование</returns>
        public bool GetUsedState() => UsedState;

        /// <summary>
        /// Установить новое значение использованию цвета
        /// </summary>
        /// <param name="NewValue">Новое значение</param>
        public void SetUsedState(bool NewValue)
        {
            UsedState = NewValue;
            if (IsEnabled)
            {
                try
                {
                    ColorChanged?.Invoke(StateSpectrum.Default, ColorData.GetIndexingColor(NewValue ? StateSpectrum.Used : StateSpectrum.Default));
                }
                catch { }
            }
        }
        #endregion

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
                if (IsEnabled) ColorChanged?.Invoke(StateSpectrum.Default, value);
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
                ColorChanged?.Invoke(StateSpectrum.NotEnabled, value);
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
            get => ColorData.GetIndexingColor(IsEnabled ? UsedState ? StateSpectrum.Default : StateSpectrum.Used : StateSpectrum.NotEnabled);
            set
            {
                ColorData.SetIndexingColor(StateSpectrum.Used, value);
            }
        }
        #endregion

        public BrushSettingQ()
        {
            IsEnabled = true;
            UsedState = false;
            ColorData = new();
        }

        /// <summary>
        /// Default -> Select -> Used -> NotEnabled
        /// </summary>
        /// <param name="ByteColorData"></param>
        public BrushSettingQ(byte[,] ByteColorData)
        {
            IsEnabled = true;
            UsedState = false;
            ColorData = new(ByteColorData);
        }

        /// <summary>
        /// Source QData
        /// </summary>
        /// <param name="ByteColorData"></param>
        public BrushSettingQ(QData ByteColorData)
        {
            IsEnabled = true;
            UsedState = false;
            ColorData = (QData)ByteColorData.Clone();
        }

        /// <summary>
        /// Клонировать объект политры
        /// </summary>
        /// <returns>Объект палитры</returns>
        public object Clone() => new BrushSettingQ(ColorData);
    }
}
