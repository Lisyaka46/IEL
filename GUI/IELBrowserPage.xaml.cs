using IEL.CORE.Classes;
using IEL.CORE.Classes.Browser;
using IEL.CORE.Classes.ObjectSettings;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELBrowserPage.xaml
    /// </summary>
    public partial class IELBrowserPage : UserControl
    {
        private IELObjectSetting _IELSettingObject = new();
        /// <summary>
        /// Настройка использования объекта
        /// </summary>
        public IELObjectSetting IELSettingObject
        {
            get => _IELSettingObject;
            set
            {
                value.BackgroundSetting.SetActionColorChanged((Spectrum, NewValue, Animated) =>
                {
                    if (Animated)
                    {
                        ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(NewValue);
                        BorderMain.Background.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                    }
                    else
                    {
                        SolidColorBrush color = new(NewValue);
                        BorderMain.Background = color;
                    }
                });
                value.BorderBrushSetting.SetActionColorChanged((Spectrum, NewValue, Animated) =>
                {
                    if (Animated)
                    {
                        ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(NewValue);
                        BorderMain.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                        BorderMainPage.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                    }
                    else
                    {
                        SolidColorBrush color = new(NewValue);
                        BorderMain.BorderBrush = color;
                        BorderMainPage.BorderBrush = color;
                    }
                });
                value.ForegroundSetting.SetActionColorChanged((Spectrum, NewValue, Animated) =>
                {
                    if (Animated)
                    {
                        ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(NewValue);
                        TextBlockNullPage.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                    }
                    else
                    {
                        SolidColorBrush color = new(NewValue);
                        TextBlockNullPage.Foreground = color;
                    }
                });
                _IELSettingObject = value;
            }
        }

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
        /// Массив активных вкладок браузера
        /// </summary>
        public ReadOnlyCollection<IELInlay> Inlays => IELInlays.AsReadOnly();

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
            IELSettingObject = new();
            QDataDefaultInlayBackground = new();
            QDataDefaultInlayBorderBrush = new();
            QDataDefaultInlayForeground = new();
            IELInlays = [];
            DefaultWidthNewInlay = 180d;

            _IELButtonAddInlay = new();
            GridMainButtons.Children.Add(_IELButtonAddInlay);
            Grid.SetColumn(_IELButtonAddInlay, 1);
            SizeChanged += (sender, e) =>
            {
                _ = e.NewSize.Width;
                double NewWidth = (e.NewSize.Width - 50) / IELInlays.Count;
                if (NewWidth > DefaultWidthNewInlay) NewWidth = DefaultWidthNewInlay;
                //if (GridMainInlays.ColumnDefinitions[0].Width.Value == NewWidth) return;
                UpdateWidthInlays(NewWidth);
            };
        }

        /// <summary>
        /// Создать вкладку в браузере
        /// </summary>
        /// <param name="Content">Страница ссылки</param>
        /// <param name="Head">Заголовок вкладки</param>
        /// <param name="Description">Сигнатура-описание вкладки</param>
        /// <returns>Созданная вкладка</returns>
        private IELInlay CreateInlay(BrowserPage Content)
        {
            IELInlay Inlay = new()
            {
                Text = Content.Title,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                //Margin = MarginDiactivateRightInlay,
                BorderThicknessBlock = new(2),
                CornerRadius = new(8, 8, 0, 0),
                IELSettingObject = new()
                {
                    AnimationMillisecond = 200,
                    BackgroundSetting = new(QDataDefaultInlayBackground),
                    BorderBrushSetting = new(QDataDefaultInlayBorderBrush),
                    ForegroundSetting = new(QDataDefaultInlayForeground),
                },
                Padding = new(4, 4, 4, 0),
            };
            Inlay.OnActivateCloseInlay += (sender, e, Key) =>
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
            Inlay.OnActivateMouseLeft += (sender, e, Key) =>
            {
                ActivateInlayInBrowserPage(Inlay.PageElement);
            };
            Inlay.IELSettingObject.MouseHover += (sender, e) =>
            {
                if (Inlay.PageElement == null) return;
                else if (Inlay.PageElement?.Description.Length == 0) return;
                EventOnDescriptionInlay?.Invoke(Inlay, Inlay.PageElement?.Description ?? string.Empty);
            };
            Inlay.BorderMain.MouseLeave += (sender, e) =>
            {
                if (Inlay.PageElement?.Description.Length == 0) return;
                EventOffDescriptionInlay?.Invoke();
            };
            Inlay.BorderMain.MouseDown += (sender, e) =>
            {
                if (Inlay.PageElement?.Description.Length == 0) return;
                EventOffDescriptionInlay?.Invoke();
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
        public void AddInlayPage(BrowserPage? Content, bool Activate = true)
        {
            if (Content == null) return;
            IELInlay inlay = CreateInlay(Content);
            inlay.OnActivateMouseRight += (sender, e, Key) => EventActiveActionInInlay?.Invoke(inlay);
            inlay.Opacity = 0d;
            if (InlaysCount == 0)
            {
                TextBlockNullPage.BeginAnimation(OpacityProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationDouble(0d));
            }
            IELInlays.Add(inlay);
            GridMainInlays.ColumnDefinitions.Add(
                new()
                {
                    Width = new(DefaultWidthNewInlay, GridUnitType.Pixel),
                    MaxWidth = 0d
                });
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

        ///// <summary>
        ///// Добавить новую страницу
        ///// </summary>
        ///// <param name="Content">Добавляемая страница в баузер страниц</param>
        ///// <param name="Head">Наименование вкладки</param>
        ///// <param name="Signature">Подпись вкладки</param>
        ///// <param name="Activate">Активировать сразу или нет страницу</param>
        //public void AddInlayPage(BrowserPage? Content, bool Activate = true)
        //{
        //    if (Content == null) return;
        //    IELInlay inlay = CreateInlay(Content);
        //    inlay.OnActivateMouseRight += (Key) => EventActiveActionInInlay?.Invoke(inlay);
        //    inlay.Opacity = 0d;
        //    if (InlaysCount == 0)
        //    {
        //        TextBlockNullPage.BeginAnimation(OpacityProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationDouble(0d));
        //    }
        //    IELInlays.Add(inlay);
        //    ColumnDefinition column = new()
        //    {
        //        Width = new(DefaultWidthNewInlay, GridUnitType.Pixel),
        //        MaxWidth = 0d
        //    };
        //    GridMainInlays.ColumnDefinitions.Add(column);
        //    GridMainInlays.Children.Add(inlay);
        //    Grid.SetColumn(inlay, IELInlays.Count - 1);

        //    double NewWidth = GridMainInlays.ActualWidth / IELInlays.Count;
        //    if (NewWidth > DefaultWidthNewInlay) NewWidth = DefaultWidthNewInlay;
        //    UpdateWidthInlays(NewWidth);

        //    UsingInlayAnimationVisible(inlay);

        //    if (Activate) ActivateInInlay(inlay);
        //}

        /// <summary>
        /// Обновить длинну вкладок браузера
        /// </summary>
        /// <param name="NewWidth">Длинна к которой обновляется размер</param>
        private void UpdateWidthInlays(double NewWidth)
        {
            DoubleAnimation animation = IELSettingObject.ObjectAnimateSetting.GetAnimationDouble();
            animation.Duration = TimeSpan.FromMilliseconds(300d);
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
            BrowserPage Page = IELInlays[index].PageElement ?? throw new Exception("Объект заголовка не может быть без страницы!");
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
        /// <param name="Page">Открываемая вкладка страницы</param>
        /// <exception cref="Exception">Исключение при пустой странице в найденой вкладке</exception>
        public void ActivateInlayInBrowserPage(BrowserPage? Page)
        {
            try
            {
                if (Page == null) return;
                BrowserPage?[] Pages = [.. IELInlays.Select((i) => i.PageElement)];
                ActivateInlayIndex(Array.IndexOf(Pages, Page));
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
            Inlay.BeginAnimation(OpacityProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationDouble(
                Visible ? 1d : 0d, TimeSpan.FromMilliseconds(DurationMillisecond)));
        }

        /// <summary>
        /// Анимировать активацию вкладки
        /// </summary>
        /// <param name="Inlay">Вкладка которая анимируется</param>
        /// <param name="Activate">Состояние стремления вкладки</param>
        /// <param name="DurationMillisecond">Количество миллисекунд для анимации</param>
        private void UsingInlayAnimationActivate(IELInlay Inlay, bool Activate = true, double DurationMillisecond = 800d)
        {
            if (!Activate) Inlay.PageElement?.EventUnfocusPage?.Invoke(Inlay.PageElement);
            Inlay.BeginAnimation(PaddingProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationThickness(
                Activate ? new(0) : new(4, 4, 4, 0), TimeSpan.FromMilliseconds(DurationMillisecond)));
        }

        /// <summary>
        /// Сделать поиск страниц по типу
        /// </summary>
        /// <typeparam name="T">Тип страницы поиска</typeparam>
        /// <returns>Найденные страницы</returns>
        public T?[]? SearchAllPageType<T>() where T : Page
        {
            if (InlaysCount == 0) return default;
            List<T?> values = [];
            foreach (IELInlay Inlay in IELInlays)
            {
                if (Inlay.PageElement?.PageContent.GetType() == typeof(T)) values.Add((T?)Inlay.PageElement?.PageContent);
            }
            return values.Count == 0 ? null : [.. values];
        }

        /// <summary>
        /// Сделать поиск страницы по типу
        /// </summary>
        /// <typeparam name="T">Тип страницы поиска</typeparam>
        /// <returns>Найденная страница</returns>
        public T? SearchPageType<T>() where T : Page
        {
            if (InlaysCount == 0) return default;
            foreach (IELInlay Inlay in IELInlays)
            {
                if (Inlay.PageElement?.PageContent.GetType() == typeof(T)) return (T?)Inlay.PageElement?.PageContent;
            }
            return null;
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
            ActualInlay.PageElement?.Dispose();
            Canvas.SetZIndex(ActualInlay, 0);

            double NewWidth = GridMainInlays.ActualWidth / (IELInlays.Count - 1);
            if (NewWidth > DefaultWidthNewInlay) NewWidth = DefaultWidthNewInlay;
            UpdateWidthInlays(NewWidth);

            DoubleAnimation animationDouble = IELSettingObject.ObjectAnimateSetting.GetAnimationDouble();
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
                TextBlockNullPage.BeginAnimation(OpacityProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationDouble(0.4d));
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
