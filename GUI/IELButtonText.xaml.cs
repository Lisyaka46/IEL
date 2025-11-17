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
            BorderRightArrow.Background = QBackground.InicializeConnectedSolidColorBrush();
            BorderLeftArrow.Background = QBackground.InicializeConnectedSolidColorBrush();
            #endregion

            #region BorderBrush
            BorderRightArrow.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            BorderLeftArrow.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            #endregion

            #region Foreground
            QForeground = new([255, 255, 255, 255]);
            TextBlockButton.Foreground = QForeground.InicializeConnectedSolidColorBrush();
            TextBlockLeftArrow.Foreground = QForeground.InicializeConnectedSolidColorBrush();
            TextBlockRightArrow.Foreground = QForeground.InicializeConnectedSolidColorBrush();
            #endregion

            VisualElementMouseEvents = ImageMouseButtonsUse;
            ImageMouseButtonsUse.Opacity = 0d;
            Text = "Text";
            CornerRadius = new CornerRadius(10);
        }
    }
}
