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
    /// Логика взаимодействия для IELButtonCommand.xaml
    /// </summary>
    public partial class IELButtonCommand : UserControl, IIELButton
    {
        /// <summary>
        /// Настройка использования объекта
        /// </summary>
        public IELUsingObjectSetting IELSettingObject { get; set; } = new();

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
            get => TextBlockButtonName.Text;
            set => TextBlockButtonName.Text = value;
        }

        /// <summary>
        /// Текст команды
        /// </summary>
        public string TextCommand
        {
            get => TextBlockButtonCommand.Text;
            set => TextBlockButtonCommand.Text = value;
        }

        private int _Index;
        /// <summary>
        /// Индекс элемента 
        /// </summary>
        public int Index
        {
            get => _Index;
            set
            {
                TextBlockNumberCommand.Text = $"#{value + 1}";
                _Index = value;
            }
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
        /// Шрифт текста в кнопке
        /// </summary>
        public FontFamily TextFontFamily
        {
            get => TextBlockButtonName.FontFamily;
            set => TextBlockButtonName.FontFamily = value;
        }

        /// <summary>
        /// Размер текста в кнопке
        /// </summary>
        public double TextFontSize
        {
            get => TextBlockButtonName.FontSize;
            set => TextBlockButtonName.FontSize = value;
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
        /// Активация действия кнопки
        /// </summary>
        private bool ButtonActivate = false;

        /// <summary>
        /// Объект события активации левым щелчком мыши
        /// </summary>
        public IIELButton.Activate? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации правым щелчком мыши
        /// </summary>
        public IIELButton.Activate? OnActivateMouseRight { get; set; }

        public IELButtonCommand(string Name, string FullTextCommand, int indexBuffer)
        {
            InitializeComponent();

            IELSettingObject.BackgroundQChanged += (NewValue) =>
            {
                SolidColorBrush color = new(NewValue);
                BorderButton.Background = color;
            };
            IELSettingObject.BorderBrushQChanged += (NewValue) =>
            {
                SolidColorBrush color = new(NewValue);
                BorderButton.BorderBrush = color;
            };
            IELSettingObject.ForegroundQChanged += (NewValue) =>
            {
                SolidColorBrush color = new(NewValue);
                TextBlockButtonCommand.Foreground = color;
                TextBlockButtonName.Foreground = color;
                TextBlockNumberCommand.Foreground = color;
            };

            TextFontFamily = new FontFamily("Arial");
            TextFontSize = 14;
            TextBlockButtonName.FontWeight = FontWeights.Bold;
            Text = Name;
            TextBlockButtonCommand.Text = FullTextCommand;
            CornerRadius = new CornerRadius(10);
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Top;
            Height = 27;
            Width = 230;
            BorderButton.CornerRadius = new CornerRadius(4);
            Index = indexBuffer;

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
                ButtonActivate = false;
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
                        ButtonActivate = true;
                        ClickDownAnimation();
                        IELSettingObject.StopHover();
                    }
                }
            };       

            MouseLeftButtonUp += (sender, e) =>
            {
                if (ButtonActivate)
                {
                    ButtonActivate = false;
                    MouseEnterAnimation();
                    OnActivateMouseLeft?.Invoke(this, e);
                }
            };

            MouseRightButtonUp += (sender, e) =>
            {
                if (ButtonActivate)
                {
                    ButtonActivate = false;
                    MouseEnterAnimation();
                    OnActivateMouseRight?.Invoke(this, e);
                }
            };

            IsEnabledChanged += (sender, e) =>
            {
                bool NewValue = (bool)e.NewValue;
                Color
                    Foreground = NewValue ? IELSettingObject.ForegroundSetting.Default : IELSettingObject.ForegroundSetting.NotEnabled,
                    Background = NewValue ? IELSettingObject.BackgroundSetting.Default : IELSettingObject.BackgroundSetting.NotEnabled,
                    BorderBrush = NewValue ? IELSettingObject.BorderBrushSetting.Default : IELSettingObject.BorderBrushSetting.NotEnabled;
                BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, null);
                BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, null);
                TextBlockButtonName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, null);
                TextBlockButtonCommand.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, null);
                BorderButton.BorderBrush = new SolidColorBrush(BorderBrush);
                BorderButton.Background = new SolidColorBrush(Background);
                TextBlockButtonName.Foreground = new SolidColorBrush(Foreground);
                TextBlockButtonCommand.Foreground = new SolidColorBrush(Foreground);
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
            BorderButton.BorderBrush = new SolidColorBrush(BorderBrush);
            BorderButton.Background = new SolidColorBrush(Background);
            TextBlockButtonName.Foreground = new SolidColorBrush(Foreground);
            TextBlockButtonCommand.Foreground = new SolidColorBrush(Foreground);
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

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background);
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush);
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground);
            TextBlockButtonName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            TextBlockButtonCommand.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
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
            ColorAnimation animation;

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background);
            BorderButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush);
            BorderButton.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

            animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground);
            TextBlockButtonName.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            TextBlockButtonCommand.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }
    }
}
