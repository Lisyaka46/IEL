using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IEL.Interfaces.Core
{
    public interface IModulePage
    {
        /// <summary>
        /// Имя страницы
        /// </summary>
        public string ModuleName { get; }
    }
}
