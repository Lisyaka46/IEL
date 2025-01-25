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
    public partial class IELImageButton : UserControl, IIELButtonDefault, IIELEventsVision
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
        /// Смещение контента в объекте
        /// </summary>
        public Thickness PaddingContent
        {
            get => BorderButton.Padding;
            set => BorderButton.Padding = value;
        }

        /// <summary>
        /// Объект события активации левым щелчком мыши
        /// </summary>
        public IIELButtonDefault.Activate? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации правым щелчком мыши
        /// </summary>
        public IIELButtonDefault.Activate? OnActivateMouseRight { get; set; }

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
        /// Инициализировать объект кнопки с изображением
        /// </summary>
        public IELImageButton()
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
                BorderRightArrow.Background = color;
                BorderLeftArrow.Background = color;
            };
            BorderBrushChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == BrushSettingQ.StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == BrushSettingQ.StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                BorderButton.BorderBrush = color;
                BorderRightArrow.BorderBrush = color;
                BorderLeftArrow.BorderBrush = color;
            };
            ForegroundChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == BrushSettingQ.StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == BrushSettingQ.StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                TextBlockLeftArrow.Foreground = color;
                TextBlockRightArrow.Foreground = color;
            };
            BackgroundSetting = new(BrushSettingQ.CreateStyle.Background);
            BorderBrushSetting = new(BrushSettingQ.CreateStyle.BorderBrush);
            ForegroundSetting = new(BrushSettingQ.CreateStyle.Foreground);

            IntervalHover = 1300d;
            ImageMouseButtonsUse.Opacity = 0d;
            TimerBorderInfo.Tick += (sender, e) =>
            {
                MouseHover?.Invoke(this, e);
                TimerBorderInfo.Stop();
            };

            BorderButton.MouseEnter += (sender, e) =>
            {
                if (IsEnabled)
                {
                    EnterButton = true;
                    MouseEnterDetect();
                    TimerBorderInfo.Start();
                }
            };
            BorderButton.MouseLeave += (sender, e) =>
            {
                if (IsEnabled)
                {
                    EnterButton = false;
                    MouseLeaveDetect();
                    TimerBorderInfo.Stop();
                }
            };

            IsEnabledChanged += (sender, e) =>
            {
                EnterButton = false;
                Color
                    Background = (bool)e.NewValue ? BackgroundSetting.Default : BackgroundSetting.NotEnabled,
                    BorderBrush = (bool)e.NewValue ? BorderBrushSetting.Default : BorderBrushSetting.NotEnabled,
                    Foreground = (bool)e.NewValue ? ForegroundSetting.Default : BackgroundSetting.NotEnabled;

                AnimationColor.To = BorderBrush;
                BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                AnimationColor.To = Background;
                BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                BorderRightArrow.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                BorderLeftArrow.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                AnimationColor.To = Foreground;
                TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            };

            MouseLeftButtonDown += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseLeft != null)
                {
                    ClickDownAnimation();
                    TimerBorderInfo.Stop();
                }
            };

            MouseRightButtonDown += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    ClickDownAnimation();
                    TimerBorderInfo.Stop();
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
            SolidColorBrush
                Background = new(BackgroundSetting.Used),
                BorderBrush = new(BorderBrushSetting.Used),
                Foreground = new(ForegroundSetting.Used);

            BorderButton.BorderBrush = BorderBrush;
            BorderRightArrow.BorderBrush = BorderBrush;
            BorderLeftArrow.BorderBrush = BorderBrush;

            BorderButton.Background = Background;
            BorderRightArrow.Background = Background;
            BorderLeftArrow.Background = Background;

            TextBlockLeftArrow.Foreground = Foreground;
            TextBlockRightArrow.Foreground = Foreground;
        }

        /// <summary>
        /// Событие прихода курсора в видимую область кнопки
        /// </summary>
        private void MouseEnterDetect()
        {
            Color
                Background = BackgroundSetting.Select,
                BorderBrush = BorderBrushSetting.Select,
                Foreground = ForegroundSetting.Select;

            AnimationColor.To = BorderBrush;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Background;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            BorderRightArrow.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            BorderLeftArrow.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Foreground;
            TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            UpdateVisibleMouseEvents();
        }

        /// <summary>
        /// Событие ухода курсора из видимой области кнопки
        /// </summary>
        private void MouseLeaveDetect()
        {
            AnimationColor.To = BorderBrushSetting.Default;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = BackgroundSetting.Default;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            BorderRightArrow.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            BorderLeftArrow.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = ForegroundSetting.Default;
            TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationDouble.To = 0d;
            ImageMouseButtonsUse.BeginAnimation(OpacityProperty, AnimationDouble);
        }
    }
}
