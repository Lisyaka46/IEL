using IEL.CORE.Classes;
using IEL.CORE.Classes.ObjectSettings;
using IEL.CORE.Enums;
using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELButtonImageKey.xaml
    /// </summary>
    public partial class IELButtonImageKey : UserControl, IIELButtonKey
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

		private IELButtonObjectSetting _IELSettingObject = new();
        /// <summary>
        /// Настройка использования объекта
        /// </summary>
        public IELButtonObjectSetting IELSettingObject
        {
            get => _IELSettingObject;
            set
            {
                value.VisualizationButtonChanged += (StateVisual) =>
                {
                    bool
                        Left = StateVisual == StateVisualButton.LeftArrow,
                        Right = StateVisual == StateVisualButton.RightArrow;
                    ColumnRightArrow.Width = new(Right ? 25 : 0);
                    BorderRightArrow.Opacity = Right ? 1d : 0d;

                    ColumnLeftArrow.Width = new(Left ? 25 : 0);
                    BorderLeftArrow.Opacity = Left ? 1d : 0d;
                    ImageMouseButtonsUse.HorizontalAlignment = Right ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                };
                _IELSettingObject = value;
            }
        }

        /// <summary>
        /// Изображение которое отображается в кнопке
        /// </summary>
        public ImageSource Imaging
        {
            get
            {
                return ImageButton.Source;
            }
            set
            {
                ImageButton.Source = value;
            }
        }

        /// <summary>
        /// Толщина границ
        /// </summary>
        public Thickness BorderThicknessBlock
        {
            get => BorderButton.BorderThickness;
            set => BorderButton.BorderThickness = value;
        }

        /// <summary>
        /// Скруглённость границ
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => BorderButton.CornerRadius;
            set => BorderButton.CornerRadius = value;
        }

        /// <summary>
        /// Позиционирование картинки в кнопке
        /// </summary>
        public Thickness ImageMargin
        {
            get => ImageButton.Margin;
            set => ImageButton.Margin = value;
        }

        /// <summary>
        /// Объект события активации левым щелчком мыши
        /// </summary>
        public IIELButton.ActivateHandler? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации правым щелчком мыши
        /// </summary>
        public IIELButton.ActivateHandler? OnActivateMouseRight { get; set; }

        private bool _CharKeyKeyboardActivate = false;
        /// <summary>
        /// Активность видимости символа действия активации кнопки
        /// </summary>
        public bool CharKeyboardActivate
        {
            get => _CharKeyKeyboardActivate;
            set
            {
                BorderButton.BeginAnimation(MarginProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationThickness(new(!value ? -24 : 0, 0, 0, 0)));
                BorderButtonKey.BeginAnimation(OpacityProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationDouble(value ? 1d : 0d));
                _CharKeyKeyboardActivate = value;
            }
        }

        private Key? _CharKeyKeyboard;
        /// <summary>
        /// Клавиша отвечающая за активацию кнопки
        /// </summary>
        public Key? CharKeyKeyboard
        {
            get => _CharKeyKeyboard;
            set
            {
                _CharKeyKeyboard = value;
                TextBlockKey.Text = IIELObject.KeyName(value).ToString();
            }
        }

        /// <summary>
        /// Смещение контента в объекте
        /// </summary>
        public Thickness PaddingContent
        {
            get => BorderButton.Padding;
            set => BorderButton.Padding = value;
        }

        /// <summary>
        /// Инициализировать объект кнопки с изображением
        /// </summary>
        public IELButtonImageKey()
        {
            InitializeComponent();
            #region Background
            Background = new();
            BorderButton.Background = new SolidColorBrush(Background.ActiveSpectrumColor);
            BorderButtonKey.Background = new SolidColorBrush(Background.ActiveSpectrumColor);
            BorderRightArrow.Background = new SolidColorBrush(Background.ActiveSpectrumColor);
            BorderLeftArrow.Background = new SolidColorBrush(Background.ActiveSpectrumColor);

            Background.ConnectSolidColorBrush((SolidColorBrush)BorderButton.Background);
            Background.ConnectSolidColorBrush((SolidColorBrush)BorderButtonKey.Background);
            Background.ConnectSolidColorBrush((SolidColorBrush)BorderRightArrow.Background);
            Background.ConnectSolidColorBrush((SolidColorBrush)BorderLeftArrow.Background);
            #endregion

            #region BorderBrush
            BorderBrush = new();
            BorderButton.BorderBrush = new SolidColorBrush(BorderBrush.ActiveSpectrumColor);
            BorderButtonKey.BorderBrush = new SolidColorBrush(BorderBrush.ActiveSpectrumColor);
            BorderRightArrow.BorderBrush = new SolidColorBrush(BorderBrush.ActiveSpectrumColor);
            BorderLeftArrow.BorderBrush = new SolidColorBrush(BorderBrush.ActiveSpectrumColor);

            BorderBrush.ConnectSolidColorBrush((SolidColorBrush)BorderButton.BorderBrush);
            BorderBrush.ConnectSolidColorBrush((SolidColorBrush)BorderButtonKey.BorderBrush);
            BorderBrush.ConnectSolidColorBrush((SolidColorBrush)BorderRightArrow.BorderBrush);
            BorderBrush.ConnectSolidColorBrush((SolidColorBrush)BorderLeftArrow.BorderBrush);
            #endregion

            #region Foreground
            Foreground = new();
            TextBlockKey.Foreground = new SolidColorBrush(Foreground.ActiveSpectrumColor);
            TextBlockLeftArrow.Foreground = new SolidColorBrush(Foreground.ActiveSpectrumColor);
            TextBlockRightArrow.Foreground = new SolidColorBrush(Foreground.ActiveSpectrumColor);

            Foreground.ConnectSolidColorBrush((SolidColorBrush)TextBlockKey.Foreground);
            Foreground.ConnectSolidColorBrush((SolidColorBrush)TextBlockLeftArrow.Foreground);
            Foreground.ConnectSolidColorBrush((SolidColorBrush)TextBlockRightArrow.Foreground);
            #endregion
            IELSettingObject = new();

            ImageButton.Margin = new Thickness(10, 10, 10, 10);
            CharKeyboardActivate = false;

            ImageMouseButtonsUse.Opacity = 0d;
            BorderButton.MouseEnter += (sender, e) =>
            {
                if (IsEnabled)
                {
                    Background.SetActiveSpecrum(StateSpectrum.Select, true);
                    BorderBrush.SetActiveSpecrum(StateSpectrum.Select, true);
                    Foreground.SetActiveSpecrum(StateSpectrum.Select, true);
                    IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
                    IELSettingObject.StartHover();
                }
            };
            BorderButton.MouseLeave += (sender, e) =>
            {
                if (IsEnabled)
                {
                    Background.SetActiveSpecrum(StateSpectrum.Default, true);
                    BorderBrush.SetActiveSpecrum(StateSpectrum.Default, true);
                    Foreground.SetActiveSpecrum(StateSpectrum.Default, true);
                    IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, false);
                    IELSettingObject.StopHover();
                }
            };

            IsEnabledChanged += (sender, e) =>
            {
                StateSpectrum Value = (bool)e.NewValue ? StateSpectrum.Default : StateSpectrum.NotEnabled;
                Background.SetActiveSpecrum(Value, true);
                BorderBrush.SetActiveSpecrum(Value, true);
                Foreground.SetActiveSpecrum(Value, true);
                IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, false);
            };

            MouseDown += (sender, e) =>
            {
                if (IsEnabled)
                {
                    if (
                    (e.LeftButton == MouseButtonState.Pressed && OnActivateMouseLeft != null) ||
                    (e.RightButton == MouseButtonState.Pressed && OnActivateMouseRight != null))
                    {
                        Background.SetActiveSpecrum(StateSpectrum.Used, false);
                        BorderBrush.SetActiveSpecrum(StateSpectrum.Used, false);
                        Foreground.SetActiveSpecrum(StateSpectrum.Used, false);
                        IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
                        IELSettingObject.StopHover();
                    }
                }
            };

            MouseLeftButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseLeft != null)
                {
                    Background.SetActiveSpecrum(StateSpectrum.Select, true);
                    BorderBrush.SetActiveSpecrum(StateSpectrum.Select, true);
                    Foreground.SetActiveSpecrum(StateSpectrum.Select, true);
                    IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
                    OnActivateMouseLeft?.Invoke(false, e);
                }
            };

            MouseRightButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    Background.SetActiveSpecrum(StateSpectrum.Select, true);
                    BorderBrush.SetActiveSpecrum(StateSpectrum.Select, true);
                    Foreground.SetActiveSpecrum(StateSpectrum.Select, true);
                    IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
                    OnActivateMouseRight?.Invoke(false, e);
                }
            };
        }

        /// <summary>
        /// Анимация мерцания
        /// </summary>
        [MTAThread()]
        public void BlinkAnimation()
        {
            Background.SetActiveSpecrum(StateSpectrum.Used, false);
            BorderBrush.SetActiveSpecrum(StateSpectrum.Used, false);
            Foreground.SetActiveSpecrum(StateSpectrum.Used, false);

            Background.SetActiveSpecrum(StateSpectrum.Select, true);
            BorderBrush.SetActiveSpecrum(StateSpectrum.Select, true);
            Foreground.SetActiveSpecrum(StateSpectrum.Select, true);
            IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
        }

        /// <summary>
        /// Анимация мерцания
        /// </summary>
        [MTAThread()]
        public void UnfocusAnimation()
        {
            Background.SetActiveSpecrum(StateSpectrum.Default, true);
            BorderBrush.SetActiveSpecrum(StateSpectrum.Default, true);
            Foreground.SetActiveSpecrum(StateSpectrum.Default, true);
            IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, false);
        }
    }
}
