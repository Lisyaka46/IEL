using IEL.Classes;
using System.Windows.Controls;

namespace IEL.Interfaces.Core
{
    public interface IPageDefault : IPage
    {
        /// <summary>
        /// Объект страницы распологающий элементы
        /// </summary>
        public Page Content { get; }
    }
}
