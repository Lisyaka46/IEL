using IEL.Classes;
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
        /// <summary>
        /// Обект настройки поведения анимации цвета
        /// </summary>
        public IELSettingAnimate SettingAnimate { get; set; }

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
                AnimationColor.Duration = time;
                AnimationDouble.Duration = time;
                AnimationThickness.Duration = TimeSpan.FromMilliseconds(value * 1.4d);
                _AnimationMillisecond = value;

            }
        }
        #endregion

        #region animateObjects
        /// <summary>
        /// Анимация color значения
        /// </summary>
        private readonly ColorAnimation AnimationColor = new()
        {
            DecelerationRatio = 0.2d,
            EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
        };

        /// <summary>
        /// Анимация thickness значения
        /// </summary>
        private readonly ThicknessAnimation AnimationThickness = new()
        {
            EasingFunction = new QuinticEase() { EasingMode = EasingMode.EaseOut }
        };

        /// <summary>
        /// Анимация double значения
        /// </summary>
        private readonly DoubleAnimation AnimationDouble = new()
        {
            DecelerationRatio = 0.2d,
            EasingFunction = new QuinticEase() { EasingMode = EasingMode.EaseOut }
        };
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
                return ImageButton.Source;
            }
            set
            {
                ImageButton.Source = value;
            }
        }

        /// <summary>
        /// Толщина границ
        /// </summary>
        public Thickness BorderThicknessBlock
        {
            get => BorderButton.BorderThickness;
            set => BorderButton.BorderThickness = value;
        }

        /// <summary>
        /// Скруглённость границ
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => BorderButton.CornerRadius;
            set => BorderButton.CornerRadius = value;
        }

        /// <summary>
        /// Позиционирование картинки в кнопке
        /// </summary>
        public Thickness ImageMargin
        {
            get => ImageButton.Margin;
            set => ImageButton.Margin = value;
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
                    AnimationDouble.To = value ? 0.4d : 0d;
                    ImageMouseButtonsUse.BeginAnimation(OpacityProperty, AnimationDouble);
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
        public bool CharKeyboardActivate
        {
            get => _CharKeyKeyboardActivate;
            set
            {
                AnimationDouble.To = value ? 1d : 0d;
                AnimationThickness.To = new(!value ? -24 : 0, 0, 0, 0);
                BorderButton.BeginAnimation(MarginProperty, AnimationThickness);
                BorderButtonKey.BeginAnimation(OpacityProperty, AnimationDouble);
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

            AnimationMillisecond = 100;
            BrushSettingDNSU BackgroundDNSU = new(BrushSettingDNSU.CreateStyle.Background,
                (Value) =>
                {
                    SolidColorBrush color = new(Value);
                    BorderButton.Background = color;
                    BorderButtonKey.Background = color;
                });
            BrushSettingDNSU BorderBrushDNSU = new(BrushSettingDNSU.CreateStyle.BorderBrush,
                (Value) =>
                {
                    SolidColorBrush color = new(Value);
                    BorderButton.BorderBrush = color;
                    BorderButtonKey.BorderBrush = color;
                });
            BrushSettingDNSU ForegroundDNSU = new(BrushSettingDNSU.CreateStyle.Foreground,
                (Value) =>
                {
                    SolidColorBrush color = new(Value);
                    TextBlockKey.Foreground = color;
                });
            SettingAnimate = new(BackgroundDNSU, BorderBrushDNSU, ForegroundDNSU);

            ImageButton.Margin = new Thickness(10, 10, 10, 10);
            CharKeyboardActivate = false;
            IntervalHover = 1300d;
            TimerBorderInfo.Tick += (sender, e) =>
            {
                MouseHover?.Invoke(this, e);
                TimerBorderInfo.Stop();
            };

            ImageMouseButtonsUse.Opacity = 0d;
            BorderButton.MouseEnter += (sender, e) =>
            {
                if (IsEnabled) MouseEnterDetect();
            };
            BorderButton.MouseLeave += (sender, e) =>
            {
                if (IsEnabled) MouseLeaveDetect();
            };

            IsEnabledChanged += (sender, e) =>
            {
                Color
                Background = (bool)e.NewValue ? SettingAnimate.BackgroundDNSU.Default : SettingAnimate.BackgroundDNSU.NotEnabled,
                BorderBrush = (bool)e.NewValue ? SettingAnimate.BorderBrushDNSU.Default : SettingAnimate.BorderBrushDNSU.NotEnabled,
                Foreground = (bool)e.NewValue ? SettingAnimate.ForegroundDNSU.Default : SettingAnimate.ForegroundDNSU.NotEnabled;

                AnimationColor.To = BorderBrush;
                BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                BorderButtonKey.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

                AnimationColor.To = Background;
                BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                BorderButtonKey.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

                AnimationColor.To = Foreground;
                TextBlockKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            };

            MouseDown += (sender, e) =>
            {
                if (IsEnabled)
                {
                    if (
                    (e.LeftButton == MouseButtonState.Pressed && OnActivateMouseLeft != null) ||
                    (e.RightButton == MouseButtonState.Pressed && OnActivateMouseRight != null)) ClickDownAnimation();
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
        /// Анимировать нажатие на кнопку (Down)
        /// </summary>
        private void ClickDownAnimation()
        {
            Color
                Background = SettingAnimate.BackgroundDNSU.Used,
                BorderBrush = SettingAnimate.BorderBrushDNSU.Used,
                Foreground = SettingAnimate.ForegroundDNSU.Used;

            BorderButton.BorderBrush = new SolidColorBrush(BorderBrush);
            BorderButton.Background = new SolidColorBrush(Background);
            TextBlockKey.Foreground = new SolidColorBrush(Foreground);
        }

        /// <summary>
        /// Событие прихода курсора в видимую область кнопки
        /// </summary>
        private void MouseEnterDetect()
        {
            Color
                Foreground = SettingAnimate.ForegroundDNSU.Select,
                Background = SettingAnimate.BackgroundDNSU.Select,
                BorderBrush = SettingAnimate.BorderBrushDNSU.Select;
            AnimationColor.To = BorderBrush;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Background;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            BorderButtonKey.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Foreground;
            TextBlockKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            if (VisibleMouseImaging)
            {
                ImageMouse = IIELObject.ImageMouseButton(OnActivateMouseLeft != null, OnActivateMouseRight != null);
                if (ImageMouse != null)
                {
                    AnimationDouble.To = 0.4d;
                    ImageMouseButtonsUse.BeginInit();
                    ImageMouseButtonsUse.Source = ImageMouse;
                    ImageMouseButtonsUse.EndInit();
                    ImageMouseButtonsUse.BeginAnimation(OpacityProperty, AnimationDouble);
                }
            }
            EnterButton = true;
        }

        /// <summary>
        /// Событие ухода курсора из видимой области кнопки
        /// </summary>
        private void MouseLeaveDetect()
        {
            Color
                Foreground = SettingAnimate.ForegroundDNSU.Default,
                Background = SettingAnimate.BackgroundDNSU.Default,
                BorderBrush = SettingAnimate.BorderBrushDNSU.Default;
            AnimationColor.To = BorderBrush;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Background;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            BorderButtonKey.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Foreground;
            TextBlockKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationDouble.To = 0d;
            ImageMouseButtonsUse.BeginAnimation(OpacityProperty, AnimationDouble);
            EnterButton = false;
        }

        /// <summary>
        /// Анимация мерцания
        /// </summary>
        [MTAThread()]
        public void BlinkAnimation()
        {
            AnimationColor.SpeedRatio = 0.6d;
            AnimationColor.From = SettingAnimate.BorderBrushDNSU.Used;
            AnimationColor.To = EnterButton ? SettingAnimate.BorderBrushDNSU.Select : SettingAnimate.BorderBrushDNSU.Default;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            BorderButtonKey.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.From = SettingAnimate.BackgroundDNSU.Used;
            AnimationColor.To = EnterButton ? SettingAnimate.BackgroundDNSU.Select : SettingAnimate.BackgroundDNSU.Default;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            BorderButtonKey.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.From = SettingAnimate.ForegroundDNSU.Used;
            AnimationColor.To = EnterButton ? SettingAnimate.ForegroundDNSU.Select : SettingAnimate.ForegroundDNSU.Default;
            TextBlockKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.SpeedRatio = 1;
            AnimationColor.From = null;
        }
    }
}
