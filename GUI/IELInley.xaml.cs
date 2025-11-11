using IEL.CORE.Classes;
using IEL.CORE.Classes.Browser;
using IEL.CORE.Classes.ObjectSettings;
using IEL.CORE.Enums;
using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELInley.xaml
    /// </summary>
    public partial class IELInlay : UserControl, IIELButton
    {
		#region Color Setting
		/// <summary>
		/// Ресурсный объект настройки состояний фона
		/// </summary>
		private readonly new BrushSettingQ Background;
		/// <summary>
		/// Объект настройки состояний фона
		/// </summary>
		public BrushSettingQ QBackground
		{
			get => Background;
			set
			{
				Background.ColorData = value.ColorData;
			}
		}

		/// <summary>
		/// Ресурсный объект настройки состояний границы
		/// </summary>
		private readonly new BrushSettingQ BorderBrush;
		/// <summary>
		/// Объект настройки состояний границы
		/// </summary>
		public BrushSettingQ QBorderBrush
		{
			get => BorderBrush;
			set
			{
				BorderBrush.ColorData = value.ColorData;
			}
		}

		/// <summary>
		/// Ресурсный объект настройки состояний текста
		/// </summary>
		private readonly new BrushSettingQ Foreground;
		/// <summary>
		/// Объект настройки состояний текста
		/// </summary>
		public BrushSettingQ QForeground
		{
			get => Foreground;
			set
			{
				Foreground.ColorData = value.ColorData;
			}
		}
		#endregion

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
                Background.SetActiveSpecrum(StateSpectrum.Default, true);
                BorderBrush.SetActiveSpecrum(StateSpectrum.Default, true);
                Foreground.SetActiveSpecrum(StateSpectrum.Default, true);
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
            #region Background
            Background = new();
            BorderMain.Background = new SolidColorBrush(Background.ActiveSpectrumColor);
                    
            Background.ConnectSolidColorBrush((SolidColorBrush)BorderMain.Background);
            #endregion

            #region BorderBrush
            BorderBrush = new();
            BorderMain.BorderBrush = new SolidColorBrush(BorderBrush.ActiveSpectrumColor);
                    
            BorderBrush.ConnectSolidColorBrush((SolidColorBrush)BorderMain.BorderBrush);
            #endregion

            #region Foreground
            Foreground = new();
            TextBlockHead.Foreground = new SolidColorBrush(Foreground.ActiveSpectrumColor);

            Foreground.ConnectSolidColorBrush((SolidColorBrush)TextBlockHead.Foreground);
            #endregion

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
                    Background.SetActiveSpecrum(StateSpectrum.Select, true);
                    BorderBrush.SetActiveSpecrum(StateSpectrum.Select, true);
                    Foreground.SetActiveSpecrum(StateSpectrum.Select, true);
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
                    Background.SetActiveSpecrum(StateSpectrum.Default, true);
                    BorderBrush.SetActiveSpecrum(StateSpectrum.Default, true);
                    Foreground.SetActiveSpecrum(StateSpectrum.Default, true);
                    IELSettingObject.StopHover();
                }
            };

            ImageCloseInlay.MouseLeftButtonDown += (sender, e) =>
            {
                OnActivateCloseInlay?.Invoke(this, e);
            };
            BorderMain.MouseDown += (sender, e) =>
            {
                Background.SetActiveSpecrum(StateSpectrum.Used, false);
                BorderBrush.SetActiveSpecrum(StateSpectrum.Used, false);
                Foreground.SetActiveSpecrum(StateSpectrum.Used, false);
                IELSettingObject.StopHover();
            };

            BorderMain.MouseLeftButtonDown += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseLeft != null)
                {
                    Background.SetActiveSpecrum(StateSpectrum.Select, true);
                    BorderBrush.SetActiveSpecrum(StateSpectrum.Select, true);
                    Foreground.SetActiveSpecrum(StateSpectrum.Select, true);
                    OnActivateMouseLeft?.Invoke(this, e);
                }
            };

            BorderMain.MouseRightButtonDown += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    Background.SetActiveSpecrum(StateSpectrum.Select, true);
                    BorderBrush.SetActiveSpecrum(StateSpectrum.Select, true);
                    Foreground.SetActiveSpecrum(StateSpectrum.Select, true);
                    OnActivateMouseRight?.Invoke(this, e);
                }
            };

            IsEnabledChanged += (sender, e) =>
            {
                StateSpectrum Value = (bool)e.NewValue ? StateSpectrum.Default : StateSpectrum.NotEnabled;
                Background.SetActiveSpecrum(Value, true);
                BorderBrush.SetActiveSpecrum(Value, true);
                Foreground.SetActiveSpecrum(Value, true);
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
