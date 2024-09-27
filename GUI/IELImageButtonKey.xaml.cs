using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELImageButtonKey.xaml
    /// </summary>
    public partial class IELImageButtonKey : UserControl, IIELButtonKey
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
                ButtonBorderKey.BorderBrush = color;
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
                ButtonBorderKey.Background = color;
                _DefaultBackground = value;
            }
        }

        private Color? _DefaultForeground;
        /// <summary>
        /// Цвет текста
        /// </summary>
        public Color DefaultForeground
        {
            get => _DefaultForeground ?? Colors.Gold;
            set
            {
                SolidColorBrush color = new(value);
                TextBlockKey.Foreground = color;
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
                ButtonAnimationDouble.Duration = time;
                ButtonAnimationThickness.Duration = time;
                _AnimationMillisecond = value;

            }
        }
        #endregion

        #region animateObjects
        /// <summary>
        /// Анимация цвета
        /// </summary>
        private readonly ColorAnimation ButtonAnimationColor;

        /// <summary>
        /// Анимация цвета
        /// </summary>
        private readonly DoubleAnimation ButtonAnimationDouble;

        /// <summary>
        /// Анимация позиции
        /// </summary>
        private readonly ThicknessAnimation ButtonAnimationThickness;
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
        /// Толщина границ
        /// </summary>
        public Thickness BorderThicknessBlock
        {
            get => ButtonBorder.BorderThickness;
            set => ButtonBorder.BorderThickness = value;
        }

        /// <summary>
        /// Скруглённость границ
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
        public IIELButtonKey.Activate? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации кнопки правым щелчком мыши
        /// </summary>
        public IIELButtonKey.Activate? OnActivateMouseRight { get; set; }

        /// <summary>
        /// Картинка действий над кнопкой
        /// </summary>
        private BitmapImage? ImageMouse;

        private bool _VisibleMouseImaging = true;
        /// <summary>
        /// Состояние активности отображения действий на кнопке
        /// </summary>
        public bool VisibleMouseImaging
        {
            get => _VisibleMouseImaging;
            set
            {
                _VisibleMouseImaging = value;
                if (EnterButton)
                {
                    ButtonAnimationDouble.To = value ? 0.4d : 0d;
                    ImageMouseButtonsUse.BeginAnimation(OpacityProperty, ButtonAnimationDouble);
                }
            }
        }

        /// <summary>
        /// Состояние активности наведения на кнопку
        /// </summary>
        private bool EnterButton = false;

        private bool _CharKeyKeyboardActivate = false;
        /// <summary>
        /// Активность видимости символа действия активации кнопки
        /// </summary>
        public bool CharKeyKeyboardActivate
        {
            get => _CharKeyKeyboardActivate;
            set
            {
                ButtonAnimationDouble.To = value ? 1d : 0d;
                ButtonAnimationThickness.To = new(!value ? -24 : 0, 0, 0, 0);
                ButtonBorder.BeginAnimation(MarginProperty, ButtonAnimationThickness);
                ButtonBorderKey.BeginAnimation(OpacityProperty, ButtonAnimationDouble);
                _CharKeyKeyboardActivate = value;
            }
        }

        private Key? _CharKeyKeyboard;
        /// <summary>
        /// Клавиша отвечающая за активацию кнопки
        /// </summary>
        public Key? CharKeyKeyboard
        {
            get => _CharKeyKeyboard;
            set
            {
                _CharKeyKeyboard = value;
                TextBlockKey.Text = IIELObject.KeyName(value).ToString();
            }
        }

        /// <summary>
        /// Инициализировать объект кнопки с изображением
        /// </summary>
        public IELImageButtonKey()
        {
            InitializeComponent();
            ButtonAnimationThickness = new();
            ButtonAnimationDouble = new();
            ButtonAnimationColor = new();
            AnimationMillisecond = 80;
            ButtonImage.Margin = new Thickness(10, 10, 10, 10);
            CharKeyKeyboardActivate = false;
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

            DefaultForeground = Colors.Black;
            SelectForeground = Colors.DimGray;
            ClickedForeground = Colors.LightGray;
            NotEnabledForeground = Colors.Red;

            ImageMouseButtonsUse.Opacity = 0d;
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
                BorderBrush = (bool)e.NewValue ? DefaultBorderBrush : NotEnabledBorderBrush,
                Foreground = (bool)e.NewValue ? DefaultForeground : NotEnabledForeground;

                ButtonAnimationColor.To = BorderBrush;
                ButtonBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                ButtonBorderKey.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

                ButtonAnimationColor.To = Background;
                ButtonBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                ButtonBorderKey.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

                ButtonAnimationColor.To = Foreground;
                TextBlockKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
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
                        TextBlockKey.Foreground = new SolidColorBrush(ClickedForeground);
                    }
                }
            };

            MouseLeftButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseLeft != null)
                {
                    MouseEnterDetect();
                    OnActivateMouseLeft?.Invoke(false);
                }
            };

            MouseRightButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    MouseEnterDetect();
                    OnActivateMouseRight?.Invoke(false);
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
            ButtonBorderKey.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SelectForeground;
            TextBlockKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            if (VisibleMouseImaging)
            {
                ImageMouse = IIELObject.ImageMouseButton(OnActivateMouseLeft != null, OnActivateMouseRight != null);
                if (ImageMouse != null)
                {
                    ButtonAnimationDouble.To = 0.4d;
                    ImageMouseButtonsUse.BeginInit();
                    ImageMouseButtonsUse.Source = ImageMouse;
                    ImageMouseButtonsUse.EndInit();
                    ImageMouseButtonsUse.BeginAnimation(OpacityProperty, ButtonAnimationDouble);
                }
            }
            EnterButton = true;
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
            ButtonBorderKey.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = DefaultForeground;
            TextBlockKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationDouble.To = 0d;
            ImageMouseButtonsUse.BeginAnimation(OpacityProperty, ButtonAnimationDouble);
            EnterButton = false;
        }

        /// <summary>
        /// Анимация мерцания
        /// </summary>
        [MTAThread()]
        public void BlinkAnimation()
        {
            ButtonAnimationColor.SpeedRatio = 0.6d;
            ButtonAnimationColor.From = Colors.White;
            ButtonAnimationColor.To = EnterButton ? SelectBorderBrush : DefaultBorderBrush;
            ButtonBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            ButtonBorderKey.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.From = Colors.White;
            ButtonAnimationColor.To = EnterButton ? SelectBackground : DefaultBackground;
            ButtonBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            ButtonBorderKey.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.From = Colors.White;
            ButtonAnimationColor.To = EnterButton ? SelectForeground : DefaultForeground;
            TextBlockKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.SpeedRatio = 1;
            ButtonAnimationColor.From = null;
        }
    }
}
