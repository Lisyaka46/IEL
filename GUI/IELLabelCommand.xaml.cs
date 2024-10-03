using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using IEL.Classes;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELLabelCommand.xaml
    /// </summary>
    public partial class IELLabelCommand : UserControl, IIELButtonDefault
    {
        /// <summary>
        /// Обект настройки поведения анимации цвета
        /// </summary>
        public IELSettingAnimate SettingAnimate { get; set; }

        /// <summary>
        /// Объект события активации кнопки левым щелчком мыши
        /// </summary>
        public IIELButtonDefault.Activate? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации кнопки правым щелчком мыши
        /// </summary>
        public IIELButtonDefault.Activate? OnActivateMouseRight { get; set; }

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
                AnimationThickness.Duration = time;
                AnimationDouble.Duration = time;
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
        /// Данные изображения объекта
        /// </summary>
        public ImageSource ImageSource
        {
            get => ImageElement.Source;
            set => ImageElement.Source = value;
        }

        /// <summary>
        /// Данные изображения тега
        /// </summary>
        public ImageSource ImageTagSource
        {
            get => ImageTag.Source;
            set => ImageTag.Source = value;
        }

        private bool _ImageTagVisible;
        /// <summary>
        /// Видимость изображения тега
        /// </summary>
        public bool ImageTagVisible
        {
            get => _ImageTagVisible;
            set
            {
                AnimationDouble.To = value ? 1d : 0d;
                ImageTag.BeginAnimation(OpacityProperty, AnimationDouble);
                _ImageTagVisible = value;
            }
        }

        public LabelAction Label { get; set; }

        private int _Index;
        public int Index
        {
            get => _Index;
            set
            {
                TextBlockIndex.Text = $"{value + 1}";
                _Index = value;
            }
        }

        private readonly Thickness StartMarginImageElement;

        public IELLabelCommand(LabelAction Label, int Index = 0)
        {
            InitializeComponent();
            this.Label = Label;
            this.Index = Index;

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
            BrushSettingDNSU ForegroundDNSU = new(BrushSettingDNSU.CreateStyle.Foreground,
                (Value) =>
                {
                    SolidColorBrush color = new(Value);
                    TextBlockName.Foreground = color;
                    TextBlockIndex.Foreground = color;
                });
            SettingAnimate = new(BackgroundDNSU, BorderBrushDNSU, ForegroundDNSU);

            StartMarginImageElement = ImageElement.Margin;
            TextBlockName.Text = this.Label.Name;
            ImageTagVisible = false;

            IntervalHover = 1300d;
            TimerBorderInfo.Tick += (sender, e) =>
            {
                MouseHover?.Invoke(this, e);
                TimerBorderInfo.Stop();
            };

            IsEnabledChanged += (sender, e) =>
            {
                Color
                Foreground = (bool)e.NewValue ? SettingAnimate.ForegroundDNSU.Default : SettingAnimate.ForegroundDNSU.NotEnabled,
                Background = (bool)e.NewValue ? SettingAnimate.BackgroundDNSU.Default : SettingAnimate.BackgroundDNSU.NotEnabled,
                BorderBrush = (bool)e.NewValue ? SettingAnimate.BorderBrushDNSU.Default : SettingAnimate.BorderBrushDNSU.NotEnabled;

                AnimationColor.To = Background;
                BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

                AnimationColor.To = BorderBrush;
                BorderButton.BorderBrush.BeginAnimation(GradientStop.ColorProperty, AnimationColor);

                AnimationColor.To = Foreground;
                TextBlockIndex.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                TextBlockName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            };

            MouseEnter += (sender, e) =>
            {
                if (IsEnabled)
                {
                    MouseEnterAnimation();
                    TimerBorderInfo.Start();
                }
            };

            MouseLeave += (sender, e) =>
            {
                if (IsEnabled)
                {
                    MouseLeaveAnimation();
                    TimerBorderInfo.Stop();
                }
            };

            MouseDown += (sender, e) =>
            {
                if (IsEnabled)
                {
                    TimerBorderInfo.Stop();
                    if (
                    (e.LeftButton == MouseButtonState.Pressed && OnActivateMouseLeft != null) ||
                    (e.RightButton == MouseButtonState.Pressed && OnActivateMouseRight != null)) ClickDownAnimation();
                }
            };

            MouseLeftButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseLeft != null)
                {
                    MouseEnterAnimation();
                    OnActivateMouseLeft?.Invoke();
                }
            };

            MouseRightButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    MouseEnterAnimation();
                    OnActivateMouseRight?.Invoke();
                }
            };
        }

        /// <summary>
        /// Анимировать нажатие на кнопку (Down)
        /// </summary>
        /// <param name="StyleClickColor">Стиль нажатия на кнопку</param>
        private void ClickDownAnimation()
        {
            Color
                Background = SettingAnimate.BackgroundDNSU.Used,
                BorderBrush = SettingAnimate.BorderBrushDNSU.Used,
                Foreground = SettingAnimate.ForegroundDNSU.Used;

            AnimationColor.To = BorderBrush;
            BorderButton.BorderBrush.BeginAnimation(GradientStop.ColorProperty, AnimationColor);

            AnimationColor.To = Background;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Foreground;
            TextBlockIndex.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            TextBlockName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationThickness.To = new(
                StartMarginImageElement.Left + 2, StartMarginImageElement.Top + 2,
                StartMarginImageElement.Right + 2, StartMarginImageElement.Bottom + 2);
            ImageElement.BeginAnimation(MarginProperty, AnimationThickness);
        }

        /// <summary>
        /// Анимация выделения объекта мышью
        /// </summary>
        private void MouseEnterAnimation()
        {
            Color
                Foreground = SettingAnimate.ForegroundDNSU.Select,
                Background = SettingAnimate.BackgroundDNSU.Select,
                BorderBrush = SettingAnimate.BorderBrushDNSU.Select;
            AnimationColor.To = BorderBrush;
            BorderButton.BorderBrush.BeginAnimation(GradientStop.ColorProperty, AnimationColor);

            AnimationColor.To = Background;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);


            AnimationColor.To = Foreground;
            TextBlockIndex.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            TextBlockName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationThickness.To = new(
                StartMarginImageElement.Left - 3, StartMarginImageElement.Top - 3,
                StartMarginImageElement.Right - 3, StartMarginImageElement.Bottom - 3);
            ImageElement.BeginAnimation(MarginProperty, AnimationThickness);
        }

        /// <summary>
        /// Анимация отключения выделения мышью
        /// </summary>
        private void MouseLeaveAnimation()
        {
            Color
                Foreground = SettingAnimate.ForegroundDNSU.Default,
                Background = SettingAnimate.BackgroundDNSU.Default,
                BorderBrush = SettingAnimate.BorderBrushDNSU.Default;

            AnimationColor.To = BorderBrush;
            BorderButton.BorderBrush.BeginAnimation(GradientStop.ColorProperty, AnimationColor);

            AnimationColor.To = Background;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Foreground;
            TextBlockIndex.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            TextBlockName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationThickness.To = StartMarginImageElement;
            ImageElement.BeginAnimation(MarginProperty, AnimationThickness);
        }
    }
}
