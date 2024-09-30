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

        private Color _DefaultBorderBrush;
        /// <summary>
        /// Цвет границы кнопки
        /// </summary>
        public Color DefaultBorderBrush
        {
            get => _DefaultBorderBrush;
            set
            {
                SolidColorBrush color = new(value);
                BorderButton.BorderBrush = color;
                _DefaultBorderBrush = value;
            }
        }

        private Color _DefaultBackground;
        /// <summary>
        /// Цвет фона кнопки
        /// </summary>
        public Color DefaultBackground
        {
            get => _DefaultBackground;
            set
            {
                SolidColorBrush color = new(value);
                BorderButton.Background = color;
                _DefaultBackground = value;
            }
        }

        private Color _DefaultForeground;
        /// <summary>
        /// Цвет текста в кнопке
        /// </summary>
        public Color DefaultForeground
        {
            get => _DefaultForeground;
            set
            {
                SolidColorBrush color = new(value);
                TextBlockButtonName.Foreground = color;
                TextBlockButtonCommand.Foreground = color;
                _DefaultForeground = value;
            }
        }

        /// <summary>
        /// Выделенный цвет границы кнопки
        /// </summary>
        public Color SelectBorderBrush { get; set; }

        /// <summary>
        /// Выделенный цвет фона кнопки
        /// </summary>
        public Color SelectBackground { get; set; }

        /// <summary>
        /// Выделенный цвет текста в кнопке
        /// </summary>
        public Color SelectForeground { get; set; }

        /// <summary>
        /// Нажатый цвет границы кнопки
        /// </summary>
        public Color ClickedBorderBrush { get; set; }

        /// <summary>
        /// Нажатый цвет фона кнопки
        /// </summary>
        public Color ClickedBackground { get; set; }

        /// <summary>
        /// Нажатый цвет текста в кнопке
        /// </summary>
        public Color ClickedForeground { get; set; }

        /// <summary>
        /// Выключенный цвет границы кнопки
        /// </summary>
        public Color NotEnabledBorderBrush { get; set; }

        /// <summary>
        /// Выключенный цвет фона кнопки
        /// </summary>
        public Color NotEnabledBackground { get; set; }

        /// <summary>
        /// Выключенный цвет текста в кнопке
        /// </summary>
        public Color NotEnabledForeground { get; set; }

        private int _AnimationMillisecond = 80;
        /// <summary>
        /// Количество миллисекунд для анимации (по умолчанию 80)
        /// </summary>
        public int AnimationMillisecond
        {
            get => _AnimationMillisecond;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Значение должно быть больше нуля!");
                else
                {
                    TimeSpan time = TimeSpan.FromMilliseconds(value);
                    ButtonAnimationColor.Duration = time;
                    ButtonAnimationThickness.Duration = time;
                    ButtonAnimationOpacity.Duration = time;
                    _AnimationMillisecond = value;
                }
            }
        }

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
            ButtonAnimationOpacity = new()
            {
                Duration = TimeSpan.FromMilliseconds(AnimationMillisecond)
            };
            ButtonAnimationThickness = new()
            {
                Duration = TimeSpan.FromMilliseconds(AnimationMillisecond)
            };
            ButtonAnimationColor = new()
            {
                Duration = TimeSpan.FromMilliseconds(AnimationMillisecond)
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

            DefaultBackground = Color.FromRgb(172, 238, 255);
            SelectBackground = Color.FromRgb(101, 193, 241);
            ClickedBackground = Colors.White;
            NotEnabledBackground = Colors.IndianRed;

            DefaultBorderBrush = Color.FromRgb(105, 71, 101);
            SelectBorderBrush = Color.FromRgb(158, 130, 155);
            ClickedBorderBrush = Color.FromRgb(136, 93, 130);
            NotEnabledBorderBrush = Colors.Brown;

            DefaultForeground = Colors.Black;
            SelectForeground = Color.FromRgb(28, 33, 32);
            ClickedForeground = Color.FromRgb(0, 49, 34);
            NotEnabledForeground = Colors.DarkRed;

            MouseEnter += (sender, e) => MouseEnterAnimation();

            MouseLeave += (sender, e) =>
            {
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
                Foreground = (bool)e.NewValue ? DefaultForeground : NotEnabledForeground,
                Background = (bool)e.NewValue ? DefaultBackground : NotEnabledBackground,
                BorderBrush = (bool)e.NewValue ? DefaultBorderBrush : NotEnabledBorderBrush;
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
            Foreground = StyleClickColor == ActivateClickColor.Clicked ? ClickedForeground : NotEnabledForeground,
            Background = StyleClickColor == ActivateClickColor.Clicked ? ClickedBackground : NotEnabledBackground,
            BorderBrush = StyleClickColor == ActivateClickColor.Clicked ? ClickedBorderBrush : NotEnabledBorderBrush;
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

            ButtonAnimationColor.To = SelectBorderBrush;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SelectBackground;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SelectForeground;
            TextBlockButtonName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            TextBlockButtonCommand.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
        }

        /// <summary>
        /// Анимация отключения выделения мышью
        /// </summary>
        private void MouseLeaveAnimation()
        {
            ButtonAnimationColor.To = DefaultBorderBrush;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = DefaultBackground;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = DefaultForeground;
            TextBlockButtonName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            TextBlockButtonCommand.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
        }

        /// <summary>
        /// Анимация мерцания
        /// </summary>
        public void BlinkAnimation()
        {
            ButtonAnimationColor.From = ClickedBorderBrush;
            ButtonAnimationColor.To = DefaultBorderBrush;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.From = ClickedBackground;
            ButtonAnimationColor.To = DefaultBackground;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.From = ClickedForeground;
            ButtonAnimationColor.To = DefaultForeground;
            TextBlockButtonName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            TextBlockButtonCommand.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
        }
    }
}
