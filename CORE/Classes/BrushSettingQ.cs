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
    public sealed class BrushSettingQ : QData
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
            StateSpectrum.Default => GetFromSpectrumColor(EnumDataSpectrum.Default),
            StateSpectrum.Used => GetFromSpectrumColor(EnumDataSpectrum.Used),
            StateSpectrum.Select => GetFromSpectrumColor(EnumDataSpectrum.Select),
            StateSpectrum.NotEnabled => GetFromSpectrumColor(EnumDataSpectrum.NotEnabled),
            StateSpectrum.Custom => Custom,
            _ => GetFromSpectrumColor(EnumDataSpectrum.Default),
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

        #region QDataManipulate

        /// <summary>
        /// Спектр обычного сотояния цвета
        /// </summary>
        public Color Default
        {
            get => GetFromSpectrumColor(EnumDataSpectrum.Default);
            set => StaticSetFromSpectrumColor(EnumDataSpectrum.Default, value);
        }

        /// <summary>
        /// Спектр выделенного состояния цвета
        /// </summary>
        public Color Select
        {
            get => GetFromSpectrumColor(EnumDataSpectrum.Select);
            set => StaticSetFromSpectrumColor(EnumDataSpectrum.Select, value);
        }

        /// <summary>
        /// Спектр используемого цвета
        /// </summary>
        public Color Used
        {
            get => GetFromSpectrumColor(EnumDataSpectrum.Used);
            set => StaticSetFromSpectrumColor(EnumDataSpectrum.Used, value);
        }

        /// <summary>
        /// Спектр отключённого цвета
        /// </summary>
        public Color NotEnabled
        {
            get => GetFromSpectrumColor(EnumDataSpectrum.NotEnabled);
            set => StaticSetFromSpectrumColor(EnumDataSpectrum.NotEnabled, value);
        }

        /// <summary>
        /// Установить значение по определённому спектру
        /// </summary>
        /// <param name="Spectrum">Спектр которому присваивается значение</param>
        /// <param name="A">Значение Alpha</param>
        /// <param name="R">Значение Red</param>
        /// <param name="G">Значение Green</param>
        /// <param name="B">Значение Blue</param>
        public new void SetFromSpectrumData(EnumDataSpectrum Spectrum, byte A, byte R, byte G, byte B)
        {
            Data[(int)Spectrum] = [A, R, G, B];
            if ((int)Spectrum == (int)ActiveSpectrum - 1) AnimateConectedBrush(true);
        }

        /// <summary>
        /// Установить цвет по определённому спектру
        /// </summary>
        /// <param name="Spectrum">Спектр которому присваивается значение</param>
        /// <param name="Value">Значение цвета</param>
        public new void SetFromSpectrumColor(EnumDataSpectrum Spectrum, Color Value)
        {
            Data[(int)Spectrum] = [Value.A, Value.R, Value.G, Value.B];
            if ((int)Spectrum == (int)ActiveSpectrum - 1) AnimateConectedBrush(true);
        }

        /// <summary>
        /// Установить цвет по определённому спектру <b>Без анимации</b>
        /// </summary>
        /// <param name="Spectrum">Спектр которому присваивается значение</param>
        /// <param name="Value">Значение цвета</param>
        private void StaticSetFromSpectrumColor(EnumDataSpectrum Spectrum, Color Value)
        {
            Data[(int)Spectrum] = [Value.A, Value.R, Value.G, Value.B];
            if ((int)Spectrum == (int)ActiveSpectrum - 1) AnimateConectedBrush(false);
        }

        /// <summary>
        /// Установить новый экземпляр данных Q-логики
        /// </summary>
        /// <param name="Source">Спектр которому присваивается значение</param>
        public void SetQData(QData Source)
        {
            Data = Source.Data;
            Source.ChangedData += Source_ChangedData;
            AnimateConectedBrush(true);
        }

        /// <summary>
        /// Функция обновление спектра данных если он равен активному
        /// </summary>
        /// <param name="Spectrum">Текущий изменённый спектр данных</param>
        private void Source_ChangedData(EnumDataSpectrum Spectrum) { if ((int)Spectrum == (int)ActiveSpectrum - 1) AnimateConectedBrush(false); }
        #endregion

        /// <summary>
        /// Инициализация объекта цветовых настроек по умолчанию
        /// </summary>
        public BrushSettingQ()
        {
            Data =
            [
                [255, 0, 0, 0],
                [255, 128, 128, 128],
                [255, 200, 200, 200],
                [255, 255, 70, 70],
            ];
            ChangedData += Source_ChangedData;
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
            if (ByteColorData.Length == 4) Data = ByteColorData;
            else throw new Exception("Не хватает данных, массив не имеет размеры 4/4");
            ChangedData += Source_ChangedData;
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
            if (ByteColorData.Length == 4)
            {
                Data =
                    [
                    ByteColorData,
                    ByteColorData,
                    ByteColorData,
                    ByteColorData,
                    ];
            }
            else throw new Exception("Не хватает данных, массив не имеет размера 4");
            ChangedData += Source_ChangedData;
            ActiveSpectrum = StateSpectrum.Default;
        }
    }
}
