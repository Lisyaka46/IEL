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
    /// Логика взаимодействия для IELButtonImageKey.xaml
    /// </summary>
    public partial class IELButtonImageKey : UserControl, IIELButtonKey
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
                    BorderButtonKey.Background = color;
                };
                value.BorderBrushQChanged += (NewValue) =>
                {
                    SolidColorBrush color = new(NewValue);
                    BorderButton.BorderBrush = color;
                    BorderButtonKey.BorderBrush = color;
                    BorderLeftArrow.BorderBrush = color;
                    BorderRightArrow.BorderBrush = color;
                };
                value.ForegroundQChanged += (NewValue) =>
                {
                    SolidColorBrush color = new(NewValue);
                    TextBlockKey.Foreground = color;
                    TextBlockLeftArrow.Foreground = color;
                    TextBlockRightArrow.Foreground = color;
                };
                _IELSettingObject = value;
            }
        }

        /// <summary>
        /// Изображение которое отображается в кнопке
        /// </summary>
        public ImageSource Imaging
        {
            get
            {
                return ImageButton.Source;
            }
            set
            {
                ImageButton.Source = value;
            }
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
        /// Скруглённость границ
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => BorderButton.CornerRadius;
            set => BorderButton.CornerRadius = value;
        }

        /// <summary>
        /// Позиционирование картинки в кнопке
        /// </summary>
        public Thickness ImageMargin
        {
            get => ImageButton.Margin;
            set => ImageButton.Margin = value;
        }

        /// <summary>
        /// Объект события активации левым щелчком мыши
        /// </summary>
        public IIELButton.Activate? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации правым щелчком мыши
        /// </summary>
        public IIELButton.Activate? OnActivateMouseRight { get; set; }

        private bool _CharKeyKeyboardActivate = false;
        /// <summary>
        /// Активность видимости символа действия активации кнопки
        /// </summary>
        public bool CharKeyboardActivate
        {
            get => _CharKeyKeyboardActivate;
            set
            {
                BorderButton.BeginAnimation(MarginProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationThickness(new(!value ? -24 : 0, 0, 0, 0)));
                BorderButtonKey.BeginAnimation(OpacityProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationDouble(value ? 1d : 0d));
                _CharKeyKeyboardActivate = value;
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
                TextBlockKey.Text = IIELObject.KeyName(value).ToString();
            }
        }

        /// <summary>
        /// Смещение контента в объекте
        /// </summary>
        public Thickness PaddingContent
        {
            get => BorderButton.Padding;
            set => BorderButton.Padding = value;
        }

        /// <summary>
        /// Инициализировать объект кнопки с изображением
        /// </summary>
        public IELButtonImageKey()
        {
            InitializeComponent();
            IELSettingObject = new();

            ImageButton.Margin = new Thickness(10, 10, 10, 10);
            CharKeyboardActivate = false;

            ImageMouseButtonsUse.Opacity = 0d;
            BorderButton.MouseEnter += (sender, e) =>
            {
                if (IsEnabled)
                {
                    MouseEnterDetect();
                    IELSettingObject.StartHover();
                }
            };
            BorderButton.MouseLeave += (sender, e) =>
            {
                if (IsEnabled)
                {
                    MouseLeaveDetect();
                    IELSettingObject.StopHover();
                }
            };

            IsEnabledChanged += (sender, e) =>
            {
                Color
                Background = (bool)e.NewValue ? IELSettingObject.BackgroundSetting.Default : IELSettingObject.BackgroundSetting.NotEnabled,
                BorderBrush = (bool)e.NewValue ? IELSettingObject.BorderBrushSetting.Default : IELSettingObject.BorderBrushSetting.NotEnabled,
                Foreground = (bool)e.NewValue ? IELSettingObject.ForegroundSetting.Default : IELSettingObject.ForegroundSetting.NotEnabled;
                ColorAnimation animation;

                animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush);
                BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                BorderButtonKey.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

                animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background);
                BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                BorderButtonKey.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

                animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground);
                TextBlockKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
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
                    MouseEnterDetect();
                    OnActivateMouseLeft?.Invoke(false);
                }
            };

            MouseRightButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    MouseEnterDetect();
                    OnActivateMouseRight?.Invoke(false);
                }
            };
        }

        /// <summary>
        /// Анимировать нажатие на кнопку (Down)
        /// </summary>
        private void ClickDownAnimation()
        {
            Color
                Background = IELSettingObject.BackgroundSetting.Used,
                BorderBrush = IELSettingObject.BorderBrushSetting.Used,
                Foreground = IELSettingObject.ForegroundSetting.Used;

            BorderButton.BorderBrush = new SolidColorBrush(BorderBrush);
            BorderButton.Background = new SolidColorBrush(Background);
            TextBlockKey.Foreground = new SolidColorBrush(Foreground);
        }

        /// <summary>
        /// Событие прихода курсора в видимую область кнопки
        /// </summary>
        private void MouseEnterDetect()
        {
            Color
                Foreground = IELSettingObject.ForegroundSetting.Select,
                Background = IELSettingObject.BackgroundSetting.Select,
                BorderBrush = IELSettingObject.BorderBrushSetting.Select;
            ColorAnimation animation;

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush);
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background);
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderButtonKey.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground);
            TextBlockKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
        }

        /// <summary>
        /// Событие ухода курсора из видимой области кнопки
        /// </summary>
        private void MouseLeaveDetect()
        {
            Color
                Foreground = IELSettingObject.ForegroundSetting.Default,
                Background = IELSettingObject.BackgroundSetting.Default,
                BorderBrush = IELSettingObject.BorderBrushSetting.Default;
            ColorAnimation animation;

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush);
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background);
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderButtonKey.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground);
            TextBlockKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);

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
            BorderButtonKey.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation.From = IELSettingObject.BackgroundSetting.Used;
            animation.To = IELSettingObject.BackgroundSetting.Select;
            BorderButtonKey.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation.From = IELSettingObject.ForegroundSetting.Used;
            animation.To = IELSettingObject.ForegroundSetting.Select;
            TextBlockKey.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }
    }
}
