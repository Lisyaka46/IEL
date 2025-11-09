using IEL.CORE.Enums;
using System.Buffers;
using System.Windows.Media;

namespace IEL.CORE.Classes
{
    /// <summary>
    /// Класс настройки поведения цвета при разных состояниях объекта
    /// </summary>
    public class BrushSettingQ
    {
        /// <summary>
        /// Структура аргументов для управления событием изменения активного спекта увета
        /// </summary>
        /// <param name="IsSpectrum">Изменяемый спектр цвета</param>
        /// <param name="IsColor">Изменяемое значение цвета</param>
        /// <param name="IsAnimated">Состояние ожидания анимирования</param>
        public readonly struct SpectrumColorChangedEventArgs(StateSpectrum IsSpectrum, Color IsColor, bool IsAnimated)
        {
            /// <summary>
            /// Спектр состояния управляемого события
            /// </summary>
            public readonly StateSpectrum Spectrum = IsSpectrum;

            /// <summary>
            /// Значение цвета управляемого события
            /// </summary>
            public readonly Color Value = IsColor;

            /// <summary>
            /// Состояние ожидания анимирования управляемого события
            /// </summary>
            public readonly bool AnimatedEvent = IsAnimated;
        }

        /// <summary>
        /// Узнать активную структуру аргументов для управляемого события
        /// </summary>
        /// <param name="Animated">Состояние ожидания анимирования спектра</param>
        /// <returns>Структура аргументов</returns>
        private SpectrumColorChangedEventArgs ActiveArgs(bool Animated) => new(ActiveSpectrum, ActiveSpectrumColor, Animated);

        /// <summary>
        /// Делегат события изменения цвета определённого спектра элемента
        /// </summary>
        /// <param name="SpectrumColorEventArgs">Объект представляющий состояние выполнения события</param>
        public delegate void SpectrumColorChangedEventHandler(SpectrumColorChangedEventArgs SpectrumColorEventArgs);

        /// <summary>
        /// Событие изменения цвета определённого спектра
        /// </summary>
        private SpectrumColorChangedEventHandler? SpectrumColorChanged;

        /// <summary>
        /// Установить событие изменения спектра цвета
        /// </summary>
        /// <param name="Action">Событие</param>
        public void SetSpectrumAction(SpectrumColorChangedEventHandler Action) => SpectrumColorChanged = Action;

        /// <summary>
        /// Копировать событие изменения спектра цвета в новый объект
        /// </summary>
        /// <param name="NewObject">Новый объект, в который копируется действие</param>
        /// <param name="ActivateUpdate">Состояние активации принудительного обновления события по новым данным <b>(Будет анимироваться)</b></param>
        public void CloneSpectrumActionInObject(BrushSettingQ NewObject, bool ActivateUpdate)
        {
            NewObject.SpectrumColorChanged = (SpectrumColorChangedEventHandler?)SpectrumColorChanged?.Clone();
            if (ActivateUpdate) NewObject.SpectrumColorChanged?.Invoke(new(NewObject.ActiveSpectrum, NewObject.ActiveSpectrumColor, true));
        }

        private QData _ColorData;
        /// <summary>
        /// Массив данных цвета
        /// </summary>
        public QData ColorData
        {
            get => _ColorData;
            set
            {
                _ColorData = value;
                SpectrumColorChanged?.Invoke(ActiveArgs(true));
            }
        }

        #region Default
        /// <summary>
        /// Цвет отключённого состояния состояния
        /// </summary>
        public Color Default
        {
            get => Color.FromArgb(ColorData.Data[0, 0], ColorData.Data[0, 1], ColorData.Data[0, 2], ColorData.Data[0, 3]);
            set
            {
                ColorData.SetIndexingColor(0, value);
                if (ActiveSpectrum == StateSpectrum.Default) SpectrumColorChanged?.Invoke(new(StateSpectrum.Default, value, false));
            }
        }
        #endregion

        #region NotEnabled
        /// <summary>
        /// Цвет отключённого состояния состояния
        /// </summary>
        public Color NotEnabled
        {
            get => Color.FromArgb(ColorData.Data[3, 0], ColorData.Data[3, 1], ColorData.Data[3, 2], ColorData.Data[3, 3]);
            set
            {
                ColorData.SetIndexingColor(3, value);
                if (ActiveSpectrum == StateSpectrum.NotEnabled) SpectrumColorChanged?.Invoke(new(StateSpectrum.NotEnabled, value, false));
            }
        }
        #endregion

