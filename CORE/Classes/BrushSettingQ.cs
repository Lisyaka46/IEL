using IEL.CORE.Enums;
using System.Buffers;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static IEL.CORE.Classes.QData;

namespace IEL.CORE.Classes
{
    /// <summary>
    /// Класс настройки поведения цвета при разных состояниях объекта
    /// </summary>
    public sealed class BrushSettingQ
    {
        /// <summary>
        /// Дата которая используется для отображения
        /// </summary>
        public QData Source { get; private set; }

        #region ConectedBrush
        /// <summary>
        /// Стек всех подключённых к настройки свойств цвета
        /// </summary>
        private Stack<SolidColorBrush> ConectedBrush = new();

        /// <summary>
        /// Подключить свойство цвета объекта к настройке Q-логики
        /// </summary>
        public SolidColorBrush InicializeConnectedSolidColorBrush()
        {
            ConectedBrush.Push(new SolidColorBrush(ActiveSpectrumColor));
            return ConectedBrush.Peek();
        }
        #endregion

        #region AnimationBrushSettingQ
        /// <summary>
        /// Объект анимации текущего свойства цвета объектов
        /// </summary>
        private ColorAnimation SourceAnimation = new()
        {
            EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseOut, Exponent = 1.4d },
            Duration = TimeSpan.FromMilliseconds(200d),
        };

        /// <summary>
        /// Время управляемое анимацией для свойств цвета
        /// </summary>
        public TimeSpan DurationBrushSettingQ
        {
            get => SourceAnimation.Duration.TimeSpan;
            set => SourceAnimation.Duration = value;
        }

        /// <summary>
        /// Объект представляющий анимацию текущего свойства цвета объектов
        /// </summary>
        public IEasingFunction EasingFunctionBrushSettingQ
        {
            get => SourceAnimation.EasingFunction;
            set => SourceAnimation.EasingFunction = value;
        }

        /// <summary>
        /// Анимировать все подключённые свойства цвета к настройке Q-логики
        /// </summary>
        /// <param name="AnimatedEvent">Ожидается ли анирование</param>
        private void AnimateConectedBrush(bool AnimatedEvent)
        {
            ColorAnimation? animation = AnimatedEvent ? SourceAnimation : null;
            if (animation != null) animation.To = ActiveSpectrumColor;
            foreach (SolidColorBrush Element in ConectedBrush.AsEnumerable())
            {
                Element.BeginAnimation(SolidColorBrush.ColorProperty, animation, HandoffBehavior.SnapshotAndReplace);
                if (!AnimatedEvent || animation == null) Element.Color = ActiveSpectrumColor;
            }
        }
        #endregion

        /// <summary>
        /// Собственный цвет выделения свойств цвета
        /// </summary>
        private Color Custom = Colors.Black;

        #region UsedState
        /// <summary>
        /// Состояние навигации использования
        /// </summary>
        /// <remarks>
        /// При включённом состоянии цвет обычного состояния становится использованным, а использованный обычным
        /// <code></code>
        /// <b>Default <![CDATA[<]]>=<![CDATA[>]]> Used</b>
        /// </remarks>
        private bool UsedState = false;

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
                AnimateConectedBrush(true);
            }
        }
        #endregion

        /// <summary>
        /// Активный цвет по используемому спектру состояния цвета
        /// </summary>
        public Color ActiveSpectrumColor => ActiveSpectrum switch
        {
            StateSpectrum.Default => Source.GetFromSpectrumColor(EnumDataSpectrum.Default),
            StateSpectrum.Used => Source.GetFromSpectrumColor(EnumDataSpectrum.Used),
            StateSpectrum.Select => Source.GetFromSpectrumColor(EnumDataSpectrum.Select),
            StateSpectrum.NotEnabled => Source.GetFromSpectrumColor(EnumDataSpectrum.NotEnabled),
            StateSpectrum.Custom => Custom,
            _ => Source.GetFromSpectrumColor(EnumDataSpectrum.Default),
        };

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
            if (ActiveSpectrum == Value || Value == StateSpectrum.Custom) return;
            else if (Value == StateSpectrum.Default || Value == StateSpectrum.Used)
                ActiveSpectrum = UsedState ? (Value == StateSpectrum.Default ? StateSpectrum.Used : StateSpectrum.Default) : Value;
            else ActiveSpectrum = Value;
            AnimateConectedBrush(AnimatedEvent);
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
            Custom = Value;
            AnimateConectedBrush(true);
        }

        /// <summary>
        /// Узнать активный спектр цвета
        /// </summary>
        /// <returns>Активный спектр</returns>
        public StateSpectrum GetActiveSpecrum() => ActiveSpectrum;
        #endregion

        /// <summary>
        /// Установить новый экземпляр данных Q-логики
        /// </summary>
        /// <param name="Data">Спектр которому присваивается значение</param>
        internal void SetQData(QData Data)
        {
            Source = Data;
            Source.ChangedData += UpdateVisualActiveSpectrumData;
            AnimateConectedBrush(true);
        }

        private void UpdateVisualActiveSpectrumData(EnumDataSpectrum Spectrum) { if ((int)Spectrum == (int)ActiveSpectrum - 1) AnimateConectedBrush(false); }

        /// <summary>
        /// Инициализация объекта цветовых настроек по умолчанию
        /// </summary>
        public BrushSettingQ()
        {
            Source = new();
            Source.ChangedData += UpdateVisualActiveSpectrumData;
            ActiveSpectrum = StateSpectrum.Default;
        }

        /// <summary>
        /// Инициализировать используемый объект цветовой палитры<br/>
        /// с константным значением цвета
        /// </summary>
        /// <remarks>
        /// <b>
        /// | A  R  G  B |<br/>
        /// <br/>
        /// | 0  0  0  0 | - Default<br/>
        /// | 0  0  0  0 | - Select<br/>
        /// | 0  0  0  0 | - Used<br/>
        /// | 0  0  0  0 | - NotEnabled<br/>
        /// </b>
        /// </remarks>
        /// <param name="ByteColorData">Массив байтовых значений цвета</param>
        public BrushSettingQ(byte[][] ByteColorData)
        {
            Source = new(ByteColorData);
            Source.ChangedData += UpdateVisualActiveSpectrumData;
            ActiveSpectrum = StateSpectrum.Default;
        }

        /// <summary>
        /// Инициализировать используемый объект цветовой палитры<br/>
        /// с одним константным значением цвета
        /// </summary>
        /// <remarks>
        /// <b>
        /// | A  R  G  B |<br/>
        /// <br/>
        /// | 0  0  0  0 | - Default / Select / Used / NotEnabled<br/>
        /// </b>
        /// </remarks>
        /// <param name="ByteColorData">Массив байтовых значений цвета</param>
        public BrushSettingQ(byte[] ByteColorData)
        {
            Source = new(ByteColorData);
            Source.ChangedData += UpdateVisualActiveSpectrumData;
            ActiveSpectrum = StateSpectrum.Default;
        }
    }
}
