using IEL.Interfaces.Front;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IEL.Interfaces.Front
{
    public interface IIELFrame : IIELObject
    {
        /// <summary>
        /// Ориентация переключения страниц
        /// </summary>
        public enum OrientationMove
        {
            Left = 0,
            Right = 1,
        }

        /// <summary>
        /// Индекс смены окна страницы
        /// </summary>
        protected int PanelVerschachtelung { get; }

        /// <summary>
        /// Объект актуального окна страницы
        /// </summary>
        internal Frame ActualFrame { get; }

        /// <summary>
        /// Объект предыдущего окна страницы
        /// </summary>
        internal Frame BackFrame { get; }

        /// <summary>
        /// Делегат главного события элемента фрейма
        /// </summary>
        public delegate void IELFrameEventHandler();

        /// <summary>
        /// Делегат события закрытия элемента фрейма
        /// </summary>
        public delegate void IELFrameChangedEventHandler(string NamePage);

        /// <summary>
        /// Событие закрытия фрейма
        /// </summary>
        public event IELFrameEventHandler? ClosingFrame;

        /// <summary>
        /// Событие изменения активной страницы
        /// </summary>
        public event IELFrameChangedEventHandler? ChangeElementPage;
    }
}
