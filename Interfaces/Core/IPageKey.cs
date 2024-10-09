using IEL.Classes;

namespace IEL.Interfaces.Core
{
    public interface IPageKey : IPage
    {
        /// <summary>
        /// Модуль страницы
        /// </summary>
        public ModulePageKey ModulePage { get; }
    }
}
