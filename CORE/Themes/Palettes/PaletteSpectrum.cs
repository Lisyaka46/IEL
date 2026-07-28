using IEL.UserElementsControl.Base;
using LibraryIEL.CORE.Themes.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace LibraryIEL.CORE.Themes.Palettes
{
    /// <summary>
    /// Объект спектра палитры
    /// </summary>
    public class PaletteSpectrum
    {
        //
        private byte[] Data;

        /// <summary>
        /// Значение по умолчанию данных отображения фона объекта
        /// </summary>
        internal static QData DefaultBG = new(Colors.White, Colors.Gray, Colors.LightGray, Colors.DarkRed);

        /// <summary>
        /// Значение по умолчанию данных отображения границ объекта
        /// </summary>
        internal static QData DefaultBB = new(Colors.Black, Colors.DarkGray, Colors.Gray, Colors.Black);

        /// <summary>
        /// Значение по умолчанию данных отображения текста
        /// </summary>
        internal static QData DefaultFG = new(Colors.Black, Colors.Black, Colors.DarkCyan, Colors.Black);

        /// <summary>
        /// Данные отображения фона
        /// </summary>
        public QData BG { get; private set; }

        /// <summary>
        /// Данные отображения границ
        /// </summary>
        public QData BB { get; private set; }

        /// <summary>
        /// Данные отображения текста
        /// </summary>
        public QData FG { get; private set; }

        /// <summary>
        /// Инициализировать пустой объект спектра темы
        /// </summary>
        private PaletteSpectrum()
        {
            BG = DefaultBG;
            BB = DefaultBB;
            FG = DefaultFG;
        }

        /// <summary>
        /// Инициализировать объект спектра темы по дайтам данных спектра
        /// </summary>
        /// <remarks>
        /// Ожидаются <see cref="QData.CountBytesFromColor"/> * <see cref="QData.CountSpectrumColor"/> * <see cref="CountQDataSpectrum"/> элементов,<br/>
        /// которые будут отражать все <see cref="QData.CountSpectrumColor"/> спектра, для <see cref="CountQDataSpectrum"/> видов отображения
        /// </remarks>
        /// <param name="BytesData">Массив данных</param>
        public PaletteSpectrum(byte[] BytesData) // 48
        {
            int CountBytesOneQData = QData.CountBytesFromColor * QData.CountSpectrumColor;
            if (CountBytesOneQData * CountQDataSpectrum != BytesData.Length)
                throw new ArgumentException("Недопустимый размер данных для создания спектра " +
                    $"({BytesData.Length} => {CountBytesOneQData * CountQDataSpectrum})");
            byte[][] ChunkDataTheme = new byte[CountQDataSpectrum][];
            for (int i = 0; i < CountQDataSpectrum; i++)
            {
                ChunkDataTheme[i] = new byte[CountBytesOneQData];
                Array.Copy(BytesData, i * CountBytesOneQData, ChunkDataTheme[i], 0, CountBytesOneQData);
            }
            BG = new(ChunkDataTheme[0]);
            BB = new(ChunkDataTheme[1]);
            FG = new(ChunkDataTheme[2]);
        }

        /// <summary>
        /// Клонировать объект спектра палитры
        /// </summary>
        public PaletteSpectrum Clone()
        {
            PaletteSpectrum Result = new()
            {
                BG = BG,
                BB = BB,
                FG = FG,
            };
            return Result;
        }
    }
}
