using IEL.CORE.BaseUserControls;
using IEL.CORE.Classes;
using IEL.CORE.Classes.Browser;
using IEL.CORE.Classes.ObjectSettings;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

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

        /// <summary>
        /// Значение длинны новой вкладки по умолчанию
        /// </summary>
        public double DefaultWidthNewInlay { get; set; }

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
        /// Инициализировать объект интерфейса отображения страничных объектов
        /// </summary>
        public IELBrowserPage()
        {
            InitializeComponent();
            IELSettingObject = new();
            QDataDefaultInlayBackground = new();
            QDataDefaultInlayBorderBrush = new();
            QDataDefaultInlayForeground = new();
            IELInlays = [];
            DefaultWidthNewInlay = 180d;

            GridMainInlays.MouseWheel += (sender, e) =>
            {
                ScrollViewerInlays.ScrollToHorizontalOffset(ScrollViewerInlays.HorizontalOffset + (e.Delta < 0 ? 28 : -28));
                e.Handled = true;
            };

            IELButtonAddInlay.OnActivateMouseLeft += (sender, e, Key) => EventAddInlay?.Invoke();
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
            inlay.OnActivateMouseRight += (sender, e, Key) => EventActiveActionInInlay?.Invoke(inlay);
            inlay.Opacity = 0d;
            if (InlaysCount == 0)
            {
                TextBlockNullPage.BeginAnimation(OpacityProperty,
                    IELSettingObject.ObjectAnimateSetting.GetAnimationDouble(0d), HandoffBehavior.SnapshotAndReplace);
            }
            if (inlay.ActualWidth > DefaultWidthNewInlay)
            {
                ColorAnimation animation = IELSettingObject.ObjectAnimateSetting.GetAnimationColor();
                animation.From = Colors.Black;
                animation.To = System.Windows.Media.Color.FromArgb(0, 0, 0, 0);
                animation.Duration = TimeSpan.FromMilliseconds(500d);
            }
            inlay.Width = DefaultWidthNewInlay;
            inlay.Margin = new(IELInlays.Count * DefaultWidthNewInlay, RowDefinitionMainInlays.Height.Value, 0, 0);

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
                Text = Content.Title,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Stretch,
                //Margin = MarginDiactivateRightInlay,
                BorderThickness = new(2),
                CornerRadius = new(8, 8, 0, 0),
                Padding = new(1, 4, 1, 0),
            };
            Inlay.Background = QDataDefaultInlayBackground;
            Inlay.BorderBrush = QDataDefaultInlayBorderBrush;
            Inlay.Foreground = QDataDefaultInlayForeground;
            Inlay.OnActivateCloseInlay += (sender, e, Key) =>
            {
                DeleteInlayPage(Inlay, ActivateIndex == IELInlays.IndexOf(Inlay));
                EventCloseInlay?.Invoke();
            };

            Inlay.SetPage(Content);
            Inlay.OnActivateMouseLeft += (sender, e, Key) =>
            {
                ActivateInlayInBrowserPage(Inlay.PageElement);
            };
            Inlay.MouseHover += (sender, e) =>
            {
                if (Inlay.PageElement == null) return;
                else if (Inlay.PageElement?.Description.Length == 0) return;
                EventOnDescriptionInlay?.Invoke(Inlay, Inlay.PageElement?.Description ?? string.Empty);
            };
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
            Inlay.BeginAnimation(OpacityProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationDouble(
                1d, TimeSpan.FromMilliseconds(800d)), HandoffBehavior.SnapshotAndReplace);
            Inlay.BeginAnimation(MarginProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationThickness(
                new(Inlay.Margin.Left, 0, 0, 0), TimeSpan.FromMilliseconds(800d)), HandoffBehavior.SnapshotAndReplace);
        }

        /// <summary>
        /// Анимировать активацию вкладки
        /// </summary>
        /// <param name="Inlay">Вкладка которая анимируется</param>
        /// <param name="Activate">Состояние стремления вкладки</param>
        private void UsingInlayAnimationActivate(IELInlay Inlay, bool Activate = true)
        {
            if (!Activate) Inlay.PageElement?.EventUnfocusPage?.Invoke(Inlay.PageElement);
            Inlay.BeginAnimation(PaddingProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationThickness(
                Activate ? new(0) : new(4, 4, 4, 0), TimeSpan.FromMilliseconds(800d)), HandoffBehavior.SnapshotAndReplace);
        }
        #endregion

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
        public T? SearchAnyPageType<T>() where T : Page
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
            ActualInlay.PageElement?.Dispose();
            Canvas.SetZIndex(ActualInlay, 0);

            DoubleAnimation animationDouble = IELSettingObject.ObjectAnimateSetting.GetAnimationDouble();
            animationDouble.FillBehavior = FillBehavior.Stop;
            animationDouble.Completed += (sender, e) =>
            {
                ActualInlay.Opacity = 0d;
                GridMainInlays.Children.Remove(ActualInlay);
            };
            IELInlays.RemoveAt(Index);
            if (IndexNext > -1)
            {
                for (int i = IndexNext; i < IELInlays.Count; i++)
                {
                    IELInlays[i].BeginAnimation(MarginProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationThickness(
                        new(i * DefaultWidthNewInlay, 0, 0, 0), TimeSpan.FromMilliseconds(800d)), HandoffBehavior.SnapshotAndReplace);
                }
            }
            ActualInlay.BeginAnimation(MarginProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationThickness(
                new(ActualInlay.Margin.Left, RowDefinitionMainInlays.Height.Value, 0, 0), TimeSpan.FromMilliseconds(800d)), HandoffBehavior.SnapshotAndReplace);
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
                TextBlockNullPage.BeginAnimation(OpacityProperty,
                    IELSettingObject.ObjectAnimateSetting.GetAnimationDouble(0.4d), HandoffBehavior.SnapshotAndReplace);
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
