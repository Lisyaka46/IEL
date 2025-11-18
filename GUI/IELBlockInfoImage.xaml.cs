using IEL.CORE.BaseUserControls;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELBlockInfo.xaml
    /// </summary>
    public partial class IELBlockInfoImage : IELContainerBase
    {
        /// <summary>
        /// Инициализировать объект интерфейса отображения информации через изображение
        /// </summary>
        public IELBlockInfoImage()
        {
            InitializeComponent();
            #region Background
            RectangleBackground.Fill = SourceBackground.InicializeConnectedSolidColorBrush();
            #endregion

            #region BorderBrush
            MainBorder.BorderBrush = SourceBorderBrush.InicializeConnectedSolidColorBrush();
            #endregion

            #region Foreground
            #endregion
        }
    }
}
