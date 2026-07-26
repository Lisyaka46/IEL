using IEL.UserElementsControl.Base;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;

namespace IEL.CORE.Classes
{
    /// <summary>
    /// Объект спектра палитры
    /// </summary>
    public class PaletteSpectrum
    {
        /// <summary>
        /// Константа количества объектов данных для 1 элемента палитры
        /// </summary>
        public static int CountQDataSpectrum => 3;

        /// <summary>
        /// Данные отображения фона
        /// </summary>
        public QData BG { get; set; }

        /// <summary>
        /// Данные отображения границ
        /// </summary>
        public QData BB { get; set; }

        /// <summary>
        /// Данные отображения текста
        /// </summary>
        public QData FG { get; set; }

        /// <summary>
        /// Инициализировать пустой объект спектра темы
        /// </summary>
        public PaletteSpectrum()
        {
            BG = new();
            BB = new();
            FG = new();
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
        /// Соеденить объект IEL с спектром палитры
        /// </summary>
        /// <param name="IelObj">Объект который присоеденяется к палитре</param>
        [Obsolete("Используйте в объекте интерфейса свойство \"PaletteElement\"")]
        public void ConnectPalleteFromIELElement([DisallowNull] IELObjectBase IelObj) => IelObj.PaletteElement = this;

        /// <summary>
        /// Записать в поток данных файла данные QData
        /// </summary>
        /// <param name="Stream">Поток файла</param>
        /// <param name="Spectrum">"Элемент палитры, который записывается в файл</param>
        /// <returns></returns>
        /// <exception cref="Exception">Исключение несоответствия режима открытия файла</exception>
        public static void WritePalettespectrum(ref FileStream Stream, ref PaletteSpectrum Spectrum)
        {
            if (!Stream.CanWrite) throw new Exception("Поток работы с файлом не открыт для записи!");
            List<byte> BytesFromPaletteSpectrum = [];
            BytesFromPaletteSpectrum.AddRange(Spectrum.BG.GetSourceBytes());
            BytesFromPaletteSpectrum.AddRange(Spectrum.BB.GetSourceBytes());
            BytesFromPaletteSpectrum.AddRange(Spectrum.FG.GetSourceBytes());
            Stream.Write([.. BytesFromPaletteSpectrum], 0, BytesFromPaletteSpectrum.Count);
        }
    }
}
