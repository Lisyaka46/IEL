using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using IEL.Classes;
using IEL.Interfaces.Front;
using static IEL.Interfaces.Front.IIELButton;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELButtonText.xaml
    /// </summary>
    public partial class IELButtonText : UserControl, IIELButtonDefault
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
                TextBlockButton.Foreground = color;
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
        /// Текст кнопки
        /// </summary>
        public string Text
        {
            get => TextBlockButton.Text;
            set => TextBlockButton.Text = value;
        }

        /// <summary>
        /// Скругление границ кнопки (по умолчанию 10, 10, 10, 10)
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => BorderButton.CornerRadius;
            set => BorderButton.CornerRadius = value;
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
        /// Шрифт текста в кнопке
        /// </summary>
        public FontFamily TextFontFamily
        {
            get => TextBlockButton.FontFamily;
            set => TextBlockButton.FontFamily = value;
        }

        /// <summary>
        /// Размер текста в кнопке
        /// </summary>
        public double TextFontSize
        {
            get => TextBlockButton.FontSize;
            set => TextBlockButton.FontSize = value;
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

        public IELButtonText()
        {
            InitializeComponent();
            StateVisualizationButton = StateButton.Default;

            AnimationMillisecond = 100;
            BrushSettingSUN BackgroundSUN = new(BrushSettingSUN.CreateStyle.Background);
            BrushSettingSUN BorderBrushSUN = new(BrushSettingSUN.CreateStyle.BorderBrush);
            BrushSettingSUN ForegroundSUN = new(BrushSettingSUN.CreateStyle.Foreground);
            SettingAnimate = new(BackgroundSUN, BorderBrushSUN, ForegroundSUN);

            IntervalHover = 1300d;
            TimerBorderInfo.Tick += (sender, e) =>
            {
                MouseHover?.Invoke(this, e);
                TimerBorderInfo.Stop();
            };

            ImageMouseButtonsUse.Opacity = 0d;
            TextFontFamily = new FontFamily("Arial");
            TextFontSize = 12;
            Text = "Text";
            CornerRadius = new CornerRadius(10);

            MouseEnter += (sender, e) =>
            {
                if (IsEnabled) MouseEnterAnimation();
            };

            MouseLeave += (sender, e) =>
            {
                if (IsEnabled) MouseLeaveAnimation();
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

            IsEnabledChanged += (sender, e) =>
            {
                Color
                Foreground = (bool)e.NewValue ? ForegroundDefault : SettingAnimate.ForegroundSUN.NotEnabled,
                Background = (bool)e.NewValue ? BackgroundDefault : SettingAnimate.BackgroundSUN.NotEnabled,
                BorderBrush = (bool)e.NewValue ? BorderBrushDefault : SettingAnimate.BorderBrushSUN.NotEnabled;
                if (StateVisualizationButton != StateButton.Default)
                {
                    if (StateVisualizationButton == StateButton.LeftArrow)
                    {
                        AnimationColor.To = Foreground;
                        TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                        BorderLeftArrow.BeginAnimation(MarginProperty, null);
                    }
                    else
                    {
                        AnimationColor.To = Foreground;
                        TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                        BorderRightArrow.BeginAnimation(MarginProperty, null);
                    }
                }
                AnimationColor.To = BorderBrush;
                BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                AnimationColor.To = Background;
                BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                AnimationColor.To = Foreground;
                TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

                AnimationDouble.To = 0d;
                ImageMouseButtonsUse.BeginAnimation(OpacityProperty, AnimationDouble);
            };
        }

        /// <summary>
        /// Анимировать нажатие на кнопку (Down)
        /// </summary>
        private void ClickDownAnimation()
        {
            Color
                Foreground = SettingAnimate.ForegroundSUN.Used,
                Background = SettingAnimate.BackgroundSUN.Used,
                BorderBrush = SettingAnimate.BorderBrushSUN.Used;
            if (StateVisualizationButton != StateButton.Default)
            {
                (StateVisualizationButton == StateButton.LeftArrow ? TextBlockLeftArrow : TextBlockRightArrow)
                    .Foreground = new SolidColorBrush(Foreground);
                AnimationThickness.To = new(
                    StateVisualizationButton == StateButton.RightArrow ? 5 : 0, 0,
                    StateVisualizationButton == StateButton.LeftArrow ? 5 : 0, 0);
                (StateVisualizationButton == StateButton.LeftArrow ? BorderLeftArrow : BorderRightArrow)
                    .BeginAnimation(MarginProperty, AnimationThickness);
            }

            BorderButton.BorderBrush = new SolidColorBrush(BorderBrush);
            BorderButton.Background = new SolidColorBrush(Background);
            TextBlockButton.Foreground = new SolidColorBrush(Foreground);
        }

        /// <summary>
        /// Анимация выделения кнопки мышью
        /// </summary>
        private void MouseEnterAnimation()
        {
            Color
                Foreground = SettingAnimate.ForegroundSUN.Select,
                Background = SettingAnimate.BackgroundSUN.Select,
                BorderBrush = SettingAnimate.BorderBrushSUN.Select;
            if (StateVisualizationButton != StateButton.Default)
            {
                AnimationThickness.To = new(
                    StateVisualizationButton == StateButton.RightArrow ? -3 : 0,
                    0,
                    StateVisualizationButton == StateButton.LeftArrow ? -3 : 0,
                    0);
                if (StateVisualizationButton == StateButton.LeftArrow)
                    BorderLeftArrow.BeginAnimation(MarginProperty, AnimationThickness);
                else BorderRightArrow.BeginAnimation(MarginProperty, AnimationThickness);
            }

            AnimationColor.To = BorderBrush;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            if (StateVisualizationButton != StateButton.Default)
            {
                if (StateVisualizationButton == StateButton.LeftArrow)
                    BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                else BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            }

            AnimationColor.To = Background;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Foreground;
            TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            if (StateVisualizationButton != StateButton.Default)
            {
                if (StateVisualizationButton == StateButton.LeftArrow)
                    TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                else TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            }

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
        /// Анимация отключения выделения мышью
        /// </summary>
        private void MouseLeaveAnimation()
        {
            Color
                Foreground = ForegroundDefault,
                Background = BackgroundDefault,
                BorderBrush = BorderBrushDefault;
            EnterButton = false;
            if (StateVisualizationButton != StateButton.Default)
            {
                AnimationThickness.To = new(0);
                if (StateVisualizationButton == StateButton.LeftArrow)
                    BorderLeftArrow.BeginAnimation(MarginProperty, AnimationThickness);
                else BorderRightArrow.BeginAnimation(MarginProperty, AnimationThickness);
            }

            AnimationColor.To = BorderBrush;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            if (StateVisualizationButton != StateButton.Default)
            {
                if (StateVisualizationButton == StateButton.LeftArrow)
                    BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                else BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            }

            AnimationColor.To = Background;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Foreground;
            TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationDouble.To = 0d;
            ImageMouseButtonsUse.BeginAnimation(OpacityProperty, AnimationDouble);
        }
    }
}
