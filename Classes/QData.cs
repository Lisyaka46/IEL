using IEL.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static IEL.Interfaces.Core.IQData;

namespace IEL.Classes
{
    public class QData : IQData
    {
        /// <summary>
        /// Массив данных цвета
        /// </summary>
        public byte[,] Data { get; }

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
        public void SetIndexingColor(StateSpectrum Spectrum, Color DataColor)
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
        /// <param name="DataColor">Цвет</param>
        public Color GetIndexingColor(StateSpectrum Spectrum) =>
            Color.FromArgb(Data[(int)Spectrum, 0], Data[(int)Spectrum, 1], Data[(int)Spectrum, 2], Data[(int)Spectrum, 3]);
    }
}
