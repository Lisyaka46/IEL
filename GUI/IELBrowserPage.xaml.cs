using IEL.Classes;
using IEL.Interfaces.Core;
using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using DataScroll;
using System.Windows.Controls.Primitives;
using static System.Net.Mime.MediaTypeNames;

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
        private readonly List<IELInlay> IELInleys;

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

        /// <summary>
        /// Скролл-бар вкладок
        /// </summary>
        private readonly CounterScrollBar ScrollBar;

        public IELBrowserPage()
        {
            InitializeComponent();
            ScrollBar = new(1);
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
                TextBlockLeftNameInlay.Foreground = color;
                TextBlockRightNameInlay.Foreground = color;
            };
            BackgroundSetting = new(BrushSettingQ.CreateStyle.Background);
            BorderBrushSetting = new(BrushSettingQ.CreateStyle.BorderBrush);
            ForegroundSetting = new(BrushSettingQ.CreateStyle.Foreground);

            TextBlockLeftNameInlay.Text = string.Empty;
            TextBlockRightNameInlay.Text = string.Empty;

            ScrollBar.ChangedValue += (NewValue) =>
            {
                UpdateTextNextBackPagesScroll(NewValue);
                IELInlay NextInlay = IELInleys[NewValue];
                if (NextInlay.TextSignature.Length > 0) NextInlay.IsAnimatedSignatureText = true;
            };
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
            foreach (IPageDefault? page in IELInleys.Select((i) => i.Page))
                if (page?.PageName.Equals(Content.PageName) ?? true) return;
            IELInlay Inlay = new()
            {
                Text = Head,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = MarginDiactivateAlvay,
                BorderThicknessBlock = new(2),
                CornerRadius = new(6),
                AnimationMillisecond = 200,
            };
            Inlay.SetPage(Content);
            Inlay.OnActivateMouseLeft += () =>
            {
                ActivateInInlay(Inlay);
            };
            Inlay.OnActivateMouseRight += () =>
            {
                Canvas.SetZIndex(Inlay, 0);
                int Index = IELInleys.IndexOf(Inlay);
                if (Index == ActivateIndex)
                {
                    if (ActivateIndex < InleysCount - 1)
                    {
                        ActivateInIndex(ActivateIndex + 1);
                    }
                    else
                    {
                        if (InleysCount > 1) ActivateInIndex(ActivateIndex - 1);
                        else
                        {
                            ActivateIndex = -1;
                            IELFrameBrowser.CloseFrame();
                        }
                    }
                }
                if (ActivateIndex != -1)
                {
                    IELInlay NextInlay = IELInleys[ActivateIndex];
                    if (NextInlay.TextSignature.Length > 0) NextInlay.IsAnimatedSignatureText = true;
                }

                ThicknessAnimation AanimationThickness = AnimationThickness.Clone();
                //AanimationThickness.BeginTime = TimeSpan.FromMilliseconds(800d);
                AanimationThickness.To = MarginDiactivateAlvay;
                AanimationThickness.Duration = TimeSpan.FromMilliseconds(800d);
                Inlay.BeginAnimation(MarginProperty, AanimationThickness);

                DoubleAnimation animationDouble = AnimationDouble.Clone();
                animationDouble.To = 0d;
                animationDouble.FillBehavior = FillBehavior.Stop;
                animationDouble.Completed += (sender, e) =>
                {
                    Inlay.Opacity = 0d;
                    GridMainInlays.Children.Remove(Inlay);
                    IELInleys.Remove(Inlay);
                    ScrollBar.MaxDown(1);
                };
                animationDouble.Duration = TimeSpan.FromMilliseconds(800d);
                Inlay.BeginAnimation(OpacityProperty, animationDouble);
            };
            Inlay.MouseWheel += (sender, e) =>
            {
                if (e.Delta > 0 && ScrollBar.Value != 0)
                {
                    IELInleys[ScrollBar.Value].IsAnimatedSignatureText = false;
                    UsingInlayAnimation(IELInleys[ScrollBar.Value], false);
                    ScrollBar.Up();
                    UsingInlayAnimation(IELInleys[ScrollBar.Value], true);
                }
                else if (e.Delta < 0 && ScrollBar.Value != ScrollBar.MaxValue)
                {
                    IELInleys[ScrollBar.Value].IsAnimatedSignatureText = false;
                    UsingInlayAnimation(IELInleys[ScrollBar.Value], false);
                    ScrollBar.Down();
                    UsingInlayAnimation(IELInleys[ScrollBar.Value], true);
                }
            };
            if (Signature != null)
            {
                Inlay.TextSignature = Signature;
                Inlay.IsAnimatedSignatureText = true;
            }
            IELInleys.Add(Inlay);
            ScrollBar.MaxUp(1);
            GridMainInlays.Children.Add(Inlay);
            if (Activate)
            {
                UsingInlayAnimation(Inlay, true);
                ActivateInInlay(Inlay);
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
            if (index.Value == ActivateIndex) return;
            UpdateTextNextBackPagesScroll(index.Value);
            IPageDefault Page = IELInleys[index].Page ?? throw new Exception("Объект заголовка не может быть без страницы!");
            if (ActivateIndex > -1)
            {
                IELInlay BackInlay = IELInleys[ActivateIndex];
                BackInlay.IsAnimatedSignatureText = false;
                UsingInlayAnimation(BackInlay, false);
            }
            IELInlay NextInlay = IELInleys[index];
            UsingInlayAnimation(NextInlay, true);
            IELFrameBrowser.NextPage(Page, index.Value < ActivateIndex ? IIELFrame.OrientationMove.Left : IIELFrame.OrientationMove.Right);
            ActivateIndex = index.Value;
        }

        /// <summary>
        /// Открыть страницу по элементу
        /// </summary>
        /// <param name="inlay">Открываемая вкладка страницы</param>
        /// <exception cref="Exception">Исключение при пустой странице в найденой вкладке</exception>
        public void ActivateInInlay(IELInlay inlay)
        {
            ActivateInIndex(IELInleys.IndexOf(inlay));
        }

        /// <summary>
        /// Анимировать вкладку
        /// </summary>
        /// <param name="Avtivate">Состояние активности вкладки</param>
        public void UsingInlayAnimation(IELInlay Inlay, bool Avtivate)
        {
            if (Avtivate) Inlay.Opacity = 0d;
            Canvas.SetZIndex(Inlay, Avtivate ? 1 : 0);

            ThicknessAnimation animationThickness = AnimationThickness.Clone();
            //animationThickness.BeginTime = TimeSpan.FromMilliseconds(800d);
            animationThickness.To = Avtivate ? MarginActivateAlvay : MarginDiactivateAlvay;
            animationThickness.Duration = TimeSpan.FromMilliseconds(800d);
            Inlay.BeginAnimation(MarginProperty, animationThickness);

            DoubleAnimation animationDouble = AnimationDouble.Clone();
            //animationDouble.BeginTime = TimeSpan.FromMilliseconds(800d);
            animationDouble.To = Avtivate ? 1d : 0d;
            animationDouble.Duration = TimeSpan.FromMilliseconds(800d);
            Inlay.BeginAnimation(OpacityProperty, animationDouble);
        }

        /// <summary>
        /// Сделать поиск страницы по типу
        /// </summary>
        /// <typeparam name="T">Тип страницы поиска</typeparam>
        /// <returns>Найденная страница</returns>
        public T? SearchPageType<T>() where T : IPageDefault
        {
            if (InleysCount == 0) return default;
            return (T?)IELInleys.First(
                (i) =>
                {
                    return i.Page?.PageName.Equals(typeof(T).Name) ?? false;
                }).Page;
        }

        //
        private void UpdateTextNextBackPagesScroll(int index)
        {
            if (index > 0)
            {
                TextBlockLeftNameInlay.Text = IELInleys[index - 1].Text;
            }
            else
            {
                TextBlockLeftNameInlay.Text = string.Empty;
            }
            if (index < InleysCount - 1 && index > -1)
            {
                TextBlockRightNameInlay.Text = IELInleys[index + 1].Text;
            }
            else
            {
                TextBlockRightNameInlay.Text = string.Empty;
            }
        }
    }
}
