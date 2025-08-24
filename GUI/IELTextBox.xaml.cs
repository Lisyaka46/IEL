using IEL.CORE.Classes.ObjectSettings;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using IEL.CORE.Enums;

namespace IEL.GUI
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
                value.BackgroundSetting.SetActionColorChanged((Spectrum, NewValue, Animated) =>
                {
                    if (Animated)
                    {
                        ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(NewValue);
                        TextBoxBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                    }
                    else
                    {
                        SolidColorBrush color = new(NewValue);
                        TextBoxBorder.Background = color;
                    }
                });
                value.BorderBrushSetting.SetActionColorChanged((Spectrum, NewValue, Animated) =>
                {
                    if (Animated)
                    {
                        ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(NewValue);
                        TextBoxBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                    }
                    else
                    {
                        SolidColorBrush color = new(NewValue);
                        TextBoxBorder.BorderBrush = color;
                    }
                });
                value.ForegroundSetting.SetActionColorChanged((Spectrum, NewValue, Animated) =>
                {
                    if (Animated)
                    {
                        ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(NewValue);
                        TextBoxMain.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                    }
                    else
                    {
                        SolidColorBrush color = new(NewValue);
                        TextBoxMain.Foreground = color;
                    }
                });
                _IELSettingObject = value;
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
                IELSettingObject.UseActiveQSetting(StateSpectrum.Used);
            };
            LostKeyboardFocus += (sender, e) =>
            {
                IsFocus = false;
                IELSettingObject.UseActiveQSetting(StateSpectrum.Default);
            };

            MouseEnter += (sender, e) =>
            {
                if (IsEnabled && !IsFocus) IELSettingObject.UseActiveQSetting(StateSpectrum.Select);
            };

            MouseLeave += (sender, e) =>
            {
                if (IsEnabled && !IsFocus) IELSettingObject.UseActiveQSetting(StateSpectrum.Default);
            };

            IsEnabledChanged += (sender, e) =>
            {
                bool NewValue = (bool)e.NewValue;
                IELSettingObject.UseActiveQSetting(NewValue ? StateSpectrum.Default : StateSpectrum.NotEnabled);
            };

            MouseDown += (sender, e) =>
            {
                TextBoxMain.Focus();
            };
        }
    }
}
