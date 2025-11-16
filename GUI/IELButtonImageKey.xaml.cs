using IEL.CORE.BaseUserControls;
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
    public partial class IELButtonImageKey : IELButtonKey
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
        /// Позиционирование картинки в кнопке
        /// </summary>
        public Thickness ImageMargin
        {
            get => ImageButton.Margin;
            set => ImageButton.Margin = value;
        }

        /// <summary>
        /// Инициализировать объект кнопки с изображением
        /// </summary>
        public IELButtonImageKey()
        {
            InitializeComponent();
            #region Background
            BorderButton.Background = QBackground.InicializeConnectedSolidColorBrush();
            BorderButtonKey.Background = QBackground.InicializeConnectedSolidColorBrush();
            BorderRightArrow.Background = QBackground.InicializeConnectedSolidColorBrush();
            BorderLeftArrow.Background = QBackground.InicializeConnectedSolidColorBrush();
            #endregion

            #region BorderBrush
            BorderButton.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            BorderButtonKey.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            BorderRightArrow.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            BorderLeftArrow.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            #endregion

            #region Foreground
            TextBlockKey.Foreground = QForeground.InicializeConnectedSolidColorBrush();
            TextBlockLeftArrow.Foreground = QForeground.InicializeConnectedSolidColorBrush();
            TextBlockRightArrow.Foreground = QForeground.InicializeConnectedSolidColorBrush();
            #endregion
            IELSettingObject = new();

            ImageButton.Margin = new Thickness(10, 10, 10, 10);

            ImageMouseButtonsUse.Opacity = 0d;
            BorderButton.MouseEnter += (sender, e) =>
            {
                if (IsEnabled)
                {
                    SetActiveSpecrum(StateSpectrum.Select, true);
                    IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
                    IELSettingObject.StartHover();
                }
            };
            BorderButton.MouseLeave += (sender, e) =>
            {
                if (IsEnabled)
                {
                    SetActiveSpecrum(StateSpectrum.Default, true);
                    IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, false);
                    IELSettingObject.StopHover();
                }
            };

            IsEnabledChanged += (sender, e) =>
            {
                StateSpectrum Value = (bool)e.NewValue ? StateSpectrum.Default : StateSpectrum.NotEnabled;
                SetActiveSpecrum(Value, true);
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
                    OnActivateMouseLeft?.Invoke(false, e);
                }
            };

            MouseRightButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    SetActiveSpecrum(StateSpectrum.Select, true);
                    IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
                    OnActivateMouseRight?.Invoke(false, e);
                }
            };
        }
    }
}