        #region Select
        /// <summary>
        /// Цвет выделенного состояния
        /// </summary>
        public Color Select
        {
            get => Color.FromArgb(ColorData.Data[1, 0], ColorData.Data[1, 1], ColorData.Data[1, 2], ColorData.Data[1, 3]);
            set
            {
                ColorData.SetIndexingColor(1, value);
                if (ActiveSpectrum == StateSpectrum.Select) SpectrumColorChanged?.Invoke(new(StateSpectrum.Select, value, false));
            }
        }

        #endregion

        #region Used
        /// <summary>
        /// Цвет нажатого или использованого состояния
        /// </summary>
        public Color Used
        {
            get => Color.FromArgb(ColorData.Data[2, 0], ColorData.Data[2, 1], ColorData.Data[2, 2], ColorData.Data[2, 3]);
            set
            {
                ColorData.SetIndexingColor(2, value);
                if (ActiveSpectrum == StateSpectrum.Used) SpectrumColorChanged?.Invoke(new(StateSpectrum.Used, value, false));
            }
        }
        #endregion

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
        /// <returns>Текущее состояние использования</returns>
        public bool GetUsedState() => UsedState;

        /// <summary>
        /// Установить новое значение использованию цвета
        /// </summary>
        /// <param name="Value">Новое значение</param>
        public void SetUsedState(bool Value)
        {
            UsedState = Value;
            if (ActiveSpectrum == StateSpectrum.Default || ActiveSpectrum == StateSpectrum.Used)
            {
                ActiveSpectrum = Value ? (ActiveSpectrum == StateSpectrum.Default ? StateSpectrum.Used : StateSpectrum.Default) : StateSpectrum.Default;
                SpectrumColorChanged?.Invoke(ActiveArgs(true));
            }
        }
        #endregion

        /// <summary>
        /// Активный цвет по используемому спектру состояния цвета
        /// </summary>
        public Color ActiveSpectrumColor =>
            Color.FromArgb(ColorData.Data[(int)ActiveSpectrum, 0], ColorData.Data[(int)ActiveSpectrum, 1],
                ColorData.Data[(int)ActiveSpectrum, 2], ColorData.Data[(int)ActiveSpectrum, 3]);

        #region ActiveSpectrum
        /// <summary>
        /// Активный спектр состояния цвета
        /// </summary>
        private StateSpectrum ActiveSpectrum;

        /// <summary>
        /// Установить значение активному спектру цвета
        /// </summary>
        /// <param name="Value">Устанавливаемое значение спектру</param>
        /// <param name="AnimatedEvent">Состояние отвечающее за анимацию установки</param>
        public void SetActiveSpecrum(StateSpectrum Value, bool AnimatedEvent)
        {
            if (ActiveSpectrum == Value) return;
            if (Value == StateSpectrum.Default || Value == StateSpectrum.Used)
                ActiveSpectrum = UsedState ? (Value == StateSpectrum.Default ? StateSpectrum.Used : StateSpectrum.Default) : Value;
            else ActiveSpectrum = Value;
            SpectrumColorChanged?.Invoke(ActiveArgs(AnimatedEvent));
        }

        /// <summary>
        /// Установить значение активному спектру цвета <b>(Всегда анимируется)</b>
        /// </summary>
        /// <remarks>
        /// После вызова этой функции будет установлено значение спекта <b><c>Custom</c></b>
        /// </remarks>
        /// <param name="Value">Устанавливаемое значение спектру</param>
        public void SetActiveSpecrum(Color Value)
        {
            ActiveSpectrum = StateSpectrum.Custom;
            SpectrumColorChanged?.Invoke(new(ActiveSpectrum, Value, true));
        }

        /// <summary>
        /// Узнать активный спектр цвета
        /// </summary>
        /// <returns>Активный спектр</returns>
        public StateSpectrum GetActiveSpecrum() => ActiveSpectrum;
        #endregion

        /// <summary>
        /// Инициализация объекта цветовых настроек по умолчанию
        /// </summary>
        public BrushSettingQ()
        {
            _ColorData = new();
            UsedState = false;
            ActiveSpectrum = StateSpectrum.Default;
        }

        /// <summary>
        /// Default -> Select -> Used -> NotEnabled
        /// </summary>
        /// <param name="ByteColorData"></param>
        public BrushSettingQ(byte[,] ByteColorData)
        {
            _ColorData = new(ByteColorData);
            UsedState = false;
            ActiveSpectrum = StateSpectrum.Default;
        }

        /// <summary>
        /// Source QData
        /// </summary>
        /// <param name="ByteColorData"></param>
        public BrushSettingQ(QData ByteColorData)
        {
            _ColorData = (QData)ByteColorData.Clone();
            UsedState = false;
            ActiveSpectrum = StateSpectrum.Default;
        }
    }
}
