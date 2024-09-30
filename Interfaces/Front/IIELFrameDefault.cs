using IEL.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEL.Interfaces.Front
{
    public interface IIELFrameDefault : IIELFrame
    {
        /// <summary>
        /// Перенаправить страницу
        /// </summary>
        /// <param name="Content">Новая страница</param>
        /// <param name="Orientation">Ориентация движения</param>
        public void NextPage([NotNull()] IPageDefault Content, OrientationMove Orientation = OrientationMove.Right);
    }
}
