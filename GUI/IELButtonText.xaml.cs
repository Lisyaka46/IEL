using IEL.CORE.BaseUserControls;

using System.Windows;
using System.Windows.Media;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELButtonText.xaml
    /// </summary>
    public partial class IELButtonText : IELButton
    {
        /// <summary>
        /// Текст кнопки
        /// </summary>
        public string Text
        {
            get => TextBlockButton.Text;
            set => TextBlockButton.Text = value;
        }

        /// <summary>
        /// Шрифт текста в кнопке
        /// </summary>
        public new FontFamily FontFamily
        {
            get => base.FontFamily;
            set
            {
                TextBlockButton.FontFamily = value;
                base.FontFamily = value;
            }
        }

        /// <summary>
        /// Инициализировать объект интерфейса кнопки с текстом
        /// </summary>
        public IELButtonText()
        {
            InitializeComponent();
            #region Background
            #endregion

            #region BorderBrush
            #endregion

            #region Foreground
            Foreground = new([255, 255, 255, 255]);
            TextBlockButton.Foreground = SourceForeground.InicializeConnectedSolidColorBrush();
            #endregion
            Text = "Text";
            CornerRadius = new CornerRadius(10);
        }
    }
}
