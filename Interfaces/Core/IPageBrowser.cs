namespace IEL.Interfaces.Core
{
    public interface IBrowserPage : IDisposable
    {
        public delegate void BrowserEvent(IBrowserPage browser_page);

        public abstract BrowserEvent? EventUnfocusPage { get; }

        public abstract BrowserEvent? EventFocusPage { get; }
    }
}
