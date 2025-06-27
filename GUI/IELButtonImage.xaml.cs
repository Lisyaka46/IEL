using IEL.CORE.Classes.ObjectSettings;
using IEL.CORE.Enums;
using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

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
                value.BackgroundSetting.SetActionColorChanged((Spectrum, NewValue, Animated) =>
                {
                    if (Animated)
                    {
                        ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(NewValue);
                        BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                        BorderRightArrow.Background.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                        BorderLeftArrow.Background.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                    }
                    else
                    {
                        SolidColorBrush color = new(NewValue);
                        BorderButton.Background = color;
                        BorderRightArrow.Background = color;
                        BorderLeftArrow.Background = color;
                    }
                });
                value.BorderBrushSetting.SetActionColorChanged((Spectrum, NewValue, Animated) =>
                {
                    if (Animated)
                    {
                        ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(NewValue);
                        BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                        BorderRightArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                        BorderLeftArrow.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                    }
                    else
                    {
                        SolidColorBrush color = new(NewValue);
                        BorderButton.BorderBrush = color;
                        BorderRightArrow.BorderBrush = color;
                        BorderLeftArrow.BorderBrush = color;
                    }
                });
                value.ForegroundSetting.SetActionColorChanged((Spectrum, NewValue, Animated) =>
                {
                    if (Animated)
                    {
                        ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(NewValue);
                        TextBlockLeftArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                        TextBlockRightArrow.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                    }
                    else
                    {
                        SolidColorBrush color = new(NewValue);
                        TextBlockLeftArrow.Foreground = color;
                        TextBlockRightArrow.Foreground = color;
                    }
                });
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
            IELObjectSetting.GlobalSetValidKey();
            InitializeComponent();
            IELSettingObject = new();

            ImageMouseButtonsUse.Opacity = 0d;

            BorderButton.MouseEnter += (sender, e) =>
            {
                if (IsEnabled)
                {
                    IELSettingObject.UseActiveQSetting(StateSpectrum.Select);
                    IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
                    IELSettingObject.StartHover();
                }
            };
            BorderButton.MouseLeave += (sender, e) =>
            {
                if (IsEnabled)
                {
                    IELSettingObject.UseActiveQSetting(StateSpectrum.Default);
                    IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, false);
                    IELSettingObject.StopHover();
                }
            };

            IsEnabledChanged += (sender, e) =>
            {
                bool NewValue = (bool)e.NewValue;
                IELSettingObject.UseActiveQSetting(NewValue ? StateSpectrum.Default : StateSpectrum.NotEnabled);
                IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, false);
            };

            MouseLeftButtonDown += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseLeft != null)
                {
                    IELSettingObject.UseActiveQSetting(StateSpectrum.Used, false);
                    IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, false);
                    IELSettingObject.StopHover();
                }
            };

            MouseRightButtonDown += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    IELSettingObject.UseActiveQSetting(StateSpectrum.Used, false);
                    IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, false);
                    IELSettingObject.StopHover();
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
        }
    }
}
