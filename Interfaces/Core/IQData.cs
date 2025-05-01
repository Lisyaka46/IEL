using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IEL.Interfaces.Core
{
    /// <summary>
    /// Интерфейс данных цвета Q-логики
    /// </summary>
    public interface IQData
    {
        /// <summary>
        /// Перечисление спектров цвета
        /// </summary>
        public enum StateSpectrum
        {
            /// <summary>
            /// Спектр обычного состояния
            /// </summary>
            Default = 0,

            /// <summary>
            /// Спектр выделенного состояния
            /// </summary>
            Select = 1,

            /// <summary>
            /// Спектр использованного цвета
            /// </summary>
            Used = 2,

            /// <summary>
            /// Спектр отключённого цвета
            /// </summary>
            NotEnabled = 3,
        }

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
