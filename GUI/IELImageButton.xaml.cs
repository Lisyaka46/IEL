using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELImageButton.xaml
    /// </summary>
    public partial class IELImageButton : UserControl, IIELButtonDefault
    {
        #region Default
        private Color? _DefaultBorderBrush;
        /// <summary>
        /// Цвет границы
        /// </summary>
        public Color DefaultBorderBrush
        {
            get => _DefaultBorderBrush ?? Colors.Gold;
            set
            {
                SolidColorBrush color = new(value);
                ButtonBorder.BorderBrush = color;
                _DefaultBorderBrush = value;
            }
        }

        private Color? _DefaultBackground;
        /// <summary>
        /// Цвет фона
        /// </summary>
        public Color DefaultBackground
        {
            get => _DefaultBackground ?? Colors.Gold;
            set
            {
                SolidColorBrush color = new(value);
                ButtonBorder.Background = color;
                _DefaultBackground = value;
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

        #region MouseHover
        /// <summary>
        /// Длительность задержки в миллисекундах
        /// </summary>
        public double IntervalHover
        {
            get => TimerBorderInfo.Interval.TotalMilliseconds;
            set => TimerBorderInfo.Interval = TimeSpan.FromMilliseconds(value);
        }

        /// <summary>
        /// Таймер события MouseHover
        /// </summary>
        private readonly DispatcherTimer TimerBorderInfo = new();

        /// <summary>
        /// Событие задержки курсора на элементе
        /// </summary>
        public event EventHandler? MouseHover;
        #endregion

        /// <summary>
        /// Изображение которое отображается в кнопке
        /// </summary>
        public ImageSource Imaging
        {
            get
            {
                return ButtonImage.Source;
            }
            set
            {
                ButtonImage.Source = value;
            }
        }

        /// <summary>
        /// Толщина границ кнопки
        /// </summary>
        public Thickness BorderThicknessBlock
        {
            get => ButtonBorder.BorderThickness;
            set => ButtonBorder.BorderThickness = value;
        }

        /// <summary>
        /// Скруглённость границ кнопки
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => ButtonBorder.CornerRadius;
            set => ButtonBorder.CornerRadius = value;
        }

        /// <summary>
        /// Позиционирование картинки в кнопке
        /// </summary>
        public Thickness ImageMargin
        {
            get => ButtonImage.Margin;
            set => ButtonImage.Margin = value;
        }

        /// <summary>
        /// Объект события активации кнопки левым щелчком мыши
        /// </summary>
        public IIELButtonDefault.Activate? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации кнопки правым щелчком мыши
        /// </summary>
        public IIELButtonDefault.Activate? OnActivateMouseRight { get; set; }

        /// <summary>
        /// Инициализировать объект кнопки с изображением
        /// </summary>
        public IELImageButton()
        {
            InitializeComponent();
            ButtonAnimationColor = new();
            AnimationMillisecond = 80;
            ButtonImage.Margin = new Thickness(10, 10, 10, 10);
            IntervalHover = 1300d;
            TimerBorderInfo.Tick += (sender, e) =>
            {
                MouseHover?.Invoke(this, e);
                TimerBorderInfo.Stop();
            };

            DefaultBorderBrush = Colors.Black;
            SelectBorderBrush = Colors.DarkGray;
            ClickedBorderBrush = Colors.Gray;
            NotEnabledBorderBrush = Colors.Brown;

            DefaultBackground = Colors.White;
            SelectBackground = Colors.Gray;
            ClickedBackground = Colors.WhiteSmoke;
            NotEnabledBackground = Colors.IndianRed;

            ButtonBorder.MouseEnter += (sender, e) =>
            {
                if (IsEnabled) MouseEnterDetect();
            };
            ButtonBorder.MouseLeave += (sender, e) =>
            {
                if (IsEnabled) MouseLeaveDetect();
            };
            IsEnabledChanged += (sender, e) =>
            {
                Color
                Background = (bool)e.NewValue ? DefaultBackground : NotEnabledBackground,
                BorderBrush = (bool)e.NewValue ? DefaultBorderBrush : NotEnabledBorderBrush;

                ButtonAnimationColor.To = BorderBrush;
                ButtonBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                ButtonAnimationColor.To = Background;
                ButtonBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            };

            MouseDown += (sender, e) =>
            {
                if (IsEnabled)
                {
                    if (
                    (e.LeftButton == MouseButtonState.Pressed && OnActivateMouseLeft != null) ||
                    (e.RightButton == MouseButtonState.Pressed && OnActivateMouseRight != null))
                    {
                        ButtonBorder.BorderBrush = new SolidColorBrush(ClickedBorderBrush);
                        ButtonBorder.Background = new SolidColorBrush(ClickedBackground);
                    }
                }
            };

            MouseLeftButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseLeft != null)
                {
                    MouseEnterDetect();
                    OnActivateMouseLeft?.Invoke();
                }
            };

            MouseRightButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    MouseEnterDetect();
                    OnActivateMouseRight?.Invoke();
                }
            };
        }

        /// <summary>
        /// Событие прихода курсора в видимую область кнопки
        /// </summary>
        private void MouseEnterDetect()
        {
            ButtonAnimationColor.To = SelectBorderBrush;
            ButtonBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SelectBackground;
            ButtonBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
        }

        /// <summary>
        /// Событие ухода курсора из видимой области кнопки
        /// </summary>
        private void MouseLeaveDetect()
        {
            ButtonAnimationColor.To = DefaultBorderBrush;
            ButtonBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = DefaultBackground;
            ButtonBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
        }
    }
}
