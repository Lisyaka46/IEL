using IEL.CORE.Classes;
using IEL.CORE.Classes.Browser;
using IEL.UserElementsControl.Base;
using System.Windows.Media;

namespace IEL.UserElementsControl
{
    /// <summary>
    /// Логика взаимодействия для IELInley.xaml
    /// </summary>
    public partial class IELInlay : IELButtonBase
    {
        /// <summary>
        /// Объект события активации закрытия вкладки
        /// </summary>
        public ActivateHandler? OnActivateCloseInlay { get; set; }

        /// <summary>
        /// Страница заголовка
        /// </summary>
        public BrowserPage? PageElement { get; private set; }

        /// <summary>
        /// Объект страницы
        /// </summary>
        internal object? ContentPage { get; private set; }

        /// <summary>
        /// Объект настройки отображения фона
        /// </summary>
        public new QData Background
        {
            get => (QData)GetValue(BackgroundProperty);
            set
            {
                IELButtonCloseInlay.Background = value;
                SetValue(BackgroundProperty, value);
            }
        }

        /// <summary>
        /// Объект настройки отображения границ
        /// </summary>
        public new QData BorderBrush
        {
            get => (QData)GetValue(BorderBrushProperty);
            set
            {
                IELButtonCloseInlay.BorderBrush = value;
                SetValue(BorderBrushProperty, value);
            }
        }

        /// <summary>
        /// Объект настройки отображения текста
        /// </summary>
        public new QData Foreground
        {
            get => (QData)GetValue(ForegroundProperty);
            set
            {
                IELButtonCloseInlay.Foreground = value;
                SetValue(ForegroundProperty, value);
            }
        }

        /// <summary>
        /// Шрифт текста в кнопке
        /// </summary>
        public new FontFamily FontFamily
        {
            get => base.FontFamily;
            set
            {
                TextBlockHead.FontFamily = value;
                base.FontFamily = value;
            }
        }

        /// <summary>
        /// Текст заголовка
        /// </summary>
        public string Text
        {
            get => TextBlockHead.Text;
            set
            {

                TextBlockHead.Text = value;
                TextBlockHead.UpdateLayout();
                if (TextBlockHead.ActualWidth > MaxWidth) Width = MaxWidth;
            }
        }

        /// <summary>
        /// Инициализировать объект интерфейса, вкладка браузера
        /// </summary>
        public IELInlay()
        {
            InitializeComponent();
            TextBlockHead.Foreground = SourceForeground.SourceBrush;
            ContentPage = null;

            IELButtonCloseInlay.OnActivateMouseLeft += (sender, e) =>
            {
                OnActivateCloseInlay?.Invoke(this, e);
            };
        }

        /// <summary>
        /// Получить кнопку закрытия вкладки
        /// </summary>
        public IELButtonImage GetButtonCloseInlay() => IELButtonCloseInlay;

        /// <summary>
        /// Установить вкладке объект страницы
        /// </summary>
        /// <param name="page">Объект страницы</param>
        internal void SetPage<T>(T page) where T : BrowserPage
        {
            PageElement = page;
            ContentPage = PageElement?.PageContent;
        }
    }
}
