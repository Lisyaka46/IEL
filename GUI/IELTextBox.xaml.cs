using IEL.CORE.Classes;
using IEL.CORE.Classes.ObjectSettings;
using IEL.CORE.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELTextBox.xaml
    /// </summary>
    public partial class IELTextBox : UserControl
    {
        #region Color Setting
        /// <summary>
        /// Ресурсный объект настройки состояний фона
        /// </summary>
        private BrushSettingQ _Background;
        /// <summary>
        /// Объект настройки состояний фона
        /// </summary>
        public new BrushSettingQ Background
        {
            get => _Background;
            set
            {
                _Background.CloneSpectrumActionInObject(value, true);
                _Background = value;
            }
        }

        /// <summary>
        /// Ресурсный объект настройки состояний границы
        /// </summary>
        private BrushSettingQ _BorderBrush;
        /// <summary>
        /// Объект настройки состояний границы
        /// </summary>
        public new BrushSettingQ BorderBrush
        {
            get => _BorderBrush;
            set
            {
                _BorderBrush.CloneSpectrumActionInObject(value, true);
                _BorderBrush = value;
            }
        }

        /// <summary>
        /// Ресурсный объект настройки состояний текста
        /// </summary>
        private BrushSettingQ _Foreground;
        /// <summary>
        /// Объект настройки состояний текста
        /// </summary>
        public new BrushSettingQ Foreground
        {
            get => _Foreground;
            set
            {
                _Foreground.CloneSpectrumActionInObject(value, true);
                _Foreground = value;
            }
        }
        #endregion

        private IELObjectSetting _IELSettingObject = new();
        /// <summary>
        /// Настройка использования объекта
        /// </summary>
        public IELObjectSetting IELSettingObject
        {
            get => _IELSettingObject;
            set
            {
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

        /// <summary>
        /// Инициализировать объект интерфейса. Текстовый ввод
        /// </summary>
        public IELTextBox()
        {
            InitializeComponent();
            #region Background
            _Background = new();
            TextBoxBorder.Background = new SolidColorBrush(Background.ActiveSpectrumColor);
            Background.SetSpectrumAction((Args) =>
            {
                if (Args.AnimatedEvent)
                {
                    ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Args.Value);
                    TextBoxBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, anim, HandoffBehavior.SnapshotAndReplace);
                }
                else
                {
                    ((SolidColorBrush)TextBoxBorder.Background).Color = Args.Value;
                }
            });
            #endregion

            #region BorderBrush
            _BorderBrush = new();
            TextBoxBorder.BorderBrush = new SolidColorBrush(BorderBrush.ActiveSpectrumColor);
            BorderBrush.SetSpectrumAction((Args) =>
            {
                if (Args.AnimatedEvent)
                {
                    ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Args.Value);
                    TextBoxBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, anim, HandoffBehavior.SnapshotAndReplace);
                }
                else
                {
                    ((SolidColorBrush)TextBoxBorder.BorderBrush).Color = Args.Value;
                }
            });
            #endregion

            #region Foreground
            _Foreground = new();
            TextBoxMain.Foreground = new SolidColorBrush(Foreground.ActiveSpectrumColor);
            Foreground.SetSpectrumAction((Args) =>
            {
                if (Args.AnimatedEvent)
                {
                    ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Args.Value);
                    TextBoxMain.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, anim, HandoffBehavior.SnapshotAndReplace);
                }
                else
                {
                    ((SolidColorBrush)TextBoxMain.Foreground).Color = Args.Value;
                }
            });
            #endregion
            IELSettingObject = new();

            TextBoxMain.ContextMenu = null;
            MaxLines = 1;

            GotKeyboardFocus += (sender, e) =>
            {
                IsFocus = true;
                Background.SetActiveSpecrum(StateSpectrum.Used, true);
                BorderBrush.SetActiveSpecrum(StateSpectrum.Used, true);
                Foreground.SetActiveSpecrum(StateSpectrum.Used, true);
            };
            LostKeyboardFocus += (sender, e) =>
            {
                IsFocus = false;
                Background.SetActiveSpecrum(StateSpectrum.Default, true);
                BorderBrush.SetActiveSpecrum(StateSpectrum.Default, true);
                Foreground.SetActiveSpecrum(StateSpectrum.Default, true);
            };

            MouseEnter += (sender, e) =>
            {
                if (IsEnabled && !IsFocus)
                {
                    Background.SetActiveSpecrum(StateSpectrum.Select, true);
                    BorderBrush.SetActiveSpecrum(StateSpectrum.Select, true);
                    Foreground.SetActiveSpecrum(StateSpectrum.Select, true);
                }
            };

            MouseLeave += (sender, e) =>
            {
                if (IsEnabled && !IsFocus)
                {
                    Background.SetActiveSpecrum(StateSpectrum.Default, true);
                    BorderBrush.SetActiveSpecrum(StateSpectrum.Default, true);
                    Foreground.SetActiveSpecrum(StateSpectrum.Default, true);
                }
            };

            IsEnabledChanged += (sender, e) =>
            {
                StateSpectrum Value = (bool)e.NewValue ? StateSpectrum.Default : StateSpectrum.NotEnabled;
                Background.SetActiveSpecrum(Value, true);
                BorderBrush.SetActiveSpecrum(Value, true);
                Foreground.SetActiveSpecrum(Value, true);
            };

            MouseDown += (sender, e) =>
            {
                TextBoxMain.Focus();
            };
        }
    }
}
