using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IEL.Interfaces.Core
{
    public interface IBrowserPage : IDisposable
    {
        public delegate void BrowserEvent(IBrowserPage browser_page);

        public abstract BrowserEvent? EventUnfocusPage { get; }

        public abstract BrowserEvent? EventFocusPage { get; }
    }
}
