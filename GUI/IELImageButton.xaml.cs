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
            ButtonAnimationColor = new();

            TimeSpan AnimTime = TimeSpan.FromMilliseconds(80d);
            BrushSettingDNSU.ChangeSpectrumEventHandler ChangeBackground = (Element, Value) =>
            {
                Brush brush = new SolidColorBrush(Value);
                BorderButton.Background = brush;
            };
            BrushSettingDNSU.ChangeSpectrumEventHandler ChangeBorderBrush = (Element, Value) =>
            {
                Brush brush = new SolidColorBrush(Value);
                BorderButton.BorderBrush = brush;
            };
            /*BrushSettingDNSU.ChangeSpectrumEventHandler ChangeForeground = (Element, Value) =>
            {
                Brush brush = new SolidColorBrush(Value);
                TextBlockButton.Foreground = brush;
                TextBlockLeftArrow.Foreground = brush;
                TextBlockRightArrow.Foreground = brush;
                TextBlockCharKey.Foreground = brush;
            };*/
            SettingAnimate = new(
                new BrushSettingDNSU(Color.FromRgb(172, 238, 255), Color.FromRgb(101, 193, 241), Colors.White, Colors.IndianRed, AnimTime, ChangeBackground),
                new BrushSettingDNSU(Color.FromRgb(105, 71, 101), Color.FromRgb(158, 130, 155), Color.FromRgb(136, 93, 130), Colors.Brown, AnimTime, ChangeBorderBrush),
                new BrushSettingDNSU(AnimTime)
                );

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

                ButtonAnimationColor.To = BorderBrush;
                BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                ButtonAnimationColor.To = Background;
                BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            };

            MouseDown += (sender, e) =>
            {
                if (IsEnabled)
                {
                    if (
                    (e.LeftButton == MouseButtonState.Pressed && OnActivateMouseLeft != null) ||
                    (e.RightButton == MouseButtonState.Pressed && OnActivateMouseRight != null))
                    {
                        BorderButton.BorderBrush = new SolidColorBrush(SettingAnimate.BorderBrushDNSU.Used);
                        BorderButton.Background = new SolidColorBrush(SettingAnimate.BackgroundDNSU.Used);
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
            ButtonAnimationColor.To = SettingAnimate.BorderBrushDNSU.Select;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SettingAnimate.BackgroundDNSU.Select;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
        }

        /// <summary>
        /// Событие ухода курсора из видимой области кнопки
        /// </summary>
        private void MouseLeaveDetect()
        {
            ButtonAnimationColor.To = SettingAnimate.BorderBrushDNSU.Default;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SettingAnimate.BackgroundDNSU.Default;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
        }
    }
}
