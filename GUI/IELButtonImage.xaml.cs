using IEL.CORE.Classes;
using IEL.CORE.Classes.ObjectSettings;
using IEL.CORE.Enums;
using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELButtonImage.xaml
    /// </summary>
    public partial class IELButtonImage : UserControl, IIELButton
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
                    BorderRightArrow.Background = color;
                    BorderLeftArrow.Background = color;
                };
                value.BorderBrushQChanged += (NewValue) =>
                {
                    SolidColorBrush color = new(NewValue);
                    BorderButton.BorderBrush = color;
                    BorderRightArrow.BorderBrush = color;
                    BorderLeftArrow.BorderBrush = color;
                };
                value.ForegroundQChanged += (NewValue) =>
                {
                    SolidColorBrush color = new(NewValue);
                    TextBlockLeftArrow.Foreground = color;
                    TextBlockRightArrow.Foreground = color;
                };
                _IELSettingObject = value;
            }
        }

        /// <summary>
        /// Изображение которое отображается в элементе
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
        /// Толщина границ кнопки
        /// </summary>
        public Thickness BorderThicknessBlock
        {
            get => BorderButton.BorderThickness;
            set => BorderButton.BorderThickness = value;
        }

        /// <summary>
        /// Скруглённость границ кнопки
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => BorderButton.CornerRadius;
            set => BorderButton.CornerRadius = value;
        }

        /// <summary>
        /// Позиционирование картинки в элементе
        /// </summary>
        public Thickness ImageMargin
        {
            get => ImageButton.Margin;
            set => ImageButton.Margin = value;
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
        /// Объект события активации левым щелчком мыши
        /// </summary>
        public IIELButton.Activate? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации правым щелчком мыши
        /// </summary>
        public IIELButton.Activate? OnActivateMouseRight { get; set; }

        /// <summary>
        /// Инициализировать объект кнопки с изображением
        /// </summary>
        public IELButtonImage()
        {
            InitializeComponent();
            IELSettingObject = new();

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
                ColorAnimation animation;
                Color
                    Background = (bool)e.NewValue ? IELSettingObject.BackgroundSetting.Default : IELSettingObject.BackgroundSetting.NotEnabled,
                    BorderBrush = (bool)e.NewValue ? IELSettingObject.BorderBrushSetting.Default : IELSettingObject.BorderBrushSetting.NotEnabled,
                    Foreground = (bool)e.NewValue ? IELSettingObject.ForegroundSetting.Default : IELSettingObject.BackgroundSetting.NotEnabled;

                animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush);
                BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

                animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background);
                BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                BorderLeftArrow.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                BorderRightArrow.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

                animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground);
                //TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                if (IELSettingObject.StateVisualizationButton != StateVisualButton.Default)
                {
                    (IELSettingObject.StateVisualizationButton == StateVisualButton.LeftArrow ? TextBlockLeftArrow : TextBlockRightArrow)
                        .Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
            };

            MouseLeftButtonDown += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseLeft != null)
                {
                    ClickDownAnimation();
                    IELSettingObject.StopHover();
                }
            };

            MouseRightButtonDown += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    ClickDownAnimation();
                    IELSettingObject.StopHover();
                }
            };

            MouseLeftButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseLeft != null)
                {
                    MouseEnterDetect();
                    OnActivateMouseLeft?.Invoke(this);
                }
            };

            MouseRightButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    MouseEnterDetect();
                    OnActivateMouseRight?.Invoke(this);
                }
            };
        }

        /// <summary>
        /// Анимировать нажатие на кнопку (Down)
        /// </summary>
        private void ClickDownAnimation()
        {
            SolidColorBrush
                Background = new(IELSettingObject.BackgroundSetting.Used),
                BorderBrush = new(IELSettingObject.BorderBrushSetting.Used),
                Foreground = new(IELSettingObject.ForegroundSetting.Used);

            BorderButton.BorderBrush = BorderBrush;
            BorderRightArrow.BorderBrush = BorderBrush;
            BorderLeftArrow.BorderBrush = BorderBrush;

            BorderButton.Background = Background;
            BorderRightArrow.Background = Background;
            BorderLeftArrow.Background = Background;

            TextBlockLeftArrow.Foreground = Foreground;
            TextBlockRightArrow.Foreground = Foreground;
        }

        /// <summary>
        /// Событие прихода курсора в видимую область кнопки
        /// </summary>
        private void MouseEnterDetect()
        {
            ColorAnimation animation;
            Color
                Foreground = IELSettingObject.ForegroundSetting.Select,
                Background = IELSettingObject.BackgroundSetting.Select,
                BorderBrush = IELSettingObject.BorderBrushSetting.Select;

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush);
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background);
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderLeftArrow.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderRightArrow.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground);
            //TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            if (IELSettingObject.StateVisualizationButton != StateVisualButton.Default)
            {
                (IELSettingObject.StateVisualizationButton == StateVisualButton.LeftArrow ? TextBlockLeftArrow : TextBlockRightArrow)
                    .Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }

            IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
        }

        /// <summary>
        /// Событие ухода курсора из видимой области кнопки
        /// </summary>
        private void MouseLeaveDetect()
        {
            ColorAnimation animation;
            Color
                Foreground = IELSettingObject.ForegroundSetting.Default,
                Background = IELSettingObject.BackgroundSetting.Default,
                BorderBrush = IELSettingObject.BorderBrushSetting.Default;

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush);
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background);
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderLeftArrow.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            BorderRightArrow.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground);
            //TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            if (IELSettingObject.StateVisualizationButton != StateVisualButton.Default)
            {
                (IELSettingObject.StateVisualizationButton == StateVisualButton.LeftArrow ? TextBlockLeftArrow : TextBlockRightArrow)
                    .Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }

            IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, false);
        }
    }
}
