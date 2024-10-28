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
    public partial class IELInlay : UserControl, IIELInley
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

        private bool _IsAnimatedSignatureText = false;
        /// <summary>
        /// Состояние анимирования текста подписи если он выходит за границы
        /// </summary>
        public bool IsAnimatedSignatureText
        {
            get => _IsAnimatedSignatureText;
            set
            {
                if (value)
                {
                    if (_TextSignature.Length > 0) SignatureAnimateStart(true);
                    TextBlockSignature.TextTrimming = TextTrimming.None;
                }
                else
                {
                    SignatureAnimateStop();
                    TextBlockSignature.TextTrimming = TextTrimming.CharacterEllipsis;
                }
                _IsAnimatedSignatureText = value;
            }
        }

        private string _TextSignature = string.Empty;
        /// <summary>
        /// Текст подписи заголовка
        /// </summary>
        public string TextSignature
        {
            get => _TextSignature;
            set
            {
                bool AnimatedStart = true;
                if ((value.Length > 0 && _TextSignature.Length == 0) ||
                    (value.Length == 0 && _TextSignature.Length > 0) ||
                    (value.Length == 0 && SignatureRowColumn.MaxHeight > 0))
                {
                    if (value.Length > 0)
                    {
                        TextBlockSignature.Text = value;
                        AnimatedStart = false;
                    }
                    else if (IsAnimatedSignatureText) SignatureAnimateStop();
                    _TextSignature = value;
                    DoubleAnimation animation = AnimationDouble.Clone();
                    animation.To = value.Length > 0 ? 25 : 0;
                    animation.Duration = TimeSpan.FromMilliseconds(700d);
                    Storyboard storyboard = new();
                    storyboard.Children.Add(animation);
                    Storyboard.SetTarget(animation, SignatureRowColumn);
                    Storyboard.SetTargetProperty(animation, new PropertyPath("(RowDefinition.MaxHeight)"));
                    storyboard.Begin();
                }
                else
                {
                    _TextSignature = value;
                    TextBlockSignature.Text = value;
                }
                BorderSignature.UpdateLayout();
                if (IsAnimatedSignatureText && value.Length > 0)
                {
                    SignatureAnimateStart(AnimatedStart);
                }
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

        private bool _UsedState;
        /// <summary>
        /// Состояние использования
        /// </summary>
        public bool UsedState
        {
            get => _UsedState;
            set
            {
                if (_UsedState == value) return;
                _UsedState = value;
                int Offset = value ? 2 : -2;
                AnimationThickness.To = new(
                    BorderMain.BorderThickness.Left + Offset, BorderMain.BorderThickness.Top + Offset,
                    BorderMain.BorderThickness.Right + Offset, BorderMain.BorderThickness.Bottom + Offset);
                AnimationThickness.Duration = TimeSpan.FromMilliseconds(800d);
                BorderMain.BeginAnimation(BorderThicknessProperty, AnimationThickness);
                MouseLeaveAnimation();
            }
        }

        public IELInlay()
        {
            InitializeComponent();
            _UsedState = false;
            StateVisualizationButton = StateButton.Default;
            TextBlockSignature.TextTrimming = TextTrimming.CharacterEllipsis;
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
                BorderMain.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                AnimationColor.To = Background;
                BorderMain.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                AnimationColor.To = Foreground;
                TextBlockSignature.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                TextBlockHead.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            };
        }

        /// <summary>
        /// Анимировать нажатие (Down)
        /// </summary>
        private void ClickDownAnimation()
        {
            SolidColorBrush
                Foreground = new(ForegroundSetting.Used),
                Background = new(BackgroundSetting.Used),
                BorderBrush = new(BorderBrushSetting.Used);
            if (StateVisualizationButton != StateButton.Default)
            {
                (StateVisualizationButton == StateButton.LeftArrow ? TextBlockLeftArrow : TextBlockRightArrow)
                    .Foreground = Foreground;
                AnimationThickness.To = new(
                    StateVisualizationButton == StateButton.RightArrow ? 5 : 0, 0,
                    StateVisualizationButton == StateButton.LeftArrow ? 5 : 0, 0);
                (StateVisualizationButton == StateButton.LeftArrow ? BorderLeftArrow : BorderRightArrow)
                    .BeginAnimation(MarginProperty, AnimationThickness);
            }

            BorderMain.BorderBrush = BorderBrush;
            BorderSignature.BorderBrush = BorderBrush;
            BorderLeftArrow.BorderBrush = BorderBrush;
            BorderRightArrow.BorderBrush = BorderBrush;

            BorderMain.Background = Background;

            TextBlockHead.Foreground = Foreground;
            TextBlockSignature.Foreground = Foreground;
            TextBlockLeftArrow.Foreground = Foreground;
            TextBlockRightArrow.Foreground = Foreground;
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
            BorderMain.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            if (StateVisualizationButton != StateButton.Default)
            {
                if (StateVisualizationButton == StateButton.LeftArrow)
                    BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
                else BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            }

            AnimationColor.To = Background;
            BorderMain.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Foreground;
            TextBlockSignature.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            TextBlockHead.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            if (StateVisualizationButton != StateButton.Default)
            {
                if (StateVisualizationButton == StateButton.LeftArrow)
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
                Foreground = UsedState ? ForegroundSetting.Used : ForegroundSetting.Default,
                Background = UsedState ? BackgroundSetting.Used : BackgroundSetting.Default,
                BorderBrush = UsedState ? BorderBrushSetting.Used : BorderBrushSetting.Default;

            if (StateVisualizationButton != StateButton.Default)
            {
                AnimationThickness.To = new(0);
                if (StateVisualizationButton == StateButton.LeftArrow)
                    BorderLeftArrow.BeginAnimation(MarginProperty, AnimationThickness);
                else BorderRightArrow.BeginAnimation(MarginProperty, AnimationThickness);
            }

            AnimationColor.To = BorderBrush;
            BorderMain.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            if (StateVisualizationButton != StateButton.Default)
            {
                if (StateVisualizationButton == StateButton.LeftArrow)
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
            if (AnimateStart)
            {
                if (BorderSignature.ActualWidth == 0) TextBlockSignature.Opacity = 0d;
                ThicknessAnimation animation = AnimationThickness.Clone();
                animation.To = new(BorderSignature.ActualWidth, 0, 0, 0);
                animation.FillBehavior = FillBehavior.Stop;
                animation.Duration = TimeSpan.FromMilliseconds(600d);
                animation.Completed += (sender, e) =>
                {
                    if (IsAnimatedSignatureText)
                    {
                        TextBlockSignature.Opacity = 1d;
                        Animate();
                    }
                };
                TextBlockSignature.BeginAnimation(MarginProperty, animation);
            }
            else Animate();
            void Animate()
            {
                int Millisecond = 300 * TextBlockSignature.Text.Length;
                if (Millisecond < 2500) Millisecond = 2500;
                ThicknessAnimation animationForever = AnimationThickness.Clone();
                animationForever.EasingFunction = null;
                animationForever.From = new(BorderSignature.ActualWidth + 10, 0, 0, 0);
                animationForever.To = new(-TextBlockSignature.ActualWidth - 10, 0, 0, 0);
                animationForever.RepeatBehavior = RepeatBehavior.Forever;
                animationForever.Duration = TimeSpan.FromMilliseconds(Millisecond);
                animationForever.FillBehavior = FillBehavior.HoldEnd;
                TextBlockSignature.BeginAnimation(MarginProperty, animationForever);
            }
        }

        /// <summary>
        /// Функция окончания анимации подписи заголовка
        /// </summary>
        private void SignatureAnimateStop()
        {
            ThicknessAnimation animation = AnimationThickness.Clone();
            animation.To = new(0);
            animation.FillBehavior = FillBehavior.HoldEnd;
            animation.Duration = TimeSpan.FromMilliseconds(600d);
            TextBlockSignature.BeginAnimation(MarginProperty, animation);
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
