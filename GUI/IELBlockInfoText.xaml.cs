using IEL.CORE.BaseUserControls;

using System.Windows;
using System.Windows.Media;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELBlockInfoText.xaml
    /// </summary>
    public partial class IELBlockInfoText : IELContainerBase
    {
        /// <summary>
        /// Текст объекта
        /// </summary>
        public string Text
        {
            get => MainTextBlock.Text;
            set => MainTextBlock.Text = value;
        }

        /// <summary>
        /// Шрифт текста в кнопке
        /// </summary>
        public new FontFamily FontFamily
        {
            get => base.FontFamily;
            set
            {
                MainTextBlock.FontFamily = value;
                base.FontFamily = value;
            }
        }

        /// <summary>
        /// Инициализировать объект интерфейса отображения информации через текст
        /// </summary>
        public IELBlockInfoText()
        {
            InitializeComponent();
            #region Background
            MainBorder.Background = SourceBackground.InicializeConnectedSolidColorBrush();
            #endregion

            #region BorderBrush
            MainBorder.BorderBrush = SourceBorderBrush.InicializeConnectedSolidColorBrush();
            #endregion

            #region Foreground
            MainTextBlock.Foreground = SourceBackground.InicializeConnectedSolidColorBrush();
            #endregion

            CornerRadius = new CornerRadius(10);
        }
    }
}
