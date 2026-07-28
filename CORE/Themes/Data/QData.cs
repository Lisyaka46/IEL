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
        public readonly byte[] GetSourceBytes()
        {
            byte[] result = new byte[CountBytes];
            byte Offset = 0;
            Buffer.BlockCopy(Default.GetSourceBytes(), 0, result, Offset, Color32.CountBytes);
            Offset += Color32.CountBytes;
            Buffer.BlockCopy(Select.GetSourceBytes(), 0, result, Offset, Color32.CountBytes);
            Offset += Color32.CountBytes;
            Buffer.BlockCopy(Used.GetSourceBytes(), 0, result, Offset, Color32.CountBytes);
            Offset += Color32.CountBytes;
            Buffer.BlockCopy(NotEnabled.GetSourceBytes(), 0, result, Offset, Color32.CountBytes);
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
    }
}
