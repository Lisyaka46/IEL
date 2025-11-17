using IEL.CORE.BaseUserControls;
using IEL.CORE.Classes.ObjectSettings;
using IEL.CORE.Enums;

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELButtonImageKey.xaml
    /// </summary>
    public partial class IELButtonImageKey : IELButtonKey
    {
        /// <summary>
        /// Изображение которое отображается в кнопке
        /// </summary>
        public ImageSource Imaging
        {
            get
            {
                return ImageButton.Source;
            }
            set
            {
                ImageButton.Source = value;
            }
        }

        /// <summary>
        /// Позиционирование картинки в кнопке
        /// </summary>
        public Thickness ImageMargin
        {
            get => ImageButton.Margin;
            set => ImageButton.Margin = value;
        }

        /// <summary>
        /// Инициализировать объект кнопки с изображением
        /// </summary>
        public IELButtonImageKey()
        {
            InitializeComponent();
            #region Background
            BorderButton.Background = QBackground.InicializeConnectedSolidColorBrush();
            BorderButtonKey.Background = QBackground.InicializeConnectedSolidColorBrush();
            BorderRightArrow.Background = QBackground.InicializeConnectedSolidColorBrush();
            BorderLeftArrow.Background = QBackground.InicializeConnectedSolidColorBrush();
            #endregion

            #region BorderBrush
            BorderButton.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            BorderButtonKey.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            BorderRightArrow.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            BorderLeftArrow.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            #endregion

            #region Foreground
            TextBlockKey.Foreground = QForeground.InicializeConnectedSolidColorBrush();
            TextBlockLeftArrow.Foreground = QForeground.InicializeConnectedSolidColorBrush();
            TextBlockRightArrow.Foreground = QForeground.InicializeConnectedSolidColorBrush();
            #endregion

            ImageButton.Margin = new Thickness(10, 10, 10, 10);

            ImageMouseButtonsUse.Opacity = 0d;
        }
    }
}
