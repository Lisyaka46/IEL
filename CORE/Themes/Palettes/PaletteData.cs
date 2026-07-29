using LibraryIEL.CORE.Themes.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls.Primitives;

namespace LibraryIEL.CORE.Themes.Palettes
{
    /// <summary>
    /// Структура данных о спектрах палитры
    /// </summary>
    public readonly ref struct PaletteData
    {
        /// <summary>
        /// Данные отображения фона
        /// </summary>
        public readonly QData BackGroundData;

        /// <summary>
        /// Данные отображения границ
        /// </summary>
        public readonly QData BorderGroundData;

        /// <summary>
        /// Данные отображения текста
        /// </summary>
        public readonly QData ForeGroundData;

        /// <summary>
        /// Количество видов данных
        /// </summary>
        public const byte CountSpectrum = 3;

        /// <summary>
        /// Количество байт хранимых для видов отображений
        /// </summary>
        public const byte CountBytes = QData.CountBytes * CountSpectrum;

        /// <summary>
        /// Инициализировать данные спектра палитры по массиву
        /// </summary>
        /// <param name="SourceData">Массив данных спектра палитры</param>
        public PaletteData(byte[] SourceData) : this(SourceData.AsSpan()) { }

        /// <summary>
        /// Инициализировать данные спектра палитры по массиву
        /// </summary>
        /// <param name="SourceData">Массив данных спектра палитры</param>
        public PaletteData(Span<byte> SourceData)
        {
            if (SourceData.Length != CountBytes) throw Palette.ExceptionArrayLength((byte)SourceData.Length, CountBytes, nameof(SourceData));
            byte Offset = 0;
            BackGroundData = new(SourceData[Offset..QData.CountBytes]);
            Offset += QData.CountBytes;
            BorderGroundData = new(SourceData[Offset..QData.CountBytes]);
            Offset += QData.CountBytes;
            ForeGroundData = new(SourceData[Offset..QData.CountBytes]);
        }

        /// <summary>
        /// Инициализировать данные спектра палитры по данным использования цвета
        /// </summary>
        /// <param name="SourceBGData">Структура использования цвета фона</param>
        /// <param name="SourceBBData">Структура использования цвета границ</param>
        /// <param name="SourceFGData">Структура использования цвета текста</param>
        public PaletteData(QData SourceBGData, QData SourceBBData, QData SourceFGData)
        {
            BackGroundData = SourceBGData;
            BorderGroundData = SourceBBData;
            ForeGroundData = SourceFGData;
        }

        /// <summary>
        /// Получить объект байтов текущего объекта <see cref="PaletteData"/>
        /// <br/>Массив представляет собой данные всех состояний цвета
        /// </summary>
        /// <returns>[<see cref="CountBytes"/> байт]</returns>
        public readonly byte[] GetSourceBytes()
        {
            byte[] result = new byte[CountBytes];
            byte Offset = 0;
            Buffer.BlockCopy(BackGroundData.GetSourceBytes(), 0, result, Offset, QData.CountBytes);
            Offset += Color32.CountBytes;
            Buffer.BlockCopy(BorderGroundData.GetSourceBytes(), 0, result, Offset, QData.CountBytes);
            Offset += Color32.CountBytes;
            Buffer.BlockCopy(ForeGroundData.GetSourceBytes(), 0, result, Offset, QData.CountBytes);
            return result;
        }
    }
}
