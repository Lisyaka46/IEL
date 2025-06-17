using IEL.CORE.Classes.Browser;
using IEL.CORE.Classes.ObjectSettings;
using IEL.CORE.Enums;
using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELInley.xaml
    /// </summary>
    public partial class IELInlay : UserControl
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
                    BorderMain.Background = color;
                };
                value.BorderBrushQChanged += (NewValue) =>
                {
                    SolidColorBrush color = new(NewValue);
                    BorderMain.BorderBrush = color;
                };
                value.ForegroundQChanged += (NewValue) =>
                {
                    SolidColorBrush color = new(NewValue);
                    TextBlockHead.Foreground = color;
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
            get => BorderMain.CornerRadius;
            set => BorderMain.CornerRadius = value;
        }

        /// <summary>
        /// Толщина границ
        /// </summary>
        public Thickness BorderThicknessBlock
        {
            get => BorderMain.BorderThickness;
            set
            {
                BorderMain.BorderThickness = value;
                BorderThicknessActive = new(
                value.Left + OffsetBorder, value.Top + OffsetBorder,
                value.Right + OffsetBorder, value.Bottom + OffsetBorder);
                BorderThicknessDiactive = value;
            }
        }

        /// <summary>
        /// Объект события активации левым щелчком мыши
        /// </summary>
        public IIELButton.Activate? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации правым щелчком мыши
        /// </summary>
        public IIELButton.Activate? OnActivateMouseRight { get; set; }

        /// <summary>
        /// Объект события активации закрытия вкладки
        /// </summary>
        public IIELButton.Activate? OnActivateCloseInlay { get; set; }

        /// <summary>
        /// Страница заголовка
        /// </summary>
        public BrowserPage? PageElement { get; private set; }

        /// <summary>
        /// Объект страницы
        /// </summary>
        internal object? ContentPage { get; private set; }

        /// <summary>
        /// Шрифт текста в кнопке
        /// </summary>
        public new FontFamily FontFamily
        {
            get => base.FontFamily;
            set
            {
                TextBlockHead.FontFamily = value;
                base.FontFamily = value;
            }
        }

        /// <summary>
        /// Текст заголовка
        /// </summary>
        public string Text
        {
            get => TextBlockHead.Text;
            set => TextBlockHead.Text = value;
        }

        private bool _UsedState;
        /// <summary>
        /// Состояние использования
        /// </summary>
        public bool UsedState
        {
            get => _UsedState;
            set
            {
                if (_UsedState == value) return;
                _UsedState = value;
                BorderMain.BeginAnimation(BorderThicknessProperty, 
                    IELSettingObject.ObjectAnimateSetting.GetAnimationThickness(value ? BorderThicknessActive : BorderThicknessDiactive, TimeSpan.FromMilliseconds(800d)));
                MouseLeaveAnimation();
            }
        }

        /// <summary>
        /// Смещение контента в объекте
        /// </summary>
        public Thickness PaddingContent
        {
            get => BorderMain.Padding;
            set => BorderMain.Padding = value;
        }

        /// <summary>
        /// Изображение кнопки закрытия вкладки
        /// </summary>
        public ImageSource SourceCloseButtonImage
        {
            get => ImageCloseInlay.Source;
            set => ImageCloseInlay.Source = value;
        }

        /// <summary>
        /// Константа оффсета изменения между состояниями барьера
        /// </summary>
        private const int OffsetBorder = 2;

        /// <summary>
        /// Активное значение барьера вкладки
        /// </summary>
        private Thickness BorderThicknessActive;

        /// <summary>
        /// Диактивированное значение барьера вкладки
        /// </summary>
        private Thickness BorderThicknessDiactive;

        public IELInlay()
        {
            InitializeComponent();
            BorderThicknessActive = new(
                BorderThicknessBlock.Left + OffsetBorder, BorderThicknessBlock.Top + OffsetBorder,
                BorderThicknessBlock.Right + OffsetBorder, BorderThicknessBlock.Bottom + OffsetBorder);
            BorderThicknessDiactive = BorderThicknessBlock;
            _UsedState = false;
            ContentPage = null;

            IELSettingObject.BackgroundQChanged += (NewValue) =>
            {
                SolidColorBrush color = new(NewValue);
                BorderMain.Background = color;
            };
            IELSettingObject.BorderBrushQChanged += (NewValue) =>
            {
                SolidColorBrush color = new(NewValue);
                BorderMain.BorderBrush = color;
            };
            IELSettingObject.ForegroundQChanged += (NewValue) =>
            {
                SolidColorBrush color = new(NewValue);
                TextBlockHead.Foreground = color;
            };

            // this

            ImageCloseInlay.MouseEnter += (sender, e) =>
            {
                if (sender.GetType() == typeof(Image))
                {
                    DoubleAnimation animation = IELSettingObject.ObjectAnimateSetting.GetAnimationDouble(25d);
                    ImageCloseInlay.BeginAnimation(WidthProperty, animation);
                    ImageCloseInlay.BeginAnimation(HeightProperty, animation);
                }
            };
            BorderMain.MouseEnter += (sender, e) =>
            {
                if (IsEnabled)
                {
                    MouseEnterAnimation();
                    IELSettingObject.StartHover();
                }
            };

            ImageCloseInlay.MouseLeave += (sender, e) =>
            {
                DoubleAnimation animation = IELSettingObject.ObjectAnimateSetting.GetAnimationDouble(20d);
                ImageCloseInlay.BeginAnimation(WidthProperty, animation);
                ImageCloseInlay.BeginAnimation(HeightProperty, animation);
            };
            BorderMain.MouseLeave += (sender, e) =>
            {
                if (IsEnabled)
                {
                    MouseLeaveAnimation();
                    IELSettingObject.StopHover();
                }
            };

            ImageCloseInlay.MouseLeftButtonDown += (sender, e) =>
            {
                OnActivateCloseInlay?.Invoke(this, e);
            };
            BorderMain.MouseDown += (sender, e) =>
            {
                ClickDownAnimation();
                IELSettingObject.StopHover();
            };

            BorderMain.MouseLeftButtonDown += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseLeft != null)
                {
                    MouseEnterAnimation();
                    OnActivateMouseLeft?.Invoke(this, e);
                }
            };

            BorderMain.MouseRightButtonDown += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
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

                BorderMain.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush));

                BorderMain.Background.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background));

                TextBlockHead.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground));
            };
        }

        /// <summary>
        /// Анимировать нажатие (Down)
        /// </summary>
        private void ClickDownAnimation()
        {
            SolidColorBrush
                Foreground = new(IELSettingObject.ForegroundSetting.Used),
                Background = new(IELSettingObject.BackgroundSetting.Used),
                BorderBrush = new(IELSettingObject.BorderBrushSetting.Used);

            BorderMain.BorderBrush = BorderBrush;

            BorderMain.Background = Background;

            TextBlockHead.Foreground = Foreground;
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

            BorderMain.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush));

            BorderMain.Background.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background));

            TextBlockHead.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground));
        }

        /// <summary>
        /// Анимация отключения выделения мышью
        /// </summary>
        private void MouseLeaveAnimation()
        {
            Color
                Foreground = UsedState ? IELSettingObject.ForegroundSetting.Used : IELSettingObject.ForegroundSetting.Default,
                Background = UsedState ? IELSettingObject.BackgroundSetting.Used : IELSettingObject.BackgroundSetting.Default,
                BorderBrush = UsedState ? IELSettingObject.BorderBrushSetting.Used : IELSettingObject.BorderBrushSetting.Default;

            BorderMain.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(BorderBrush));

            BorderMain.Background.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Background));

            TextBlockHead.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Foreground));
        }

        /// <summary>
        /// Установить вкладке объект страницы
        /// </summary>
        /// <param name="page">Объект страницы</param>
        internal void SetPage<T>(T page) where T : BrowserPage
        {
            PageElement = page;
            ContentPage = PageElement?.PageContent;
        }
    }
}
