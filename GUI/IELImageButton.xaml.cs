using IEL.Classes;
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
        /// Толщина границ кнопки
        /// </summary>
        public Thickness BorderThicknessBlock
        {
            get => BorderButton.BorderThickness;
            set => BorderButton.BorderThickness = value;
        }

        /// <summary>
        /// Скруглённость границ кнопки
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
            AnimationMillisecond = 100;
            BrushSettingDNSU BackgroundDNSU = new(BrushSettingDNSU.CreateStyle.Background,
                (Value) =>
                {
                    SolidColorBrush color = new(Value);
                    BorderButton.Background = color;
                });
            BrushSettingDNSU BorderBrushDNSU = new(BrushSettingDNSU.CreateStyle.BorderBrush,
                (Value) =>
                {
                    SolidColorBrush color = new(Value);
                    BorderButton.BorderBrush = color;
                });
            BrushSettingDNSU ForegroundDNSU = new();
            SettingAnimate = new(BackgroundDNSU, BorderBrushDNSU, ForegroundDNSU);

            ImageButton.Margin = new Thickness(10, 10, 10, 10);
            IntervalHover = 1300d;
            TimerBorderInfo.Tick += (sender, e) =>
            {
                MouseHover?.Invoke(this, e);
                TimerBorderInfo.Stop();
            };

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
                    BorderBrush = (bool)e.NewValue ? SettingAnimate.BorderBrushDNSU.Default : SettingAnimate.BorderBrushDNSU.NotEnabled;

                AnimationColor.To = BorderBrush;
                BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                AnimationColor.To = Background;
                BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
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
        /// Анимировать нажатие на кнопку (Down)
        /// </summary>
        private void ClickDownAnimation()
        {
            Color
                Background = SettingAnimate.BackgroundDNSU.Used,
                BorderBrush = SettingAnimate.BorderBrushDNSU.Used;

            BorderButton.BorderBrush = new SolidColorBrush(BorderBrush);
            BorderButton.Background = new SolidColorBrush(Background);
        }

        /// <summary>
        /// Событие прихода курсора в видимую область кнопки
        /// </summary>
        private void MouseEnterDetect()
        {
            Color
                Background = SettingAnimate.BackgroundDNSU.Select,
                BorderBrush = SettingAnimate.BorderBrushDNSU.Select;

            AnimationColor.To = BorderBrush;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Background;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
        }

        /// <summary>
        /// Событие ухода курсора из видимой области кнопки
        /// </summary>
        private void MouseLeaveDetect()
        {
            Color
                Background = SettingAnimate.BackgroundDNSU.Default,
                BorderBrush = SettingAnimate.BorderBrushDNSU.Default;

            AnimationColor.To = BorderBrush;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Background;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
        }
    }
}
