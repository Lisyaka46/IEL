using IEL.CORE.BaseUserControls;
using IEL.CORE.BaseUserControls.Interfaces;
using IEL.CORE.Classes;
using IEL.CORE.Classes.Browser;
using IEL.CORE.Classes.ObjectSettings;
using IEL.CORE.Enums;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELInley.xaml
    /// </summary>
    public partial class IELInlay : IELButton, IVisualIELButton
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

                _IELSettingObject = value;
            }
        }

        #region IVisualIELButton
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
        public new Thickness BorderThickness
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
        /// Смещение контента в объекте
        /// </summary>
        public Thickness PaddingContent
        {
            get => BorderMain.Padding;
            set => BorderMain.Padding = value;
        }
        #endregion

        /// <summary>
        /// Объект события активации закрытия вкладки
        /// </summary>
        public ActivateHandler? OnActivateCloseInlay { get; set; }

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
            set
            {

                TextBlockHead.Text = value;
                TextBlockHead.UpdateLayout();
                if (TextBlockHead.ActualWidth > MaxWidth) Width = MaxWidth;
            }
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
                SetActiveSpecrum(StateSpectrum.Default, true);
            }
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
            #region Background     
            BorderMain.Background = QBackground.InicializeConnectedSolidColorBrush();
            #endregion

            #region BorderBrush
            BorderMain.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            #endregion

            #region Foreground
            TextBlockHead.Foreground = QForeground.InicializeConnectedSolidColorBrush();
            #endregion

            BorderThicknessActive = new(
                BorderThickness.Left + OffsetBorder, BorderThickness.Top + OffsetBorder,
                BorderThickness.Right + OffsetBorder, BorderThickness.Bottom + OffsetBorder);
            BorderThicknessDiactive = BorderThickness;
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
                    SetActiveSpecrum(StateSpectrum.Select, true);
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
                    SetActiveSpecrum(StateSpectrum.Default, true);
                    IELSettingObject.StopHover();
                }
            };

            ImageCloseInlay.MouseLeftButtonDown += (sender, e) =>
            {
                OnActivateCloseInlay?.Invoke(this, e);
            };
            BorderMain.MouseDown += (sender, e) =>
            {
                SetActiveSpecrum(StateSpectrum.Used, false);
                IELSettingObject.StopHover();
            };

            BorderMain.MouseLeftButtonDown += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseLeft != null)
                {
                    SetActiveSpecrum(StateSpectrum.Select, true);
                    OnActivateMouseLeft?.Invoke(this, e);
                }
            };

            BorderMain.MouseRightButtonDown += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    SetActiveSpecrum(StateSpectrum.Select, true);
                    OnActivateMouseRight?.Invoke(this, e);
                }
            };

            IsEnabledChanged += (sender, e) =>
            {
                StateSpectrum Value = (bool)e.NewValue ? StateSpectrum.Default : StateSpectrum.NotEnabled;
                SetActiveSpecrum(Value, true);
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
