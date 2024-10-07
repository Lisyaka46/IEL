using IEL.Classes;
using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using static IEL.Interfaces.Front.IIELButton;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELImageButton.xaml
    /// </summary>
    public partial class IELImageButton : UserControl, IIELButtonDefault
    {
        private StateButton _StateVisualizationButton = StateButton.LeftArrow;
        /// <summary>
        /// Состояние отображения кнопки
        /// </summary>
        public StateButton StateVisualizationButton
        {
            get => _StateVisualizationButton;
            set
            {
                if (_StateVisualizationButton == value) return;
                ColumnLeftArrow.Width = new(value == StateButton.LeftArrow ? 25 : 0);
                ColumnRightArrow.Width = new(value == StateButton.RightArrow ? 25 : 0);
                BorderLeftArrow.Opacity = value == StateButton.LeftArrow ? 1d : 0d;
                BorderRightArrow.Opacity = value == StateButton.RightArrow ? 1d : 0d;
                _StateVisualizationButton = value;
            }
        }

        #region Default
        /// <summary>
        /// Цвет обычного состояния фона
        /// </summary>
        public Color BackgroundDefault
        {
            get => SettingAnimate.BackgroundSUN.Default;
            set
            {
                SolidColorBrush color = new(value);
                BorderButton.Background = color;
                SettingAnimate.BackgroundSUN.Default = value;
            }
        }

        /// <summary>
        /// Цвет обычного состояния границы
        /// </summary>
        public Color BorderBrushDefault
        {
            get => SettingAnimate.BorderBrushSUN.Default;
            set
            {
                SolidColorBrush color = new(value);
                BorderButton.BorderBrush = color;
                SettingAnimate.BorderBrushSUN.Default = value;
            }
        }

        /// <summary>
        /// Цвет обычного состояния текста
        /// </summary>
        public Color ForegroundDefault
        {
            get => SettingAnimate.ForegroundSUN.Default;
            set
            {
                SolidColorBrush color = new(value);
                TextBlockLeftArrow.Foreground = color;
                TextBlockRightArrow.Foreground = color;
                SettingAnimate.ForegroundSUN.Default = value;
            }
        }
        #endregion

        #region SettingAnimate
        private IELSettingAnimate _SettingAnimate = new();
        /// <summary>
        /// Обект настройки поведения анимации цвета
        /// </summary>
        public IELSettingAnimate SettingAnimate
        {
            get => _SettingAnimate;
            set
            {
                BorderBrushDefault = value.BorderBrushSUN.Default;
                BackgroundDefault = value.BackgroundSUN.Default;
                ForegroundDefault = value.ForegroundSUN.Default;
                _SettingAnimate = value;
            }
        }
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
            StateVisualizationButton = StateButton.Default;

            AnimationMillisecond = 100;
            BrushSettingSUN BackgroundSUN = new(BrushSettingSUN.CreateStyle.Background);
            BrushSettingSUN BorderBrushSUN = new(BrushSettingSUN.CreateStyle.BorderBrush);
            BrushSettingSUN ForegroundSUN = new(BrushSettingSUN.CreateStyle.Foreground);
            SettingAnimate = new(BackgroundSUN, BorderBrushSUN, ForegroundSUN);

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
                    Background = (bool)e.NewValue ? BackgroundDefault : SettingAnimate.BackgroundSUN.NotEnabled,
                    BorderBrush = (bool)e.NewValue ? BorderBrushDefault : SettingAnimate.BorderBrushSUN.NotEnabled,
                    Foreground = (bool)e.NewValue ? ForegroundDefault : SettingAnimate.BackgroundSUN.NotEnabled;

                AnimationColor.To = BorderBrush;
                BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                AnimationColor.To = Background;
                BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                AnimationColor.To = Foreground;
                TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
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
                Background = SettingAnimate.BackgroundSUN.Used,
                BorderBrush = SettingAnimate.BorderBrushSUN.Used,
                Foreground = SettingAnimate.ForegroundSUN.Used;

            BorderButton.BorderBrush = new SolidColorBrush(BorderBrush);
            BorderButton.Background = new SolidColorBrush(Background);
            TextBlockLeftArrow.Foreground = new SolidColorBrush(Foreground);
            TextBlockRightArrow.Foreground = new SolidColorBrush(Foreground);
        }

        /// <summary>
        /// Событие прихода курсора в видимую область кнопки
        /// </summary>
        private void MouseEnterDetect()
        {
            Color
                Background = SettingAnimate.BackgroundSUN.Select,
                BorderBrush = SettingAnimate.BorderBrushSUN.Select,
                Foreground = SettingAnimate.ForegroundSUN.Select;

            AnimationColor.To = BorderBrush;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Background;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Foreground;
            TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
        }

        /// <summary>
        /// Событие ухода курсора из видимой области кнопки
        /// </summary>
        private void MouseLeaveDetect()
        {
            AnimationColor.To = BorderBrushDefault;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = BackgroundDefault;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = ForegroundDefault;
            TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
        }
    }
}
