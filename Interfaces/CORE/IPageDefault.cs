using IEL.Interfaces.Core;

namespace IEL.Classes
{
    public interface IPageDefault : IPage
    {
        /// <summary>
        /// Модуль страницы
        /// </summary>
        public ModulePage ModulePage { get; }
    }
}
