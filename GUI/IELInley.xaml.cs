using IEL.CORE.Classes.Browser;
using IEL.CORE.Classes.ObjectSettings;
using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using IEL.CORE.Enums;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELInley.xaml
    /// </summary>
    public partial class IELInlay : UserControl, IIELButton
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
                    }
                    else
                    {
                        SolidColorBrush color = new(NewValue);
                        BorderMain.BorderBrush = color;
                    }
                });
                value.ForegroundSetting.SetActionColorChanged((Spectrum, NewValue, Animated) =>
                {
                    if (Animated)
                    {
                        ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(NewValue);
                        TextBlockHead.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                    }
                    else
                    {
                        SolidColorBrush color = new(NewValue);
                        TextBlockHead.Foreground = color;
                    }
                });
                _IELSettingObject = value;
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
        public IIELButton.ActivateHandler? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации правым щелчком мыши
        /// </summary>
        public IIELButton.ActivateHandler? OnActivateMouseRight { get; set; }

        /// <summary>
        /// Объект события активации закрытия вкладки
        /// </summary>
        public IIELButton.ActivateHandler? OnActivateCloseInlay { get; set; }

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
                IELSettingObject.UseActiveQSetting(StateSpectrum.Default);
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

        /// <summary>
        /// Инициализировать объект интерфейса, вкладка браузера
        /// </summary>
        public IELInlay()
        {
            InitializeComponent();
            BorderThicknessActive = new(
                BorderThicknessBlock.Left + OffsetBorder, BorderThicknessBlock.Top + OffsetBorder,
                BorderThicknessBlock.Right + OffsetBorder, BorderThicknessBlock.Bottom + OffsetBorder);
            BorderThicknessDiactive = BorderThicknessBlock;
            _UsedState = false;
            ContentPage = null;
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
                    IELSettingObject.UseActiveQSetting(StateSpectrum.Select);
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
                    IELSettingObject.UseActiveQSetting(StateSpectrum.Default);
                    IELSettingObject.StopHover();
                }
            };

            ImageCloseInlay.MouseLeftButtonDown += (sender, e) =>
            {
                OnActivateCloseInlay?.Invoke(this, e);
            };
            BorderMain.MouseDown += (sender, e) =>
            {
                IELSettingObject.UseActiveQSetting(StateSpectrum.Used, false);
                IELSettingObject.StopHover();
            };

            BorderMain.MouseLeftButtonDown += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseLeft != null)
                {
                    IELSettingObject.UseActiveQSetting(StateSpectrum.Select);
                    OnActivateMouseLeft?.Invoke(this, e);
                }
            };

            BorderMain.MouseRightButtonDown += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    IELSettingObject.UseActiveQSetting(StateSpectrum.Select);
                    OnActivateMouseRight?.Invoke(this, e);
                }
            };

            IsEnabledChanged += (sender, e) =>
            {
                bool NewValue = (bool)e.NewValue;
                IELSettingObject.UseActiveQSetting(NewValue ? StateSpectrum.Default : StateSpectrum.NotEnabled);
            };
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
