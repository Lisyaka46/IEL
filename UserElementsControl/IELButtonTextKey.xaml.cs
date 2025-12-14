using IEL.UserElementsControl.Base;
using System.Windows;

namespace IEL.UserElementsControl
{
    /// <summary>
    /// Логика взаимодействия для IELButtonTextKey.xaml
    /// </summary>
    public partial class IELButtonTextKey : IELButtonKeyBase
    {
        #region Text
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(IELButtonTextKey),
                new("Text",
                    (sender, e) =>
                    {
                        ((IELButtonTextKey)sender).TextBlockButton.Text = (string)e.NewValue;
                    }));

        /// <summary>
        /// Текст кнопки
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        #endregion

        /// <summary>
        /// Инициализировать объект интерфейса кнопки с текстом поддерживающую возможность нажатия с помощью клавиши
        /// </summary>
        public IELButtonTextKey()
        {
            InitializeComponent();
            #region Background
            #endregion

            #region BorderBrush
            #endregion

            #region Foreground
            TextBlockButton.Foreground = SourceForeground.SourceBrush;
            #endregion

            Text = "Text";
            CornerRadius = new CornerRadius(10);
        }
    }
}
