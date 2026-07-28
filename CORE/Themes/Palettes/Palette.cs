using LibraryIEL.CORE.Themes.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Animation;

namespace LibraryIEL.CORE.Themes.Palettes
{
    /// <summary>
    /// Главный класс управления данными обработки состояний цвета
    /// </summary>
    public static class Palette
    {
        //
        public static PaletteSpectrum UnknownSpectrum = new PaletteSpectrum();

        /// <summary>
        /// Объект анимации текущего свойства цвета объектов
        /// </summary>
        public static ColorAnimation SourceAnimation = new()
        {
            EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseOut, Exponent = 1.4d },
            Duration = TimeSpan.FromMilliseconds(200d),
        };

        /// <summary>
        /// Получить исключение параметра из-за несоответствия размера массива
        /// </summary>
        /// <param name="ExpectedLength">Ожидаемый результат</param>
        /// <param name="ObtainedLength">Полученный результат</param>
        /// <param name="NameArgument"></param>
        /// <returns></returns>
        internal static ArgumentException ExceptionArrayLength(byte ObtainedLength, byte ExpectedLength, string NameArgument) =>
            new($"Размер массива данных не совпадает с ожидаемым: \"Пол.{ObtainedLength} -> {ExpectedLength}\"", NameArgument);

        /// <summary>
        /// Получить объект данных спектра палитры
        /// </summary>
        /// <param name="BG">Данные для отображения фона</param>
        /// <param name="BB">Данные для отображения границ</param>
        /// <param name="FG">Данные для отображения текста</param>
        public static PaletteSpectrumData GetPaletteData(QData BG, QData BB, QData FG)
        {
            byte[] Data = new byte[PaletteSpectrumData.CountBytes];
            byte Offset = 0;
            Buffer.BlockCopy(BG.GetSourceBytes(), 0, Data, Offset, QData.CountBytes);
            Offset += QData.CountBytes;
            Buffer.BlockCopy(BB.GetSourceBytes(), 0, Data, Offset, QData.CountBytes);
            Offset += QData.CountBytes;
            Buffer.BlockCopy(FG.GetSourceBytes(), 0, Data, Offset, QData.CountBytes);
            return new(Data);
        }
    }
}
