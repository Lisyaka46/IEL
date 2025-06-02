using IEL.CORE.Classes.ObjectSettings;
using IEL.CORE.Enums;
using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELButtonText.xaml
    /// </summary>
    public partial class IELButtonText : UserControl, IIELButton
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
                    TextBlockButton.Foreground = color;
                    TextBlockLeftArrow.Foreground = color;
                    TextBlockRightArrow.Foreground = color;
                };
                _IELSettingObject = value;
            }
        }

        /// <summary>
        /// Скругление границ
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
        /// Смещение контента в объекте
        /// </summary>
        public Thickness PaddingContent
        {
            get => BorderButton.Padding;
            set => BorderButton.Padding = value;
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
        /// Шрифт текста в кнопке
        /// </summary>
        public new FontFamily FontFamily
        {
            get => base.FontFamily;
            set
            {
                TextBlockButton.FontFamily = value;
                base.FontFamily = value;
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

        public IELButtonText()
        {
            InitializeComponent();
            IELSettingObject = new();

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
                    MouseLeaveAnimation();
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
                if (IELSettingObject.StateVisualizationButton != StateVisualButton.Default)
                {
                    (IELSettingObject.StateVisualizationButton == StateVisualButton.LeftArrow ? TextBlockLeftArrow : TextBlockRightArrow)
                        .Foreground.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground));
                }

                BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush));

                BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background));

                TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground));

                IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, false);
            };
        }

        /// <summary>
        /// Анимировать нажатие на элемент (Down)
        /// </summary>
        private void ClickDownAnimation()
        {
            Color
                Foreground = IELSettingObject.ForegroundSetting.Used,
                Background = IELSettingObject.BackgroundSetting.Used,
                BorderBrush = IELSettingObject.BorderBrushSetting.Used;
            if (IELSettingObject.StateVisualizationButton != StateVisualButton.Default)
            {
                (IELSettingObject.StateVisualizationButton == StateVisualButton.LeftArrow ? TextBlockLeftArrow : TextBlockRightArrow)
                    .Foreground = new SolidColorBrush(Foreground);
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
            TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            if (IELSettingObject.StateVisualizationButton != StateVisualButton.Default)
            {
                (IELSettingObject.StateVisualizationButton == StateVisualButton.LeftArrow ? TextBlockLeftArrow : TextBlockRightArrow)
                    .Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }

            IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
        }

        /// <summary>
        /// Анимация отключения выделения мышью
        /// </summary>
        private void MouseLeaveAnimation()
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
            TextBlockButton.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            if (IELSettingObject.StateVisualizationButton != StateVisualButton.Default)
            {
                (IELSettingObject.StateVisualizationButton == StateVisualButton.LeftArrow ? TextBlockLeftArrow : TextBlockRightArrow)
                    .Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }

            IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, false);
        }
    }
}
