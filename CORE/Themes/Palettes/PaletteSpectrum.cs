using LibraryIEL.CORE.Themes.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LibraryIEL.CORE.Themes.Palettes
{
    /// <summary>
    /// Объект спектра палитры
    /// </summary>
    public sealed class PaletteSpectrum
    {
        /// <summary>
        /// Данные для использования спектра
        /// </summary>
        private byte[] Data;

        /// <summary>
        /// Активный спектр состояния цвета
        /// </summary>
        public SpectrumColor ActiveSpectrum { get; private set; }

        /// <summary>
        /// Собственный цвет выделения свойств цвета
        /// </summary>
        private Color32 Custom = Colors.Black;

        /// <summary>
        /// Активный цвет по используемому спектру состояния цвета
        /// </summary>
        public Color32 GetActiveSpectrumColor()
        {
            if (ActiveSpectrum == SpectrumColor.Custom) return Custom;
            byte Offset = (byte)(Color32.CountBytes * (byte)(ActiveSpectrum - 1));
            return new Color32(Data[Offset..(Offset + Color32.CountBytes)]);
        }

        /// <summary>
        /// Объект отображения состояния спектра цвета
        /// </summary>
        public readonly SolidColorBrush SourceBrush;

        private bool _UsedState = false;
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
                if (_UsedState == value) return;
                _UsedState = value;
                if (ActiveSpectrum == SpectrumColor.Default || ActiveSpectrum == SpectrumColor.Used)
                {
                    ActiveSpectrum = value ? (ActiveSpectrum == SpectrumColor.Default ? SpectrumColor.Used : SpectrumColor.Default) :
                        SpectrumColor.Default;
                    AnimateConectedBrush(true);
                }
            }
        }

        /// <summary>
        /// Анимировать все подключённые свойства цвета к настройке Q-логики
        /// </summary>
        /// <param name="AnimatedEvent">Ожидается ли анирование</param>
        private void AnimateConectedBrush(bool AnimatedEvent)
        {
            if (AnimatedEvent)
            {
                Palette.SourceAnimation.To = GetActiveSpectrumColor();
                SourceBrush.BeginAnimation(SolidColorBrush.ColorProperty, Palette.SourceAnimation, HandoffBehavior.SnapshotAndReplace);
            }
            else SourceBrush.Color = GetActiveSpectrumColor();
        }

        /// <summary>
        /// Установить значение активному спектру цвета
        /// </summary>
        /// <remarks>
        /// Через эту функцию нельзя установить принудительно активным спектром, спектр <see cref="SpectrumColor.Custom"/>
        /// </remarks>
        /// <param name="Value">Устанавливаемое значение спектру</param>
        /// <param name="AnimatedEvent">Анимировать ли изменение</param>
        public void SetActiveSpecrum(SpectrumColor Value, bool AnimatedEvent)
        {
            if (ActiveSpectrum == Value || Value == SpectrumColor.Custom) return;
            else if (Value == SpectrumColor.Default || Value == SpectrumColor.Used)
                ActiveSpectrum = UsedState ? (Value == SpectrumColor.Default ? SpectrumColor.Used : SpectrumColor.Default) : Value;
            else ActiveSpectrum = Value;
            AnimateConectedBrush(AnimatedEvent);
        }

        /// <summary>
        /// Установить значение активному спектру цвета
        /// </summary>
        /// <remarks>
        /// После вызова этой функции будет установлено значение спекта <see cref="SpectrumColor.Custom"/>
        /// </remarks>
        /// <param name="Value">Устанавливаемое значение спектру</param>
        /// <param name="AnimatedEvent">Анимировать ли изменение</param>
        public void SetActiveSpecrum(Color Value, bool AnimatedEvent)
        {
            ActiveSpectrum = SpectrumColor.Custom;
            Custom = Value;
            AnimateConectedBrush(AnimatedEvent);
        }

        /// <summary>
        /// Изменить данные спектра палитры
        /// </summary>
        /// <param name="SourceData">Данные на которые меняется спектр</param>
        /// <param name="AnimatedEvent">Анимировать ли изменение</param>
        public void ChangeData(QData SourceData, bool AnimatedEvent)
        {
            Data = SourceData;
            AnimateConectedBrush(AnimatedEvent);
        }

        /// <summary>
        /// Получить структуру данных использования цветов
        /// </summary>
        public QData GetData() => new(Data);

        /// <summary>
        /// Инициализировать пустой объект спектра темы
        /// </summary>
        internal PaletteSpectrum(QData SourceData)
        {
            SourceBrush = new()
            {
                Color = SourceData.Default,
            };
            Data = SourceData;
        }
    }
}
