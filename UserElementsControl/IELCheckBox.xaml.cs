using IEL.CORE.Animation;
using IEL.UserElementsControl.Base;
using IEL.CORE.Themes.Palettes;
using System.Windows;
using System.Windows.Media;

namespace IEL.UserElementsControl
{
    /// <summary>
    /// Логика взаимодействия для IELCheckBox.xaml
    /// </summary>
    public partial class IELCheckBox : IELContainerBase
    {
        #region Properties

        #region CheckBoxBorderThickness
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty CheckBoxBorderThicknessProperty =
            DependencyProperty.Register(nameof(CheckBoxBorderThickness), typeof(Thickness), typeof(IELCheckBox),
                new(new Thickness(2),
                    (sender, e) =>
                    {
                        ((IELCheckBox)sender).BorderCheck.BorderThickness = (Thickness)e.NewValue;
                    }));

        /// <summary>
        /// Толщина границ контейнера индикатора объекта
        /// </summary>
        public Thickness CheckBoxBorderThickness
        {
            get => (Thickness)GetValue(CheckBoxBorderThicknessProperty);
            set => SetValue(CheckBoxBorderThicknessProperty, value);
        }
        #endregion

        #region CheckBoxCornerRadius
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty CheckBoxCornerRadiusProperty =
            DependencyProperty.Register(nameof(CheckBoxCornerRadius), typeof(double), typeof(IELCheckBox),
                new(0d, CheckBoxCornerRadiusHandler));

        /// <summary>
        /// Установить элементу свойство
        /// </summary>
        /// <param name="Element">Объект которому устанавливается свойство</param>
        /// <param name="e">Данные о устанавливаемом свойстве</param>
        private static void CheckBoxCornerRadiusHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            IELCheckBox Source = (IELCheckBox)Element;
            double SourceNewValue = (double)e.NewValue;
            if (SourceNewValue - 2 >= 0)
            {
                Source.RectangleCheck.RadiusX = SourceNewValue - 2;
                Source.RectangleCheck.RadiusY = SourceNewValue - 2;
            }
            CornerRadius NewValue = new(SourceNewValue);
            Source.BorderCheck.CornerRadius = NewValue;
        }

        /// <summary>
        /// Скругление границ контейнера индикатора объекта
        /// </summary>
        public double CheckBoxCornerRadius
        {
            get => (double)GetValue(CheckBoxCornerRadiusProperty);
            set => SetValue(CheckBoxCornerRadiusProperty, value);
        }
        #endregion

        #region Text
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(IELCheckBox),
                new(string.Empty, TextHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="Text"/>
        /// </summary>
        private static void TextHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            IELCheckBox Source = (IELCheckBox)Element;
            string SourceNewValue = (string)e.NewValue;
            Source.TextBlockElement.Text = SourceNewValue;
        }

        /// <summary>
        /// Отображаемый текст в поле
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        #endregion

        #region CheckedTexture
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty CheckedTextureProperty =
            DependencyProperty.Register(nameof(CheckedTexture), typeof(ImageSource), typeof(IELCheckBox),
                new(null, CheckedTextureHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="CheckedTexture"/>
        /// </summary>
        private static void CheckedTextureHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            IELCheckBox Source = (IELCheckBox)Element;
            ImageSource? SourceNewValue = (ImageSource?)e.NewValue;
            if (SourceNewValue == null) Source.RectangleCheck.OpacityMask = null;
            else if (Source.RectangleCheck.OpacityMask == null)
                Source.RectangleCheck.OpacityMask = new ImageBrush(SourceNewValue);
            else
                ((ImageBrush)Source.RectangleCheck.OpacityMask).ImageSource = SourceNewValue;
        }

        /// <summary>
        /// Текстура изображения используемая для индикатора выделения
        /// </summary>
        public ImageSource CheckedTexture
        {
            get => (ImageSource)GetValue(CheckedTextureProperty);
            set => SetValue(CheckedTextureProperty, value);
        }
        #endregion

        #region IsChecked
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(IELCheckBox),
                new(false, IsCheckedHandler));

        /// <summary>
        /// Установить элементу свойство
        /// </summary>
        /// <param name="Element">Объект которому устанавливается свойство</param>
        /// <param name="e">Данные о устанавливаемом свойстве</param>
        private static void IsCheckedHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            IELCheckBox Source = (IELCheckBox)Element;
            bool SourceNewValue = (bool)e.NewValue;
            Source.IsCheckedChanged.Invoke(Source, SourceNewValue);
        }

        /// <summary>
        /// Состояние выделения
        /// </summary>
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        /// <summary>
        /// Событие изменения состояния выделения
        /// </summary>
        public event EventHandler<bool> IsCheckedChanged;

        /// <summary>
        /// Активировать состояние активации
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="e">Устанавливаемое состояние активации</param>
        private void IsCheckedChangedHandler(object? sender, bool e)
        {
            AnimationManager.AnimateTakingZeroTo(ManagerAnimation, RectangleCheck, OpacityProperty,
                e ? 1d : 0d, TimeSpan.FromMilliseconds(400d));
            AnimationManager.AnimateTakingZeroTo(ManagerAnimation, RectangleCheck, MarginProperty,
                new Thickness(e ? 0d : 2d), TimeSpan.FromMilliseconds(400d));
        }
        #endregion

        #endregion

        /// <summary>
        /// Инициализировать объект CheckBox
        /// </summary>
        public IELCheckBox()
        {
            InitializeComponent();
            RectangleCheck.Opacity = 0d;
            RectangleCheck.Margin = new(2d);
            RectangleCheck.RadiusX = 0d;
            RectangleCheck.RadiusY = 0d;
            BorderCheck.CornerRadius = new(0d);
            BorderCheck.BorderBrush = SourceBorderBrush.SourceBrush;
            RectangleCheck.Fill = SourceForeground.SourceBrush;
            TextBlockElement.Text = string.Empty;
            TextBlockElement.Foreground = SourceForeground.SourceBrush;

            MouseDown += (sender, e) =>
            {
                SetActiveSpecrum(SpectrumColor.Used);
            };

            MouseUp += (sender, e) =>
            {
                SetActiveSpecrum(SpectrumColor.Select);
            };

            MouseLeftButtonUp += (sender, e) => IsChecked = !IsChecked;

            IsCheckedChanged += IsCheckedChangedHandler;
        }
    }
}
