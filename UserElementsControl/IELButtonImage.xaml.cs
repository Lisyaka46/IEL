using LibraryIEL.UserElementsControl.Base;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LibraryIEL.UserElementsControl
{
    /// <summary>
    /// Логика взаимодействия для IELButtonImage.xaml
    /// </summary>
    public partial class IELButtonImage : IELButtonBase
    {
        #region Properties

        #region Source
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(IELButtonImage),
                new(null,
                    (sender, e) =>
                    {
                        ((IELButtonImage)sender).ImageButton.Source = (ImageSource)e.NewValue;
                    }));

        /// <summary>
        /// Ссылка на элемент изображения
        /// </summary>
        [Description("Изображение, отображаемое внутри объекта кнопки")]
        [Category(Category_Main)]
        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        #endregion

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

        /// <summary>
        /// Изменить отображение изображения
        /// </summary>
        /// <param name="Source">Устанавливаемая картинка</param>
        /// <param name="animation">Анимация которая управляет изменением</param>
        public void ChangeImageIcon(in ImageSource Source, in DoubleAnimation? animation)
        {
            if (animation == null)
            {
                ImageButton.Source = Source;
                return;
            }
            ImageButton.Opacity = 0d;
            ImageButton.Source = Source;
            animation.Duration = TimeSpan.FromMilliseconds(1000d);
            animation.To = 1d;
            animation.EasingFunction = new CircleEase()
            {
                EasingMode = EasingMode.EaseOut,
            };
            ImageButton.BeginAnimation(OpacityProperty, animation);
        }
    }
}
