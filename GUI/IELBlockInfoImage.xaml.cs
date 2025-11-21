using IEL.CORE.BaseUserControls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELBlockInfo.xaml
    /// </summary>
    public partial class IELBlockInfoImage : IELContainerBase
    {
        #region Source
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(IELBlockInfoImage),
                new(null,
                    (sender, e) =>
                    {
                        ((IELBlockInfoImage)sender).ImageBlock.Source = (ImageSource)e.NewValue;
                    }));

        /// <summary>
        /// Картинка отображаемая в элементе
        /// </summary>
        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        #endregion

        /// <summary>
        /// Инициализировать объект интерфейса отображения информации через изображение
        /// </summary>
        public IELBlockInfoImage()
        {
            InitializeComponent();
        }
    }
}
