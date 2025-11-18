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
            BorderButton.Background = SourceBackground.InicializeConnectedSolidColorBrush();
            BorderButtonKey.Background = SourceBackground.InicializeConnectedSolidColorBrush();
            #endregion

            #region BorderBrush
            BorderButton.BorderBrush = SourceBorderBrush.InicializeConnectedSolidColorBrush();
            BorderButtonKey.BorderBrush = SourceBorderBrush.InicializeConnectedSolidColorBrush();
            #endregion

            #region Foreground
            TextBlockKey.Foreground = SourceForeground.InicializeConnectedSolidColorBrush();
            #endregion

            ImageButton.Margin = new Thickness(10, 10, 10, 10);

            ImageMouseButtonsUse.Opacity = 0d;
        }
    }
}
