using IEL.CORE.Classes.ObjectSettings;
using IEL.CORE.Enums;
using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELButtonTextKey.xaml
    /// </summary>
    public partial class IELButtonTextKey : UserControl, IIELButtonKey
    {
        private IELButtonObjectSetting _IELSettingObject = new();
        /// <summary>
        /// Настройка использования объекта
        /// </summary>
        public IELButtonObjectSetting IELSettingObject
        {
            get => _IELSettingObject;
            set
            {
                value.VisualizationButtonChanged += (StateVisual) =>
                {
                    bool
                        Left = StateVisual == StateVisualButton.LeftArrow,
                        Right = StateVisual == StateVisualButton.RightArrow;
                    ColumnRightArrow.Width = new(Right ? 25 : 0);
                    BorderRightArrow.Opacity = Right ? 1d : 0d;

                    ColumnLeftArrow.Width = new(Left ? 25 : 0);
                    BorderLeftArrow.Opacity = Left ? 1d : 0d;
                    ImageMouseButtonsUse.HorizontalAlignment = Right ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                };
                value.BackgroundSetting.SetActionColorChanged((Spectrum, NewValue, Animated) =>
                {
                    if (Animated)
                    {
                        ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(NewValue);
                        BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                        BorderCharKeyboard.Background.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                        BorderRightArrow.Background.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                        BorderLeftArrow.Background.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                    }
                    else
                    {
                        SolidColorBrush color = new(NewValue);
                        BorderButton.Background = color;
                        BorderCharKeyboard.Background = color;
                        BorderLeftArrow.Background = color;
                        BorderRightArrow.Background = color;
                    }
                });
                value.BorderBrushSetting.SetActionColorChanged((Spectrum, NewValue, Animated) =>
                {
                    if (Animated)
                    {
                        ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(NewValue);
                        BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                        BorderCharKeyboard.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                        BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                        BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                    }
                    else
                    {
                        SolidColorBrush color = new(NewValue);
                        BorderButton.BorderBrush = color;
                        BorderCharKeyboard.BorderBrush = color;
                        BorderLeftArrow.BorderBrush = color;
                        BorderRightArrow.BorderBrush = color;
                    }
                });
                value.ForegroundSetting.SetActionColorChanged((Spectrum, NewValue, Animated) =>
                {
                    if (Animated)
                    {
                        ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(NewValue);
                        TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                        TextBlockCharKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                        TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                        TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                    }
                    else
                    {
                        SolidColorBrush color = new(NewValue);
                        TextBlockButton.Foreground = color;
                        TextBlockCharKey.Foreground = color;
                        TextBlockLeftArrow.Foreground = color;
                        TextBlockRightArrow.Foreground = color;
                    }
                });
                _IELSettingObject = value;
            }
        }

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
        public new FontFamily FontFamily
        {
            get => base.FontFamily;
            set
            {
                TextBlockCharKey.FontFamily = value;
                TextBlockButton.FontFamily = value;
                base.FontFamily = value;
            }
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
                BorderButton.BeginAnimation(MarginProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationThickness(new(!value ? -24 : 0, 0, 0, 0)));
                BorderCharKeyboard.BeginAnimation(OpacityProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationDouble(value ? 1d : 0d));
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

        /// <summary>
        /// Объект события активации левым щелчком мыши
        /// </summary>
        public IIELButton.ActivateHandler? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации правым щелчком мыши
        /// </summary>
        public IIELButton.ActivateHandler? OnActivateMouseRight { get; set; }

        /// <summary>
        /// Смещение контента в объекте
        /// </summary>
        public Thickness PaddingContent
        {
            get => BorderButton.Padding;
            set => BorderButton.Padding = value;
        }

        /// <summary>
        /// Инициализировать объект интерфейса кнопки с текстом поддерживающую возможность нажатия с помощью клавиши
        /// </summary>
        public IELButtonTextKey()
        {
            InitializeComponent();
            IELSettingObject = new();

            BorderButton.Margin = new(-24, 0, 0, 0);
            BorderCharKeyboard.Opacity = 0d;
            ImageMouseButtonsUse.Opacity = 0d;
            Text = "Text";
            CornerRadius = new CornerRadius(10);

            MouseEnter += (sender, e) =>
            {
                if (IsEnabled)
                {
                    IELSettingObject.UseActiveQSetting(StateSpectrum.Select);
                    IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
                    IELSettingObject.StartHover();
                }
            };

            MouseLeave += (sender, e) =>
            {
                if (IsEnabled)
                {
                    IELSettingObject.UseActiveQSetting(StateSpectrum.Default);
                    IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, false);
                    IELSettingObject.StopHover();
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
                        IELSettingObject.UseActiveQSetting(StateSpectrum.Used);
                        IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
                        IELSettingObject.StopHover();
                    }
                }
            };

            MouseLeftButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseLeft != null)
                {
                    IELSettingObject.UseActiveQSetting(StateSpectrum.Select);
                    IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
                    OnActivateMouseLeft?.Invoke(this, e);
                }
            };

            MouseRightButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    IELSettingObject.UseActiveQSetting(StateSpectrum.Select);
                    IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
                    OnActivateMouseRight?.Invoke(this, e);
                }
            };

            IsEnabledChanged += (sender, e) =>
            {
                bool NewValue = (bool)e.NewValue;
                IELSettingObject.UseActiveQSetting(NewValue ? StateSpectrum.Default : StateSpectrum.NotEnabled);
                IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, false);
            };
        }

        /// <summary>
        /// Анимация мерцания
        /// </summary>
        [MTAThread()]
        public void BlinkAnimation()
        {
            IELSettingObject.UseActiveQSetting(StateSpectrum.Used, false);
            IELSettingObject.UseActiveQSetting(StateSpectrum.Select);
            IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
        }

        /// <summary>
        /// Анимация мерцания
        /// </summary>
        [MTAThread()]
        public void UnfocusAnimation()
        {
            IELSettingObject.UseActiveQSetting(StateSpectrum.Default);
            IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, false);
        }
    }
}
