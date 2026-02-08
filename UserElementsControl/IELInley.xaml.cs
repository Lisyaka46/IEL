using IEL.CORE.Classes;
using IEL.CORE.Classes.Browser;
using IEL.UserElementsControl.Base;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace IEL.UserElementsControl
{
    /// <summary>
    /// Логика взаимодействия для IELInley.xaml
    /// </summary>
    public partial class IELInlay : IELObjectBase
    {
        /// <summary>
        /// Объект события активации закрытия вкладки
        /// </summary>
        public EventHandler? OnActivateCloseInlay { get; set; }

        /// <summary>
        /// Страница заголовка
        /// </summary>
        public BrowserPage? PageElement { get; private set; }

        /// <summary>
        /// Объект страницы
        /// </summary>
        internal object? ContentPage { get; private set; }

        #region CornerRadius
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(IELInlay),
                new(new CornerRadius(0),
                    (sender, e) =>
                    {
                        ((IELInlay)sender).BorderMain.CornerRadius = (CornerRadius)e.NewValue;
                    }));

        /// <summary>
        /// Скругление границ объекта
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        #endregion

        #region BorderThickness
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(IELInlay),
                new(new Thickness(2),
                    (sender, e) =>
                    {
                        ((IELInlay)sender).BorderMain.BorderThickness = (Thickness)e.NewValue;
                    }));

        /// <summary>
        /// Толщина границ объекта
        /// </summary>
        public new Thickness BorderThickness
        {
            get => (Thickness)BorderMain.GetValue(BorderThicknessProperty);
            set => BorderMain.SetValue(BorderThicknessProperty, value);
        }
        #endregion

        #region Padding
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty PaddingProperty =
            DependencyProperty.Register("Padding", typeof(Thickness), typeof(IELInlay),
                new(new Thickness(0),
                    (sender, e) =>
                    {
                        ((IELInlay)sender).BorderMain.Padding = (Thickness)e.NewValue;
                    }));

        /// <summary>
        /// Внутреннее смещение в объекте
        /// </summary>
        public new Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }
        #endregion

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
            ContentPage = null;
            BorderMain.Background = SourceBackground.SourceBrush;
            BorderMain.BorderBrush = SourceBorderBrush.SourceBrush;
            TextBlockHead.Foreground = SourceForeground.SourceBrush;

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
