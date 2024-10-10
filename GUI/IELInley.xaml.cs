using IEL.Classes;
using IEL.Interfaces.Core;
using IEL.Interfaces.Front;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using static IEL.Interfaces.Front.IIELButton;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELInley.xaml
    /// </summary>
    public partial class IELInley : UserControl, IIELInley
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
        /// Скругление границ
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => BorderMain.CornerRadius;
            set => BorderMain.CornerRadius = value;
        }

        /// <summary>
        /// Толщина границ
        /// </summary>
        public Thickness BorderThicknessBlock
        {
            get => BorderMain.BorderThickness;
            set => BorderMain.BorderThickness = value;
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
        /// Страница заголовка
        /// </summary>
        public IPageDefault? Page { get; private set; }

        /// <summary>
        /// Объект страницы
        /// </summary>
        internal new object? Content { get; private set; }

        /// <summary>
        /// Шрифт текста в кнопке
        /// </summary>
        public new FontFamily FontFamily
        {
            get => base.FontFamily;
            set
            {
                TextBlockHead.FontFamily = value;
                base.FontFamily = value;
            }
        }

        /// <summary>
        /// Состояние анимирования текста подписи если он выходит за границы
        /// </summary>
        private bool IsAnimatedSignatureText = false;

        private string _TextSignature = string.Empty;
        /// <summary>
        /// Текст подписи заголовка
        /// </summary>
        public string TextSignature
        {
            get => _TextSignature;
            set
            {
                bool AnimateStart = true;
                if ((value.Length > 0 && _TextSignature.Length == 0) ||
                    (value.Length == 0 && _TextSignature.Length > 0))
                {
                    if (value.Length > 0) TextBlockSignature.Text = value;
                    DoubleAnimation animation = AnimationDouble.Clone();
                    animation.To = value.Length > 0 ? 25 : 0;
                    animation.Duration = TimeSpan.FromMilliseconds(700d);
                    Storyboard storyboard = new();
                    storyboard.Children.Add(animation);
                    Storyboard.SetTarget(animation, SignatureRowColumn);
                    Storyboard.SetTargetProperty(animation, new PropertyPath("(RowDefinition.MaxHeight)"));
                    storyboard.Begin();
                    _TextSignature = value;
                    AnimateStart = false;
                }
                else
                {
                    _TextSignature = value;
                    TextBlockSignature.Text = value;
                }
                if (!IsAnimatedSignatureText && TextBlockSignature.ActualWidth >= BorderSignature.ActualWidth)
                    SignatureAnimateStart(AnimateStart);
                else if (IsAnimatedSignatureText) SignatureAnimateStop();
            }
        }

        /// <summary>
        /// Текст заголовка
        /// </summary>
        public string Text
        {
            get => TextBlockHead.Text;
            set => TextBlockHead.Text = value;
        }

        public IELInley()
        {
            InitializeComponent();
            StateVisualization = StateVisual.Default;
            Page = null;
            Content = null;

            AnimationMillisecond = 100;
            BackgroundChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == BrushSettingQ.StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == BrushSettingQ.StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                BorderMain.Background = color;
            };
            BorderBrushChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == BrushSettingQ.StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == BrushSettingQ.StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                BorderMain.BorderBrush = color;
                BorderLeftArrow.BorderBrush = color;
                BorderRightArrow.BorderBrush = color;
                BorderSignature.BorderBrush = color;
            };
            ForegroundChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == BrushSettingQ.StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == BrushSettingQ.StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                TextBlockLeftArrow.Foreground = color;
                TextBlockRightArrow.Foreground = color;
                TextBlockHead.Foreground = color;
                TextBlockSignature.Foreground = color;
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

            // this

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
                        //ClickDownAnimation();
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
                BorderMain.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                AnimationColor.To = Background;
                BorderMain.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                AnimationColor.To = Foreground;
                TextBlockSignature.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                TextBlockHead.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            };
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
            BorderMain.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            if (StateVisualization != StateVisual.Default)
            {
                if (StateVisualization == StateVisual.LeftArrow)
                    BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                else BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            }

            AnimationColor.To = Background;
            BorderMain.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Foreground;
            TextBlockSignature.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            TextBlockHead.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            if (StateVisualization != StateVisual.Default)
            {
                if (StateVisualization == StateVisual.LeftArrow)
                    TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                else TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            }
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

            if (StateVisualization != StateVisual.Default)
            {
                AnimationThickness.To = new(0);
                if (StateVisualization == StateVisual.LeftArrow)
                    BorderLeftArrow.BeginAnimation(MarginProperty, AnimationThickness);
                else BorderRightArrow.BeginAnimation(MarginProperty, AnimationThickness);
            }

            AnimationColor.To = BorderBrush;
            BorderMain.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            if (StateVisualization != StateVisual.Default)
            {
                if (StateVisualization == StateVisual.LeftArrow)
                    BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                else BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            }

            AnimationColor.To = Background;
            BorderMain.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Foreground;
            TextBlockSignature.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            TextBlockHead.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
        }

        /// <summary>
        /// Функция старта анимации подписи заголовка
        /// </summary>
        /// <param name="AnimateStart">Создать анимацию старта или нет</param>
        private void SignatureAnimateStart(bool AnimateStart)
        {
            if (TextBlockSignature.ActualWidth > BorderSignature.ActualWidth)
            {
                IsAnimatedSignatureText = true;
                if (AnimateStart)
                {
                    ThicknessAnimation animation = AnimationThickness.Clone();
                    animation.To = new(BorderSignature.ActualWidth, 0, 0, 0);
                    animation.FillBehavior = FillBehavior.Stop;
                    animation.Duration = TimeSpan.FromMilliseconds(600d);
                    animation.Completed += (sender, e) =>
                    {
                        if (TextBlockSignature.ActualWidth <= BorderSignature.ActualWidth) return;
                        TextBlockSignature.Text = _TextSignature;
                        Animate();
                    };
                    TextBlockSignature.BeginAnimation(MarginProperty, animation);
                }
                else Animate();
                void Animate()
                {
                    int Millisecond = 300 * TextBlockSignature.Text.Length;
                    ThicknessAnimation animationForever = AnimationThickness.Clone();
                    animationForever.EasingFunction = null;
                    animationForever.From = new(BorderSignature.ActualWidth, 0, 0, 0);
                    animationForever.To = new(-TextBlockSignature.ActualWidth, 0, 0, 0);
                    animationForever.RepeatBehavior = RepeatBehavior.Forever;
                    animationForever.Duration = TimeSpan.FromMilliseconds(Millisecond);
                    animationForever.FillBehavior = FillBehavior.HoldEnd;
                    TextBlockSignature.BeginAnimation(MarginProperty, animationForever);
                }
            }
        }

        /// <summary>
        /// Функция окончания анимации подписи заголовка
        /// </summary>
        private void SignatureAnimateStop()
        {
            if (IsAnimatedSignatureText)
            {
                IsAnimatedSignatureText = false;
                ThicknessAnimation animation = AnimationThickness.Clone();
                animation.To = new(0);
                animation.FillBehavior = FillBehavior.HoldEnd;
                animation.Duration = TimeSpan.FromMilliseconds(600d);
                TextBlockSignature.BeginAnimation(MarginProperty, animation);
            }
        }

        /// <summary>
        /// Установить вкладке объект страницы
        /// </summary>
        /// <param name="page">Объект страницы</param>
        internal void SetPage<T>(T page) where T : IPageDefault
        {
            Page = page;
            Content = Page;
        }
    }
}
