using IEL.CORE.Classes.ObjectSettings;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELBlockInfoText.xaml
    /// </summary>
    public partial class IELBlockInfoText : UserControl
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
                value.BackgroundQChanged += (NewValue) =>
                {
                    SolidColorBrush color = new(NewValue);
                    MainBorder.Background = color;
                };
                value.BorderBrushQChanged += (NewValue) =>
                {
                    SolidColorBrush color = new(NewValue);
                    MainBorder.BorderBrush = color;
                };
                value.ForegroundQChanged += (NewValue) =>
                {
                    SolidColorBrush color = new(NewValue);
                    MainTextBlock.Foreground = color;
                };
                _IELSettingObject = value;
                _IELSettingObject.UseActiveQSetting();
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
        /// Текст объекта
        /// </summary>
        public string Text
        {
            get => MainTextBlock.Text;
            set => MainTextBlock.Text = value;
        }

        /// <summary>
        /// Шрифт текста в кнопке
        /// </summary>
        public new FontFamily FontFamily
        {
            get => base.FontFamily;
            set
            {
                MainTextBlock.FontFamily = value;
                base.FontFamily = value;
            }
        }

        public IELBlockInfoText()
        {
            IELObjectSetting.GlobalSetValidKey();
            InitializeComponent();
            IELSettingObject = new();
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

            IsEnabledChanged += (sender, e) =>
            {
                bool NewValue = (bool)e.NewValue;
                Color
                    Foreground = NewValue ? IELSettingObject.ForegroundSetting.Default : IELSettingObject.ForegroundSetting.NotEnabled,
                    Background = NewValue ? IELSettingObject.BackgroundSetting.Default : IELSettingObject.BackgroundSetting.NotEnabled,
                    BorderBrush = NewValue ? IELSettingObject.BorderBrushSetting.Default : IELSettingObject.BorderBrushSetting.NotEnabled;

                MainBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush));

                MainBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background));

                MainTextBlock.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground));
            };
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

            MainBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush));

            MainBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background));

            MainTextBlock.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground));
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

            MainBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush));

            MainBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background));

            MainTextBlock.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground));
        }
    }
}
