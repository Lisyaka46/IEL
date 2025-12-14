using IEL.UserElementsControl.Base;
using System.Windows.Media;

namespace IEL.UserElementsControl
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
            #endregion

            #region BorderBrush
            #endregion

            #region Foreground
            MainTextBlock.Foreground = SourceForeground.SourceBrush;
            #endregion
        }
    }
}
