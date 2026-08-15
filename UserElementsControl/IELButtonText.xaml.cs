using IEL.UserElementsControl.Base;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace IEL.UserElementsControl
{
    /// <summary>
    /// Логика взаимодействия для IELButtonText.xaml
    /// </summary>
    public partial class IELButtonText : IELButtonBase
    {
        #region Properties

        #region Text
        /// <summary>
        /// Данные свойства <see cref="Text"/>
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(IELButtonText),
                new("Text", TextHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="Text"/>
        /// </summary>
        private static void TextHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonText Source && e.NewValue is string SourceNewValue)
            {
                Source.TextBlockButton.Text = SourceNewValue;
            }
        }

        /// <summary>
        /// Отображаемый текст в кнопке
        /// </summary>
        [Description("Текст, который отображается внутри контейнера кнопки")]
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        #endregion

        #region FontFamily
        /// <summary>
        /// Данные свойства <see cref="FontFamily"/>
        /// </summary>
        public static readonly new DependencyProperty FontFamilyProperty =
            DependencyProperty.Register(nameof(FontFamily), typeof(FontFamily), typeof(IELButtonText),
                new(new FontFamily(), FontFamilyHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="FontFamily"/>
        /// </summary>
        private static void FontFamilyHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonText Source && e.NewValue is FontFamily SourceNewValue)
            {
                Source.TextBlockButton.FontFamily = SourceNewValue;
            }
        }

        /// <summary>
        /// Шрифт отображаемого текста в кнопке
        /// </summary>
        [Description("Использующийся шрифт для отображения текста в контейнере кнопки")]
        public new FontFamily FontFamily
        {
            get => (FontFamily)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }
        #endregion

        #endregion

        /// <summary>
        /// Инициализировать объект интерфейса кнопки с текстом
        /// </summary>
        public IELButtonText()
        {
            InitializeComponent();
            TextBlockButton.Foreground = SourceForeground.SourceBrush;
            TextBlockButton.FontFamily = FontFamily;
            TextBlockButton.Text = Text;
        }
    }
}
