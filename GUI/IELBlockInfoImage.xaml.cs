using IEL.CORE.Classes;
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
    /// Логика взаимодействия для IELBlockInfo.xaml
    /// </summary>
    public partial class IELBlockInfoImage : UserControl, IIELObject
    {
        #region Color Setting
        /// <summary>
        /// Ресурсный объект настройки состояний фона
        /// </summary>
        private BrushSettingQ _Background;
        /// <summary>
        /// Объект настройки состояний фона
        /// </summary>
        public new BrushSettingQ Background
        {
            get => _Background;
            set
            {
                _Background.CloneSpectrumActionInObject(value, true);
                _Background = value;
            }
        }

        /// <summary>
        /// Ресурсный объект настройки состояний границы
        /// </summary>
        private BrushSettingQ _BorderBrush;
        /// <summary>
        /// Объект настройки состояний границы
        /// </summary>
        public new BrushSettingQ BorderBrush
        {
            get => _BorderBrush;
            set
            {
                _BorderBrush.CloneSpectrumActionInObject(value, true);
                _BorderBrush = value;
            }
        }

        /// <summary>
        /// Ресурсный объект настройки состояний текста
        /// </summary>
        private BrushSettingQ _Foreground;
        /// <summary>
        /// Объект настройки состояний текста
        /// </summary>
        public new BrushSettingQ Foreground
        {
            get => _Foreground;
            set
            {
                _Foreground.CloneSpectrumActionInObject(value, true);
                _Foreground = value;
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
            get => MainBorder.CornerRadius;
            set => MainBorder.CornerRadius = value;
        }

        /// <summary>
        /// Толщина границ
        /// </summary>
        public Thickness BorderThicknessBlock
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

        /// <summary>
        /// Инициализировать объект интерфейса отображения информации через изображение
        /// </summary>
        public IELBlockInfoImage()
        {
            InitializeComponent();
            #region Background
            _Background = new();
            RectangleBackground.Fill = new SolidColorBrush(Background.ActiveSpectrumColor);
            Background.SetSpectrumAction((Args) =>
            {
                if (Args.AnimatedEvent)
                {
                    ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Args.Value);
                    RectangleBackground.Fill.BeginAnimation(SolidColorBrush.ColorProperty, anim, HandoffBehavior.SnapshotAndReplace);
                }
                else
                {
                    ((SolidColorBrush)RectangleBackground.Fill).Color = Args.Value;
                }
            });
            #endregion

            #region BorderBrush
            _BorderBrush = new();
            MainBorder.BorderBrush = new SolidColorBrush(BorderBrush.ActiveSpectrumColor);
            BorderBrush.SetSpectrumAction((Args) =>
            {
                if (Args.AnimatedEvent)
                {
                    ColorAnimation anim = IELSettingObject.ObjectAnimateSetting.GetAnimationColor(Args.Value);
                    MainBorder.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, anim, HandoffBehavior.SnapshotAndReplace);
                }
                else
                {
                    ((SolidColorBrush)MainBorder.BorderBrush).Color = Args.Value;
                }
            });
            #endregion
            #region Foreground
            _Foreground = new();
            Foreground.SetSpectrumAction((Args) =>
            {
                //SolidColorBrush color = new(Args.Value);
            });
            #endregion
            IELSettingObject = new();
            CornerRadius = new CornerRadius(10);

            MouseEnter += (sender, e) =>
            {
                if (IsEnabled)
                {
                    Background.SetActiveSpecrum(StateSpectrum.Select, true);
                    BorderBrush.SetActiveSpecrum(StateSpectrum.Select, true);
                    Foreground.SetActiveSpecrum(StateSpectrum.Select, true);
                    IELSettingObject.StartHover();
                }
            };

            MouseLeave += (sender, e) =>
            {
                if (IsEnabled)
                {
                    Background.SetActiveSpecrum(StateSpectrum.Default, true);
                    BorderBrush.SetActiveSpecrum(StateSpectrum.Default, true);
                    Foreground.SetActiveSpecrum(StateSpectrum.Default, true);
                    IELSettingObject.StopHover();
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
    }
}
