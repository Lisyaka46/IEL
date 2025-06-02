using IEL.CORE.Classes;
using IEL.CORE.Classes.ObjectSettings;
using IEL.CORE.Enums;
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
                value.BackgroundQChanged += (NewValue) =>
                {
                    SolidColorBrush color = new(NewValue);
                    BorderButton.Background = color;
                    BorderCharKeyboard.Background = color;
                };
                value.BorderBrushQChanged += (NewValue) =>
                {
                    SolidColorBrush color = new(NewValue);
                    BorderButton.BorderBrush = color;
                    BorderCharKeyboard.BorderBrush = color;
                    BorderLeftArrow.BorderBrush = color;
                    BorderRightArrow.BorderBrush = color;
                };
                value.ForegroundQChanged += (NewValue) =>
                {
                    SolidColorBrush color = new(NewValue);
                    TextBlockCharKey.Foreground = color;
                    TextBlockButton.Foreground = color;
                    TextBlockLeftArrow.Foreground = color;
                    TextBlockRightArrow.Foreground = color;
                };
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
        public IIELButton.Activate? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации правым щелчком мыши
        /// </summary>
        public IIELButton.Activate? OnActivateMouseRight { get; set; }

        /// <summary>
        /// Смещение контента в объекте
        /// </summary>
        public Thickness PaddingContent
        {
            get => BorderButton.Padding;
            set => BorderButton.Padding = value;
        }

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
                    MouseEnterAnimation();
                    IELSettingObject.StartHover();
                }
            };

            MouseLeave += (sender, e) =>
            {
                if (IsEnabled)
                {
                    LeaveAnimation();
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
                        ClickDownAnimation();
                        IELSettingObject.StopHover();
                    }
                }
            };

            MouseLeftButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseLeft != null)
                {
                    MouseEnterAnimation();
                    OnActivateMouseLeft?.Invoke(this);
                }
            };

            MouseRightButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    MouseEnterAnimation();
                    OnActivateMouseRight?.Invoke(this);
                }
            };

            IsEnabledChanged += (sender, e) =>
            {
                Color
                Foreground = (bool)e.NewValue ? IELSettingObject.ForegroundSetting.Default : IELSettingObject.ForegroundSetting.NotEnabled,
                Background = (bool)e.NewValue ? IELSettingObject.BackgroundSetting.Default : IELSettingObject.BackgroundSetting.NotEnabled,
                BorderBrush = (bool)e.NewValue ? IELSettingObject.BorderBrushSetting.Default : IELSettingObject.BorderBrushSetting.NotEnabled;
                ColorAnimation animation;

                animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush);
                BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                BorderCharKeyboard.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

                animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background);
                BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                BorderCharKeyboard.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

                animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground);
                TextBlockCharKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);

                IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, false);
            };
        }

        /// <summary>
        /// Анимировать нажатие на кнопку (Down)
        /// </summary>
        private void ClickDownAnimation()
        {
            Color
            Foreground = IELSettingObject.ForegroundSetting.Used,
            Background = IELSettingObject.BackgroundSetting.Used,
            BorderBrush = IELSettingObject.BorderBrushSetting.Used;

            BorderCharKeyboard.BorderBrush = new SolidColorBrush(BorderBrush);
            BorderLeftArrow.BorderBrush = new SolidColorBrush(BorderBrush);
            BorderRightArrow.BorderBrush = new SolidColorBrush(BorderBrush);
            BorderButton.BorderBrush = new SolidColorBrush(BorderBrush);

            BorderButton.Background = new SolidColorBrush(Background);
            BorderCharKeyboard.Background = new SolidColorBrush(Background);

            TextBlockCharKey.Foreground = new SolidColorBrush(Foreground);
            TextBlockLeftArrow.Foreground = new SolidColorBrush(Foreground);
            TextBlockRightArrow.Foreground = new SolidColorBrush(Foreground);
            TextBlockButton.Foreground = new SolidColorBrush(Foreground);
        }

        /// <summary>
        /// Анимация выделения кнопки мышью
        /// </summary>
        private void MouseEnterAnimation()
        {
            Color
                Foreground = IELSettingObject.ForegroundSetting.Select,
                Background = IELSettingObject.BackgroundSetting.Select,
                BorderBrush = IELSettingObject.BorderBrushSetting.Select;
            ColorAnimation animation;

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush);
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderCharKeyboard.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background);
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderCharKeyboard.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground);
            TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            TextBlockCharKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
        }

        /// <summary>
        /// Анимация отключения выделения мышью
        /// </summary>
        public void LeaveAnimation()
        {
            Color
                Foreground = IELSettingObject.ForegroundSetting.Default,
                Background = IELSettingObject.BackgroundSetting.Default,
                BorderBrush = IELSettingObject.BorderBrushSetting.Default;
            ColorAnimation animation;

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush);
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderCharKeyboard.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background);
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderCharKeyboard.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground);
            TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            TextBlockCharKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, false);
        }

        /// <summary>
        /// Анимация мерцания
        /// </summary>
        [MTAThread()]
        public void BlinkAnimation()
        {
            ColorAnimation animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor();
            animation.SpeedRatio = 0.6d;
            animation.From = IELSettingObject.BorderBrushSetting.Used;
            animation.To = IELSettingObject.BorderBrushSetting.Select;
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderCharKeyboard.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation.From = IELSettingObject.BackgroundSetting.Used;
            animation.To = IELSettingObject.BackgroundSetting.Select;
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderCharKeyboard.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation.From = IELSettingObject.ForegroundSetting.Used;
            animation.To = IELSettingObject.ForegroundSetting.Select;
            TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            TextBlockCharKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }
    }
}
