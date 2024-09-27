using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IEL.Interfaces.Front
{
    public interface IIELButtonDefault : IIELButton
    {
        /// <summary>
        /// Объект события активации кнопки левым щелчком мыши
        /// </summary>
        public Activate? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации кнопки правым щелчком мыши
        /// </summary>
        public Activate? OnActivateMouseRight { get; set; }

        /// <summary>
        /// Делегат события активации
        /// </summary>
        public delegate void Activate();
    }
}
