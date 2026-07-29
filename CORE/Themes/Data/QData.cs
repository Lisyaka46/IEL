using LibraryIEL.CORE.Themes.Palettes;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media;
using WnColor = System.Windows.Media.Color;

namespace LibraryIEL.CORE.Themes.Data
{
    /// <summary>
    /// Класс управляемых данных цветовой палитры отображения объекта
    /// </summary>
    public ref struct QData
    {
        /// <summary>
        /// Неизвестный спектр палитры для отображения фона
        /// </summary>
        public static QData UnknownSpectrumBackGround =>
            new(Colors.White, Colors.LightGray, Colors.LightSkyBlue, Colors.OrangeRed);

        /// <summary>
        /// Неизвестный спектр палитры для отображения границ
        /// </summary>
        public static QData UnknownSpectrumBorderGround =>
            new(Colors.Black, Colors.DarkGray, Colors.Gray, Colors.DarkRed);

        /// <summary>
        /// Неизвестный спектр палитры для отображения текста
        /// </summary>
        public static QData UnknownSpectrumForeGround =>
            new(Colors.Black, Colors.Black, Colors.DarkGray, Colors.Black);

        /// <summary>
        /// Цвет покоя
        /// </summary>
        public Color32 Default;

        /// <summary>
        /// Цвет выделенный
        /// </summary>
        public Color32 Select;

        /// <summary>
        /// Цвет нажатый
        /// </summary>
        public Color32 Used;

        /// <summary>
        /// Цвет отключённый
        /// </summary>
        public Color32 NotEnabled;

        /// <summary>
        /// Количество состояний цветов
        /// </summary>
        public const byte CountSpectrumColor = 4;

        /// <summary>
        /// Количество байт, которое хранится для состояний цветов
        /// </summary>
        public const byte CountBytes = Color32.CountBytes * CountSpectrumColor;

        /// <summary>
        /// Получить объект байтов текущего объекта <see cref="QData"/>
        /// <br/>Массив представляет собой данные всех состояний цвета
        /// </summary>
        /// <returns>[<see cref="CountBytes"/> байт]</returns>
        private readonly byte[] GetSourceBytes()
        {
            byte[] result = new byte[CountBytes];
            byte Offset = 0;
            Buffer.BlockCopy(Default, 0, result, Offset, Color32.CountBytes);
            Offset += Color32.CountBytes;
            Buffer.BlockCopy(Select, 0, result, Offset, Color32.CountBytes);
            Offset += Color32.CountBytes;
            Buffer.BlockCopy(Used, 0, result, Offset, Color32.CountBytes);
            Offset += Color32.CountBytes;
            Buffer.BlockCopy(NotEnabled, 0, result, Offset, Color32.CountBytes);
            return result;
        }

        /// <summary>
        /// Инициализировать управляемый объект данных цветовой палитры отображения объекта<br/>
        /// с константным значением цвета
        /// </summary>
        /// <remarks>
        /// Используется 4 байта для цвета
        /// <b> | A  R  G  B | </b>
        /// </remarks>
        /// <param name="SourceDefault">Цвет состояния покоя</param>
        /// <param name="SourceSelect">Цвет выделения элемента</param>
        /// <param name="SourceUsed">Цвет нажатого элемента</param>
        /// <param name="SourceNotEnabled">Цвет отключённого элемента</param>
        public QData(WnColor SourceDefault, WnColor SourceSelect, WnColor SourceUsed, WnColor SourceNotEnabled)
        {
            Default = SourceDefault;
            Select = SourceSelect;
            Used = SourceUsed;
            NotEnabled = SourceNotEnabled;
        }

        /// <summary>
        /// Инициализировать объект данных управления цвета по массиву данных
        /// </summary>
        /// <param name="SourceData">Массив данных</param>
        public QData(Span<byte> SourceData)
        {
            if (SourceData.Length != CountBytes)
                throw Palette.ExceptionArrayLength((byte)SourceData.Length, CountBytes, nameof(SourceData));
            byte Offset = 0;
            Default = new(SourceData.Slice(Offset, Color32.CountBytes));
            Offset += Color32.CountBytes;
            Select = new(SourceData.Slice(Offset, Color32.CountBytes));
            Offset += Color32.CountBytes;
            Used = new(SourceData.Slice(Offset, Color32.CountBytes));
            Offset += Color32.CountBytes;
            NotEnabled = new(SourceData.Slice(Offset, Color32.CountBytes));
        }

        /// <summary>
        /// Стравнить данную обёртку данных
        /// </summary>
        /// <param name="Source">Сравниваемый объект</param>
        public override readonly bool Equals(object? Source) => GetSourceBytes().Equals(Source);

        /// <summary>
        /// Получить ключ объекта данных
        /// </summary>
        public override readonly int GetHashCode() => GetSourceBytes().GetHashCode();

        /// <summary>
        /// Преобразование в массив байтовых данных
        /// </summary>
        /// <param name="SourceData">Обёртка оригинальных данных</param>
        public static implicit operator byte[](QData SourceData) => SourceData.GetSourceBytes();

        /// <summary>
        /// Сравнение две обёртки между собой
        /// </summary>
        /// <param name="A">Обёртка оригинальных данных</param>
        /// <param name="B">Обёртка сравниваемых данных</param>
        public static bool operator ==(QData A, QData B)
        {
            return A.GetSourceBytes().SequenceEqual(B);
        }

        /// <summary>
        /// Сравнение две обёртки между собой
        /// </summary>
        /// <param name="A">Обёртка оригинальных данных</param>
        /// <param name="B">Обёртка сравниваемых данных</param>
        public static bool operator !=(QData A, QData B)
        {
            return !(A == B);
        }
    }
}
