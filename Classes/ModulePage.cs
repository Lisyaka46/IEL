using IEL.Interfaces.Core;
using static IEL.Interfaces.Core.IModulePage;

namespace IEL.Classes
{
    public class ModulePage(string Name) : IModulePage
    {
        /// <summary>
        /// Имя страницы
        /// </summary>
        public string ModuleName { get; set; } = Name;

        /// <summary>
        /// Событие открытия данного модуля страницы
        /// </summary>
        internal event OpenHandler? OpenModulePage;
    }
}
