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
    /// Логика взаимодействия для IELButtonCommand.xaml
    /// </summary>
    public partial class IELButtonCommand : UserControl, IIELObject
    {
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
                TextBlockButtonCommand.Foreground = color;
                TextBlockButtonName.Foreground = color;
                TextBlockNumberCommand.Foreground = color;
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
            get => TextBlockButtonName.Text;
            set => TextBlockButtonName.Text = value;
        }

        /// <summary>
        /// Текст команды
        /// </summary>
        public string TextCommand
        {
            get => TextBlockButtonCommand.Text;
            set => TextBlockButtonCommand.Text = value;
        }

        private int _Index;
        /// <summary>
        /// Индекс элемента 
        /// </summary>
        public int Index
        {
            get => _Index;
            set
            {
                TextBlockNumberCommand.Text = $"#{value + 1}";
                _Index = value;
            }
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
        /// Шрифт текста в кнопке
        /// </summary>
        public FontFamily TextFontFamily
        {
            get => TextBlockButtonName.FontFamily;
            set => TextBlockButtonName.FontFamily = value;
        }

        /// <summary>
        /// Размер текста в кнопке
        /// </summary>
        public double TextFontSize
        {
            get => TextBlockButtonName.FontSize;
            set => TextBlockButtonName.FontSize = value;
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
        /// Активация действия кнопки
        /// </summary>
        private bool ButtonActivate = false;

        /// <summary>
        /// Объект события активации кнопки левым щелчком мыши
        /// </summary>
        public IIELButtonDefault.Activate? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации кнопки правым щелчком мыши
        /// </summary>
        public IIELButtonDefault.Activate? OnActivateMouseRight { get; set; }

        public IELButtonCommand(string Name, string FullTextCommand, int indexBuffer)
        {
            InitializeComponent();

            AnimationMillisecond = 100;
            BrushSettingSUN BackgroundSUN = new(BrushSettingSUN.CreateStyle.Background);
            BrushSettingSUN BorderBrushSUN = new(BrushSettingSUN.CreateStyle.BorderBrush);
            BrushSettingSUN ForegroundSUN = new(BrushSettingSUN.CreateStyle.Foreground);
            SettingAnimate = new(BackgroundSUN, BorderBrushSUN, ForegroundSUN);
            BorderBrushDefault = SettingAnimate.BorderBrushSUN.Default;
            BackgroundDefault = SettingAnimate.BackgroundSUN.Default;
            ForegroundDefault = SettingAnimate.ForegroundSUN.Default;

            TextFontFamily = new FontFamily("Arial");
            TextFontSize = 14;
            TextBlockButtonName.FontWeight = FontWeights.Bold;
            Text = Name;
            TextBlockButtonCommand.Text = FullTextCommand;
            CornerRadius = new CornerRadius(10);
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Top;
            Height = 27;
            Width = 230;
            BorderButton.CornerRadius = new CornerRadius(4);
            Opacity = 0;
            AnimationDouble.To = 1;
            BeginAnimation(OpacityProperty, AnimationDouble);
            Index = indexBuffer;
            IntervalHover = 1300d;
            TimerBorderInfo.Tick += (sender, e) =>
            {
                MouseHover?.Invoke(this, e);
                TimerBorderInfo.Stop();
            };

            MouseEnter += (sender, e) =>
            {
                MouseEnterAnimation();
            };

            MouseLeave += (sender, e) =>
            {
                ButtonActivate = false;
                MouseLeaveAnimation();
            };

            MouseDown += (sender, e) =>
            {
                if (IsEnabled)
                {
                    if (
                    (e.LeftButton == MouseButtonState.Pressed && OnActivateMouseLeft != null) ||
                    (e.RightButton == MouseButtonState.Pressed && OnActivateMouseRight != null))
                    {
                        ButtonActivate = true;
                        ClickDownAnimation();
                    }
                }
            };       

            MouseLeftButtonUp += (sender, e) =>
            {
                if (ButtonActivate)
                {
                    ButtonActivate = false;
                    MouseEnterAnimation();
                    OnActivateMouseLeft?.Invoke();
                }
            };

            MouseRightButtonUp += (sender, e) =>
            {
                if (ButtonActivate)
                {
                    ButtonActivate = false;
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
                BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, null);
                BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, null);
                TextBlockButtonName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, null);
                TextBlockButtonCommand.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, null);
                BorderButton.BorderBrush = new SolidColorBrush(BorderBrush);
                BorderButton.Background = new SolidColorBrush(Background);
                TextBlockButtonName.Foreground = new SolidColorBrush(Foreground);
                TextBlockButtonCommand.Foreground = new SolidColorBrush(Foreground);
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
            BorderButton.BorderBrush = new SolidColorBrush(BorderBrush);
            BorderButton.Background = new SolidColorBrush(Background);
            TextBlockButtonName.Foreground = new SolidColorBrush(Foreground);
            TextBlockButtonCommand.Foreground = new SolidColorBrush(Foreground);
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
            AnimationColor.To = BorderBrush;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Background;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Foreground;
            TextBlockButtonName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            TextBlockButtonCommand.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
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
            AnimationColor.To = BorderBrush;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Background;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Foreground;
            TextBlockButtonName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            TextBlockButtonCommand.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
        }
    }
}
