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
        #region StateVisualization
        private StateVisual _StateVisualization = StateVisual.LeftArrow;
        /// <summary>
        /// Состояние отображения направления
        /// </summary>
        public StateVisual StateVisualization
        {
            get => _StateVisualization;
            set
            {
                if (_StateVisualization == value) return;
                ColumnLeftArrow.Width = new(value == StateVisual.LeftArrow ? 25 : 0);
                ColumnRightArrow.Width = new(value == StateVisual.RightArrow ? 25 : 0);
                BorderLeftArrow.Opacity = value == StateVisual.LeftArrow ? 1d : 0d;
                BorderRightArrow.Opacity = value == StateVisual.RightArrow ? 1d : 0d;
                _StateVisualization = value;
            }
        }
        #endregion

        #region Color Setting
        private BrushSettingQ? _BackgroundSetting;
        /// <summary>
        /// Объект обычного состояния фона
        /// </summary>
        public BrushSettingQ BackgroundSetting
        {
            get => _BackgroundSetting ?? new();
            set
            {
                BackgroundChangeDefaultColor.Invoke(BrushSettingQ.StateSpectrum.Default, value.Default);
                value.ColorDefaultChange += BackgroundChangeDefaultColor;
                _BackgroundSetting = value;
            }
        }

        private BrushSettingQ? _BorderBrushSetting;
        /// <summary>
        /// Объект обычного состояния границы
        /// </summary>
        public BrushSettingQ BorderBrushSetting
        {
            get => _BorderBrushSetting ?? new();
            set
            {
                BorderBrushChangeDefaultColor.Invoke(BrushSettingQ.StateSpectrum.Default, value.Default);
                value.ColorDefaultChange += BorderBrushChangeDefaultColor;
                _BorderBrushSetting = value;
            }
        }

        private BrushSettingQ? _ForegroundSetting;
        /// <summary>
        /// Объект обычного состояния текста
        /// </summary>
        public BrushSettingQ ForegroundSetting
        {
            get => _ForegroundSetting ?? new();
            set
            {
                ForegroundChangeDefaultColor.Invoke(BrushSettingQ.StateSpectrum.Default, value.Default);
                value.ColorDefaultChange += ForegroundChangeDefaultColor;
                _ForegroundSetting = value;
            }
        }

        #region Event Change Color
        /// <summary>
        /// Обект события изменения цвета обычного состояния фона
        /// </summary>
        private readonly BrushSettingQ.ColorDefaultChangeEventHandler BackgroundChangeDefaultColor;

        /// <summary>
        /// Обект события изменения цвета обычного состояния границы
        /// </summary>
        private readonly BrushSettingQ.ColorDefaultChangeEventHandler BorderBrushChangeDefaultColor;

        /// <summary>
        /// Обект события изменения цвета обычного состояния текста
        /// </summary>
        private readonly BrushSettingQ.ColorDefaultChangeEventHandler ForegroundChangeDefaultColor;
        #endregion
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
        /// Скругление границ (по умолчанию 10, 10, 10, 10)
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
        /// Текст кнопки
        /// </summary>
        public string Text
        {
            get => TextBlockButton.Text;
            set => TextBlockButton.Text = value;
        }

        /// <summary>
        /// Шрифт текста в кнопке
        /// </summary>
        public new FontFamily FontFamily
        {
            get => base.FontFamily;
            set
            {
                TextBlockButton.FontFamily = value;
                base.FontFamily = value;
            }
        }

        /// <summary>
        /// Объект события активации левым щелчком мыши
        /// </summary>
        public IIELButtonDefault.Activate? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации правым щелчком мыши
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
            StateVisualization = StateVisual.Default;

            AnimationMillisecond = 100;
            BackgroundChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == BrushSettingQ.StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == BrushSettingQ.StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                BorderButton.Background = color;
            };
            BorderBrushChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == BrushSettingQ.StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == BrushSettingQ.StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                BorderButton.BorderBrush = color;
            };
            ForegroundChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == BrushSettingQ.StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == BrushSettingQ.StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                TextBlockButton.Foreground = color;
                TextBlockLeftArrow.Foreground = color;
                TextBlockRightArrow.Foreground = color;
            };
            BackgroundSetting = new(BrushSettingQ.CreateStyle.Background);
            BorderBrushSetting = new(BrushSettingQ.CreateStyle.BorderBrush);
            ForegroundSetting = new(BrushSettingQ.CreateStyle.Foreground);

            IntervalHover = 1300d;
            TimerBorderInfo.Tick += (sender, e) =>
            {
                MouseHover?.Invoke(this, e);
                TimerBorderInfo.Stop();
            };

            ImageMouseButtonsUse.Opacity = 0d;
            Text = "Text";
            CornerRadius = new CornerRadius(10);

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
                    if (
                    (e.LeftButton == MouseButtonState.Pressed && OnActivateMouseLeft != null) ||
                    (e.RightButton == MouseButtonState.Pressed && OnActivateMouseRight != null))
                    {
                        ClickDownAnimation();
                        TimerBorderInfo.Stop();
                    }
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
                Foreground = (bool)e.NewValue ? ForegroundSetting.Default : ForegroundSetting.NotEnabled,
                Background = (bool)e.NewValue ? BackgroundSetting.Default : BackgroundSetting.NotEnabled,
                BorderBrush = (bool)e.NewValue ? BorderBrushSetting.Default : BorderBrushSetting.NotEnabled;
                if (StateVisualization != StateVisual.Default)
                {
                    if (StateVisualization == StateVisual.LeftArrow)
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
                Foreground = ForegroundSetting.Used,
                Background = BackgroundSetting.Used,
                BorderBrush = BorderBrushSetting.Used;
            if (StateVisualization != StateVisual.Default)
            {
                (StateVisualization == StateVisual.LeftArrow ? TextBlockLeftArrow : TextBlockRightArrow)
                    .Foreground = new SolidColorBrush(Foreground);
                AnimationThickness.To = new(
                    StateVisualization == StateVisual.RightArrow ? 5 : 0, 0,
                    StateVisualization == StateVisual.LeftArrow ? 5 : 0, 0);
                (StateVisualization == StateVisual.LeftArrow ? BorderLeftArrow : BorderRightArrow)
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
                Foreground = ForegroundSetting.Select,
                Background = BackgroundSetting.Select,
                BorderBrush = BorderBrushSetting.Select;
            if (StateVisualization != StateVisual.Default)
            {
                AnimationThickness.To = new(
                    StateVisualization == StateVisual.RightArrow ? -3 : 0,
                    0,
                    StateVisualization == StateVisual.LeftArrow ? -3 : 0,
                    0);
                if (StateVisualization == StateVisual.LeftArrow)
                    BorderLeftArrow.BeginAnimation(MarginProperty, AnimationThickness);
                else BorderRightArrow.BeginAnimation(MarginProperty, AnimationThickness);
            }

            AnimationColor.To = BorderBrush;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            if (StateVisualization != StateVisual.Default)
            {
                if (StateVisualization == StateVisual.LeftArrow)
                    BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                else BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            }

            AnimationColor.To = Background;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Foreground;
            TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            if (StateVisualization != StateVisual.Default)
            {
                if (StateVisualization == StateVisual.LeftArrow)
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
                Foreground = ForegroundSetting.Default,
                Background = BackgroundSetting.Default,
                BorderBrush = BorderBrushSetting.Default;
            EnterButton = false;
            if (StateVisualization != StateVisual.Default)
            {
                AnimationThickness.To = new(0);
                if (StateVisualization == StateVisual.LeftArrow)
                    BorderLeftArrow.BeginAnimation(MarginProperty, AnimationThickness);
                else BorderRightArrow.BeginAnimation(MarginProperty, AnimationThickness);
            }

            AnimationColor.To = BorderBrush;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            if (StateVisualization != StateVisual.Default)
            {
                if (StateVisualization == StateVisual.LeftArrow)
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
