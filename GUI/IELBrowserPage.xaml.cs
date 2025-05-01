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
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.IO;
using static IEL.Interfaces.Core.IQData;
using IEL.Classes.Browser;

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
                BackgroundChangeDefaultColor.Invoke(StateSpectrum.Default, value.Default);
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
                BorderBrushChangeDefaultColor.Invoke(StateSpectrum.Default, value.Default);
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

        /// <summary>
        /// Данные об стиле цветов фона вкладок в браузере
        /// </summary>
        public QData QDataDefaultInlayBackground { get; set; }

        /// <summary>
        /// Данные об стиле цветов границ вкладок в браузере
        /// </summary>
        public QData QDataDefaultInlayBorderBrush { get; set; }

        /// <summary>
        /// Данные об стиле цветов текста вкладок в браузере
        /// </summary>
        public QData QDataDefaultInlayForeground { get; set; }

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
        /// Делегат события без параметров
        /// </summary>
        public delegate void DelegateVoidHandler();

        /// <summary>
        /// Делегат события взаимодействия с описанием вкладки
        /// </summary>
        public delegate void DelegateDescriptionInlayHandler(FrameworkElement Element, string Text);

        /// <summary>
        /// Делегат события активации действий над выбранной вкладкой
        /// </summary>
        public delegate void ActiveActionInInlay(IELInlay Inlay);

        /// <summary>
        /// Событие закрытия последней вкладки браузера
        /// </summary>
        public event DelegateVoidHandler? EventCloseBrowser;

        /// <summary>
        /// Событие изменения активной вкладки
        /// </summary>
        public event DelegateVoidHandler? EventChangeActiveInlay;

        /// <summary>
        /// Событие показания описания вкладки
        /// </summary>
        public event DelegateDescriptionInlayHandler? EventOnDescriptionInlay;

        /// <summary>
        /// Событие скрытия описания вкладки
        /// </summary>
        public event DelegateVoidHandler? EventOffDescriptionInlay;

        /// <summary>
        /// Событие закрытия вкладки
        /// </summary>
        public event DelegateVoidHandler? EventCloseInlay;

        /// <summary>
        /// Событие открытия действий над выбранной вкладкой
        /// </summary>
        public event ActiveActionInInlay? EventActiveActionInInlay;



        private IELButtonImage _IELButtonAddInlay;

        /// <summary>
        /// Кнопка добавления новой вкладки
        /// </summary>
        public IELButtonImage IELButtonAddInlay
        {
            get => _IELButtonAddInlay;
            set
            {
                if (_IELButtonAddInlay != null)
                {
                    GridMainButtons.Children.Remove(_IELButtonAddInlay);
                }
                _IELButtonAddInlay = value;
                GridMainButtons.Children.Add(_IELButtonAddInlay);
                Grid.SetColumn(_IELButtonAddInlay, 1);
            }
        }

        /// <summary>
        /// Активная вкладка в браузере
        /// </summary>
        public IELInlay? ActualInlay => ActivateIndex > -1 ? IELInlays[ActivateIndex] : null;

        /// <summary>
        /// Значение длинны новой вкладки по умолчанию
        /// </summary>
        public double DefaultWidthNewInlay { get; set; }

        public IELBrowserPage()
        {
            InitializeComponent();
            QDataDefaultInlayBackground = new();
            QDataDefaultInlayBorderBrush = new();
            QDataDefaultInlayForeground = new();
            IELInlays = [];
            DefaultWidthNewInlay = 180d;

            _IELButtonAddInlay = new();
            GridMainButtons.Children.Add(_IELButtonAddInlay);
            Grid.SetColumn(_IELButtonAddInlay, 1);

            AnimationMillisecond = 100;
            BackgroundChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                BorderMain.Background = color;
            };
            BorderBrushChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                BorderMain.BorderBrush = color;
                BorderMainPage.BorderBrush = color;
            };
            ForegroundChangeDefaultColor = (Spectrum, Value) =>
            {
                if ((Spectrum == StateSpectrum.Default && !IsEnabled) ||
                (Spectrum == StateSpectrum.NotEnabled && IsEnabled)) return;
                SolidColorBrush color = new(Value);
                TextBlockNullPage.Foreground = color;
            };
            BackgroundSetting = new(BrushSettingQ.CreateStyle.Background);
            BorderBrushSetting = new(BrushSettingQ.CreateStyle.BorderBrush);
            ForegroundSetting = new(BrushSettingQ.CreateStyle.Foreground);
        }

        /// <summary>
        /// Создать вкладку в браузере
        /// </summary>
        /// <param name="Content">Страница ссылки</param>
        /// <param name="Head">Заголовок вкладки</param>
        /// <param name="Signature">Сигнатура-описание вкладки</param>
        /// <returns>Созданная вкладка</returns>
        private IELInlay CreateInlay(BrowserPage Content, string Head, string Signature)
        {
            IELInlay Inlay = CreateInlay(Content, Head);
            Inlay.TextSignature = Signature;
            Inlay.MouseHover += (sender, e) =>
            {
                if (Inlay.TextSignature.Length == 0) return;
                EventOnDescriptionInlay?.Invoke(Inlay, Inlay.TextSignature);
            };
            Inlay.BorderMain.MouseLeave += (sender, e) =>
            {
                if (Inlay.TextSignature.Length == 0) return;
                EventOffDescriptionInlay?.Invoke();
            };
            Inlay.BorderMain.MouseDown += (sender, e) =>
            {
                if (Inlay.TextSignature.Length == 0) return;
                EventOffDescriptionInlay?.Invoke();
            };
            return Inlay;
        }

        /// <summary>
        /// Создать вкладку в браузере
        /// </summary>
        /// <param name="Content">Страница ссылки</param>
        /// <param name="Head">Заголовок вкладки</param>
        /// <param name="Signature">Сигнатура-описание вкладки</param>
        /// <returns>Созданная вкладка</returns>
        private IELInlay CreateInlay(BrowserPage Content, string Head)
        {
            IELInlay Inlay = new()
            {
                Text = Head,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                //Margin = MarginDiactivateRightInlay,
                BorderThicknessBlock = new(2),
                CornerRadius = new(8, 8, 0, 0),
                AnimationMillisecond = 200,
                Padding = new(4, 4, 4, 0),
            };
            Inlay.BackgroundSetting.ColorData = QDataDefaultInlayBackground;
            Inlay.BorderBrushSetting.ColorData = QDataDefaultInlayBorderBrush;
            Inlay.ForegroundSetting.ColorData = QDataDefaultInlayForeground;
            Inlay.OnActivateCloseInlay += () =>
            {
                DeleteInlayPage(Inlay, ActivateIndex == IELInlays.IndexOf(Inlay));
                EventCloseInlay?.Invoke();
            };

            BitmapImage bitmap = new();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(Properties.Resources.Cross);
            bitmap.EndInit();
            Inlay.SourceCloseButtonImage = bitmap;

            Inlay.SetPage(Content);
            Inlay.OnActivateMouseLeft += () =>
            {
                ActivateInInlay(Inlay);
            };
            return Inlay;
        }

        /// <summary>
        /// Добавить новую страницу
        /// </summary>
        /// <param name="Content">Добавляемая страница в баузер страниц</param>
        /// <param name="Head">Наименование вкладки</param>
        /// <param name="Signature">Подпись вкладки</param>
        /// <param name="Activate">Активировать сразу или нет страницу</param>
        public void AddInlayPage(BrowserPage Content, string Head, bool Activate = true)
        {
            foreach (BrowserPage? page in IELInlays.Select((i) => i.Page))
                if (page?.PageContent.GetType().Equals(Content.GetType()) ?? true) return;
            DoubleAnimation animation = AnimationDouble.Clone();
            animation.Duration = TimeSpan.FromMilliseconds(300d);
            IELInlay inlay = CreateInlay(Content, Head);
            inlay.OnActivateMouseRight += () => EventActiveActionInInlay?.Invoke(inlay);
            inlay.Opacity = 0d;
            if (InlaysCount == 0)
            {
                animation.To = 0d;
                TextBlockNullPage.BeginAnimation(OpacityProperty, animation);
            }
            IELInlays.Add(inlay);
            if (IELInlays.Count > GridMainInlays.ColumnDefinitions.Count)
            {
                GridMainInlays.ColumnDefinitions.Add(
                    new()
                    {
                        Width = new(DefaultWidthNewInlay, GridUnitType.Pixel),
                        MaxWidth = 0d
                    });
            }
            GridMainInlays.Children.Add(inlay);
            Grid.SetColumn(inlay, IELInlays.Count - 1);

            double NewWidth = GridMainInlays.ActualWidth / IELInlays.Count;
            if (NewWidth > DefaultWidthNewInlay) NewWidth = DefaultWidthNewInlay;
            UpdateWidthInlays(NewWidth);

            UsingInlayAnimationVisible(inlay);

            if (Activate)
            {
                ActivateInlayIndex(IELInlays.Count - 1);
            }
        }

        /// <summary>
        /// Добавить новую страницу
        /// </summary>
        /// <param name="Content">Добавляемая страница в баузер страниц</param>
        /// <param name="Head">Наименование вкладки</param>
        /// <param name="Signature">Подпись вкладки</param>
        /// <param name="Activate">Активировать сразу или нет страницу</param>
        public void AddInlayPage(BrowserPage? Content, string Head, string Signature, bool Activate = true)
        {
            if (Content == null) return;
            foreach (BrowserPage? page in IELInlays.Select((i) => i.Page))
                if (page?.PageContent.GetType().Equals(Content.GetType()) ?? true) return;
            DoubleAnimation animation = AnimationDouble.Clone();
            animation.Duration = TimeSpan.FromMilliseconds(300d);
            IELInlay inlay = CreateInlay(Content, Head, Signature);
            inlay.OnActivateMouseRight += () => EventActiveActionInInlay?.Invoke(inlay);
            inlay.Opacity = 0d;
            IELInlays.Add(inlay);
            ColumnDefinition column = new()
            {
                Width = new(DefaultWidthNewInlay, GridUnitType.Pixel),
                MaxWidth = 0d
            };
            if (InlaysCount == 0)
            {
                animation.To = 0d;
                TextBlockNullPage.BeginAnimation(OpacityProperty, animation);
            }
            GridMainInlays.ColumnDefinitions.Add(column);
            GridMainInlays.Children.Add(inlay);
            Grid.SetColumn(inlay, IELInlays.Count - 1);

            double NewWidth = GridMainInlays.ActualWidth / IELInlays.Count;
            if (NewWidth > DefaultWidthNewInlay) NewWidth = DefaultWidthNewInlay;
            UpdateWidthInlays(NewWidth);

            UsingInlayAnimationVisible(inlay);

            if (Activate) ActivateInInlay(inlay);
        }

        /// <summary>
        /// Обновить длинну вкладок браузера
        /// </summary>
        /// <param name="NewWidth">Длинна к которой обновляется размер</param>
        private void UpdateWidthInlays(double NewWidth)
        {
            DoubleAnimation animation = AnimationDouble.Clone();
            animation.Duration = TimeSpan.FromMilliseconds(400d);
            animation.To = NewWidth;
            for (int i = 0; i < IELInlays.Count; i++)
            {
                //definition.Width = GridLength.Auto;
                Storyboard storyboard = new();
                storyboard.Children.Add(animation);
                Storyboard.SetTarget(animation, GridMainInlays.ColumnDefinitions[Grid.GetColumn(IELInlays[i])]);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(ColumnDefinition.MaxWidth)"));
                storyboard.Begin();
                //definition.BeginAnimation(, animation);
            }
        }

        /// <summary>
        /// Открыть страницу по индексу
        /// </summary>
        /// <param name="index">Индекс открываемой страницы</param>
        /// <exception cref="Exception">Исключение при пустой странице в найденой вкладке</exception>
        public void ActivateInlayIndex(Index index)
        {
            if (index.Value == ActivateIndex && IELInlays[index].UsedState) return;
            BrowserPage Page = IELInlays[index].Page ?? throw new Exception("Объект заголовка не может быть без страницы!");
            if (ActivateIndex > -1 && IELInlays.Count > ActivateIndex)
            {
                IELInlay BackInlay = IELInlays[ActivateIndex];
                BackInlay.UsedState = false;
                UsingInlayAnimationActivate(BackInlay, false);
            }
            IELInlay NextInlay = IELInlays[index];
            UsingInlayAnimationActivate(NextInlay);
            NextInlay.UsedState = true;
            MainPageController.NextPage(Page.PageContent, index.Value >= ActivateIndex);
            ActivateIndex = index.Value;
            Page.EventFocusPage?.Invoke(Page);
            EventChangeActiveInlay?.Invoke();
        }

        /// <summary>
        /// Открыть страницу по элементу
        /// </summary>
        /// <param name="inlay">Открываемая вкладка страницы</param>
        /// <exception cref="Exception">Исключение при пустой странице в найденой вкладке</exception>
        public void ActivateInInlay(IELInlay inlay)
        {
            try
            {
                ActivateInlayIndex(IELInlays.IndexOf(inlay));
            }
            catch { }
        }

        /// <summary>
        /// Анимировать видимость вкладки
        /// </summary>
        /// <param name="Inlay">Вкладка которая анимируется</param>
        /// <param name="Visible">Состояние стремления вкладки</param>
        /// <param name="DurationMillisecond">Количество миллисекунд для анимации</param>
        private void UsingInlayAnimationVisible(IELInlay Inlay, bool Visible = true, double DurationMillisecond = 800d)
        {
            Canvas.SetZIndex(Inlay, Visible ? 1 : 0);
            DoubleAnimation animation = AnimationDouble.Clone();
            animation.Duration = TimeSpan.FromMilliseconds(DurationMillisecond);
            animation.To = Visible ? 1d : 0d;
            Inlay.BeginAnimation(OpacityProperty, animation);
        }

        /// <summary>
        /// Анимировать активацию вкладки
        /// </summary>
        /// <param name="Inlay">Вкладка которая анимируется</param>
        /// <param name="Activate">Состояние стремления вкладки</param>
        /// <param name="DurationMillisecond">Количество миллисекунд для анимации</param>
        private void UsingInlayAnimationActivate(IELInlay Inlay, bool Activate = true, double DurationMillisecond = 800d)
        {
            if (!Activate) Inlay.Page?.EventUnfocusPage?.Invoke(Inlay.Page);
            ThicknessAnimation animation = AnimationThickness.Clone();
            animation.Duration = TimeSpan.FromMilliseconds(DurationMillisecond);
            animation.To = Activate ? new(0) : new(4, 4, 4, 0);
            Inlay.BeginAnimation(PaddingProperty, animation);
        }

        /// <summary>
        /// Сделать поиск страницы по типу
        /// </summary>
        /// <typeparam name="T">Тип страницы поиска</typeparam>
        /// <returns>Найденная страница</returns>
        public T? SearchPageType<T>() where T : BrowserPage
        {
            if (InlaysCount == 0) return default;
            BrowserPage? page = IELInlays.FirstOrDefault((i) => { return i.Page?.PageName.Equals(typeof(T).Name) ?? false; })?.Page;
            return page != null ? (T?)page : default;
        }

        /// <summary>
        /// Удалить вкладку в браузере
        /// </summary>
        /// <param name="inlay">Объект вкладки</param>
        /// <param name="ActivateNextInlay">Активировать ли следующую после удалённой вкладки вкладку</param>
        public void DeleteInlayPage(IELInlay inlay, bool ActivateNextInlay = true)
        {
            int Index = IELInlays.IndexOf(inlay);
            if (Index == -1) return;
            int IndexNext = NextIndex(Index, InlaysCount - 1), IndexColumn = Grid.GetColumn(inlay);
            IELInlay ActualInlay = IELInlays[Index];
            GridMainInlays.ColumnDefinitions[IndexColumn].IsEnabled = false;
            //ActualInlay.IsEnabled = false;
            ActualInlay.SetPage<BrowserPage>(null);
            Canvas.SetZIndex(ActualInlay, 0);

            double NewWidth = GridMainInlays.ActualWidth / IELInlays.Count(i => !i.IsEnabled);
            if (NewWidth > DefaultWidthNewInlay) NewWidth = DefaultWidthNewInlay;
            UpdateWidthInlays(NewWidth);

            DoubleAnimation animationDouble = AnimationDouble.Clone();
            //GridMainInlays.ColumnDefinitions[Index].MaxWidth = ActualInlay.ActualWidth;
            //animationDouble.From = ActualInlay.ActualWidth;
            animationDouble.To = 0d;
            animationDouble.FillBehavior = FillBehavior.HoldEnd;
            animationDouble.Duration = TimeSpan.FromMilliseconds(400d);
            Storyboard storyboard = new();
            storyboard.Children.Add(animationDouble);
            storyboard.FillBehavior = FillBehavior.Stop;
            storyboard.Completed += (sender, e) =>
            {
                if (Index < IELInlays.Count)
                {
                    while (GridMainInlays.ColumnDefinitions[IndexColumn].IsEnabled) IndexColumn--;
                    GridMainInlays.ColumnDefinitions.RemoveAt(IndexColumn);
                    for (int i = Index; i < IELInlays.Count; i++)
                    {
                        if (IELInlays[i].IsEnabled) Grid.SetColumn(IELInlays[i], Grid.GetColumn(IELInlays[i]) - 1);
                    }
                }
            };
            Storyboard.SetTarget(animationDouble, GridMainInlays.ColumnDefinitions[IndexColumn]);
            Storyboard.SetTargetProperty(animationDouble, new PropertyPath("(ColumnDefinition.MaxWidth)"));
            storyboard.Begin();

            animationDouble.FillBehavior = FillBehavior.Stop;
            animationDouble.Completed += (sender, e) =>
            {
                ActualInlay.Opacity = 0d;
                //Grid.SetColumnSpan(ActualInlay, 2);
                //GridMainInlays.ColumnDefinitions[Index].MaxWidth = double.MaxValue;
                GridMainInlays.Children.Remove(ActualInlay);
                //GridMainInlays.Children.Remove(ActualInlay);
                //Grid.SetColumnSpan(ActualInlay, 1);
            };
            IELInlays.RemoveAt(Index);
            ActualInlay.BeginAnimation(OpacityProperty, animationDouble);

            //ActualInlay.BeginAnimation(OpacityProperty, animationDouble);

            if (ActivateNextInlay)
            {
                if (IndexNext == -1)
                {
                    ActivateIndex = -1;
                    MainPageController.ClosePage();
                    EventCloseBrowser?.Invoke();
                }
                else
                {
                    ActivateInlayIndex(IndexNext);
                    //if (NextInlay.TextSignature.Length > 0) NextInlay.IsAnimatedSignatureText = true;
                }
            }
            else if (ActivateIndex >= Index) ActivateIndex--;
            if (InlaysCount == 0)
            {
                DoubleAnimation animation = AnimationDouble.Clone();
                animation.Duration = TimeSpan.FromMilliseconds(300d);
                animation.To = 0.4d;
                TextBlockNullPage.BeginAnimation(OpacityProperty, animation);
            }
        }

        /// <summary>
        /// Узнать следующий индекс элемента
        /// </summary>
        /// <param name="ActualIndex">Текущий индекс</param>
        /// <param name="Count">Количество элементов</param>
        /// <returns>Ожидаемый индекс элемента</returns>
        private static int NextIndex(int ActualIndex, int Count) => ActualIndex < Count ? ActualIndex : --ActualIndex;
    }
}
