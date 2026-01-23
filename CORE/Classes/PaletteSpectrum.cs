using IEL.UserElementsControl.Base;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace IEL.CORE.Classes
{
    /// <summary>
    /// Объект элемента палитры
    /// </summary>
    public class PaletteSpectrum : ICloneable
    {
        /// <summary>
        /// Константа количества объектов данных для 1 элемента палитры
        /// </summary>
        public static int CountQDataSpectrum => 3;

        /// <summary>
        /// Данные отображения фона
        /// </summary>
        public QData BG { get; set; } = new();

        /// <summary>
        /// Данные отображения границ
        /// </summary>
        public QData BB { get; set; } = new();

        /// <summary>
        /// Данные отображения текста
        /// </summary>
        public QData FG { get; set; } = new();

        /// <summary>
        /// Соеденить объект IEL с спектром палитры
        /// </summary>
        /// <param name="IelObj">Объект который присоеденяется к палитре</param>
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

        /// <summary>
        /// Скопировать объект спектра палитры в новый объект
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            PaletteSpectrum Result = new();
            Result.BG.Data = BG.Data;
            Result.BB.Data = BB.Data;
            Result.FG.Data = FG.Data;
            return Result;
        }
    }
}
