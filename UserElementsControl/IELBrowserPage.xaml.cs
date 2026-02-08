using IEL.CORE.Classes;
using IEL.CORE.Classes.Browser;
using IEL.UserElementsControl.Base;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IEL.UserElementsControl
{
    /// <summary>
    /// Логика взаимодействия для IELBrowserPage.xaml
    /// </summary>
    public partial class IELBrowserPage : UserControl
    {
        [LibraryImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool SetCursorPos(int X, int Y);

        /// <summary>
        /// Объект анимации Point значения
        /// </summary>
        private PointAnimation SourcePointAnimation = new()
        {
            Duration = TimeSpan.FromMilliseconds(1000d),
            EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 3.5d },
        };

        /// <summary>
        /// Объект анимации double значения
        /// </summary>
        private DoubleAnimation SourceDoubleAnimation = new()
        {
            Duration = TimeSpan.FromMilliseconds(1000d),
            EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 3.5d },
        };

        /// <summary>
        /// Объект анимации Thickness значения
        /// </summary>
        private ThicknessAnimation SourceThicknessAnimation = new()
        {
            Duration = TimeSpan.FromMilliseconds(1000d),
            EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 3.5d },
        };

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
        public delegate void DelegateDescriptionInlayHandler(FrameworkElement Element, string? Text);

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
        /// Событие Добавления новой вкладки
        /// </summary>
        public event DelegateVoidHandler? EventAddInlay;

        /// <summary>
        /// Событие открытия действий над выбранной вкладкой
        /// </summary>
        public event ActiveActionInInlay? EventActiveActionInInlay;

        /// <summary>
        /// Активная вкладка в браузере
        /// </summary>
        public IELInlay? ActualInlay => ActivateIndex > -1 ? IELInlays[ActivateIndex] : null;

        #region BorderBrush
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty BorderBrushProperty =
            DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(IELBrowserPage),
                new(
                    (sender, e) =>
                    {
                        ((IELBrowserPage)sender).BorderMain.BorderBrush = (Brush)e.NewValue;
                        ((IELBrowserPage)sender).BorderMainPage.BorderBrush = (Brush)e.NewValue;
                    }));

        /// <summary>
        /// Цвет отображения границ элемента
        /// </summary>
        public new Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }
        #endregion

        /// <summary>
        /// Размер текста в элементе
        /// </summary>
        public new double FontSize
        {
            get => base.FontSize;
            set
            {
                TextBlockNullPage.FontSize = value;
                base.FontSize = value;
            }
        }

        /// <summary>
        /// Стиль текста в элементе
        /// </summary>
        public new FontFamily FontFamily
        {
            get => base.FontFamily;
            set
            {
                TextBlockNullPage.FontFamily = value;
                base.FontFamily = value;
            }
        }

        /// <summary>
        /// Константа размера ширины объекта страницы
        /// </summary>
        private const double WidthInlayDefault = 160.35d;

        /// <summary>
        /// Состояние нажатия на барьер прокрутки вкладок
        /// </summary>
        private bool ScrollMouseUse = false;

        /// <summary>
        /// Позиция мыши при захвате прокрутки вкладок
        /// </summary>
        private Point? PointSelectMouse;

        /// <summary>
        /// Значение отступа прокрутки при её выделении мышью
        /// </summary>
        private double? MarginLeftBorderScrollSelect;

        /// <summary>
        /// Коэффициент эквивалента прокрутки к изменении позиции элемента
        /// </summary>
        private double OneScrollMarginLeft = 1d;

        /// <summary>
        /// Инициализировать объект интерфейса отображения страничных объектов
        /// </summary>
        public IELBrowserPage()
        {
            InitializeComponent();
            TextBlockNullPage.Opacity = 0.4d;
            RectangleScrollbar.Opacity = 0d;
            RectangleScrollbar.IsEnabled = false;
            LinearGradientScroll.StartPoint = new(-0.1d, 0);
            LinearGradientScroll.EndPoint = new(1.1d, 0);
            IELInlays = [];

            #region ScrollViewerInlays
            GridMainScrollViewer.MouseWheel += (sender, e) =>
            { 
                ScrollViewerInlays.ScrollToHorizontalOffset(ScrollViewerInlays.HorizontalOffset + (e.Delta < 0 ? 28 : -28));
                ScrollViewerInlays.UpdateLayout();
                RectangleScrollbar.BeginAnimation(MarginProperty, null);
                if (OneScrollMarginLeft * ScrollViewerInlays.HorizontalOffset + RectangleScrollbar.Width <= GridMainButtons.ActualWidth)
                    RectangleScrollbar.Margin = new(OneScrollMarginLeft * ScrollViewerInlays.HorizontalOffset, 0, 0, 0);
                e.Handled = true;
            };
            GridMainScrollViewer.SizeChanged += (sender, e) =>
            {
                if (GridMainScrollViewer.ActualWidth > ScrollViewerInlays.ActualWidth)
                {
                    SourceDoubleAnimation.Duration = TimeSpan.FromMilliseconds(300d);
                    if (!RectangleScrollbar.IsEnabled)
                    {
                        SourcePointAnimation.Duration = TimeSpan.FromMilliseconds(300d);
                        RectangleScrollbar.Width = ScrollViewerInlays.ActualWidth;
                        SourceDoubleAnimation.To = 1d;
                        RectangleScrollbar.BeginAnimation(OpacityProperty, SourceDoubleAnimation, HandoffBehavior.SnapshotAndReplace);

                        SourcePointAnimation.To = new(0d, 0);
                        LinearGradientScroll.BeginAnimation(LinearGradientBrush.StartPointProperty, SourcePointAnimation, HandoffBehavior.SnapshotAndReplace);
                        SourcePointAnimation.To = new(1d, 0);
                        LinearGradientScroll.BeginAnimation(LinearGradientBrush.EndPointProperty, SourcePointAnimation, HandoffBehavior.SnapshotAndReplace);

                        RectangleScrollbar.IsEnabled = true;
                    }
                    double NextWidth = ScrollViewerInlays.ActualWidth - (GridMainScrollViewer.ActualWidth - ScrollViewerInlays.ActualWidth);
                    SourceDoubleAnimation.To = NextWidth;
                    RectangleScrollbar.BeginAnimation(WidthProperty, SourceDoubleAnimation, HandoffBehavior.SnapshotAndReplace);
                    OneScrollMarginLeft =
                        (GridMainInlays.ActualWidth - GridBorderScroll.ActualWidth) /
                        (GridBorderScroll.ActualWidth - NextWidth);
                    if (RectangleScrollbar.Margin.Left + NextWidth > GridBorderScroll.ActualWidth)
                    {
                        SourceThicknessAnimation.To = new(
                            RectangleScrollbar.Margin.Left - (RectangleScrollbar.Margin.Left + NextWidth - GridBorderScroll.ActualWidth),
                            0, 0, 0);
                        RectangleScrollbar.BeginAnimation(MarginProperty, SourceThicknessAnimation);
                    }
                }
                else if (RectangleScrollbar.IsEnabled)
                {
                    SourcePointAnimation.Duration = TimeSpan.FromMilliseconds(1000d);
                    SourceDoubleAnimation.Duration = TimeSpan.FromMilliseconds(300d);
                    SourceDoubleAnimation.To = 0d;
                    SourceThicknessAnimation.To = new(0);

                    SourcePointAnimation.To = new(-0.1d, 0);
                    LinearGradientScroll.BeginAnimation(LinearGradientBrush.StartPointProperty, SourcePointAnimation, HandoffBehavior.SnapshotAndReplace);
                    SourcePointAnimation.To = new(1.1d, 0);
                    LinearGradientScroll.BeginAnimation(LinearGradientBrush.EndPointProperty, SourcePointAnimation, HandoffBehavior.SnapshotAndReplace);

                    RectangleScrollbar.BeginAnimation(OpacityProperty, SourceDoubleAnimation, HandoffBehavior.SnapshotAndReplace);
                    RectangleScrollbar.BeginAnimation(MarginProperty, SourceThicknessAnimation);
                    RectangleScrollbar.IsEnabled = false;
                    ScrollViewerInlays.ScrollToHorizontalOffset(0d);
                    OneScrollMarginLeft = 1d;
                }
            };
            RectangleScrollbar.MouseDown += (sender, e) =>
            {
                if (ScrollMouseUse) return;
                PointSelectMouse = PointToScreen(Mouse.GetPosition(this));
                MarginLeftBorderScrollSelect = RectangleScrollbar.Margin.Left;
                //OneScrollMarginLeft =
                //    (GridMainInlays.ActualWidth - GridBorderScroll.ActualWidth) / 
                //    (GridBorderScroll.ActualWidth - RectangleScrollbar.Width);
                RectangleScrollbar.BeginAnimation(MarginProperty, null);
                ScrollMouseUse = true;
                SourceDoubleAnimation.Duration = TimeSpan.FromMilliseconds(600d);
                SourceDoubleAnimation.To = 5d;
                RectangleScrollbar.BeginAnimation(System.Windows.Shapes.Rectangle.StrokeThicknessProperty,
                    SourceDoubleAnimation, HandoffBehavior.SnapshotAndReplace);
            };
            BorderMainPage.MouseUp += (sender, e) =>
            {
                ScrollMouseUse = false;
                SourceDoubleAnimation.Duration = TimeSpan.FromMilliseconds(600d);
                SourceDoubleAnimation.To = 0d;
                RectangleScrollbar.BeginAnimation(System.Windows.Shapes.Rectangle.StrokeThicknessProperty,
                    SourceDoubleAnimation, HandoffBehavior.SnapshotAndReplace);
                e.Handled = true;
            };
            BorderMainPage.MouseMove += (sender, e) =>
            {
                if (ScrollMouseUse && PointSelectMouse.HasValue && MarginLeftBorderScrollSelect.HasValue)
                {
                    Point p = PointToScreen(Mouse.GetPosition(this));
                    double x = p.X - PointSelectMouse.Value.X;
                    if (MarginLeftBorderScrollSelect.Value + x >= 0 &&
                        MarginLeftBorderScrollSelect.Value + x + RectangleScrollbar.Width < GridBorderScroll.ActualWidth)
                    {
                        RectangleScrollbar.Margin = new(MarginLeftBorderScrollSelect.Value + p.X - PointSelectMouse.Value.X, 0, 0, 0);
                        ScrollViewerInlays.ScrollToHorizontalOffset(RectangleScrollbar.Margin.Left * OneScrollMarginLeft);
                    }
                    SetCursorPos((int)p.X, (int)PointSelectMouse.Value.Y);
                }
            };
            BorderMainPage.MouseLeave += (sender, e) =>
            {
                if (ScrollMouseUse)
                {
                    ScrollMouseUse = false;
                    SourceDoubleAnimation.Duration = TimeSpan.FromMilliseconds(600d);
                    SourceDoubleAnimation.To = 0d;
                    RectangleScrollbar.BeginAnimation(System.Windows.Shapes.Rectangle.StrokeThicknessProperty,
                        SourceDoubleAnimation, HandoffBehavior.SnapshotAndReplace);
                }
            };
            #endregion

            IELButtonAddInlay.OnActivateMouseLeft += (sender, e) => EventAddInlay?.Invoke();
        }

        #region IELButtonAddInlay
        /// <summary>
        /// Установить картинку для кнопки добавления вкладки
        /// </summary>
        /// <param name="Source"></param>
        public void SetSourceImageButtonAddInlay(ImageSource Source) => IELButtonAddInlay.Source = Source;

        /// <summary>
        /// Получить объект кнопки добавления вкладки
        /// </summary>
        public IELObjectBase GetButtonAddInlay() => IELButtonAddInlay;
        #endregion

        #region ManipulateInlayAdd
        /// <summary>
        /// Добавить новую страницу
        /// </summary>
        /// <param name="Content">Добавляемая страница в баузер страниц</param>
        /// <param name="Activate">Активировать сразу или нет страницу</param>
        public IELInlay? AddInlayPage(BrowserPage? Content, bool Activate = true)
        {
            if (Content == null) return null;
            IELInlay inlay = CreateInlay(Content);
            inlay.MouseRightButtonUp += (sender, e) => EventActiveActionInInlay?.Invoke(inlay);
            inlay.Margin = new(
                (InlaysCount > 0 ? IELInlays[^1].Margin.Left + WidthInlayDefault : 0),
                RowDefinitionMainInlays.Height.Value, 0, 0);
            if (InlaysCount == 0)
            {
                SourceDoubleAnimation.Duration = TimeSpan.FromMilliseconds(800d);
                SourceDoubleAnimation.To = 0d;
                TextBlockNullPage.BeginAnimation(OpacityProperty, SourceDoubleAnimation, HandoffBehavior.SnapshotAndReplace);
            }

            IELInlays.Add(inlay);
            GridMainInlays.Children.Add(inlay);
            AnimationNewInlayVisible(inlay);

            if (Activate) ActivateInlayIndex(IELInlays.Count - 1);
            return inlay;
        }

        /// <summary>
        /// Создать вкладку в браузере
        /// </summary>
        /// <param name="Content">Страница ссылки</param>
        /// <returns>Созданная вкладка</returns>
        private IELInlay CreateInlay(BrowserPage Content)
        {
            IELInlay Inlay = new()
            {
                Width = WidthInlayDefault,
                Text = Content.Title,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                BorderThickness = new(2),
                CornerRadius = new(8, 8, 0, 0),
                Padding = new(1, 4, 1, 0),
                Opacity = 0d,
            };
            Inlay.OnActivateCloseInlay += (sender, e) =>
            {
                if (ScrollMouseUse) return;
                Inlay.IsEnabled = false;
                DeleteInlayPage(Inlay, ActivateIndex == IELInlays.IndexOf(Inlay));
                EventCloseInlay?.Invoke();
            };

            Inlay.SetPage(Content);
            Inlay.MouseLeftButtonUp += (sender, e) =>
            {
                if (ScrollMouseUse)
                {
                    ScrollMouseUse = false;
                    return;
                }
                ActivateInlayInBrowserPage(Inlay.PageElement);
            };
            //Inlay.MouseHover += (sender, e) =>
            //{
            //    if (Inlay.PageElement == null) return;
            //    else if (Inlay.PageElement?.Description.Length == 0) return;
            //    EventOnDescriptionInlay?.Invoke(Inlay, Inlay.PageElement?.Description);
            //};
            Inlay.MouseLeave += (sender, e) =>
            {
                if (Inlay.PageElement?.Description.Length == 0) return;
                EventOffDescriptionInlay?.Invoke();
            };
            Inlay.MouseDown += (sender, e) =>
            {
                if (Inlay.PageElement?.Description.Length == 0) return;
                EventOffDescriptionInlay?.Invoke();
            };
            return Inlay;
        }
        #endregion

        #region ManipulateInlay
        /// <summary>
        /// Открыть страницу по индексу
        /// </summary>
        /// <param name="index">Индекс открываемой страницы</param>
        /// <exception cref="Exception">Исключение при пустой странице в найденой вкладке</exception>
        public void ActivateInlayIndex(Index index)
        {
            if (index.Value == ActivateIndex && IELInlays[index].SourceBackground.GetUsedState()) return;
            BrowserPage Page = IELInlays[index].PageElement ?? throw new Exception("Объект заголовка не может быть без страницы!");
            if (ActivateIndex > -1 && IELInlays.Count > ActivateIndex)
            {
                IELInlay BackInlay = IELInlays[ActivateIndex];
                BackInlay.SourceBackground.SetUsedState(false);
                UsingInlayAnimationActivate(BackInlay, false);
            }
            IELInlay NextInlay = IELInlays[index];
            UsingInlayAnimationActivate(NextInlay);
            NextInlay.SourceBackground.SetUsedState(true);
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
        #endregion

        #region ManipulateInlayAnimation
        /// <summary>
        /// Анимировать видимость новой вкладки
        /// </summary>
        /// <param name="Inlay">Вкладка которая анимируется</param>
        private void AnimationNewInlayVisible(IELInlay Inlay)
        {
            SourceDoubleAnimation.Duration = TimeSpan.FromMilliseconds(400d);
            SourceDoubleAnimation.To = 1d;
            Inlay.BeginAnimation(OpacityProperty, SourceDoubleAnimation, HandoffBehavior.SnapshotAndReplace);

            SourceThicknessAnimation.Duration = TimeSpan.FromMilliseconds(400d);
            SourceThicknessAnimation.To = new(Inlay.Margin.Left, 8, 0, 0);
            Inlay.BeginAnimation(MarginProperty, SourceThicknessAnimation, HandoffBehavior.SnapshotAndReplace);
        }

        /// <summary>
        /// Анимировать активацию вкладки
        /// </summary>
        /// <param name="Inlay">Вкладка которая анимируется</param>
        /// <param name="Activate">Состояние стремления вкладки</param>
        private void UsingInlayAnimationActivate(IELInlay Inlay, bool Activate = true)
        {
            if (!Activate) Inlay.PageElement?.EventUnfocusPage?.Invoke(Inlay.PageElement);
            SourceThicknessAnimation.To = new(Activate ? 0 : 4);
            Inlay.BeginAnimation(PaddingProperty, SourceThicknessAnimation, HandoffBehavior.Compose);
            //Inlay.Padding = Activate ? new(0) : new(4, 4, 4, 0);
        }
        #endregion

        /// <summary>
        /// Сделать поиск страниц по типу
        /// </summary>
        /// <typeparam name="T">Тип страницы поиска</typeparam>
        /// <returns>Найденные страницы</returns>
        public T?[]? SearchAllPageType<T>() where T : Page
        {
            if (InlaysCount == 0) return null;
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
        public T? SearchAnyPageType<T>() where T : Page
        {
            if (InlaysCount == 0) return null;
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
            if (IELInlays.IndexOf(inlay) is int Index && Index == -1) return;
            int IndexNext = NextIndex(Index, InlaysCount - 1);
            IELInlay ActualInlay = IELInlays[Index];
            ActualInlay.PageElement?.Dispose();
            Canvas.SetZIndex(ActualInlay, 0);

            DoubleAnimation animationDouble = SourceDoubleAnimation.Clone();
            animationDouble.FillBehavior = FillBehavior.Stop;
            animationDouble.Completed += (sender, e) =>
            {
                ActualInlay.Opacity = 0d;
                GridMainInlays.Children.Remove(ActualInlay);
            };
            IELInlays.RemoveAt(Index);
            SourceThicknessAnimation.Duration = TimeSpan.FromMilliseconds(400d);
            for (int i = Index; i < IELInlays.Count; i++)
            {
                SourceThicknessAnimation.To = new(IELInlays[i].Margin.Left - inlay.ActualWidth, 8, 0, 0);
                IELInlays[i].BeginAnimation(MarginProperty, SourceThicknessAnimation, HandoffBehavior.SnapshotAndReplace);
            }
            SourceThicknessAnimation.To = new(ActualInlay.Margin.Left, RowDefinitionMainInlays.Height.Value, 0, 0);
            ActualInlay.BeginAnimation(MarginProperty, SourceThicknessAnimation, HandoffBehavior.SnapshotAndReplace);
            ActualInlay.BeginAnimation(OpacityProperty, animationDouble);

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
                }
            }
            else if (ActivateIndex >= Index) ActivateIndex--;
            if (InlaysCount == 0)
            {
                SourceDoubleAnimation.Duration = TimeSpan.FromMilliseconds(800d);
                SourceDoubleAnimation.To = 0.4d;
                TextBlockNullPage.BeginAnimation(OpacityProperty, SourceDoubleAnimation, HandoffBehavior.SnapshotAndReplace);
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
