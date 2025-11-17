using IEL.CORE.BaseUserControls;
using IEL.CORE.BaseUserControls.Interfaces;
using IEL.CORE.Classes;
using IEL.CORE.Classes.ObjectSettings;
using IEL.CORE.Enums;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELButtonTextKey.xaml
    /// </summary>
    public partial class IELButtonTextKey : IELButtonKey, IVisualIELButtonKey
    {
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
        /// Текст кнопки
        /// </summary>
        public string Text
        {
            get => TextBlockButton.Text;
            set => TextBlockButton.Text = value;
        }

        #region IVisualIELButtonKey
        /// <summary>
        /// Скругление границ
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => BorderButton.CornerRadius;
            set
            {
                BorderButton.CornerRadius = value;
                BorderCharKeyboard.CornerRadius = value;
            }
        }

        /// <summary>
        /// Толщина границ
        /// </summary>
        public new Thickness BorderThickness
        {
            get => BorderButton.BorderThickness;
            set
            {
                BorderButton.BorderThickness = value;
                BorderCharKeyboard.BorderThickness = value;
            }
        }

        /// <summary>
        /// Смещение контента в объекте
        /// </summary>
        public Thickness PaddingContent
        {
            get => BorderButton.Padding;
            set
            {
                BorderButton.Padding = value;
                BorderCharKeyboard.Padding = value;
            }
        }

        private bool _CharKeyboardActivate = false;
        /// <summary>
        /// Активность видимости символа действия активации кнопки
        /// </summary>
        public bool CharKeyboardActivate
        {
            get => _CharKeyboardActivate;
            set
            {
                BorderButton.BeginAnimation(MarginProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationThickness(new(!value ? -24 : 0, 0, 0, 0)));
                BorderCharKeyboard.BeginAnimation(OpacityProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationDouble(value ? 1d : 0d));
                _CharKeyboardActivate = value;
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
                //TextBlockCharKey.Text = this.KeyName(value).ToString();
            }
        }
        #endregion

        /// <summary>
        /// Шрифт текста в кнопке
        /// </summary>
        public new FontFamily FontFamily
        {
            get => base.FontFamily;
            set
            {
                TextBlockCharKey.FontFamily = value;
                TextBlockButton.FontFamily = value;
                base.FontFamily = value;
            }
        }

        /// <summary>
        /// Инициализировать объект интерфейса кнопки с текстом поддерживающую возможность нажатия с помощью клавиши
        /// </summary>
        public IELButtonTextKey()
        {
            InitializeComponent();
            #region Background
            BorderButton.Background = QBackground.InicializeConnectedSolidColorBrush();
            BorderCharKeyboard.Background = QBackground.InicializeConnectedSolidColorBrush();
            BorderLeftArrow.Background = QBackground.InicializeConnectedSolidColorBrush();
            BorderRightArrow.Background = QBackground.InicializeConnectedSolidColorBrush();
            #endregion

            #region BorderBrush
            BorderButton.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            BorderCharKeyboard.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            BorderLeftArrow.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            BorderRightArrow.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            #endregion

            #region Foreground
            QForeground = new([255, 255, 255, 255]);
            TextBlockButton.Foreground = QForeground.InicializeConnectedSolidColorBrush();
            TextBlockCharKey.Foreground = QForeground.InicializeConnectedSolidColorBrush();
            TextBlockLeftArrow.Foreground = QForeground.InicializeConnectedSolidColorBrush();
            TextBlockRightArrow.Foreground = QForeground.InicializeConnectedSolidColorBrush();
            #endregion
            IELSettingObject = new();

            BorderButton.Margin = new(-24, 0, 0, 0);
            BorderCharKeyboard.Opacity = 0d;
            ImageMouseButtonsUse.Opacity = 0d;
            Text = "Text";
            CornerRadius = new CornerRadius(10);

            MouseEnter += (sender, e) =>
            {
                if (IsEnabled)
                {
                    SetActiveSpecrum(StateSpectrum.Select, true);
                    IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
                    IELSettingObject.StartHover();
                }
            };

            MouseLeave += (sender, e) =>
            {
                if (IsEnabled)
                {
                    SetActiveSpecrum(StateSpectrum.Default, true);
                    IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, false);
                    IELSettingObject.StopHover();
                }
            };

            MouseDown += (sender, e) =>
            {
                if (IsEnabled)
                {
                    if (
                    (e.LeftButton == MouseButtonState.Pressed && OnActivateMouseLeft != null) ||
                    (e.RightButton == MouseButtonState.Pressed && OnActivateMouseRight != null))
                    {
                        SetActiveSpecrum(StateSpectrum.Used, false);
                        IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
                        IELSettingObject.StopHover();
                    }
                }
            };

            MouseLeftButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseLeft != null)
                {
                    SetActiveSpecrum(StateSpectrum.Select, true);
                    IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
                    OnActivateMouseLeft?.Invoke(this, e);
                }
            };

            MouseRightButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    SetActiveSpecrum(StateSpectrum.Select, true);
                    IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
                    OnActivateMouseRight?.Invoke(this, e);
                }
            };

            IsEnabledChanged += (sender, e) =>
            {
                StateSpectrum Value = (bool)e.NewValue ? StateSpectrum.Default : StateSpectrum.NotEnabled;
                SetActiveSpecrum(Value, true);
                IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, false);
            };
        }
    }
}
