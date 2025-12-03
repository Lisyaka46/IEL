using IEL.CORE.BaseUserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEL.CORE.Classes
{
    /// <summary>
    /// Объект элемента палитры
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
        /// Инициализировать
        /// </summary>
        public PaletteSpectrum()
        {
            BG = new();
            BB = new();
            FG = new();
        }

        /// <summary>
        /// Соеденить объект IEL с спектром палитры
        /// </summary>
        /// <param name="IelObj">Объект который присоеденяется к палитре</param>
        public void ConnectPalleteFromIELElement([DisallowNull] IELObjectBase IelObj) => IelObj.PaletteElement = this;
    }
}
