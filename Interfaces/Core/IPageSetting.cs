using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEL.Interfaces.Core
{
    public interface IPageSetting<T> : IPageDefault where T : Enum
    {
        /// <summary>
        /// Делегат события изменения настроек
        /// </summary>
        /// <param name="Name">Имя парметра</param>
        /// <param name="NewValue">Новое значение параметрв</param>
        public delegate void ChangeValue(T Name, string NewValue);
    }
}
