using IEL.Classes;

namespace IEL.Interfaces.Core
{
    public interface IPageDefault : IPage, IModulePage
    {
        /// <summary>
        /// Модуль страницы
        /// </summary>
        public ModulePage ModulePage { get; }
    }
}
