using IEL.Classes;
using IEL.Interfaces.Front;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static IEL.Interfaces.Front.IIELStateVisualizationButton;
using static IEL.Interfaces.Core.IQData;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELBlockInfoText.xaml
    /// </summary>
    public partial class IELBlockInfoText : UserControl, IIELHover
    {
        #region Color Setting
        private BrushSettingQ? _BackgroundSetting;
        /// <summary>
        /// Объект настройки состояний фона
        /// </summary>
        public BrushSettingQ BackgroundSetting
        {
            get => _BackgroundSetting ?? new();
            set
            {
                BackgroundChangeDefaultColor.Invoke(StateSpectrum.Default, value.Default);
                value.ColorDefaultChange += BackgroundChangeDefaultColor;
                _BackgroundSetting = value;
            }
        }

        private BrushSettingQ? _BorderBrushSetting;
        /// <summary>
        /// Объект настройки состояний границы
        /// </summary>
        public BrushSettingQ BorderBrushSetting
        {
            get => _BorderBrushSetting ?? new();
            set
            {
                BorderBrushChangeDefaultColor.Invoke(StateSpectrum.Default, value.Default);
                value.ColorDefaultChange += BorderBrushChangeDefaultColor;
                _BorderBrushSetting = value;
            }
        }

        private BrushSettingQ? _ForegroundSetting;
        /// <summary>
        /// Объект настройки состояний текста
        /// </summary>
        public BrushSettingQ ForegroundSetting
        {
            get => _ForegroundSetting ?? new();
            set
            {
                ForegroundChangeDefaultColor.Invoke(StateSpectrum.Default, value.Default);
                value.ColorDefaultChange += ForegroundChangeDefaultColor;
                _ForegroundSetting = value;
            }
        }

        #region Event Change Color
        /// <summary>
        /// Обект события изменения цвета обычного состояния фона
        /// </summary>
        private readonly BrushSettingQ.ColorDefaultChangeEventHandler BackgroundChangeDefaultColor;

        /// <summary>
        /// Обект события изменения цвета обычного состояния границы
        /// </summary>
        private readonly BrushSettingQ.ColorDefaultChangeEventHandler BorderBrushChangeDefaultColor;

        /// <summary>
        /// Обект события изменения цвета обычного состояния текста
        /// </summary>
        private readonly BrushSettingQ.ColorDefaultChangeEventHandler ForegroundChangeDefaultColor;
        #endregion
        #endregion

        #region AnimationMillisecond
        private int _AnimationMillisecond;
        /// <summary>
        /// Длительность анимации в миллисекундах
        /// </summary>
        public int AnimationMillisecond
        {
            get => _AnimationMillisecond;
            set
            {
                TimeSpan time = TimeSpan.FromMilliseconds(value);
                AnimationColor.Duration = time;
                AnimationThickness.Duration = time;
                AnimationDouble.Duration = time;
                _AnimationMillisecond = value;

            }
        }
        #endregion

        #region animateObjects
        /// <summary>
        /// Анимация color значения
        /// </summary>
        private readonly ColorAnimation AnimationColor = new()
        {
            DecelerationRatio = 0.2d,
            EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
        };

        /// <summary>
        /// Анимация thickness значения
        /// </summary>
        private readonly ThicknessAnimation AnimationThickness = new()
        {
            EasingFunction = new QuinticEase() { EasingMode = EasingMode.EaseOut }
        };

        /// <summary>
        /// Анимация double значения
        /// </summary>
        private readonly DoubleAnimation AnimationDouble = new()
        {
            DecelerationRatio = 0.2d,
            EasingFunction = new QuinticEase() { EasingMode = EasingMode.EaseOut }
        };
        #endregion

        #region MouseHover
        /// <summary>
        /// Длительность задержки в миллисекундах
        /// </summary>
        public double IntervalHover
        {
            get => TimerBorderInfo.Interval.TotalMilliseconds;
            set => TimerBorderInfo.Interval = TimeSpan.FromMilliseconds(value);
        }

        /// <summary>
        /// Таймер события MouseHover
        /// </summary>
        private readonly DispatcherTimer TimerBorderInfo = new();

        /// <summary>
        /// Событие задержки курсора на элементе
        /// </summary>
        public event EventHandler? MouseHover;
        #endregion

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
            InitializeComponent();
            IntervalHover = 1300d;
            TimerBorderInfo.Tick += (sender, e) =>
            {
                MouseHover?.Invoke(this, e);
                TimerBorderInfo.Stop();
            };

            AnimationMillisecond = 100;
            BackgroundChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                MainBorder.Background = color;
            };
            BorderBrushChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                MainBorder.BorderBrush = color;
            };
            ForegroundChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                MainTextBlock.Foreground = color;
            };
            BackgroundSetting = new(BrushSettingQ.CreateStyle.Background);
            BorderBrushSetting = new(BrushSettingQ.CreateStyle.BorderBrush);
            ForegroundSetting = new(BrushSettingQ.CreateStyle.Foreground);

            CornerRadius = new CornerRadius(10);

            MouseEnter += (sender, e) =>
            {
                if (IsEnabled)
                {
                    MouseEnterAnimation();
                    TimerBorderInfo.Start();
                }
            };

            MouseLeave += (sender, e) =>
            {
                if (IsEnabled)
                {
                    MouseLeaveAnimation();
                    TimerBorderInfo.Stop();
                }
            };

            IsEnabledChanged += (sender, e) =>
            {
                Color
                Foreground = (bool)e.NewValue ? ForegroundSetting.Default : ForegroundSetting.NotEnabled,
                Background = (bool)e.NewValue ? BackgroundSetting.Default : BackgroundSetting.NotEnabled,
                BorderBrush = (bool)e.NewValue ? BorderBrushSetting.Default : BorderBrushSetting.NotEnabled;

                AnimationColor.To = BorderBrush;
                MainBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

                AnimationColor.To = Background;
                MainBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

                AnimationColor.To = Foreground;
                MainTextBlock.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
            };
        }

        /// <summary>
        /// Анимация выделения кнопки мышью
        /// </summary>
        private void MouseEnterAnimation()
        {
            Color
                Foreground = ForegroundSetting.Select,
                Background = BackgroundSetting.Select,
                BorderBrush = BorderBrushSetting.Select;

            AnimationColor.To = BorderBrush;
            MainBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Background;
            MainBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Foreground;
            MainTextBlock.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
        }

        /// <summary>
        /// Анимация отключения выделения мышью
        /// </summary>
        private void MouseLeaveAnimation()
        {
            Color
                Foreground = ForegroundSetting.Default,
                Background = BackgroundSetting.Default,
                BorderBrush = BorderBrushSetting.Default;

            AnimationColor.To = BorderBrush;
            MainBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Background;
            MainBorder.Background.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);

            AnimationColor.To = Foreground;
            MainTextBlock.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, AnimationColor);
        }
    }
}
