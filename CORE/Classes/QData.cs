using IEL.CORE.Enums;
using System.Windows.Media;

namespace IEL.CORE.Classes
{
    /// <summary>
    /// Класс управляемых данных цветовой палитры отображения объекта
    /// </summary>
    public class QData : ICloneable
    {
        /// <summary>
        /// Массив данных цвета
        /// </summary>
        internal byte[,] Data { get; }

        /// <summary>
        /// Инициализировать данные по умолчанию
        /// </summary>
        public QData()
        {
            Data = new byte[4, 4]
            {
                { 255, 0, 0, 0 },
                { 255, 0, 0, 0 },
                { 255, 0, 0, 0 },
                { 255, 0, 0, 0 },
            };
        }

        /// <summary>
        /// Инициализировать управляемый объект данных цветовой палитры отображения объекта<br/>
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
        public QData(byte[,] ByteColorData)
        {
            if (ByteColorData.Length / 4 == 4) Data = ByteColorData;
            else throw new Exception("Не хватает данных, массив не имеет размеры 4/4");
            Data = ByteColorData;
        }

        /// <summary>
        /// Установить цвет по спектру элемента
        /// </summary>
        /// <param name="Spectrum">Спектр</param>
        /// <param name="DataColor">Цвет</param>
        internal void SetIndexingColor(StateSpectrum Spectrum, Color DataColor)
        {
            Data[(int)Spectrum, 0] = DataColor.A;
            Data[(int)Spectrum, 1] = DataColor.R;
            Data[(int)Spectrum, 2] = DataColor.G;
            Data[(int)Spectrum, 3] = DataColor.B;
        }

        /// <summary>
        /// Установить цвет по спектру элемента
        /// </summary>
        /// <param name="Spectrum">Спектр</param>
        public Color GetIndexingColor(StateSpectrum Spectrum) =>
            Color.FromArgb(Data[(int)Spectrum, 0], Data[(int)Spectrum, 1], Data[(int)Spectrum, 2], Data[(int)Spectrum, 3]);

        /// <summary>
        /// Скопировать данные расположения цветов
        /// </summary>
        /// <returns>Склонированный элемент</returns>
        public object Clone()
        {
            return new QData(Data);
        }
    }
}
