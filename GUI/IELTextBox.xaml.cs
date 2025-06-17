using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static IEL.Interfaces.Front.IIELButton;
using static IEL.Interfaces.Core.IQData;
using IEL.CORE.Classes;
using IEL.CORE.Classes.ObjectSettings;
using IEL.CORE.Enums;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELTextBox.xaml
    /// </summary>
    public partial class IELTextBox : UserControl
    {

        private IELObjectSetting _IELSettingObject = new();
        /// <summary>
        /// Настройка использования объекта
        /// </summary>
        public IELObjectSetting IELSettingObject
        {
            get => _IELSettingObject;
            set
            {
                value.BackgroundQChanged += (NewValue) =>
                {
                    SolidColorBrush color = new(NewValue);
                    TextBoxBorder.Background = color;
                };
                value.BorderBrushQChanged += (NewValue) =>
                {
                    SolidColorBrush color = new(NewValue);
                    TextBoxBorder.BorderBrush = color;
                };
                value.ForegroundQChanged += (NewValue) =>
                {
                    SolidColorBrush color = new(NewValue);
                    TextBoxMain.Foreground = color;
                };
                _IELSettingObject = value;
                _IELSettingObject.UseActiveQSetting();
            }
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
        public double FontSizeText
        {
            get => TextBoxMain.FontSize;
            set
            {
                TextBoxMain.FontSize = value;
            }
        }

        /// <summary>
        /// Доступна или нет печать в объекте
        /// </summary>
        public bool IsReadOnly
        {
            get => TextBoxMain.IsReadOnly;
            set => TextBoxMain.IsReadOnly = value;
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
            IELSettingObject = new();

            TextBoxMain.ContextMenu = null;
            MaxLines = 1;

            GotKeyboardFocus += (sender, e) =>
            {
                IsFocus = true;
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
                bool NewValue = (bool)e.NewValue;
                Color
                    Foreground = NewValue ? IELSettingObject.ForegroundSetting.Default : IELSettingObject.ForegroundSetting.NotEnabled,
                    Background = NewValue ? IELSettingObject.BackgroundSetting.Default : IELSettingObject.BackgroundSetting.NotEnabled,
                    BorderBrush = NewValue ? IELSettingObject.BorderBrushSetting.Default : IELSettingObject.BorderBrushSetting.NotEnabled;
                ColorAnimation animation;

                animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background);
                TextBoxBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

                animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush);
                TextBoxBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

                animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground);
                TextBoxMain.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
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
                Foreground = IELSettingObject.ForegroundSetting.Used,
                Background = IELSettingObject.BackgroundSetting.Used,
                BorderBrush = IELSettingObject.BorderBrushSetting.Used;
            ColorAnimation animation;

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background);
            TextBoxBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush);
            TextBoxBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground);
            TextBoxMain.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }

        /// <summary>
        /// Анимация выделения мышью
        /// </summary>
        private void MouseEnterAnimation()
        {
            Color
                Foreground = IELSettingObject.ForegroundSetting.Select,
                Background = IELSettingObject.BackgroundSetting.Select,
                BorderBrush = IELSettingObject.BorderBrushSetting.Select;
            ColorAnimation animation;

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background);
            TextBoxBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush);
            TextBoxBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground);
            TextBoxMain.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }

        /// <summary>
        /// Анимация отключения выделения мышью
        /// </summary>
        private void MouseLeaveAnimation()
        {
            Color
                Foreground = IELSettingObject.ForegroundSetting.Default,
                Background = IELSettingObject.BackgroundSetting.Default,
                BorderBrush = IELSettingObject.BorderBrushSetting.Default;
            ColorAnimation animation;

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background);
            TextBoxBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush);
            TextBoxBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground);
            TextBoxMain.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }
    }
}
