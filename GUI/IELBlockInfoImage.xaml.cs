using IEL.CORE.Classes.ObjectSettings;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using IEL.CORE.Enums;
using System.Windows.Media.Animation;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELBlockInfo.xaml
    /// </summary>
    public partial class IELBlockInfoImage : UserControl
    {
        private IELUsingObjectSetting _IELSettingObject = new();
        /// <summary>
        /// Настройка использования объекта
        /// </summary>
        public IELUsingObjectSetting IELSettingObject
        {
            get => _IELSettingObject;
            set
            {
                value.BackgroundSetting.SetActionColorChanged((Spectrum, NewValue, Animated) =>
                {
                    if (Animated)
                    {
                        ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(NewValue);
                        MainBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                    }
                    else
                    {
                        SolidColorBrush color = new(NewValue);
                        MainBorder.Background = color;
                    }
                });
                value.BorderBrushSetting.SetActionColorChanged((Spectrum, NewValue, Animated) =>
                {
                    if (Animated)
                    {
                        ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(NewValue);
                        MainBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                    }
                    else
                    {
                        SolidColorBrush color = new(NewValue);
                        MainBorder.BorderBrush = color;
                    }
                });
                value.ForegroundSetting.SetActionColorChanged((Spectrum, NewValue, Animated) =>
                {
                    SolidColorBrush color = new(NewValue);
                });
                _IELSettingObject = value;
            }
        }

        /// <summary>
        /// Скругление границ
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => MainBorder.CornerRadius;
            set => MainBorder.CornerRadius = value;
        }

        /// <summary>
        /// Толщина границ
        /// </summary>
        public Thickness BorderThicknessBlock
        {
            get => MainBorder.BorderThickness;
            set => MainBorder.BorderThickness = value;
        }

        /// <summary>
        /// Смещение контента в объекте
        /// </summary>
        public Thickness PaddingContent
        {
            get => MainBorder.Padding;
            set => MainBorder.Padding = value;
        }

        /// <summary>
        /// Изображение которое отображается в элементе
        /// </summary>
        public ImageSource Imaging
        {
            get
            {
                return MainImage.Source;
            }
            set
            {
                MainImage.Source = value;
            }
        }

        /// <summary>
        /// Позиционирование картинки в элементе
        /// </summary>
        public Thickness ImageMargin
        {
            get => MainImage.Margin;
            set => MainImage.Margin = value;
        }

        /// <summary>
        /// Объект изображения отображаемого в блоке информации
        /// </summary>
        public Image MainFrontImage => MainImage;

        /// <summary>
        /// Инициализировать объект интерфейса отображения информации через изображение
        /// </summary>
        public IELBlockInfoImage()
        {
            InitializeComponent();
            IELSettingObject = new();
            CornerRadius = new CornerRadius(10);

            MouseEnter += (sender, e) =>
            {
                if (IsEnabled)
                {
                    IELSettingObject.UseActiveQSetting(StateSpectrum.Select);
                    IELSettingObject.StartHover();
                }
            };

            MouseLeave += (sender, e) =>
            {
                if (IsEnabled)
                {
                    IELSettingObject.UseActiveQSetting(StateSpectrum.Default);
                    IELSettingObject.StopHover();
                }
            };

            IsEnabledChanged += (sender, e) =>
            {
                bool NewValue = (bool)e.NewValue;
                IELSettingObject.UseActiveQSetting(NewValue ? StateSpectrum.Default : StateSpectrum.NotEnabled);
            };
        }
    }
}
