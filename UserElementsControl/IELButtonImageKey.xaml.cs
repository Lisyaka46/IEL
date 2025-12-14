using IEL.UserElementsControl.Base;
using System.Windows;
using System.Windows.Media;

namespace IEL.UserElementsControl
{
    /// <summary>
    /// Логика взаимодействия для IELButtonImageKey.xaml
    /// </summary>
    public partial class IELButtonImageKey : IELButtonKeyBase
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
        }
    }
}
