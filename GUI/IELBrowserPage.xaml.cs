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
using System;

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
        private readonly List<IELInlay> IELInlays;

        /// <summary>
        /// Индекс активированной вкладки
        /// </summary>
        private int ActivateIndex = -1;

        /// <summary>
        /// Количество вкладок в браузере страниц
        /// </summary>
        public int InlaysCount => IELInlays.Count;

        /// <summary>
        /// Диактивированная позиция вкладки (слева)
        /// </summary>
        private readonly Thickness MarginDiactivateLeftInlay = new(4, 10, 20, 10);

        /// <summary>
        /// Диактивированная позиция вкладки (справа)
        /// </summary>
        private readonly Thickness MarginDiactivateRightInlay = new(20, 10, 4, 10);

        /// <summary>
        /// Активированная позиция вкладки
        /// </summary>
        private readonly Thickness MarginActivateInlay = new(8, 5, 8, 5);

        /// <summary>
        /// Скролл-бар вкладок
        /// </summary>
        private readonly CounterScrollBar ScrollBar;

        public IELBrowserPage()
        {
            InitializeComponent();
            ScrollBar = new(1);
            IELInlays = [];

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
                TextBlockNullPage.Foreground = color;
            };
            BackgroundSetting = new(BrushSettingQ.CreateStyle.Background);
            BorderBrushSetting = new(BrushSettingQ.CreateStyle.BorderBrush);
            ForegroundSetting = new(BrushSettingQ.CreateStyle.Foreground);

            TextBlockLeftNameInlay.Text = string.Empty;
            TextBlockRightNameInlay.Text = string.Empty;

            ScrollBar.ChangedValue += (NewValue) =>
            {
                UpdateTextNextBackPagesScroll(NewValue);
            };
        }

        //
        private IELInlay CreateInlay(IPageDefault Content, string Head, string? Signature = null)
        {
            IELInlay Inlay = new()
            {
                Text = Head,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = MarginDiactivateRightInlay,
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
                DeleteInlayPage(Inlay);
            };
            Inlay.MouseWheel += (sender, e) =>
            {
                if (e.Delta > 0 && ScrollBar.Value != 0)
                {
                    InlayAnimationClose(IELInlays[ScrollBar.Value], MarginDiactivateRightInlay);
                    ScrollBar.Up();
                    InlayAnimationOpen(IELInlays[ScrollBar.Value]);
                }
                else if (e.Delta < 0 && ScrollBar.Value != ScrollBar.MaxValue)
                {
                    InlayAnimationClose(IELInlays[ScrollBar.Value], MarginDiactivateLeftInlay);
                    ScrollBar.Down();
                    InlayAnimationOpen(IELInlays[ScrollBar.Value]);
                }
            };
            if (Signature != null)
            {
                Inlay.TextSignature = Signature;
                Inlay.IsAnimatedSignatureText = true;
            }
            return Inlay;
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
            foreach (IPageDefault? page in IELInlays.Select((i) => i.Page))
                if (page?.PageName.Equals(Content.PageName) ?? true) return;
            IELInlay inlay = CreateInlay(Content, Head, Signature);
            if (InlaysCount == 0)
            {
                DoubleAnimation animation = AnimationDouble.Clone();
                animation.Duration = TimeSpan.FromMilliseconds(300d);
                animation.To = 0d;
                TextBlockNullPage.BeginAnimation(OpacityProperty, animation);
            }
            IELInlays.Add(inlay);
            ScrollBar.MaxUp(1);
            GridMainInlays.Children.Add(inlay);
            if (Activate)
            {
                InlayAnimationOpen(inlay);
                ActivateInInlay(inlay);
            }
            else
            {
                inlay.Opacity = 0d;
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
            IPageDefault Page = IELInlays[index].Page ?? throw new Exception("Объект заголовка не может быть без страницы!");
            if (ActivateIndex > -1)
            {
                IELInlay BackInlay = IELInlays[ActivateIndex];
                BackInlay.UsedState = false;
                InlayAnimationClose(BackInlay);
            }
            IELInlay NextInlay = IELInlays[index];
            InlayAnimationOpen(NextInlay);
            NextInlay.UsedState = true;
            IELFrameBrowser.NextPage(Page, index.Value < ActivateIndex ? IIELFrame.OrientationMove.Left : IIELFrame.OrientationMove.Right);
            ActivateIndex = index.Value;
            ScrollBar.Value = index.Value;
        }

        /// <summary>
        /// Открыть страницу по элементу
        /// </summary>
        /// <param name="inlay">Открываемая вкладка страницы</param>
        /// <exception cref="Exception">Исключение при пустой странице в найденой вкладке</exception>
        public void ActivateInInlay(IELInlay inlay)
        {
            ActivateInIndex(IELInlays.IndexOf(inlay));
        }

        /// <summary>
        /// Анимировать вкладку
        /// </summary>
        /// <param name="Avtivate">Состояние активности вкладки</param>
        public void InlayAnimationOpen(IELInlay Inlay, Thickness? MarginStart = null)
        {
            Inlay.Opacity = 0d;
            Canvas.SetZIndex(Inlay, 1);

            ThicknessAnimation animationThickness = AnimationThickness.Clone();
            if (MarginStart != null) animationThickness.From = MarginStart.Value;
            animationThickness.To = MarginActivateInlay;
            animationThickness.Duration = TimeSpan.FromMilliseconds(800d);
            Inlay.BeginAnimation(MarginProperty, animationThickness);

            DoubleAnimation animationDouble = AnimationDouble.Clone();
            animationDouble.To = 1d;
            animationDouble.Duration = TimeSpan.FromMilliseconds(800d);
            Inlay.BeginAnimation(OpacityProperty, animationDouble);
        }

        /// <summary>
        /// Анимировать вкладку
        /// </summary>
        /// <param name="Avtivate">Состояние активности вкладки</param>
        public void InlayAnimationClose(IELInlay Inlay, Thickness? MarginComplete = null)
        {
            Canvas.SetZIndex(Inlay, 0);

            ThicknessAnimation animationThickness = AnimationThickness.Clone();
            //animationThickness.BeginTime = TimeSpan.FromMilliseconds(800d);
            animationThickness.To = MarginComplete != null ? MarginComplete.Value : MarginDiactivateLeftInlay;
            animationThickness.Duration = TimeSpan.FromMilliseconds(800d);
            Inlay.BeginAnimation(MarginProperty, animationThickness);

            DoubleAnimation animationDouble = AnimationDouble.Clone();
            //animationDouble.BeginTime = TimeSpan.FromMilliseconds(800d);
            animationDouble.To = 0d;
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
            if (InlaysCount == 0) return default;
            return (T?)IELInlays.First(
                (i) =>
                {
                    return i.Page?.PageName.Equals(typeof(T).Name) ?? false;
                }).Page;
        }

        //
        private void UpdateTextNextBackPagesScroll(int index)
        {
            if (InlaysCount > 1)
            {
                TextBlockLeftNameInlay.Text = index > 0 ? IELInlays[index - 1].Text : string.Empty;
                TextBlockRightNameInlay.Text = (index < InlaysCount - 1 && index > -1) ? IELInlays[index + 1].Text : string.Empty;
            }
            else if (InlaysCount == 1)
            {
                TextBlockLeftNameInlay.Text = string.Empty;
                TextBlockRightNameInlay.Text = string.Empty;
            }
        }

        private void DeleteInlayPage(IELInlay inlay)
        {
            int Index = IELInlays.IndexOf(inlay),
                IndexNext = NextIndex(Index, InlaysCount);
            if (Index == -1) return;
            IELInlay ActualInlay = IELInlays[Index];
            ActualInlay.SetPage<IPageDefault>(null);
            Canvas.SetZIndex(ActualInlay, 0);

            ThicknessAnimation AanimationThickness = AnimationThickness.Clone();
            //AanimationThickness.BeginTime = TimeSpan.FromMilliseconds(800d);
            AanimationThickness.To = MarginDiactivateRightInlay;
            AanimationThickness.Duration = TimeSpan.FromMilliseconds(800d);
            ActualInlay.BeginAnimation(MarginProperty, AanimationThickness);

            DoubleAnimation animationDouble = AnimationDouble.Clone();
            animationDouble.To = 0d;
            animationDouble.FillBehavior = FillBehavior.Stop;
            animationDouble.Completed += (sender, e) =>
            {
                ActualInlay.Opacity = 0d;
                GridMainInlays.Children.Remove(ActualInlay);
            };
            animationDouble.Duration = TimeSpan.FromMilliseconds(800d);
            ActualInlay.BeginAnimation(OpacityProperty, animationDouble);

            if (IndexNext == -1)
            {
                ActivateIndex = -1;
                IELFrameBrowser.CloseFrame();
            }
            else
            {
                IELInlay NextInlay = IELInlays[IndexNext];
                if (Index == ActivateIndex)
                {
                    ActivateInIndex(IndexNext);
                }
                else
                {
                    InlayAnimationOpen(NextInlay);
                }
                if (NextInlay.TextSignature.Length > 0) NextInlay.IsAnimatedSignatureText = true;
            }
            IELInlays.RemoveAt(Index);
            if (InlaysCount == 0)
            {
                DoubleAnimation animation = AnimationDouble.Clone();
                animation.Duration = TimeSpan.FromMilliseconds(300d);
                animation.To = 0.4d;
                TextBlockNullPage.BeginAnimation(OpacityProperty, animation);
            }
            ScrollBar.MaxDown(1);
            UpdateTextNextBackPagesScroll(IndexNext);
        }

        /// <summary>
        /// Узнать следующий индекс элемента
        /// </summary>
        /// <param name="ActualIndex">Текущий индекс</param>
        /// <param name="Count">Количество элементов</param>
        /// <returns>Ожидаемый индекс элемента</returns>
        private static int NextIndex(int ActualIndex, int Count)
        {
            if (ActualIndex < Count - 1) return ++ActualIndex;
            else if (Count > 1) return --ActualIndex;
            return -1;
        }
    }
}
