using IEL.CORE.BaseUserControls;
using IEL.CORE.Classes.Browser;
using IEL.CORE.Enums;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELInley.xaml
    /// </summary>
    public partial class IELInlay : IELButton
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

        private bool _UsedState;
        /// <summary>
        /// Состояние использования
        /// </summary>
        public bool UsedState
        {
            get => _UsedState;
            set
            {
                if (_UsedState == value) return;
                _UsedState = value;
                //BorderMain.BeginAnimation(BorderThicknessProperty,
                //    IELSettingObject.ObjectAnimateSetting.GetAnimationThickness(value ? BorderThicknessActive : BorderThicknessDiactive, TimeSpan.FromMilliseconds(800d)));
                SetActiveSpecrum(StateSpectrum.Default, true);
            }
        }

        /// <summary>
        /// Изображение кнопки закрытия вкладки
        /// </summary>
        public ImageSource SourceCloseButtonImage
        {
            get => ImageCloseInlay.Source;
            set => ImageCloseInlay.Source = value;
        }

        /// <summary>
        /// Константа оффсета изменения между состояниями барьера
        /// </summary>
        private const int OffsetBorder = 2;

        /// <summary>
        /// Активное значение барьера вкладки
        /// </summary>
        private Thickness BorderThicknessActive;

        /// <summary>
        /// Диактивированное значение барьера вкладки
        /// </summary>
        private Thickness BorderThicknessDiactive;

        /// <summary>
        /// Инициализировать объект интерфейса, вкладка браузера
        /// </summary>
        public IELInlay()
        {
            InitializeComponent();
            #region Background     
            #endregion

            #region BorderBrush
            #endregion

            #region Foreground
            TextBlockHead.Foreground = QForeground.InicializeConnectedSolidColorBrush();
            #endregion

            BorderThicknessActive = new(
                BorderThickness.Left + OffsetBorder, BorderThickness.Top + OffsetBorder,
                BorderThickness.Right + OffsetBorder, BorderThickness.Bottom + OffsetBorder);
            BorderThicknessDiactive = BorderThickness;
            _UsedState = false;
            ContentPage = null;
            // this

            ImageCloseInlay.MouseEnter += (sender, e) =>
            {
                if (sender.GetType() == typeof(Image))
                {
                    //DoubleAnimation animation = IELSettingObject.ObjectAnimateSetting.GetAnimationDouble(25d);
                    //ImageCloseInlay.BeginAnimation(WidthProperty, animation);
                    //ImageCloseInlay.BeginAnimation(HeightProperty, animation);
                }
            };

            ImageCloseInlay.MouseLeave += (sender, e) =>
            {
                //DoubleAnimation animation = IELSettingObject.ObjectAnimateSetting.GetAnimationDouble(20d);
                //ImageCloseInlay.BeginAnimation(WidthProperty, animation);
                //ImageCloseInlay.BeginAnimation(HeightProperty, animation);
            };

            ImageCloseInlay.MouseLeftButtonDown += (sender, e) =>
            {
                OnActivateCloseInlay?.Invoke(this, e);
            };
        }

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
