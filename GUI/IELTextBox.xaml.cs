using IEL.Classes;
using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static IEL.Interfaces.Front.IIELButton;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELTextBox.xaml
    /// </summary>
    public partial class IELTextBox : UserControl, IIELTextBox
    {
        /// <summary>
        /// Цвет текста наименования
        /// </summary>
        public Brush ForgroundNaming
        {
            get => TextBlockNaming.Foreground;
            set => TextBlockNaming.Foreground = value;
        }

        /// <summary>
        /// Цвет фона наименования
        /// </summary>
        public Brush BackgroundNaming
        {
            get => TextBlockNaming.Background;
            set => TextBlockNaming.Background = value;
        }

        /// <summary>
        /// Скруглённость границ объекта
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => TextBoxBorder.CornerRadius;
            set => TextBoxBorder.CornerRadius = value;
        }

        /// <summary>
        /// Толщина границ объекта
        /// </summary>
        public Thickness BorderThicknessBlock
        {
            get => TextBoxBorder.BorderThickness;
            set => TextBoxBorder.BorderThickness = value;
        }

        /// <summary>
        /// Смещение контента наименования
        /// </summary>
        public Thickness PaddingNaming
        {
            get => TextBlockNaming.Padding;
            set => TextBlockNaming.Padding = value;
        }

        /// <summary>
        /// Вертикальная ориентация контента элемента
        /// </summary>
        public VerticalAlignment TextBoxVerticalAlignment
        {
            get => TextBoxMain.VerticalAlignment;
            set => TextBoxMain.VerticalAlignment = value;
        }

        /// <summary>
        /// Горизонтальная ориентация контента элемента
        /// </summary>
        public HorizontalAlignment TextBoxHorizontalAlignment
        {
            get => TextBoxMain.HorizontalAlignment;
            set => TextBoxMain.HorizontalAlignment = value;
        }

        /// <summary>
        /// Размер текста в элементе
        /// </summary>
        public new double FontSize
        {
            get => TextBoxMain.FontSize;
            set => TextBoxMain.FontSize = value;
        }

        /// <summary>
        /// Максимальный горизонтальный размер элемента текста
        /// </summary>
        public new double MaxWidth
        {
            get => TextBoxMain.MaxWidth;
            set => TextBoxMain.MaxWidth = value;
        }

        /// <summary>
        /// Максимальный вертикальный размер элемента текста
        /// </summary>
        public new double MaxHeight
        {
            get => TextBoxMain.MaxHeight;
            set => TextBoxMain.MaxHeight = value;
        }

        /// <summary>
        /// Максимальное значение линий
        /// </summary>
        public int MaxLines
        {
            get => TextBoxMain.MaxLines;
            set => TextBoxMain.MaxLines = value;
        }

        /// <summary>
        /// Минимальное значение линий
        /// </summary>
        public int MinLines
        {
            get => TextBoxMain.MinLines;
            set => TextBoxMain.MinLines = value;
        }

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
                ButtonAnimationColor.Duration = time;
                _AnimationMillisecond = value;

            }
        }
        #endregion



        #region animateObjects
        /// <summary>
        /// Анимация цвета
        /// </summary>
        private readonly ColorAnimation ButtonAnimationColor;

        /// <summary>
        /// Объект анимации для управления double значением
        /// </summary>
        private static readonly DoubleAnimation DoubleAnimateObj = new(0, TimeSpan.FromMilliseconds(250d))
        {
            DecelerationRatio = 0.2d,
            EasingFunction = new QuinticEase() { EasingMode = EasingMode.EaseOut }
        };
        #endregion

        /// <summary>
        /// Текст элемента
        /// </summary>
        public string Text
        {
            get => TextBoxMain.Text;
            set
            {
                TextBoxMain.Text = value;
                TextBoxMain.SelectionStart = value.Length;
            }
        }

        /// <summary>
        /// Текст наименования
        /// </summary>
        public string TextName
        {
            get => TextBlockNaming.Text;
            set => TextBlockNaming.Text = value;
        }

        private bool _ShowNameText;
        /// <summary>
        /// Состояние видимости текста наименования
        /// </summary>
        public bool ShowNameText
        {
            get => _ShowNameText;
            set
            {
                DoubleAnimateObj.To = value ? 1d : 0d;
                TextBlockNaming.BeginAnimation(OpacityProperty, DoubleAnimateObj);
                _ShowNameText = value;
            }
        }

        /// <summary>
        /// Переопределённый объект фона
        /// </summary>
        public new Brush Background
        {
            get => TextBoxBorder.Background;
            set => TextBoxBorder.Background = value;
        }

        /// <summary>
        /// Сделать фокус на элементе
        /// </summary>
        public new void Focus()
        {
            TextBoxMain.Focus();
        }

        /// <summary>
        /// Событие нажатия клавиши
        /// </summary>
        public new event KeyEventHandler? KeyDown
        {
            add => TextBoxMain.KeyDown += value;
            remove => TextBoxMain.KeyDown -= value;
        }

        /// <summary>
        /// Событие отпускания клавиши
        /// </summary>
        public new event KeyEventHandler? KeyUp
        {
            add => TextBoxMain.KeyUp += value;
            remove => TextBoxMain.KeyUp -= value;
        }

        /// <summary>
        /// Событие изменения текста
        /// </summary>
        public event TextChangedEventHandler? TextChanged
        {
            add => TextBoxMain.TextChanged += value;
            remove => TextBoxMain.TextChanged -= value;
        }

        /// <summary>
        /// Фокусировка текста
        /// </summary>
        public bool IsFocus { get; private set; } = false;

        public IELTextBox()
        {
            InitializeComponent();
            TextName = string.Empty;
            ShowNameText = false;
            TextBoxMain.ContextMenu = null;
            MaxLines = 1;

            ButtonAnimationColor = new();
            AnimationMillisecond = 100;
            BackgroundChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == BrushSettingQ.StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == BrushSettingQ.StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                TextBoxBorder.Background = color;
            };
            BorderBrushChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == BrushSettingQ.StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == BrushSettingQ.StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                TextBoxBorder.BorderBrush = color;
            };
            ForegroundChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == BrushSettingQ.StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == BrushSettingQ.StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                TextBoxMain.Foreground = color;
            };
            BackgroundSetting = new(BrushSettingQ.CreateStyle.Background);
            BorderBrushSetting = new(BrushSettingQ.CreateStyle.BorderBrush);
            ForegroundSetting = new(BrushSettingQ.CreateStyle.Foreground);


            GotKeyboardFocus += (sender, e) =>
            {
                FocusAnimation();
            };
            LostKeyboardFocus += (sender, e) =>
            {
                IsFocus = false;
                MouseLeaveAnimation();
            };

            MouseEnter += (sender, e) =>
            {
                if (IsEnabled && !IsFocus) MouseEnterAnimation();
            };

            MouseLeave += (sender, e) =>
            {
                if (IsEnabled && !IsFocus) MouseLeaveAnimation();
            };

            IsEnabledChanged += (sender, e) =>
            {
                Color
                Foreground = (bool)e.NewValue ? ForegroundSetting.Default : ForegroundSetting.NotEnabled,
                Background = (bool)e.NewValue ? BackgroundSetting.Default : BackgroundSetting.NotEnabled,
                BorderBrush = (bool)e.NewValue ? BorderBrushSetting.Default : BorderBrushSetting.NotEnabled;

                ButtonAnimationColor.To = BorderBrush;
                TextBoxBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                ButtonAnimationColor.To = Background;
                TextBoxBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
                ButtonAnimationColor.To = Foreground;
                TextBoxMain.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            };

            MouseDown += (sender, e) =>
            {
                TextBoxMain.Focus();
            };
        }

        /// <summary>
        /// Анимировать назначение фокуса на элемент
        /// </summary>
        private void FocusAnimation()
        {
            Color
                Foreground = ForegroundSetting.Used,
                Background = BackgroundSetting.Used,
                BorderBrush = BorderBrushSetting.Used;

            IsFocus = true;
            ButtonAnimationColor.Duration = TimeSpan.FromMilliseconds(_AnimationMillisecond * 2);
            ButtonAnimationColor.To = BorderBrush;
            TextBoxBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            ButtonAnimationColor.To = Background;
            TextBoxBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            ButtonAnimationColor.To = Foreground;
            TextBoxMain.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
            ButtonAnimationColor.Duration = TimeSpan.FromMilliseconds(_AnimationMillisecond);
        }

        /// <summary>
        /// Анимация выделения мышью
        /// </summary>
        private void MouseEnterAnimation()
        {
            Color
                Foreground = ForegroundSetting.Select,
                Background = BackgroundSetting.Select,
                BorderBrush = BorderBrushSetting.Select;

            ButtonAnimationColor.To = BorderBrush;
            TextBoxBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = Background;
            TextBoxBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = Foreground;
            TextBoxMain.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
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

            ButtonAnimationColor.To = BorderBrush;
            TextBoxBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = Background;
            TextBoxBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);

            ButtonAnimationColor.To = Foreground;
            TextBoxMain.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ButtonAnimationColor);
        }
    }
}
