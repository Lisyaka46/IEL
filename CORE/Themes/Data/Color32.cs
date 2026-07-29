using LibraryIEL.CORE.Themes.Palettes;
using WnColor = System.Windows.Media.Color;

namespace LibraryIEL.CORE.Themes.Data
{
    /// <summary>
    /// Структура данных цвета
    /// </summary>
    public readonly struct Color32
    {
        /// <summary>
        /// Данные цвета
        /// </summary>
        private readonly byte[] Data;

        /// <summary>
        /// Значение прозрачности
        /// </summary>
        public readonly byte Alpha => Data[0];

        /// <summary>
        /// Значение крассного цвета
        /// </summary>
        public readonly byte Red => Data[1];

        /// <summary>
        /// Значение зелёного цвета
        /// </summary>
        public readonly byte Green => Data[2];

        /// <summary>
        /// Значение синего цвета
        /// </summary>
        public readonly byte Blue => Data[3];

        /// <summary>
        /// Количество байт, которое хранится для цвета
        /// </summary>
        public const byte CountBytes = 4;

        /// <summary>
        /// Инициализировать свой цвет
        /// </summary>
        /// <param name="SourceAlpha">Значение прозрачности</param>
        /// <param name="SourceRed">Значение красного</param>
        /// <param name="SourceGreen">Значение зелёного</param>
        /// <param name="SourceBlue">Значение синего</param>
        public Color32(byte SourceAlpha, byte SourceRed, byte SourceGreen, byte SourceBlue)
        {
            Data = [SourceAlpha, SourceRed, SourceGreen, SourceBlue];
        }

        /// <summary>
        /// Инициализировать свой цвет по данным байтов
        /// </summary>
        public Color32(byte[] SourceData) : this(SourceData.AsSpan()) { }

        /// <summary>
        /// Инициализировать свой цвет по данным байтов
        /// </summary>
        public Color32(Span<byte> SourceData)
        {
            if (SourceData.Length != CountBytes)
                throw Palette.ExceptionArrayLength((byte)SourceData.Length, CountBytes, nameof(SourceData));
            Data = SourceData.ToArray();
        }

        /// <summary>
        /// Преобразование в структуру цвета
        /// </summary>
        /// <param name="color">Оригинальный цвет другого типа</param>
        public static implicit operator Color32(WnColor color)
            => new(color.A, color.R, color.G, color.B);

        /// <summary>
        /// Преобразование в структуру цвета
        /// </summary>
        /// <param name="color">Оригинальный цвет другого типа</param>
        public static implicit operator WnColor(Color32 color)
            => WnColor.FromArgb(color.Alpha, color.Red, color.Green, color.Blue);

        /// <summary>
        /// Преобразование в массив байтовых данных
        /// </summary>
        /// <param name="SourceData">Обёртка оригинальных данных</param>
        public static implicit operator byte[](Color32 SourceData) => [.. SourceData.Data];
    }
}
