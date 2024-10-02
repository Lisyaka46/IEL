using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using IEL.Classes;
using IEL.Interfaces.Front;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELButtonText.xaml
    /// </summary>
    public partial class IELButtonText : UserControl, IIELButtonDefault
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

        /// <summary>
        /// Обект настройки поведения анимации цвета
        /// </summary>
        public IELSettingAnimate SettingAnimate { get; set; }

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
                    ButtonAnimationOpacity.To = value ? 0.4d : 0d;
                    ImageMouseButtonsUse.BeginAnimation(OpacityProperty, ButtonAnimationOpacity);
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

            ButtonAnimationOpacity = new();
            ButtonAnimationThickness = new();
            ButtonAnimationColor = new();

            TimeSpan AnimTime = TimeSpan.FromMilliseconds(80d);
            BrushSettingDNSU.ChangeSpectrumEventHandler ChangeBackground = (Element, Value) => BorderButton.Background = new SolidColorBrush(Value);
            BrushSettingDNSU.ChangeSpectrumEventHandler ChangeBorderBrush = (Element, Value) => BorderButton.BorderBrush = new SolidColorBrush(Value);
            BrushSettingDNSU.ChangeSpectrumEventHandler ChangeForeground = (Element, Value) =>
            {
                Brush brush = new SolidColorBrush(Value);
                TextBlockButton.Foreground = brush;
                TextBlockLeftArrow.Foreground = brush;
                TextBlockRightArrow.Foreground = brush;
            };
            SettingAnimate = new(
                new BrushSettingDNSU(Color.FromRgb(172, 238, 255), Color.FromRgb(101, 193, 241), Colors.White, Colors.IndianRed, AnimTime, ChangeBackground),
                new BrushSettingDNSU(Color.FromRgb(105, 71, 101), Color.FromRgb(158, 130, 155), Color.FromRgb(136, 93, 130), Colors.Brown, AnimTime, ChangeBorderBrush),
                new BrushSettingDNSU(Colors.Black, Color.FromRgb(28, 33, 32), Color.FromRgb(0, 49, 34), Colors.DarkRed, AnimTime, ChangeForeground)
                );

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
                    if (e.LeftButton == MouseButtonState.Pressed && OnActivateMouseLeft != null) ClickDownAnimation(ActivateClickColor.Clicked);
                    else if (e.RightButton == MouseButtonState.Pressed && OnActivateMouseRight != null) ClickDownAnimation(ActivateClickColor.Clicked);
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
                Foreground = (bool)e.NewValue ? SettingAnimate.ForegroundDNSU.Default : SettingAnimate.ForegroundDNSU.NotEnabled,
                Background = (bool)e.NewValue ? SettingAnimate.BackgroundDNSU.Default : SettingAnimate.BackgroundDNSU.NotEnabled,
                BorderBrush = (bool)e.NewValue ? SettingAnimate.BorderBrushDNSU.Default : SettingAnimate.BorderBrushDNSU.NotEnabled;
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
                Foreground = StyleClickColor == ActivateClickColor.Clicked ? SettingAnimate.ForegroundDNSU.Used : SettingAnimate.ForegroundDNSU.NotEnabled,
                Background = StyleClickColor == ActivateClickColor.Clicked ? SettingAnimate.BackgroundDNSU.Used : SettingAnimate.BackgroundDNSU.NotEnabled,
                BorderBrush = StyleClickColor == ActivateClickColor.Clicked ? SettingAnimate.BorderBrushDNSU.Used : SettingAnimate.BorderBrushDNSU.NotEnabled;
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

            ButtonAnimationColor.To = SettingAnimate.BorderBrushDNSU.Select;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            if (StateVisualizationButton != StateButton.Default)
            {
                if (StateVisualizationButton == StateButton.LeftArrow)
                    BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                else BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            }

            ButtonAnimationColor.To = SettingAnimate.BackgroundDNSU.Select;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SettingAnimate.ForegroundDNSU.Select;
            TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            if (StateVisualizationButton != StateButton.Default)
            {
                if (StateVisualizationButton == StateButton.LeftArrow)
                    TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                else TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            }

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
            EnterButton = true;
        }

        /// <summary>
        /// Анимация отключения выделения мышью
        /// </summary>
        private void MouseLeaveAnimation()
        {
            EnterButton = false;
            if (StateVisualizationButton != StateButton.Default)
            {
                ButtonAnimationThickness.To = new(0);
                if (StateVisualizationButton == StateButton.LeftArrow)
                    BorderLeftArrow.BeginAnimation(MarginProperty, ButtonAnimationThickness);
                else BorderRightArrow.BeginAnimation(MarginProperty, ButtonAnimationThickness);
            }

            ButtonAnimationColor.To = SettingAnimate.BorderBrushDNSU.Default;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            if (StateVisualizationButton != StateButton.Default)
            {
                if (StateVisualizationButton == StateButton.LeftArrow)
                    BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                else BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            }

            ButtonAnimationColor.To = SettingAnimate.BackgroundDNSU.Default;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = SettingAnimate.ForegroundDNSU.Default;
            TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

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
            ButtonAnimationColor.To = EnterButton ? SettingAnimate.BorderBrushDNSU.Select : SettingAnimate.BorderBrushDNSU.Default;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.From = Colors.White;
            ButtonAnimationColor.To = EnterButton ? SettingAnimate.BackgroundDNSU.Select : SettingAnimate.BackgroundDNSU.Default;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.From = Colors.White;
            ButtonAnimationColor.To = EnterButton ? SettingAnimate.ForegroundDNSU.Select : SettingAnimate.ForegroundDNSU.Default;
            TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.SpeedRatio = 1;
            ButtonAnimationColor.From = null;
        }
    }
}
