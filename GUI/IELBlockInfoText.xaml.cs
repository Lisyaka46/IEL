using IEL.CORE.BaseUserControls;
using IEL.CORE.BaseUserControls.Interfaces;
using IEL.CORE.Classes;
using IEL.CORE.Classes.ObjectSettings;
using IEL.CORE.Enums;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELBlockInfoText.xaml
    /// </summary>
    public partial class IELBlockInfoText : IELObject, IVisualIELButton
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
            get => MainBorder.CornerRadius;
            set => MainBorder.CornerRadius = value;
        }

        /// <summary>
        /// Толщина границ
        /// </summary>
        public new Thickness BorderThickness
        {
            get => MainBorder.BorderThickness;
            set => MainBorder.BorderThickness = value;
        }

        /// <summary>
        /// Смещение контента в объекте
        /// </summary>
        public Thickness PaddingContent
        {
            get => MainBorder.Padding;
            set => MainBorder.Padding = value;
        }
        #endregion

        /// <summary>
        /// Текст объекта
        /// </summary>
        public string Text
        {
            get => MainTextBlock.Text;
            set => MainTextBlock.Text = value;
        }

        /// <summary>
        /// Шрифт текста в кнопке
        /// </summary>
        public new FontFamily FontFamily
        {
            get => base.FontFamily;
            set
            {
                MainTextBlock.FontFamily = value;
                base.FontFamily = value;
            }
        }

        /// <summary>
        /// Инициализировать объект интерфейса отображения информации через текст
        /// </summary>
        public IELBlockInfoText()
        {
            InitializeComponent();
            #region Background
            MainBorder.Background = QBackground.InicializeConnectedSolidColorBrush();
            #endregion

            #region BorderBrush
            MainBorder.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            #endregion

            #region Foreground
            MainTextBlock.Foreground = QBackground.InicializeConnectedSolidColorBrush();
            #endregion

            IELSettingObject = new();
            CornerRadius = new CornerRadius(10);

            MouseEnter += (sender, e) =>
            {
                if (IsEnabled)
                {
                    SetActiveSpecrum(StateSpectrum.Select, true);
                    IELSettingObject.StartHover();
                }
            };

            MouseLeave += (sender, e) =>
            {
                if (IsEnabled)
                {
                    SetActiveSpecrum(StateSpectrum.Default, true);
                    IELSettingObject.StopHover();
                }
            };

            IsEnabledChanged += (sender, e) =>
            {
                StateSpectrum Value = (bool)e.NewValue ? StateSpectrum.Default : StateSpectrum.NotEnabled;
                SetActiveSpecrum(Value, true);
            };
        }
    }
}
