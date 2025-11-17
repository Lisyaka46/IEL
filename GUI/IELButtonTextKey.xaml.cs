using IEL.CORE.BaseUserControls;
using IEL.CORE.Classes.ObjectSettings;
using IEL.CORE.Enums;

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELButtonTextKey.xaml
    /// </summary>
    public partial class IELButtonTextKey : IELButtonKey
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
                TextBlockCharKey.FontFamily = value;
                TextBlockButton.FontFamily = value;
                base.FontFamily = value;
            }
        }

        /// <summary>
        /// Инициализировать объект интерфейса кнопки с текстом поддерживающую возможность нажатия с помощью клавиши
        /// </summary>
        public IELButtonTextKey()
        {
            InitializeComponent();
            #region Background
            BorderButton.Background = QBackground.InicializeConnectedSolidColorBrush();
            BorderCharKeyboard.Background = QBackground.InicializeConnectedSolidColorBrush();
            BorderLeftArrow.Background = QBackground.InicializeConnectedSolidColorBrush();
            BorderRightArrow.Background = QBackground.InicializeConnectedSolidColorBrush();
            #endregion

            #region BorderBrush
            BorderButton.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            BorderCharKeyboard.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            BorderLeftArrow.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            BorderRightArrow.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            #endregion

            #region Foreground
            QForeground = new([255, 255, 255, 255]);
            TextBlockButton.Foreground = QForeground.InicializeConnectedSolidColorBrush();
            TextBlockCharKey.Foreground = QForeground.InicializeConnectedSolidColorBrush();
            TextBlockLeftArrow.Foreground = QForeground.InicializeConnectedSolidColorBrush();
            TextBlockRightArrow.Foreground = QForeground.InicializeConnectedSolidColorBrush();
            #endregion

            BorderButton.Margin = new(-24, 0, 0, 0);
            BorderCharKeyboard.Opacity = 0d;
            ImageMouseButtonsUse.Opacity = 0d;
            Text = "Text";
            CornerRadius = new CornerRadius(10);
        }
    }
}
