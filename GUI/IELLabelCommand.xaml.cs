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

        #region animateObjects
        /// <summary>
        /// Анимация цвета кнопки
        /// </summary>
        private readonly ColorAnimation ButtonAnimationColor = new()
        {
            DecelerationRatio = 0.2d,
            EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
        };

        /// <summary>
        /// Анимация позиции
        /// </summary>
        private readonly ThicknessAnimation ButtonAnimationThickness = new()
        {
            EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseOut }
        };

        /// <summary>
        /// Анимация прозрачности
        /// </summary>
        private readonly DoubleAnimation ButtonAnimationDouble = new()
        {
            DecelerationRatio = 0.2d,
            EasingFunction = new QuinticEase() { EasingMode = EasingMode.EaseOut }
        };
        #endregion

        /// <summary>
        /// Толщина границ
        /// </summary>
        public Thickness BorderThicknessBlock
        {
            get => BorderMain.BorderThickness;
            set => BorderMain.BorderThickness = value;
        }

        /// <summary>
        /// Скруглённость границ
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => BorderMain.CornerRadius;
            set => BorderMain.CornerRadius = value;
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
                ButtonAnimationDouble.To = value ? 1d : 0d;
                ImageTag.BeginAnimation(OpacityProperty, ButtonAnimationDouble);
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

            TimeSpan AnimTime = TimeSpan.FromMilliseconds(80d);
            SettingAnimate = new();

            StartMarginImageElement = ImageElement.Margin;
            TextBlockName.Text = this.Label.Name;
            ImageTagVisible = false;

            IntervalHover = 1300d;
            TimerBorderInfo.Tick += (sender, e) =>
            {
                MouseHover?.Invoke(this, e);
                TimerBorderInfo.Stop();
            };

            BorderMain.Background = new SolidColorBrush(Colors.Black);
            BorderMain.BorderBrush = new RadialGradientBrush(Colors.White, Colors.Black);

            TextBlockName.Foreground = new SolidColorBrush(Colors.Black);
            TextBlockName.Foreground = new SolidColorBrush(Colors.Black);

            IsEnabledChanged += (sender, e) =>
            {
                Color
                Foreground = (bool)e.NewValue ? SettingAnimate.ForegroundDNSU.Default : SettingAnimate.ForegroundDNSU.NotEnabled,
                Background = (bool)e.NewValue ? SettingAnimate.BackgroundDNSU.Default : SettingAnimate.BackgroundDNSU.NotEnabled,
                BorderBrush = (bool)e.NewValue ? SettingAnimate.BorderBrushDNSU.Default : SettingAnimate.BorderBrushDNSU.NotEnabled;

                ButtonAnimationColor.To = Background;
                BorderMain.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

                ButtonAnimationColor.To = BorderBrush;
                BorderMain.BorderBrush.BeginAnimation(GradientStop.ColorProperty, ButtonAnimationColor);

                ButtonAnimationColor.To = Foreground;
                TextBlockIndex.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                TextBlockName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
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
                    if (e.LeftButton == MouseButtonState.Pressed && OnActivateMouseLeft != null) ClickDownAnimation();
                    else if (e.RightButton == MouseButtonState.Pressed && OnActivateMouseRight != null) ClickDownAnimation();
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
        /// Анимация выделения объекта мышью
        /// </summary>
        private void MouseEnterAnimation()
        {
            ButtonAnimationColor.To = SettingAnimate.BackgroundDNSU.Select;
            BorderMain.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SettingAnimate.BorderBrushDNSU.Select;
            BorderMain.BorderBrush.BeginAnimation(GradientStop.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SettingAnimate.ForegroundDNSU.Select;
            TextBlockIndex.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            TextBlockName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationThickness.To = new(
                StartMarginImageElement.Left - 3, StartMarginImageElement.Top - 3,
                StartMarginImageElement.Right - 3, StartMarginImageElement.Bottom - 3);
            ImageElement.BeginAnimation(MarginProperty, ButtonAnimationThickness);
        }

        /// <summary>
        /// Анимация отключения выделения мышью
        /// </summary>
        private void MouseLeaveAnimation()
        {
            ButtonAnimationColor.To = SettingAnimate.BackgroundDNSU.Default;
            BorderMain.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SettingAnimate.BorderBrushDNSU.Default;
            BorderMain.BorderBrush.BeginAnimation(GradientStop.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SettingAnimate.ForegroundDNSU.Default;
            TextBlockIndex.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            TextBlockName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationThickness.To = StartMarginImageElement;
            ImageElement.BeginAnimation(MarginProperty, ButtonAnimationThickness);
        }

        /// <summary>
        /// Анимировать нажатие на кнопку (Down)
        /// </summary>
        /// <param name="StyleClickColor">Стиль нажатия на кнопку</param>
        private void ClickDownAnimation()
        {
            ButtonAnimationColor.To = SettingAnimate.BackgroundDNSU.Used;
            BorderMain.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SettingAnimate.BorderBrushDNSU.Used;
            BorderMain.BorderBrush.BeginAnimation(GradientStop.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SettingAnimate.ForegroundDNSU.Used;
            TextBlockIndex.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            TextBlockName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationThickness.To = new(
                StartMarginImageElement.Left + 2, StartMarginImageElement.Top + 2,
                StartMarginImageElement.Right + 2, StartMarginImageElement.Bottom + 2);
            ImageElement.BeginAnimation(MarginProperty, ButtonAnimationThickness);
        }
    }
}
