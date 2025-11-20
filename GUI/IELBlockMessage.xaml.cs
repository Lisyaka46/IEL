using IEL.CORE.BaseUserControls;
using IEL.CORE.Enums;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IEL.GUI
{
    /// <summary>
    /// Логика взаимодействия для IELBlockMessage.xaml
    /// </summary>
    public partial class IELBlockMessage : IELObjectBase
    {
        #region Flags
        /// <summary>
        /// Флаг состояния активности панели сообщения
        /// </summary>
        public bool FlagMessage { get; private set; } = false;
        #endregion

        #region Color Objects
        #region Default Color
        private Color? _DefaultBorderBrush;
        /// <summary>
        /// Обычный цвет границы кнопки
        /// </summary>
        public Color DefaultBorderBrush
        {
            get => _DefaultBorderBrush ?? Colors.Gold;
            set
            {
                SolidColorBrush color = new(value);
                BorderMessage.BorderBrush = color;
                _DefaultBorderBrush = value;
            }
        }

        private Color? _DefaultBackground;
        /// <summary>
        /// Обычный цвет фона кнопки
        /// </summary>
        public Color DefaultBackground
        {
            get => _DefaultBackground ?? Colors.Gold;
            set
            {
                SolidColorBrush color = new(value);
                BorderMessage.Background = color;
                _DefaultBackground = value;
            }
        }

        private Color? _DefaultForeground;
        /// <summary>
        /// Обычный цвет текста
        /// </summary>
        public Color DefaultForeground
        {
            get => _DefaultForeground ?? Colors.Gold;
            set
            {
                SolidColorBrush color = new(value);
                TextBlockMessage.Foreground = color;
                _DefaultForeground = value;
            }
        }
        #endregion

        #region Select Color
        /// <summary>
        /// Выделенный цвет границы
        /// </summary>
        public Color SelectBorderBrush { get; set; }

        /// <summary>
        /// Выделенный цвет фона
        /// </summary>
        public Color SelectBackground { get; set; }

        /// <summary>
        /// Выделенный цвет текста
        /// </summary>
        public Color SelectForeground { get; set; }
        #endregion
        #endregion

        #region Animate Objects
        /// <summary>
        /// Объект анимации для управления позицией
        /// </summary>
        private static readonly ThicknessAnimation ThicknessAnimate = new(new Thickness(0), TimeSpan.FromMilliseconds(300d))
        {
            DecelerationRatio = 0.6d,
            EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut }
        };

        /// <summary>
        /// Объект анимации для управления double значением
        /// </summary>
        private static readonly DoubleAnimation DoubleAnimateObj = new(0, TimeSpan.FromMilliseconds(250d))
        {
            DecelerationRatio = 0.2d,
            EasingFunction = new QuinticEase() { EasingMode = EasingMode.EaseOut }
        };
        #endregion

        #region Values Object
        #region Default
        private uint _RadiusDefault;
        /// <summary>
        /// Радиус скруглённости обычной стороны панели сообщения
        /// </summary>
        public uint RadiusDefault
        {
            get => _RadiusDefault;
            set
            {
                BorderMessage.Padding = new(value, 0, value, 0);
                _RadiusDefault = value;
            }
        }

        /// <summary>
        /// Радиус скруглённости примагниченной стороны панели сообщения к объекту
        /// </summary>
        public uint RadiusMagnite { get; set; }

        /// <summary>
        /// Сдвиг позиции (Лево, Право) относительно отображения
        /// </summary>
        public uint OffsetLeftRight { get; set; }

        /// <summary>
        /// Сдвиг позиции (Верх, Низ) относительно отображения
        /// </summary>
        public uint OffsetUpDown { get; set; }

        /// <summary>
        /// Код контейнера привязки
        /// </summary>
        public int CodeParentObject { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Шрифт использующийся в панели сообщения
        /// </summary>
        public new FontFamily FontFamily
        {
            get => TextBlockMessage.FontFamily;
            set => TextBlockMessage.FontFamily = value;
        }

        /// <summary>
        /// Текст отображаемый в панели сообщения
        /// </summary>
        public string Text
        {
            get => TextBlockMessage.Text;
            set
            {
                TextBlockMessage.Text = value;
                TextBlockMessage.UpdateLayout();
            }
        }
        #endregion
        #endregion

        #region IVisualIELButton
        /// <summary>
        /// Скругление границ
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => BorderMessage.CornerRadius;
            set => BorderMessage.CornerRadius = value;
        }

        /// <summary>
        /// Толщина границ
        /// </summary>
        public new Thickness BorderThickness
        {
            get => BorderMessage.BorderThickness;
            set => BorderMessage.BorderThickness = value;
        }

        /// <summary>
        /// Смещение контента в объекте
        /// </summary>
        public new Thickness Padding
        {
            get => BorderMessage.Padding;
            set => BorderMessage.Padding = value;
        }
        #endregion

        #region ** Inicialize Object **
        /// <summary>
        /// Инициализировать объект интерфейса отображения сообщения
        /// </summary>
        public IELBlockMessage()
        {
            InitializeComponent();
            #region Background
            #endregion

            #region BorderBrush
            #endregion

            #region Foreground
            #endregion

            #region Set Values Object
            TextBlockMessage.Margin = new(0);
            TextBlockMessage.UpdateLayout();
            #region Values
            RadiusDefault = 13u;
            RadiusMagnite = 3u;

            OffsetLeftRight = 3u;
            OffsetUpDown = 3u;

            CodeParentObject = int.MinValue;
            #endregion

            #region Colors
            DefaultBackground = Colors.White;
            DefaultBorderBrush = Colors.Black;
            DefaultForeground = Colors.Black;

            SelectBackground = Colors.White;
            SelectBorderBrush = Colors.Black;
            SelectForeground = Colors.Black;
            #endregion
            #endregion
        }
        #endregion

        #region Manipulate Object
        #region Using Object
        /// <summary>
        /// Активировать панель сообщения по привязке к объекту
        /// </summary>
        /// <param name="Element">Элемент к которому прикрепляется панель сообщения</param>
        /// <param name="TextVisible">Текст который выводится панелью сообщения</param>
        /// <param name="Orientation">Привязка к позиционированию панели</param>
        public void UsingBorderInformation(FrameworkElement Element, string TextVisible, OrientationBorderPosition Orientation)
        {
            CodeParentObject = Element.GetHashCode();
            TextBlockMessage.Text = TextVisible;
            TextBlockMessage.UpdateLayout();
            GridMessage.UpdateLayout();
            if (GridMessage.ActualHeight < TextBlockMessage.ActualHeight)
            {
                int Offset = (int)TextBlockMessage.ActualHeight - (int)GridMessage.ActualHeight;
                ThicknessAnimation animation = new()
                {
                    EasingFunction = new SineEase() { EasingMode = EasingMode.EaseInOut, },
                    BeginTime = TimeSpan.FromMilliseconds(160d),
                    Duration = TimeSpan.FromMilliseconds(80d * Offset < 1200d ? 1200d : 80d * Offset),
                    RepeatBehavior = RepeatBehavior.Forever,
                    AutoReverse = true,
                    From = TextBlockMessage.Margin,
                    To = new(0, -Offset, 0, 0)
                };
                TextBlockMessage.BeginAnimation(MarginProperty, animation);
            }
            else TextBlockMessage.Margin = new(0);
            #region Auto
            if (Orientation == OrientationBorderPosition.Auto)
            {
                FrameworkElement ParentElement = VisualTreeHelper.GetParent(this) as FrameworkElement ??
                    throw new Exception("У объекта нет родительского элемента");
                Point LocationPointElement =
                    Element.TransformToAncestor(ParentElement).TransformBounds(new Rect(ParentElement.RenderSize)).Location;
                Size SizeParentElement = ParentElement.RenderSize;
                Point LeftRightOrientationLogic =
                    new(Math.Abs(LocationPointElement.X - ActualWidth), SizeParentElement.Width - (LocationPointElement.X + ActualWidth));
                double UpDownOrientationLogic = LocationPointElement.Y - ActualHeight;
                if (LeftRightOrientationLogic.X >= LeftRightOrientationLogic.Y)
                {
                    Orientation = UpDownOrientationLogic >= 0 ? OrientationBorderPosition.LeftUp : OrientationBorderPosition.LeftDown;
                }
                else if (LeftRightOrientationLogic.X < LeftRightOrientationLogic.Y)
                {
                    Orientation = UpDownOrientationLogic >= 0 ? OrientationBorderPosition.RightUp : OrientationBorderPosition.RightDown;
                }
            }
            #endregion
            AnimateBorderInformation(SetPositionOrientation(Element, Orientation), Orientation);
        }

        /// <summary>
        /// Активировать панель сообщения по позиции
        /// </summary>
        /// <param name="TextVisible">Текст который выводится панелью сообщения</param>
        /// <param name="Orientation">Привязка к позиционированию панели</param>
        public void UsingBorderInformationCursor(string TextVisible, OrientationBorderPosition Orientation)
        {
            CodeParentObject = 0;
            Text = TextVisible;
            AnimateBorderInformation(SetPositionOrientation(Orientation), Orientation);
        }
        #endregion

        #region Close Object
        /// <summary>
        /// Закрыть панель сообщения
        /// </summary>
        public void CloseBorderInformation()
        {
            if (!FlagMessage) return;
            FlagMessage = false;
            DoubleAnimation animation = DoubleAnimateObj.Clone();
            TextBlockMessage.BeginAnimation(MarginProperty, null);
            void SetZOne(object? sender, EventArgs e)
            {
                if (FlagMessage) return;
                Canvas.SetZIndex(this, -2);
                animation.FillBehavior = FillBehavior.HoldEnd;
                animation.Completed -= SetZOne;
                Opacity = 0d;
            }
            animation.FillBehavior = FillBehavior.Stop;
            animation.Completed += SetZOne;
            animation.To = 0d;
            BeginAnimation(OpacityProperty, animation);
        }
        #endregion

        #region Set Position
        /// <summary>
        /// Узнать стартовое значение позиции
        /// </summary>
        /// <param name="Orientation">Привязка к позиционированию панели</param>
        private Point SetPositionOrientation(OrientationBorderPosition Orientation)
        {
            Point CursorPos = Mouse.GetPosition((IInputElement)VisualParent);
            return Orientation switch
            {
                OrientationBorderPosition.LeftDown => new(CursorPos.X - BorderMessage.ActualWidth, CursorPos.Y),
                OrientationBorderPosition.LeftUp => new(CursorPos.X - BorderMessage.ActualWidth, CursorPos.Y - BorderMessage.ActualHeight),
                OrientationBorderPosition.RightUp => new(CursorPos.X, CursorPos.Y - BorderMessage.ActualHeight),
                OrientationBorderPosition.RightDown => new(CursorPos.X, CursorPos.Y),
                _ => CursorPos
            };
        }

        /// <summary>
        /// Узнать стартовое значение позиции относительно объекта
        /// </summary>
        /// <param name="Element">Элемент к которому прикрепляется панель описания</param>
        /// <param name="Orientation">Привязка к позиционированию панели</param>
        private Point SetPositionOrientation(FrameworkElement Element, OrientationBorderPosition Orientation)
        {
            Point ElementPos = Element.TransformToAncestor((Visual)VisualParent).TransformBounds(new Rect(Element.RenderSize)).Location;
            return Orientation switch
            {
                OrientationBorderPosition.LeftDown => new(ElementPos.X - BorderMessage.ActualWidth, ElementPos.Y + Element.ActualHeight),
                OrientationBorderPosition.LeftUp => new(ElementPos.X - BorderMessage.ActualWidth, ElementPos.Y - BorderMessage.ActualHeight),
                OrientationBorderPosition.RightUp => new(ElementPos.X + Element.ActualWidth, ElementPos.Y - BorderMessage.ActualHeight),
                OrientationBorderPosition.RightDown => new(ElementPos.X + Element.ActualWidth, ElementPos.Y + Element.ActualHeight),
                _ => ElementPos
            };
        }
        #endregion

        /// <summary>
        /// Анимировать панель сообщения по позиции
        /// </summary>
        /// <param name="Position">Позиция привязки</param>
        /// <param name="Orientation">Привязка к позиционированию панели</param>
        private void AnimateBorderInformation(Point Position, OrientationBorderPosition Orientation)
        {
            FlagMessage = true;

            Canvas.SetZIndex(this, 2);
            DoubleAnimation animationD = DoubleAnimateObj.Clone();
            animationD.To = 0.8d;
            BeginAnimation(OpacityProperty, animationD);

            BorderMessage.CornerRadius = Orientation switch
            {
                OrientationBorderPosition.LeftDown => new(RadiusDefault, RadiusMagnite, RadiusDefault, RadiusDefault),
                OrientationBorderPosition.LeftUp => new(RadiusDefault, RadiusDefault, RadiusMagnite, RadiusDefault),
                OrientationBorderPosition.RightUp => new(RadiusDefault, RadiusDefault, RadiusDefault, RadiusMagnite),
                OrientationBorderPosition.RightDown => new(RadiusMagnite, RadiusDefault, RadiusDefault, RadiusDefault),
                _ => new(RadiusDefault),
            };
            Point Offset = new(
                Orientation == OrientationBorderPosition.LeftDown || Orientation == OrientationBorderPosition.LeftUp ? -OffsetLeftRight : OffsetLeftRight,
                Orientation == OrientationBorderPosition.LeftUp || Orientation == OrientationBorderPosition.RightUp ? -OffsetUpDown : OffsetUpDown);
            ThicknessAnimation animation = ThicknessAnimate.Clone();
            animation.From = new(Position.X, Position.Y, 0, 0);
            animation.To = new(Position.X + Offset.X, Position.Y + Offset.Y, 0, 0);
            animation.Duration = TimeSpan.FromMilliseconds(400d);
            BeginAnimation(MarginProperty, animation);
        }
        #endregion
    }
}
