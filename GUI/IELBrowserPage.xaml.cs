using IEL.Classes;
using IEL.Interfaces.Core;
using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELBrowserPage.xaml
    /// </summary>
    public partial class IELBrowserPage : UserControl, IIELBrowserPage
    {
        #region Color Setting
        private BrushSettingQ? _BackgroundSetting;
        /// <summary>
        /// Объект обычного состояния фона
        /// </summary>
        public BrushSettingQ BackgroundSetting
        {
            get => _BackgroundSetting ?? new();
            set
            {
                BackgroundChangeDefaultColor.Invoke(BrushSettingQ.StateSpectrum.Default, value.Default);
                value.ColorDefaultChange += BackgroundChangeDefaultColor;
                _BackgroundSetting = value;
            }
        }

        private BrushSettingQ? _BorderBrushSetting;
        /// <summary>
        /// Объект обычного состояния границы
        /// </summary>
        public BrushSettingQ BorderBrushSetting
        {
            get => _BorderBrushSetting ?? new();
            set
            {
                BorderBrushChangeDefaultColor.Invoke(BrushSettingQ.StateSpectrum.Default, value.Default);
                value.ColorDefaultChange += BorderBrushChangeDefaultColor;
                _BorderBrushSetting = value;
            }
        }

        private BrushSettingQ? _ForegroundSetting;
        /// <summary>
        /// Объект обычного состояния текста
        /// </summary>
        public BrushSettingQ ForegroundSetting
        {
            get => _ForegroundSetting ?? new();
            set
            {
                ForegroundChangeDefaultColor.Invoke(BrushSettingQ.StateSpectrum.Default, value.Default);
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

        /// <summary>
        /// Массив объектов страниц
        /// </summary>
        private readonly List<IELInley> IELInleys;

        /// <summary>
        /// Индекс активированной вкладки
        /// </summary>
        private int ActivateIndex = -1;

        /// <summary>
        /// Количество вкладок в браузере страниц
        /// </summary>
        public int InleysCount => IELInleys.Count;

        /// <summary>
        /// Диактивированная позиция вкладки
        /// </summary>
        private readonly Thickness MarginDiactivateAlvay = new(11, 8, 11, 8);

        /// <summary>
        /// Активированная позиция вкладки
        /// </summary>
        private readonly Thickness MarginActivateAlvay = new(8, 5, 8, 5);

        public IELBrowserPage()
        {
            InitializeComponent();
            IELInleys = [];

            AnimationMillisecond = 100;
            BackgroundChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == BrushSettingQ.StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == BrushSettingQ.StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                BorderMain.Background = color;
            };
            BorderBrushChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == BrushSettingQ.StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == BrushSettingQ.StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                BorderMain.BorderBrush = color;
                BorderMainPage.BorderBrush = color;
            };
            ForegroundChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == BrushSettingQ.StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == BrushSettingQ.StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
            };
            BackgroundSetting = new(BrushSettingQ.CreateStyle.Background);
            BorderBrushSetting = new(BrushSettingQ.CreateStyle.BorderBrush);
            ForegroundSetting = new(BrushSettingQ.CreateStyle.Foreground);
        }

        /// <summary>
        /// Добавить новую страницу
        /// </summary>
        /// <param name="Content">Добавляемая страница в баузер страниц</param>
        /// <param name="Head">Наименование вкладки</param>
        /// <param name="Signature">Подпись вкладки</param>
        /// <param name="Activate">Активировать сразу или нет страницу</param>
        public void AddInlayPage(IPageDefault Content, string Head, string? Signature = null, bool Activate = true)
        {
            IELInley Inlay = new()
            {
                Text = Head,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = MarginDiactivateAlvay,
                BorderThicknessBlock = new(2),
                CornerRadius = new(6),
                AnimationMillisecond = 200,
            };
            Inlay.SetPage(Content);
            Inlay.TextSignature = Signature ?? string.Empty;
            GridMainInlays.Children.Add(Inlay);
            IELInleys.Add(Inlay);
            if (Activate)
            {
                UsingInlayAnimation(Inlay, true);
                ActivateIndex = IELInleys.Count - 1;
                IELFrameBrowser.NextPage(Content);
            }
            else
            {
                Inlay.Opacity = 0d;
            }
        }

        /// <summary>
        /// Открыть страницу по индексу
        /// </summary>
        /// <param name="index">Индекс открываемой страницы</param>
        /// <exception cref="Exception">Исключение при пустой странице в найденой вкладке</exception>
        public void ActivateInIndex(Index index)
        {
            IPageDefault Page = IELInleys[index].Page ?? throw new Exception("Объект заголовка не может быть без страницы!");
            if (ActivateIndex > -1)
            {
                UsingInlayAnimation(IELInleys[ActivateIndex], false);
            }
            UsingInlayAnimation(IELInleys[index], true);
            ActivateIndex = index.Value;
            IELFrameBrowser.NextPage(Page);
        }

        /// <summary>
        /// Анимировать вкладку
        /// </summary>
        /// <param name="Avtivate">Состояние активности вкладки</param>
        public void UsingInlayAnimation(IELInley Inlay, bool Avtivate)
        {
            if (Avtivate) Inlay.Opacity = 0d;
            Canvas.SetZIndex(Inlay, Avtivate ? 1 : 0);

            ThicknessAnimation AanimationThickness = AnimationThickness.Clone();
            AanimationThickness.To = Avtivate ? MarginActivateAlvay : MarginDiactivateAlvay;
            AanimationThickness.Duration = TimeSpan.FromMilliseconds(800d);
            Inlay.BeginAnimation(MarginProperty, AanimationThickness);

            DoubleAnimation animationDouble = AnimationDouble.Clone();
            animationDouble.To = Avtivate ? 1d : 0d;
            animationDouble.Duration = TimeSpan.FromMilliseconds(800d);
            Inlay.BeginAnimation(OpacityProperty, animationDouble);
        }
    }
}
