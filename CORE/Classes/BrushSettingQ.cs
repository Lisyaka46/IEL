using IEL.CORE.Enums;
using System.Buffers;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IEL.CORE.Classes
{
    /// <summary>
    /// Класс настройки поведения цвета при разных состояниях объекта
    /// </summary>
    public sealed class BrushSettingQ
    {
        #region ConectedBrush
        /// <summary>
        /// Стек всех подключённых к настройки свойств цвета
        /// </summary>
        private Stack<SolidColorBrush> ConectedBrush = new();

        /// <summary>
        /// Подключить свойство цвета объекта к настройке Q-логики
        /// </summary>
        /// <param name="SourceBrush">Подключаемое свойство цвета</param>
        public void ConnectSolidColorBrush(SolidColorBrush SourceBrush) => ConectedBrush.Push(SourceBrush);
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
                AnimateConectedBrush(true);
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
                if (ActiveSpectrum == StateSpectrum.Default) AnimateConectedBrush(false);
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
                if (ActiveSpectrum == StateSpectrum.NotEnabled) AnimateConectedBrush(false);
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
                if (ActiveSpectrum == StateSpectrum.Select) AnimateConectedBrush(false);
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
                if (ActiveSpectrum == StateSpectrum.Used) AnimateConectedBrush(false);
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
            StateSpectrum.Default => Default,
            StateSpectrum.Used => Used,
            StateSpectrum.Select => Select,
            StateSpectrum.NotEnabled => NotEnabled,
            StateSpectrum.Custom => Custom,
            _ => Default,
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
        /// Инициализация объекта цветовых настроек по умолчанию
        /// </summary>
        public BrushSettingQ()
        {
            _ColorData = new();
            ActiveSpectrum = StateSpectrum.Default;
        }

        /// <summary>
        /// Default -> Select -> Used -> NotEnabled
        /// </summary>
        /// <param name="ByteColorData"></param>
        public BrushSettingQ(byte[,] ByteColorData)
        {
            _ColorData = new(ByteColorData);
            ActiveSpectrum = StateSpectrum.Default;
        }

        /// <summary>
        /// Source QData
        /// </summary>
        /// <param name="ByteColorData"></param>
        public BrushSettingQ(QData ByteColorData)
        {
            _ColorData = (QData)ByteColorData.Clone();
            ActiveSpectrum = StateSpectrum.Default;
        }
    }
}
