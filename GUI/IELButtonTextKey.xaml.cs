using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELButtonTextKey.xaml
    /// </summary>
    public partial class IELButtonTextKey : UserControl, IIELButtonKey
    {
        /// <summary>
        /// Перечисление состояний отображения кнопки
        /// </summary>
        public enum StateButton
        {
            /// <summary>
            /// Обычное отображение кнопки
            /// </summary>
            Default = 0,

            /// <summary>
            /// Отображение кнопки с левосторонней стрелкой
            /// </summary>
            LeftArrow = 1,

            /// <summary>
            /// Отображение кнопки с правосторонней стрелкой
            /// </summary>
            RightArrow = 2,
        }

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

        private Color? _DefaultBorderBrush;
        /// <summary>
        /// Цвет границы кнопки
        /// </summary>
        public Color DefaultBorderBrush
        {
            get => _DefaultBorderBrush ?? Colors.Gold;
            set
            {
                SolidColorBrush color = new(value);
                BorderButton.BorderBrush = color;
                BorderLeftArrow.BorderBrush = color;
                BorderRightArrow.BorderBrush = color;
                BorderCharKeyboard.BorderBrush = color;
                _DefaultBorderBrush = value;
            }
        }

        private Color? _DefaultBackground;
        /// <summary>
        /// Цвет фона кнопки
        /// </summary>
        public Color DefaultBackground
        {
            get => _DefaultBackground ?? Colors.Gold;
            set
            {
                SolidColorBrush color = new(value);
                BorderButton.Background = color;
                BorderCharKeyboard.Background = color;
                _DefaultBackground = value;
            }
        }

        private Color? _DefaultForeground;
        /// <summary>
        /// Цвет текста в кнопке
        /// </summary>
        public Color DefaultForeground
        {
            get => _DefaultForeground ?? Colors.Gold;
            set
            {
                SolidColorBrush color = new(value);
                TextBlockButton.Foreground = color;
                TextBlockLeftArrow.Foreground = color;
                TextBlockRightArrow.Foreground = color;
                TextBlockCharKey.Foreground = color;
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
                ButtonAnimationColor.Duration = time;
                ButtonAnimationThickness.Duration = time;
                ButtonAnimationOpacity.Duration = time;
                _AnimationMillisecond = value;

            }
        }
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

        private bool _CharKeyboardActivate = false;
        /// <summary>
        /// Активность видимости символа действия активации кнопки
        /// </summary>
        public bool CharKeyboardActivate
        {
            get => _CharKeyboardActivate;
            set
            {
                ButtonAnimationOpacity.To = value ? 1d : 0d;
                ButtonAnimationThickness.To = new(!value ? -24 : 0, 0, 0, 0);
                BorderButton.BeginAnimation(MarginProperty, ButtonAnimationThickness);
                BorderCharKeyboard.BeginAnimation(OpacityProperty, ButtonAnimationOpacity);
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

        #region animateObjects
        /// <summary>
        /// Анимация цвета кнопки
        /// </summary>
        private readonly ColorAnimation ButtonAnimationColor;

        /// <summary>
        /// Анимация позиции
        /// </summary>
        private readonly ThicknessAnimation ButtonAnimationThickness;

        /// <summary>
        /// Анимация прозрачности
        /// </summary>
        private readonly DoubleAnimation ButtonAnimationOpacity;
        #endregion

        /// <summary>
        /// Объект события активации кнопки левым щелчком мыши
        /// </summary>
        public IIELButtonKey.Activate? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации кнопки правым щелчком мыши
        /// </summary>
        public IIELButtonKey.Activate? OnActivateMouseRight { get; set; }

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
                    ButtonAnimationOpacity.To = value ? 0.4d : 0d;
                    ImageMouseButtonsUse.BeginAnimation(OpacityProperty, ButtonAnimationOpacity);
                }
            }
        }

        /// <summary>
        /// Состояние активности наведения на кнопку
        /// </summary>
        private bool EnterButton = false;

        public IELButtonTextKey()
        {
            InitializeComponent();
            StateVisualizationButton = StateButton.Default;

            ButtonAnimationOpacity = new();
            ButtonAnimationThickness = new();
            ButtonAnimationColor = new();
            AnimationMillisecond = 80;
            IntervalHover = 1300d;
            TimerBorderInfo.Tick += (sender, e) =>
            {
                MouseHover?.Invoke(this, e);
                TimerBorderInfo.Stop();
            };

            BorderButton.Margin = new(-24, 0, 0, 0);
            BorderCharKeyboard.Opacity = 0d;
            ImageMouseButtonsUse.Opacity = 0d;
            TextFontFamily = new FontFamily("Arial");
            TextFontSize = 12;
            Text = "Text";
            CornerRadius = new CornerRadius(10);

            DefaultBorderBrush = Colors.Black;
            SelectBorderBrush = Colors.DarkGray;
            ClickedBorderBrush = Colors.Gray;
            NotEnabledBorderBrush = Colors.Brown;

            DefaultBackground = Colors.White;
            SelectBackground = Colors.Gray;
            ClickedBackground = Colors.WhiteSmoke;
            NotEnabledBackground = Colors.IndianRed;

            DefaultForeground = Colors.Black;
            SelectForeground = Colors.DarkGray;
            ClickedForeground = Colors.Gray;
            NotEnabledForeground = Colors.DarkRed;

            MouseEnter += (sender, e) =>
            {
                EnterButton = true;
                if (IsEnabled) MouseEnterAnimation();
            };

            MouseLeave += (sender, e) =>
            {
                EnterButton = false;
                if (IsEnabled) MouseLeaveAnimation();
            };

            MouseDown += (sender, e) =>
            {
                if (IsEnabled)
                {
                    if (e.LeftButton == MouseButtonState.Pressed && OnActivateMouseLeft != null) ClickDownAnimation(ActivateClickColor.Clicked);
                    else if (e.RightButton == MouseButtonState.Pressed && OnActivateMouseRight != null) ClickDownAnimation(ActivateClickColor.Clicked);
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
                Foreground = (bool)e.NewValue ? DefaultForeground : NotEnabledForeground,
                Background = (bool)e.NewValue ? DefaultBackground : NotEnabledBackground,
                BorderBrush = (bool)e.NewValue ? DefaultBorderBrush : NotEnabledBorderBrush;
                if (StateVisualizationButton != StateButton.Default)
                {
                    if (StateVisualizationButton == StateButton.LeftArrow)
                    {
                        ButtonAnimationColor.To = Foreground;
                        TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                        BorderLeftArrow.BeginAnimation(MarginProperty, null);
                    }
                    else
                    {
                        ButtonAnimationColor.To = Foreground;
                        TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                        BorderRightArrow.BeginAnimation(MarginProperty, null);
                    }
                }
                ButtonAnimationColor.To = BorderBrush;
                BorderCharKeyboard.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                ButtonAnimationColor.To = Background;
                BorderCharKeyboard.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                ButtonAnimationColor.To = Foreground;
                TextBlockCharKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

                ButtonAnimationColor.To = BorderBrush;
                BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                ButtonAnimationColor.To = Background;
                BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                ButtonAnimationColor.To = Foreground;
                TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

                ButtonAnimationOpacity.To = 0d;
                ImageMouseButtonsUse.BeginAnimation(OpacityProperty, ButtonAnimationOpacity);
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
            if (StateVisualizationButton != StateButton.Default)
            {
                (StateVisualizationButton == StateButton.LeftArrow ? TextBlockLeftArrow : TextBlockRightArrow)
                    .Foreground = new SolidColorBrush(Foreground);
                ButtonAnimationThickness.To = new(
                    StateVisualizationButton == StateButton.RightArrow ? 5 : 0, 0,
                    StateVisualizationButton == StateButton.LeftArrow ? 5 : 0, 0);
                (StateVisualizationButton == StateButton.LeftArrow ? BorderLeftArrow : BorderRightArrow)
                    .BeginAnimation(MarginProperty, ButtonAnimationThickness);
            }
            BorderCharKeyboard.BorderBrush = new SolidColorBrush(BorderBrush);
            BorderCharKeyboard.Background = new SolidColorBrush(Background);
            TextBlockCharKey.Foreground = new SolidColorBrush(Foreground);

            BorderButton.BorderBrush = new SolidColorBrush(BorderBrush);
            BorderButton.Background = new SolidColorBrush(Background);
            TextBlockButton.Foreground = new SolidColorBrush(Foreground);
        }

        /// <summary>
        /// Анимация выделения кнопки мышью
        /// </summary>
        private void MouseEnterAnimation()
        {
            if (StateVisualizationButton != StateButton.Default)
            {
                ButtonAnimationThickness.To = new(
                    StateVisualizationButton == StateButton.RightArrow ? -3 : 0,
                    0,
                    StateVisualizationButton == StateButton.LeftArrow ? -3 : 0,
                    0);
                if (StateVisualizationButton == StateButton.LeftArrow)
                    BorderLeftArrow.BeginAnimation(MarginProperty, ButtonAnimationThickness);
                else BorderRightArrow.BeginAnimation(MarginProperty, ButtonAnimationThickness);
            }

            ButtonAnimationColor.To = SelectBorderBrush;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            if (StateVisualizationButton != StateButton.Default)
            {
                if (StateVisualizationButton == StateButton.LeftArrow)
                    BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                else BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            }
            BorderCharKeyboard.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SelectBackground;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            BorderCharKeyboard.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SelectForeground;
            TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            if (StateVisualizationButton != StateButton.Default)
            {
                if (StateVisualizationButton == StateButton.LeftArrow)
                    TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                else TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            }
            TextBlockCharKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            if (VisibleMouseImaging)
            {
                ImageMouse = IIELObject.ImageMouseButton(OnActivateMouseLeft != null, OnActivateMouseRight != null);
                if (ImageMouse != null)
                {
                    ButtonAnimationOpacity.To = 0.4d;
                    ImageMouseButtonsUse.BeginInit();
                    ImageMouseButtonsUse.Source = ImageMouse;
                    ImageMouseButtonsUse.EndInit();
                    ImageMouseButtonsUse.BeginAnimation(OpacityProperty, ButtonAnimationOpacity);
                }
            }
        }

        /// <summary>
        /// Анимация отключения выделения мышью
        /// </summary>
        private void MouseLeaveAnimation()
        {
            if (StateVisualizationButton != StateButton.Default)
            {
                ButtonAnimationThickness.To = new(0);
                if (StateVisualizationButton == StateButton.LeftArrow)
                    BorderLeftArrow.BeginAnimation(MarginProperty, ButtonAnimationThickness);
                else BorderRightArrow.BeginAnimation(MarginProperty, ButtonAnimationThickness);
            }

            ButtonAnimationColor.To = DefaultBorderBrush;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            if (StateVisualizationButton != StateButton.Default)
            {
                if (StateVisualizationButton == StateButton.LeftArrow)
                    BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                else BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            }
            BorderCharKeyboard.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = DefaultBackground;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            BorderCharKeyboard.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = DefaultForeground;
            TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            TextBlockCharKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationOpacity.To = 0d;
            ImageMouseButtonsUse.BeginAnimation(OpacityProperty, ButtonAnimationOpacity);
        }

        /// <summary>
        /// Анимация мерцания
        /// </summary>
        [MTAThread()]
        public void BlinkAnimation()
        {
            ButtonAnimationColor.SpeedRatio = 0.6d;
            ButtonAnimationColor.From = Colors.White;
            ButtonAnimationColor.To = EnterButton ? SelectBorderBrush : DefaultBorderBrush;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            BorderCharKeyboard.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.From = Colors.White;
            ButtonAnimationColor.To = EnterButton ? SelectBackground : DefaultBackground;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            BorderCharKeyboard.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.From = Colors.White;
            ButtonAnimationColor.To = EnterButton ? SelectForeground : DefaultForeground;
            TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            TextBlockCharKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.SpeedRatio = 1;
            ButtonAnimationColor.From = null;
        }
    }
}
