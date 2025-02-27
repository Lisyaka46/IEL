using IEL.Classes;
using IEL.Interfaces.Front;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using static IEL.Interfaces.Front.IIELStateVisualizationButton;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELButtonTextKey.xaml
    /// </summary>
    public partial class IELButtonTextKey : UserControl, IIELButtonKey
    {
        #region StateVisualizationButton
        private StateButton _StateVisualizationButton = StateButton.LeftArrow;
        /// <summary>
        /// Состояние отображения направления
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
                AnimationThickness.Duration = TimeSpan.FromMilliseconds(value * 1.4d);
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

        #region ImagedEventsButton
        /// <summary>
        /// Изображение отображения событий нажатия при отсутствии возможности нажатия
        /// </summary>
        public ImageSource? NotEventImageMouse { get; set; }

        /// <summary>
        /// Изображение отображения событий нажатия только при левой возможности нажатия
        /// </summary>
        public ImageSource? OnlyLeftEventImageMouse { get; set; }

        /// <summary>
        /// Изображение отображения событий нажатия только при правой возможности нажатия
        /// </summary>
        public ImageSource? OnlyRightEventImageMouse { get; set; }

        /// <summary>
        /// Изображение отображения событий нажатия при двусторонней возможности нажатия
        /// </summary>
        public ImageSource? FullEventImageMouse { get; set; }
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
        public new FontFamily FontFamily
        {
            get => base.FontFamily;
            set
            {
                TextBlockCharKey.FontFamily = value;
                TextBlockButton.FontFamily = value;
                base.FontFamily = value;
            }
        }

        private bool _CharKeyboardActivate = false;
        /// <summary>
        /// Активность видимости символа действия активации кнопки
        /// </summary>
        public bool CharKeyboardActivate
        {
            get => _CharKeyboardActivate;
            set
            {
                AnimationDouble.To = value ? 1d : 0d;
                AnimationThickness.To = new(!value ? -24 : 0, 0, 0, 0);
                BorderButton.BeginAnimation(MarginProperty, AnimationThickness);
                BorderCharKeyboard.BeginAnimation(OpacityProperty, AnimationDouble);
                _CharKeyboardActivate = value;
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
                TextBlockCharKey.Text = IIELObject.KeyName(value).ToString();
            }
        }

        private IIELButtonKey.Activate? _OnActivateMouseLeft;
        /// <summary>
        /// Объект события активации левым щелчком мыши
        /// </summary>
        public IIELButtonKey.Activate? OnActivateMouseLeft
        {
            get => _OnActivateMouseLeft;
            set
            {
                _OnActivateMouseLeft = value;
                UpdateVisibleMouseEvents();
            }
        }

        private IIELButtonKey.Activate? _OnActivateMouseRight;
        /// <summary>
        /// Объект события активации правым щелчком мыши
        /// </summary>
        public IIELButtonKey.Activate? OnActivateMouseRight
        {
            get => _OnActivateMouseRight;
            set
            {
                _OnActivateMouseRight = value;
                UpdateVisibleMouseEvents();
            }
        }

        private bool _VisibleMouseImaging;
        /// <summary>
        /// Состояние активности отображения действий на кнопке
        /// </summary>
        /// <remarks>
        /// При включённом состоянии отображает изображение действий производимое над кнопкой.
        /// <code></code>
        /// <b>Изображение поменять нельзя.</b>
        /// </remarks>
        public bool VisibleMouseImaging
        {
            get => _VisibleMouseImaging;
            set
            {
                if (_VisibleMouseImaging != value) UpdateVisibleMouseEvents();
                _VisibleMouseImaging = value;
            }
        }

        /// <summary>
        /// Состояние активности наведения на кнопку
        /// </summary>
        private bool EnterButton = false;

        /// <summary>
        /// Смещение контента в объекте
        /// </summary>
        public Thickness PaddingContent
        {
            get => BorderButton.Padding;
            set => BorderButton.Padding = value;
        }

        public IELButtonTextKey()
        {
            InitializeComponent();
            StateVisualizationButton = StateButton.Default;

            AnimationMillisecond = 100;
            BackgroundChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == BrushSettingQ.StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == BrushSettingQ.StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                BorderButton.Background = color;
                BorderCharKeyboard.Background = color;
            };
            BorderBrushChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == BrushSettingQ.StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == BrushSettingQ.StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                BorderButton.BorderBrush = color;
                BorderCharKeyboard.BorderBrush = color;
                BorderLeftArrow.BorderBrush = color;
                BorderRightArrow.BorderBrush = color;
            };
            ForegroundChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == BrushSettingQ.StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == BrushSettingQ.StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                TextBlockCharKey.Foreground = color;
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

            BorderButton.Margin = new(-24, 0, 0, 0);
            BorderCharKeyboard.Opacity = 0d;
            ImageMouseButtonsUse.Opacity = 0d;
            Text = "Text";
            CornerRadius = new CornerRadius(10);

            MouseEnter += (sender, e) =>
            {
                if (IsEnabled)
                {
                    EnterButton = true;
                    MouseEnterAnimation();
                    TimerBorderInfo.Start();
                }
            };

            MouseLeave += (sender, e) =>
            {
                if (IsEnabled)
                {
                    EnterButton = false;
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
                    OnActivateMouseLeft?.Invoke(false);
                }
            };

            MouseRightButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    MouseEnterAnimation();
                    OnActivateMouseRight?.Invoke(false);
                }
            };

            IsEnabledChanged += (sender, e) =>
            {
                EnterButton = false;
                Color
                Foreground = (bool)e.NewValue ? ForegroundSetting.Default : ForegroundSetting.NotEnabled,
                Background = (bool)e.NewValue ? BackgroundSetting.Default : BackgroundSetting.NotEnabled,
                BorderBrush = (bool)e.NewValue ? BorderBrushSetting.Default : BorderBrushSetting.NotEnabled;
               
                AnimationColor.To = BorderBrush;
                BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

                BorderCharKeyboard.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                if (StateVisualizationButton == StateButton.LeftArrow)
                {
                    BorderLeftArrow.BeginAnimation(MarginProperty, null);
                    BorderLeftArrow.Margin = new(0);
                    BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

                    AnimationColor.To = Foreground;
                    TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                }
                else
                {
                    BorderRightArrow.BeginAnimation(MarginProperty, null);
                    BorderRightArrow.Margin = new(0);
                    BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

                    AnimationColor.To = Foreground;
                    TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                }


                AnimationColor.To = Background;
                BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

                BorderCharKeyboard.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

                AnimationColor.To = Foreground;
                TextBlockCharKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

                TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

                AnimationDouble.To = 0d;
                ImageMouseButtonsUse.BeginAnimation(OpacityProperty, AnimationDouble);
            };
        }

        /// <summary>
        /// Обновить видимость событий мыши
        /// </summary>
        public void UpdateVisibleMouseEvents()
        {
            if (_VisibleMouseImaging)
            {
                ImageMouseButtonsUse.Source = ((IIELEventsVision)this).ImageMouseButton(this);
                ImageMouseButtonsUse.UpdateLayout();
            }
            AnimationDouble.To = EnterButton ? 0.4d : 0d;
            ImageMouseButtonsUse.BeginAnimation(OpacityProperty, AnimationDouble);
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
            BorderCharKeyboard.BorderBrush = new SolidColorBrush(BorderBrush);
            BorderCharKeyboard.Background = new SolidColorBrush(Background);
            TextBlockCharKey.Foreground = new SolidColorBrush(Foreground);

            TextBlockLeftArrow.Foreground = new SolidColorBrush(Foreground);
            TextBlockRightArrow.Foreground = new SolidColorBrush(Foreground);

            BorderButton.BorderBrush = new SolidColorBrush(BorderBrush);
            BorderButton.Background = new SolidColorBrush(Background);
            TextBlockButton.Foreground = new SolidColorBrush(Foreground);

            BorderLeftArrow.BorderBrush = new SolidColorBrush(BorderBrush);

            BorderRightArrow.BorderBrush = new SolidColorBrush(BorderBrush);
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

            BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            BorderCharKeyboard.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Background;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            BorderCharKeyboard.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Foreground;
            TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            TextBlockCharKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            
            UpdateVisibleMouseEvents();
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
            if (StateVisualizationButton != StateButton.Default)
            {
                AnimationThickness.To = new(0);
                if (StateVisualizationButton == StateButton.LeftArrow)
                    BorderLeftArrow.BeginAnimation(MarginProperty, AnimationThickness);
                else BorderRightArrow.BeginAnimation(MarginProperty, AnimationThickness);
            }

            AnimationColor.To = BorderBrush;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            BorderCharKeyboard.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Background;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            BorderCharKeyboard.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Foreground;
            TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            TextBlockCharKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationDouble.To = 0d;
            ImageMouseButtonsUse.BeginAnimation(OpacityProperty, AnimationDouble);
        }

        /// <summary>
        /// Анимация мерцания
        /// </summary>
        [MTAThread()]
        public void BlinkAnimation()
        {
            AnimationColor.SpeedRatio = 0.6d;
            AnimationColor.From = BorderBrushSetting.Used;
            AnimationColor.To = EnterButton ? BorderBrushSetting.Select : BorderBrushSetting.Default;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            BorderCharKeyboard.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.From = BackgroundSetting.Used;
            AnimationColor.To = EnterButton ? BackgroundSetting.Select : BackgroundSetting.Default;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            BorderCharKeyboard.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.From = ForegroundSetting.Used;
            AnimationColor.To = EnterButton ? ForegroundSetting.Select : ForegroundSetting.Default;
            TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            TextBlockCharKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.SpeedRatio = 1;
            AnimationColor.From = null;
        }
    }
}
