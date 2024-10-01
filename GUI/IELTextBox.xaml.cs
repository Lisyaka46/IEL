using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELTextBox.xaml
    /// </summary>
    public partial class IELTextBox : UserControl
    {
        /// <summary>
        /// Скруглённость границ объекта
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => TextBoxBorder.CornerRadius;
            set => TextBoxBorder.CornerRadius = value;
        }

        /// <summary>
        /// Толщина границ объекта
        /// </summary>
        public Thickness BorderThicknessBlock
        {
            get => TextBoxBorder.BorderThickness;
            set => TextBoxBorder.BorderThickness = value;
        }

        /// <summary>
        /// Размер текста в элементе
        /// </summary>
        public new double FontSize
        {
            get => TextBoxMain.FontSize;
            set => TextBoxMain.FontSize = value;
        }

        #region Default
        private Color _DefaultBorderBrush;
        /// <summary>
        /// Цвет границы
        /// </summary>
        public Color DefaultBorderBrush
        {
            get => _DefaultBorderBrush;
            set
            {
                SolidColorBrush color = new(value);
                TextBoxBorder.BorderBrush = color;
                _DefaultBorderBrush = value;
            }
        }

        private Color _DefaultBackground;
        /// <summary>
        /// Цвет фона
        /// </summary>
        public Color DefaultBackground
        {
            get => _DefaultBackground;
            set
            {
                SolidColorBrush color = new(value);
                TextBoxBorder.Background = color;
                _DefaultBackground = value;
            }
        }

        private Color _DefaultForeground;
        /// <summary>
        /// Цвет текста
        /// </summary>
        public Color DefaultForeground
        {
            get => _DefaultForeground;
            set
            {
                SolidColorBrush color = new(value);
                TextBoxMain.Foreground = color;
                _DefaultForeground = value;
            }
        }
        #endregion


        #region Select
        /// <summary>
        /// Выделенный цвет границы
        /// </summary>
        public Color SelectBorderBrush { get; set; }

        /// <summary>
        /// Выделенный цвет фона
        /// </summary>
        public Color SelectBackground { get; set; }

        /// <summary>
        /// Выделенный цвет текста
        /// </summary>
        public Color SelectForeground { get; set; }
        #endregion


        #region Clicked
        /// <summary>
        /// Нажатый цвет границы
        /// </summary>
        public Color ClickedBorderBrush { get; set; }

        /// <summary>
        /// Нажатый цвет фона
        /// </summary>
        public Color ClickedBackground { get; set; }

        /// <summary>
        /// Нажатый цвет текста
        /// </summary>
        public Color ClickedForeground { get; set; }
        #endregion


        #region NotEnabled
        /// <summary>
        /// Выключенный цвет границы
        /// </summary>
        public Color NotEnabledBorderBrush { get; set; }

        /// <summary>
        /// Выключенный цвет фона
        /// </summary>
        public Color NotEnabledBackground { get; set; }

        /// <summary>
        /// Выключенный цвет текста
        /// </summary>
        public Color NotEnabledForeground { get; set; }
        #endregion

        #region AnimationMillisecond
        private int _AnimationMillisecond;
        /// <summary>
        /// Длительность анимации в миллисекундах
        /// </summary>
        public int AnimationMillisecond
        {
            get => _AnimationMillisecond;
            set
            {
                TimeSpan time = TimeSpan.FromMilliseconds(value);
                ButtonAnimationColor.Duration = time;
                _AnimationMillisecond = value;

            }
        }
        #endregion

        #region animateObjects
        /// <summary>
        /// Анимация цвета
        /// </summary>
        private readonly ColorAnimation ButtonAnimationColor;
        #endregion

        /// <summary>
        /// Текст элемента
        /// </summary>
        public string Text
        {
            get => TextBoxMain.Text;
            set => TextBoxMain.Text = value;
        }

        /// <summary>
        /// Переопределённый объект фона
        /// </summary>
        public new Brush Background
        {
            get => TextBoxBorder.Background;
            set => TextBoxBorder.Background = value;
        }

        /// <summary>
        /// Сделать фокус на элементе
        /// </summary>
        public new void Focus()
        {
            TextBoxMain.Focus();
        }

        /// <summary>
        /// Фокусировка текста
        /// </summary>
        private bool FocusText = false;

        public IELTextBox()
        {
            InitializeComponent();

            ButtonAnimationColor = new();
            AnimationMillisecond = 80;

            DefaultBorderBrush = Colors.Black;
            DefaultBackground = Color.FromRgb(66, 183, 121);
            DefaultForeground = Colors.Black;

            SelectBorderBrush = Color.FromRgb(29, 33, 18);
            SelectBackground = Color.FromRgb(204, 201, 120);
            SelectForeground = Color.FromRgb(33, 43, 38);

            ClickedBorderBrush = Color.FromRgb(1, 68, 101);
            ClickedBackground = Color.FromRgb(120, 204, 160);
            ClickedForeground = Color.FromRgb(1, 43, 28);

            NotEnabledBorderBrush = Colors.Brown;
            NotEnabledBackground = Colors.IndianRed;
            NotEnabledForeground = Colors.DarkRed;

            GotKeyboardFocus += (sender, e) =>
            {
                FocusText = true;
                ButtonAnimationColor.Duration = TimeSpan.FromMilliseconds(_AnimationMillisecond * 2);
                ButtonAnimationColor.To = ClickedBorderBrush;
                TextBoxBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                ButtonAnimationColor.To = ClickedBackground;
                TextBoxBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                ButtonAnimationColor.To = ClickedForeground;
                TextBoxMain.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                ButtonAnimationColor.Duration = TimeSpan.FromMilliseconds(_AnimationMillisecond);
            };
            LostKeyboardFocus += (sender, e) =>
            {
                FocusText = false;
                MouseLeaveAnimation();
            };

            MouseEnter += (sender, e) =>
            {
                if (IsEnabled && !FocusText) MouseEnterAnimation();
            };

            MouseLeave += (sender, e) =>
            {
                if (IsEnabled && !FocusText) MouseLeaveAnimation();
            };

            IsEnabledChanged += (sender, e) =>
            {
                Color
                Foreground = (bool)e.NewValue ? DefaultForeground : NotEnabledForeground,
                Background = (bool)e.NewValue ? DefaultBackground : NotEnabledBackground,
                BorderBrush = (bool)e.NewValue ? DefaultBorderBrush : NotEnabledBorderBrush;

                ButtonAnimationColor.To = BorderBrush;
                TextBoxBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                ButtonAnimationColor.To = Background;
                TextBoxBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                ButtonAnimationColor.To = Foreground;
                TextBoxMain.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            };

            MouseDown += (sender, e) =>
            {
                TextBoxMain.Focus();
            };
        }

        /// <summary>
        /// Анимация выделения мышью
        /// </summary>
        private void MouseEnterAnimation()
        {

            ButtonAnimationColor.To = SelectBorderBrush;
            TextBoxBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SelectBackground;
            TextBoxBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SelectForeground;
            TextBoxMain.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
        }

        /// <summary>
        /// Анимация отключения выделения мышью
        /// </summary>
        private void MouseLeaveAnimation()
        {

            ButtonAnimationColor.To = DefaultBorderBrush;
            TextBoxBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = DefaultBackground;
            TextBoxBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = DefaultForeground;
            TextBoxMain.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
        }
    }
}
