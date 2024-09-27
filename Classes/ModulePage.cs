using IEL.Interfaces.Core;

namespace IEL.Classes
{
    public class ModulePage(string Name) : IModulePage
    {
        /// <summary>
        /// Имя страницы
        /// </summary>
        public string ModuleName { get; set; } = Name;
    }
}
