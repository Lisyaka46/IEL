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
        public bool CharKeyboardActivate
        {
            get => _CharKeyKeyboardActivate;
            set
            {
                ButtonAnimationDouble.To = value ? 1d : 0d;
                ButtonAnimationThickness.To = new(!value ? -24 : 0, 0, 0, 0);
                BorderButton.BeginAnimation(MarginProperty, ButtonAnimationThickness);
                BorderButtonKey.BeginAnimation(OpacityProperty, ButtonAnimationDouble);
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

            TimeSpan AnimTime = TimeSpan.FromMilliseconds(80d);
            BrushSettingDNSU.ChangeSpectrumEventHandler ChangeBackground = (Element, Value) =>
            {
                Brush brush = new SolidColorBrush(Value);
                BorderButton.Background = brush;
                BorderButtonKey.Background = brush;
            };
            BrushSettingDNSU.ChangeSpectrumEventHandler ChangeBorderBrush = (Element, Value) =>
            {
                Brush brush = new SolidColorBrush(Value);
                BorderButton.BorderBrush = brush;
                BorderButtonKey.BorderBrush = brush;
            };
            BrushSettingDNSU.ChangeSpectrumEventHandler ChangeForeground = (Element, Value) =>
            {
                Brush brush = new SolidColorBrush(Value);
                TextBlockKey.Foreground = brush;
            };
            SettingAnimate = new(
                new BrushSettingDNSU(Color.FromRgb(172, 238, 255), Color.FromRgb(101, 193, 241), Colors.White, Colors.IndianRed, AnimTime, ChangeBackground),
                new BrushSettingDNSU(Color.FromRgb(105, 71, 101), Color.FromRgb(158, 130, 155), Color.FromRgb(136, 93, 130), Colors.Brown, AnimTime, ChangeBorderBrush),
                new BrushSettingDNSU(Colors.Black, Color.FromRgb(28, 33, 32), Color.FromRgb(0, 49, 34), Colors.DarkRed, AnimTime, ChangeForeground)
                );

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

                ButtonAnimationColor.To = BorderBrush;
                BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                BorderButtonKey.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

                ButtonAnimationColor.To = Background;
                BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                BorderButtonKey.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

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
                        BorderButton.BorderBrush = new SolidColorBrush(SettingAnimate.BorderBrushDNSU.Used);
                        BorderButton.Background = new SolidColorBrush(SettingAnimate.BackgroundDNSU.Used);
                        TextBlockKey.Foreground = new SolidColorBrush(SettingAnimate.ForegroundDNSU.Used);
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
            ButtonAnimationColor.To = SettingAnimate.BorderBrushDNSU.Select;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SettingAnimate.BackgroundDNSU.Select;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            BorderButtonKey.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SettingAnimate.ForegroundDNSU.Select;
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
            ButtonAnimationColor.To = SettingAnimate.BorderBrushDNSU.Default;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SettingAnimate.BackgroundDNSU.Default;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            BorderButtonKey.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SettingAnimate.ForegroundDNSU.Default;
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
            ButtonAnimationColor.To = EnterButton ? SettingAnimate.BorderBrushDNSU.Select : SettingAnimate.BorderBrushDNSU.Default;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            BorderButtonKey.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.From = Colors.White;
            ButtonAnimationColor.To = EnterButton ? SettingAnimate.BackgroundDNSU.Select : SettingAnimate.BackgroundDNSU.Default;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            BorderButtonKey.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.From = Colors.White;
            ButtonAnimationColor.To = EnterButton ? SettingAnimate.ForegroundDNSU.Select : SettingAnimate.ForegroundDNSU.Default;
            TextBlockKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.SpeedRatio = 1;
            ButtonAnimationColor.From = null;
        }
    }
}
