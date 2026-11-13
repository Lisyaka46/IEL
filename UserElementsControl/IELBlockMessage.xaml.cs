using LibraryIEL.CORE.Animation;
using LibraryIEL.CORE.Enums;
using LibraryIEL.Interfaces;
using LibraryIEL.UserElementsControl.Base;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LibraryIEL.UserElementsControl
{
    /// <summary>
    /// Логика взаимодействия для IELBlockMessage.xaml
    /// </summary>
    public partial class IELBlockMessage : IELObjectBase
    {
        #region Properties

        #region RadiusMagnite
        /// <summary>
        /// Данные свойства <see cref="RadiusMagnite"/>
        /// </summary>
        public static readonly DependencyProperty RadiusMagniteProperty =
            DependencyProperty.Register(nameof(RadiusMagnite), typeof(uint), typeof(IELBlockMessage),
                new(2u, RadiusMagniteHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="RadiusMagnite"/>
        /// </summary>
        private static void RadiusMagniteHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELBlockMessage Source && e.NewValue is uint SourceNewValue)
            {
                Source.BorderMessage.CornerRadius = UpdateVisualFromPositionMagnite(Source);
            }
        }

        /// <summary>
        /// Значение скруглённости примагниченной стороны панели сообщения к объекту
        /// </summary>
        [Description("Значение скруглённости объекта с примагниченной стороны к объекту")]
        public uint RadiusMagnite
        {
            get => (uint)GetValue(RadiusMagniteProperty);
            set => SetValue(RadiusMagniteProperty, value);
        }
        #endregion

        #region RadiusOrdinary
        /// <summary>
        /// Данные свойства <see cref="RadiusOrdinary"/>
        /// </summary>
        public static readonly DependencyProperty RadiusOrdinaryProperty =
            DependencyProperty.Register(nameof(RadiusOrdinary), typeof(uint), typeof(IELBlockMessage),
                new(10u, RadiusOrdinaryHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="RadiusOrdinary"/>
        /// </summary>
        private static void RadiusOrdinaryHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELBlockMessage Source && e.NewValue is uint SourceNewValue)
            {
                Source.BorderMessage.CornerRadius = UpdateVisualFromPositionMagnite(Source);
            }
        }

        /// <summary>
        /// Значение скруглённости независимой стороны панели сообщения от объекта
        /// </summary>
        [Description("Значение скруглённости объекта независимой стороны от объекта")]
        public uint RadiusOrdinary
        {
            get => (uint)GetValue(RadiusOrdinaryProperty);
            set => SetValue(RadiusOrdinaryProperty, value);
        }
        #endregion

        #region Position
        /// <summary>
        /// Данные свойства <see cref="Position"/>
        /// </summary>
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register(nameof(Position), typeof(EnumPositionMessage), typeof(IELBlockMessage),
                new(EnumPositionMessage.LeftUp, PositionHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="Position"/>
        /// </summary>
        private static void PositionHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELBlockMessage Source && e.NewValue is EnumPositionMessage SourceNewValue)
            {
                if (Source.IsSwapActualPosition && Source.MagniteElement != null)
                {
                    Source.GetStartEndPointsAnimation(out Point _, out Point End);
                    PositionMessageAnimate.EasingFunction = EaseSpecific;
                    PositionMessageAnimate.From = null;
                    PositionMessageAnimate.To = new(End.X, End.Y, 0, 0);

                    OpacityMessageAnimate.To = 0.8d;

                    ManagerAnimation.AnimateTakingZeroTo(Source.BorderMessage,
                        Border.CornerRadiusProperty, UpdateVisualFromPositionMagnite(Source),
                        TimeSpan.FromMilliseconds(550d));
                    Source.BeginAnimation(MarginProperty, PositionMessageAnimate);
                }
                else Source.BorderMessage.CornerRadius = UpdateVisualFromPositionMagnite(Source);
            }
        }

        /// <summary>
        /// Значение позиции элемента относительно примагниченного элемента
        /// </summary>
        [Description("Позиция отображения сообщения относительно примагниченной стороны к объекту")]
        public EnumPositionMessage Position
        {
            get => (EnumPositionMessage)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }
        #endregion

        #region IsSwapActualPosition
        /// <summary>
        /// Данные свойства <see cref="IsSwapActualPosition"/>
        /// </summary>
        public static readonly DependencyProperty IsSwapActualPositionProperty =
            DependencyProperty.Register(nameof(IsSwapActualPosition), typeof(bool), typeof(IELBlockMessage),
                new(false));

        /// <summary>
        /// Автоматическое смещение позиции объекта при изменении свойства <see cref="Position"/><br/>
        /// и активном отображении сообщения
        /// </summary>
        [Description($"Значение отвечающее за автоматическое смещение позиции, " +
            $"при изменении свойства \"{nameof(Position)}\" и активном отображении сообщения")]
        public bool IsSwapActualPosition
        {
            get => (bool)GetValue(IsSwapActualPositionProperty);
            set => SetValue(IsSwapActualPositionProperty, value);
        }
        #endregion

        #region Offset
        /// <summary>
        /// Данные свойства <see cref="Offset"/>
        /// </summary>
        public static readonly DependencyProperty OffsetProperty =
            DependencyProperty.Register(nameof(Offset), typeof(uint), typeof(IELBlockMessage),
                new(3u));

        /// <summary>
        /// Значение позиции смещения элемента относительно примагниченного элемента
        /// </summary>
        [Description("Значение смещения объекта сообщения от примагниченного объекта, при отображении")]
        public uint Offset
        {
            get => (uint)GetValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }
        #endregion

        #region BorderThickness
        /// <summary>
        /// Данные свойства <see cref="IELBlockMessage.BorderThickness"/>
        /// </summary>
        public static new readonly DependencyProperty BorderThicknessProperty = Border.BorderThicknessProperty;

        /// <summary>
        /// Толщина границ контейнера сообщения
        /// </summary>
        [Description("Толщина границ контейнера сообщения")]
        public new Thickness BorderThickness
        {
            get => (Thickness)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }
        #endregion

        #region FontFamily
        /// <summary>
        /// Данные свойства <see cref="IELBlockMessage.FontFamily"/>
        /// </summary>
        public static new readonly DependencyProperty FontFamilyProperty = Run.FontFamilyProperty;

        /// <summary>
        /// Шрифт отображаемого сообщения
        /// </summary>
        [Description("Шрифт, отображаемый в элементе сообщения")]
        public new FontFamily FontFamily
        {
            get => (FontFamily)RunElementText.GetValue(FontFamilyProperty);
            set => RunElementText.SetValue(FontFamilyProperty, value);
        }
        #endregion

        #region Text
        /// <summary>
        /// Данные свойства <see cref="Text"/>
        /// </summary>
        public static readonly DependencyProperty TextProperty = Run.TextProperty;

        /// <summary>
        /// Текст отображаемого сообщения
        /// </summary>
        [Description("Текст, отображаемый в элементе сообщения")]
        public string Text
        {
            get => (string)RunElementText.GetValue(TextProperty);
            set => RunElementText.SetValue(TextProperty, value);
        }
        #endregion

        #endregion

        /// <summary>
        /// Объект к которому при магничено сообщение
        /// </summary>
        public IIELObjectUsingMessage? MagniteElement { get; private set; }

        #region Animate Objects
        /// <summary>
        /// Функция разсчёта анимирования по умолчанию
        /// </summary>
        private static IEasingFunction EaseDefault = new PowerEase() { EasingMode = EasingMode.EaseOut };

        /// <summary>
        /// Функция разсчёта анимирования для динамического обновления свойств
        /// </summary>
        private static IEasingFunction EaseSpecific = new BackEase() { EasingMode = EasingMode.EaseInOut, Amplitude = 1.2d };

        /// <summary>
        /// Объект анимации для управления позицией
        /// </summary>
        private static readonly ThicknessAnimation PositionMessageAnimate = new(new Thickness(0d), TimeSpan.FromMilliseconds(400d))
        {
            DecelerationRatio = 0.6d,
            EasingFunction = EaseDefault,
        };

        /// <summary>
        /// Объект анимации для управления double значением
        /// </summary>
        private static readonly DoubleAnimation OpacityMessageAnimate = new(0d, TimeSpan.FromMilliseconds(200d))
        {
            DecelerationRatio = 0.6d,
            EasingFunction = EaseDefault,
        };
        #endregion

        /// <summary>
        /// Инициализировать объект интерфейса отображения сообщения
        /// </summary>
        public IELBlockMessage()
        {
            InitializeComponent();
            BorderMessage.BorderBrush = SourceBorderBrush.SourceBrush;
            BorderMessage.Background = SourceBackground.SourceBrush;
            RunElementText.Foreground = SourceForeground.SourceBrush;

            UpdateVisualFromPositionMagnite(this);
        }

        #region Manipulate Object

        /// <summary>
        /// Активировать панель сообщения по привязке к объекту
        /// </summary>
        /// <param name="Element">Элемент к которому прикрепляется панель сообщения</param>
        public void OpenFrom(IIELObjectUsingMessage Element)
        {
            MagniteElement = Element;
            Palette = Element.Palette;
            RunElementText.Text = Element.Message;
            AnimateFromToPosition();
        }

        /// <summary>
        /// Закрыть панель сообщения
        /// </summary>
        public void Close()
        {
            if (MagniteElement == null) return;
            MagniteElement = null;
            DoubleAnimation animation = OpacityMessageAnimate.Clone();
            TextBlockMessage.BeginAnimation(MarginProperty, null);
            void SetZOne(object? sender, EventArgs e)
            {
                if (MagniteElement != null) return;
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

        /// <summary>
        /// Анимировать панель сообщения от стартовой до конечной позиции
        /// </summary>
        private void AnimateFromToPosition()
        {
            if (MagniteElement == null) return;
            Canvas.SetZIndex(this, 2);
            GetStartEndPointsAnimation(out Point Start, out Point End);
            PositionMessageAnimate.EasingFunction = EaseDefault;
            PositionMessageAnimate.From = new(Start.X, Start.Y, 0, 0);
            PositionMessageAnimate.To = new(End.X, End.Y, 0, 0);

            OpacityMessageAnimate.To = 0.8d;

            BeginAnimation(OpacityProperty, OpacityMessageAnimate, HandoffBehavior.SnapshotAndReplace);
            BeginAnimation(MarginProperty, PositionMessageAnimate, HandoffBehavior.SnapshotAndReplace);
        }
        #endregion

        /// <summary>
        /// Расчитать позиции анимирования относительно примагниченного элемента
        /// </summary>
        /// <param name="Start">Стартовая позиция анимирования</param>
        /// <param name="End">Конечная позиция анимирования</param>
        private void GetStartEndPointsAnimation(out Point Start, out Point End)
        {
            Start = default;
            End = default;
            if (MagniteElement == null) return;
            Rect MagniteLocation = MagniteElement.GetRectFrom((UIElement)Parent);
            Start = TranslatePoint(MagniteLocation.Location, this);

            if (Position is EnumPositionMessage.LeftUp or EnumPositionMessage.CenterLeft or EnumPositionMessage.LeftDown)
                Start.X -= ActualWidth;
            else if (Position is EnumPositionMessage.CenterUp or EnumPositionMessage.CenterDown)
                Start.X += MagniteLocation.Width / 2 - ActualWidth / 2;
            else Start.X += MagniteLocation.Width;

            if (Position is EnumPositionMessage.LeftUp or EnumPositionMessage.CenterUp or EnumPositionMessage.RightUp)
                Start.Y -= ActualHeight;
            else if (Position is EnumPositionMessage.CenterLeft or EnumPositionMessage.CenterRight)
                Start.Y += MagniteLocation.Height / 2 - ActualHeight / 2;
            else Start.Y += MagniteLocation.Height;

            End = Start;
            if (Position is EnumPositionMessage.LeftUp or EnumPositionMessage.CenterLeft or EnumPositionMessage.LeftDown)
                End.X -= Offset;
            else if (Position is EnumPositionMessage.RightUp or EnumPositionMessage.CenterRight or EnumPositionMessage.RightDown)
                End.X += Offset;
            if (Position is EnumPositionMessage.LeftUp or EnumPositionMessage.CenterUp or EnumPositionMessage.RightUp)
                End.Y -= Offset;
            else if (Position is EnumPositionMessage.LeftDown or EnumPositionMessage.CenterDown or EnumPositionMessage.RightDown)
                End.Y += Offset;
        }

        /// <summary>
        /// Обновить отображение примагриченного состояния
        /// </summary>
        /// <param name="Source">Обновляемый объект</param>
        private static CornerRadius UpdateVisualFromPositionMagnite(in IELBlockMessage Source) =>
            new(
            Source.Position is EnumPositionMessage.RightDown or EnumPositionMessage.CenterDown or EnumPositionMessage.CenterRight ?
                Source.RadiusMagnite : Source.RadiusOrdinary,
            Source.Position is EnumPositionMessage.LeftDown or EnumPositionMessage.CenterDown or EnumPositionMessage.CenterLeft ?
                Source.RadiusMagnite : Source.RadiusOrdinary,
            Source.Position is EnumPositionMessage.LeftUp or EnumPositionMessage.CenterLeft or EnumPositionMessage.CenterUp ?
                Source.RadiusMagnite : Source.RadiusOrdinary,
            Source.Position is EnumPositionMessage.RightUp or EnumPositionMessage.CenterUp or EnumPositionMessage.CenterRight ?
                Source.RadiusMagnite : Source.RadiusOrdinary
            );
    }
}
