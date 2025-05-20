using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using IEL.CORE.Enums;

namespace IEL.Interfaces.Core
{
    /// <summary>
    /// Интерфейс данных цвета Q-логики
    /// </summary>
    public interface IQData
    {
        /// <summary>
        /// Массив данных цвета
        /// </summary>
        internal byte[,] Data { get; }

        /// <summary>
        /// Установить цвет по спектру элемента
        /// </summary>
        /// <param name="Spectrum">Спектр</param>
        /// <param name="DataColor">Цвет</param>
        public void SetIndexingColor(StateSpectrum Spectrum, Color DataColor);

        /// <summary>
        /// Установить цвет по спектру элемента
        /// </summary>
        /// <param name="Spectrum">Спектр</param>
        /// <param name="DataColor">Цвет</param>
        public Color GetIndexingColor(StateSpectrum Spectrum);
    }
}
