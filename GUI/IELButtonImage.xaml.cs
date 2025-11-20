using IEL.CORE.BaseUserControls;
using System.Windows;
using System.Windows.Media;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELButtonImage.xaml
    /// </summary>
    public partial class IELButtonImage : IELButtonBase
    {
        #region Source
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("BorderThicknessGuides", typeof(ImageSource), typeof(IELButtonImage),
                new(null,
                    (sender, e) =>
                    {
                        ((IELButtonImage)sender).ImageButton.Source = (ImageSource)e.NewValue;
                    }));

        /// <summary>
        /// Ссылка на элемент изображения
        /// </summary>
        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        #endregion

        /// <summary>
        /// Инициализировать объект кнопки с изображением
        /// </summary>
        public IELButtonImage()
        {
            InitializeComponent();
            #region Background
            #endregion

            #region BorderBrush
            #endregion

            #region Foreground
            #endregion
        }
    }
}
