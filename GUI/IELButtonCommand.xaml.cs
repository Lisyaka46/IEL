using IEL.Classes;
using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELButtonCommand.xaml
    /// </summary>
    public partial class IELButtonCommand : UserControl, IIELButtonDefault
    {    
        /// <summary>
        /// Перечисление стилей цвета нажатия на кнопку
        /// </summary>
        private enum ActivateClickColor
        {
            /// <summary>
            /// Обычный цвет нажатия на кнопку
            /// </summary>
            Clicked = 0,

            /// <summary>
            /// Отключённый цвет нажатия на кнопку
            /// </summary>
            IsNotEnabled = 1
        }

        /// <summary>
        /// Обект настройки поведения анимации цвета
        /// </summary>
        public IELSettingAnimate SettingAnimate { get; set; }

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
        /// Анимация цвета кнопки
        /// </summary>
        private readonly ColorAnimation ButtonAnimationColor;

        /// <summary>
        /// Анимация позиции стрелок кнопки
        /// </summary>
        private readonly ThicknessAnimation ButtonAnimationThickness;

        /// <summary>
        /// Анимация прозрачности для символа клавиатуры
        /// </summary>
        private readonly DoubleAnimation ButtonAnimationOpacity;

        /// <summary>
        /// Активация действия кнопки
        /// </summary>
        private bool ButtonActivate = false;

        /// <summary>
        /// Состояние выделения кнопки
        /// </summary>
        private bool EnterButton = false;

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

            TimeSpan AnimTime = TimeSpan.FromMilliseconds(80d);
            BrushSettingDNSU BackgroundDNSU = new(Color.FromRgb(172, 238, 255), Color.FromRgb(101, 193, 241), Colors.White, Colors.IndianRed);
            BrushSettingDNSU BorderBrushDNSU = new(Color.FromRgb(105, 71, 101), Color.FromRgb(158, 130, 155), Color.FromRgb(136, 93, 130), Colors.Brown);
            BrushSettingDNSU ForegroundDNSU = new(Colors.Black, Color.FromRgb(28, 33, 32), Color.FromRgb(0, 49, 34), Colors.DarkRed);

            BackgroundDNSU.SpectrumChange += (Spectrum, Value) =>
            {
                switch (Spectrum)
                {
                    case BrushSettingDNSU.SpectrumElement.Default:
                        if (!IsEnabled || EnterButton || ButtonActivate) return;
                        BorderButton.Background = new SolidColorBrush(Value);
                        break;
                    case BrushSettingDNSU.SpectrumElement.Select:
                        if (!IsEnabled || !EnterButton || ButtonActivate) return;
                        BorderButton.Background = new SolidColorBrush(Value);
                        break;
                    case BrushSettingDNSU.SpectrumElement.Used:
                        if (!IsEnabled || !EnterButton || !ButtonActivate) return;
                        BorderButton.Background = new SolidColorBrush(Value);
                        break;
                    case BrushSettingDNSU.SpectrumElement.NotEnabled:
                        if (IsEnabled || EnterButton || ButtonActivate) return;
                        BorderButton.Background = new SolidColorBrush(Value);
                        break;
                }  
            };

            BrushSettingDNSU.ChangeSpectrumEventHandler ChangeBackground = (Element, Value) => BorderButton.Background = new SolidColorBrush(Value);
            BrushSettingDNSU.ChangeSpectrumEventHandler ChangeBorderBrush = (Element, Value) => BorderButton.BorderBrush = new SolidColorBrush(Value);
            BrushSettingDNSU.ChangeSpectrumEventHandler ChangeForeground = (Element, Value) =>
            {
                Brush brush = new SolidColorBrush(Value);
                TextBlockButtonName.Foreground = brush;
                TextBlockButtonCommand.Foreground = brush;
                TextBlockNumberCommand.Foreground = brush;
            };
            SettingAnimate = new(
                new BrushSettingDNSU(Color.FromRgb(172, 238, 255), Color.FromRgb(101, 193, 241), Colors.White, Colors.IndianRed),
                new BrushSettingDNSU(Color.FromRgb(105, 71, 101), Color.FromRgb(158, 130, 155), Color.FromRgb(136, 93, 130), Colors.Brown),
                new BrushSettingDNSU(Colors.Black, Color.FromRgb(28, 33, 32), Color.FromRgb(0, 49, 34), Colors.DarkRed)
                );

            ButtonAnimationOpacity = new()
            {
                Duration = AnimTime
            };
            ButtonAnimationThickness = new()
            {
                Duration = AnimTime
            };
            ButtonAnimationColor = new()
            {
                Duration = AnimTime
            };
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
            ButtonAnimationOpacity.To = 1;
            BeginAnimation(OpacityProperty, ButtonAnimationOpacity);
            Index = indexBuffer;
            IntervalHover = 1300d;
            TimerBorderInfo.Tick += (sender, e) =>
            {
                MouseHover?.Invoke(this, e);
                TimerBorderInfo.Stop();
            };

            MouseEnter += (sender, e) =>
            {
                EnterButton = true;
                MouseEnterAnimation();
            };

            MouseLeave += (sender, e) =>
            {
                EnterButton = false;
                ButtonActivate = false;
                MouseLeaveAnimation();
            };

            MouseDown += (sender, e) =>
            {
                ButtonActivate = true;
                ClickDownAnimation(ActivateClickColor.Clicked);
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
                    Foreground = (bool)e.NewValue ? SettingAnimate.ForegroundDNSU.Default : SettingAnimate.ForegroundDNSU.NotEnabled,
                    Background = (bool)e.NewValue ? SettingAnimate.BackgroundDNSU.Default : SettingAnimate.BackgroundDNSU.NotEnabled,
                    BorderBrush = (bool)e.NewValue ? SettingAnimate.BorderBrushDNSU.Default : SettingAnimate.BorderBrushDNSU.NotEnabled;
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
        /// <param name="StyleClickColor">Стиль нажатия на кнопку</param>
        private void ClickDownAnimation(ActivateClickColor StyleClickColor)
        {
            Color
                Foreground = StyleClickColor == ActivateClickColor.Clicked ? SettingAnimate.ForegroundDNSU.Used : SettingAnimate.ForegroundDNSU.NotEnabled,
                Background = StyleClickColor == ActivateClickColor.Clicked ? SettingAnimate.BackgroundDNSU.Used : SettingAnimate.BackgroundDNSU.NotEnabled,
                BorderBrush = StyleClickColor == ActivateClickColor.Clicked ? SettingAnimate.BorderBrushDNSU.Used : SettingAnimate.BorderBrushDNSU.NotEnabled;
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

            ButtonAnimationColor.To = SettingAnimate.BorderBrushDNSU.Select;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SettingAnimate.BackgroundDNSU.Select;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SettingAnimate.ForegroundDNSU.Select;
            TextBlockButtonName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            TextBlockButtonCommand.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
        }

        /// <summary>
        /// Анимация отключения выделения мышью
        /// </summary>
        private void MouseLeaveAnimation()
        {
            ButtonAnimationColor.To = SettingAnimate.BorderBrushDNSU.Default;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SettingAnimate.BackgroundDNSU.Default;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SettingAnimate.ForegroundDNSU.Default;
            TextBlockButtonName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            TextBlockButtonCommand.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
        }

        /// <summary>
        /// Анимация мерцания
        /// </summary>
        public void BlinkAnimation()
        {
            ButtonAnimationColor.From = SettingAnimate.BorderBrushDNSU.Used;
            ButtonAnimationColor.To = SettingAnimate.BorderBrushDNSU.Default;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.From = SettingAnimate.BackgroundDNSU.Used;
            ButtonAnimationColor.To = SettingAnimate.BackgroundDNSU.Default;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.From = SettingAnimate.ForegroundDNSU.Used;
            ButtonAnimationColor.To = SettingAnimate.ForegroundDNSU.Default;
            TextBlockButtonName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            TextBlockButtonCommand.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
        }
    }
}
