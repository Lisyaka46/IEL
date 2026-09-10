using IEL.CORE.Enums;
using IEL.CORE.Themes.Palettes;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace IEL.UserElementsControl.Base
{
    /// <summary>
    /// <b>БАЗОВЫЙ КЛАСС</b><br/>
    /// Кнопка IEL
    /// </summary>
    public class IELButtonBase : IELContainerBase
    {
        #region ConstDescriptions
        /// <summary>
        /// Коментарий к описанию устаревших событий
        /// </summary>
        private const string DescriptionCommentObsoleteEvent = "Событие является недопустимым для объекта.\n" +
            $"Используйте события {nameof(BasicActivate)} и {nameof(AdditionalActivate)}, а также свойства управления к ним";

        /// <summary>
        /// Комментарий к описанию активации кнопки с помощью клавиатуры, при фокусе на элементе
        /// </summary>
        private const string DescriptionCommentActivateElementFocusKeyboard =
            "При получении фокуса на элемент кнопки, подразумевается что её можно активировать с помощью:\n" +
            $"- {nameof(Key.Enter)}, основное событие кнопки {nameof(BasicActivate)} (Левая кнопка мыши)\n" +
            $"- {nameof(ActivateKey)}, дополнительное событие кнопки {nameof(AdditionalActivate)} (Правая кнопка мыши)";

        /// <summary>
        /// Комментарий к описанию направляющих
        /// </summary>
        private const string DescriptionCommentGuide =
            "Направляющие представляют собой индикаторы векторных линий, которые являются лишь визуальной состовляющей кнопки.\n" +
            $"Направляющие НЕ являются частью свойства {nameof(Content)}";

        /// <summary>
        /// Комментарий к описанию направляющих о изменении на обоих объектах
        /// </summary>
        private const string DescriptionCommentGuideChangeDouble =
            "Отражается сразу на обоих направляющих!\n" + DescriptionCommentGuide;
        #endregion

        /// <summary>
        /// Состояние принудительной установки отображения клавиши
        /// </summary>
        private bool IsVisibleSetConst = false;

        #region UIElements
        /// <summary>
        /// Главный объект отображения содержимого кнопки
        /// </summary>
        private Viewbox Base_ViewBoxButton;

        /// <summary>
        /// Главный контейнер кнопки
        /// </summary>
        private Grid Base_HeadGridButton;

        /// <summary>
        /// Главный контейнер контента кнопки
        /// </summary>
        private Grid Base_HeadGridContentButton;

        /// <summary>
        /// Контейнер отображения клавиши
        /// </summary>
        private Border Base_BorderKeyVisible;

        /// <summary>
        /// Объект текста отображения названия клавиши
        /// </summary>
        private TextBlock Base_TextBlockKeyVisible;

        /// <summary>
        /// Объект контекста названия клавиши
        /// </summary>
        private Run Base_RunTextKey;

        #region LeftGuide < |=|
        /// <summary>
        /// Левый контейнер отображения направляющей
        /// </summary>
        private Border Base_LeftGuideContainer;

        /// <summary>
        /// Элемент левой направляющей
        /// </summary>
        private Grid Base_LeftGuideGrid;

        /// <summary>
        /// Элемент горизонтальной части левой направляющей -
        /// </summary>
        private Line Base_LeftGuideLine;

        /// <summary>
        /// Элемент изгибающей части левой направляющей <![CDATA[<]]>
        /// </summary>
        private Polyline Base_LeftGuidePolyLine;
        #endregion

        #region RightGuide |=| >
        /// <summary>
        /// Правый контейнер отображения направляющей
        /// </summary>
        private Border Base_RightGuideContainer;

        /// <summary>
        /// Элемент правой направляющей
        /// </summary>
        private Grid Base_RightGuideGrid;

        /// <summary>
        /// Элемент горизонтальной части правой направляющей -
        /// </summary>
        private Line Base_RightGuideLine;

        /// <summary>
        /// Элемент изгибающей части правой направляющей <![CDATA[>]]>
        /// </summary>
        private Polyline Base_RightGuidePolyLine;
        #endregion

        /// <summary>
        /// Главный объект отображения содержимого кнопки
        /// </summary>
        protected readonly ContentControl Base_ButtonContentContainer;
        #endregion

        #region Events
        /// <summary>
        /// Объект события базовой активации (левая кнопка мыши)
        /// </summary>
        [Description("Событие управляемая активацией основного события кнопки.\n" +
            "Активация кнопки зависит от состояний параметров:\n" +
            $"- С помощью клавиатуры {nameof(IsEnabledActivateKeyboard)} и {nameof(IsActivateKeyBasicEvent)}\n" +
            $"- С помощью мыши {nameof(IsEnabledActivateMouse)}")]
        public event MouseButtonEventHandler? BasicActivate;

        /// <summary>
        /// Объект события дополнительной активации (правая кнопка мыши)
        /// </summary>
        [Description("Событие управляемая активацией дополнительного события кнопки.\n" +
            "Активация кнопки зависит от состояний параметров:\n" +
            $"- С помощью клавиатуры {nameof(IsEnabledActivateKeyboard)} и {nameof(IsActivateKeyBasicEvent)}\n" +
            $"- С помощью мыши {nameof(IsEnabledActivateMouse)}")]
        public event MouseButtonEventHandler? AdditionalActivate;

        #region ObsoleteEvents
        [Obsolete(DescriptionCommentObsoleteEvent, true)]
        private new event MouseButtonEventHandler? MouseDown;

        [Obsolete(DescriptionCommentObsoleteEvent, true)]
        private new event MouseButtonEventHandler? MouseUp;

        [Obsolete(DescriptionCommentObsoleteEvent, true)]
        private new event MouseButtonEventHandler? MouseLeftButtonDown;

        [Obsolete(DescriptionCommentObsoleteEvent, true)]
        private new event MouseButtonEventHandler? MouseLeftButtonUp;

        [Obsolete(DescriptionCommentObsoleteEvent, true)]
        private new event MouseButtonEventHandler? MouseRightButtonDown;

        [Obsolete(DescriptionCommentObsoleteEvent, true)]
        private new event MouseButtonEventHandler? MouseRightButtonUp;

        [Obsolete(DescriptionCommentObsoleteEvent, true)]
        private new event MouseButtonEventHandler? MouseDoubleClick;

        [Obsolete(DescriptionCommentObsoleteEvent, true)]
        private new event MouseButtonEventHandler? PreviewMouseDown;

        [Obsolete(DescriptionCommentObsoleteEvent, true)]
        private new event MouseButtonEventHandler? PreviewMouseUp;

        [Obsolete(DescriptionCommentObsoleteEvent, true)]
        private new event MouseButtonEventHandler? PreviewMouseLeftButtonDown;

        [Obsolete(DescriptionCommentObsoleteEvent, true)]
        private new event MouseButtonEventHandler? PreviewMouseLeftButtonUp;

        [Obsolete(DescriptionCommentObsoleteEvent, true)]
        private new event MouseButtonEventHandler? PreviewMouseRightButtonDown;

        [Obsolete(DescriptionCommentObsoleteEvent, true)]
        private new event MouseButtonEventHandler? PreviewMouseRightButtonUp;

        [Obsolete(DescriptionCommentObsoleteEvent, true)]
        private new event MouseButtonEventHandler? PreviewMouseDoubleClick;

        [Obsolete(DescriptionCommentObsoleteEvent, true)]
        private new event KeyEventHandler? KeyDown;

        [Obsolete(DescriptionCommentObsoleteEvent, true)]
        private new event KeyEventHandler? KeyUp;

        [Obsolete(DescriptionCommentObsoleteEvent, true)]
        private new event KeyEventHandler? PreviewKeyDown;

        [Obsolete(DescriptionCommentObsoleteEvent, true)]
        private new event KeyEventHandler? PreviewKeyUp;
        #endregion
        #endregion

        #region Properties

        #region IsEnabledActivateMouse
        /// <summary>
        /// Данные свойства <see cref="IsEnabledActivateMouse"/>
        /// </summary>
        public static readonly DependencyProperty IsEnabledActivateMouseProperty =
            DependencyProperty.Register(nameof(IsEnabledActivateMouse), typeof(bool), typeof(IELButtonBase),
                new(true));

        /// <summary>
        /// Состояние отвечающее за возможность активации кнопки с помощью мыши
        /// </summary>
        [Description("Состояние возможности активации кнопки с помощью мыши")]
        public bool IsEnabledActivateMouse
        {
            get => (bool)GetValue(IsEnabledActivateMouseProperty);
            set => SetValue(IsEnabledActivateMouseProperty, value);
        }
        #endregion

        #region IsEnabledActivateKeyboard
        /// <summary>
        /// Данные свойства <see cref="IsEnabledActivateKeyboard"/>
        /// </summary>
        public static readonly DependencyProperty IsEnabledActivateKeyboardProperty =
            DependencyProperty.Register(nameof(IsEnabledActivateKeyboard), typeof(bool), typeof(IELButtonBase),
                new(false));

        /// <summary>
        /// Состояние отвечающее за возможность активации кнопки с помощью клавиатуры
        /// </summary>
        [Description("Состояние возможности активации кнопки с помощью клавиатуры.\n" +
            DescriptionCommentActivateElementFocusKeyboard)]
        public bool IsEnabledActivateKeyboard
        {
            get => (bool)GetValue(IsEnabledActivateKeyboardProperty);
            set => SetValue(IsEnabledActivateKeyboardProperty, value);
        }
        #endregion

        #region IsActivateKeyBasicEvent
        /// <summary>
        /// Данные свойства <see cref="IsActivateKeyBasicEvent"/>
        /// </summary>
        public static readonly DependencyProperty IsActivateKeyBasicEventProperty =
            DependencyProperty.Register(nameof(IsActivateKeyBasicEvent), typeof(bool), typeof(IELButtonBase),
                new(true));

        /// <summary>
        /// Состояние отвечающее за тип активируемого события исполнения активации кнопки с помощью клавиатуры
        /// </summary>
        [Description("Состояние активации события исполнения активации кнопки с помощью клавиатуры.\n" +
            $"Если TRUE, то будет активирован {nameof(BasicActivate)}, иначе {nameof(AdditionalActivate)}.\n" +
            DescriptionCommentActivateElementFocusKeyboard)]
        public bool IsActivateKeyBasicEvent
        {
            get => (bool)GetValue(IsActivateKeyBasicEventProperty);
            set => SetValue(IsActivateKeyBasicEventProperty, value);
        }
        #endregion

        #region IsVisibleActivateKey
        /// <summary>
        /// Данные свойства <see cref="IsVisibleActivateKey"/>
        /// </summary>
        public static readonly DependencyProperty IsVisibleActivateKeyProperty =
            DependencyProperty.Register(nameof(IsVisibleActivateKey), typeof(bool), typeof(IELButtonBase),
                new(false, IsVisibleActivateKeyHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="IsVisibleActivateKey"/>
        /// </summary>
        private static void IsVisibleActivateKeyHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonBase Source && e.NewValue is bool SourceNewValue)
            {
                Source.Base_BorderKeyVisible.Visibility = SourceNewValue ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Состояние отображения клавиши активации кнопки
        /// </summary>
        [Description("Состояние отображения названия клавиши для клавиши активации кнопки")]
        public bool IsVisibleActivateKey
        {
            get => (bool)GetValue(IsVisibleActivateKeyProperty);
            set => SetValue(IsVisibleActivateKeyProperty, value);
        }
        #endregion

        #region ActivateKeyFontSize
        /// <summary>
        /// Данные свойства <see cref="ActivateKeyFontSize"/>
        /// </summary>
        public static readonly DependencyProperty ActivateKeyFontSizeProperty =
            DependencyProperty.Register(nameof(ActivateKeyFontSize), typeof(double), typeof(IELButtonBase),
                new(8d, ActivateKeyFontSizeHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="ActivateKeyFontSize"/>
        /// </summary>
        private static void ActivateKeyFontSizeHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonBase Source && e.NewValue is double SourceNewValue)
            {
                Source.Base_TextBlockKeyVisible.FontSize = SourceNewValue;
            }
        }

        /// <summary>
        /// Размер текста названия кнопки отображения прикреплённой клавиши активации
        /// </summary>
        [Description("Размер названия клавиши прикреплённой для активации кнопки с помощью клавиатуры")]
        public double ActivateKeyFontSize
        {
            get => (double)GetValue(ActivateKeyFontSizeProperty);
            set => SetValue(ActivateKeyFontSizeProperty, value);
        }
        #endregion

        #region ActivateKey
        /// <summary>
        /// Данные свойства <see cref="ActivateKey"/>
        /// </summary>
        public static readonly DependencyProperty ActivateKeyProperty =
            DependencyProperty.Register(nameof(ActivateKey), typeof(Key), typeof(IELButtonBase),
                new(ActivateKeyHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="ActivateKey"/>
        /// </summary>
        private static void ActivateKeyHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonBase Source && e.NewValue is Key SourceNewValue)
            {
                Source.Base_RunTextKey.Text = SourceNewValue.ToString();
            }
        }

        /// <summary>
        /// Прикреплённая клавиша для активации кнопки с помощью клавиатуры
        /// </summary>
        [Description("Прикреплённая клавиша для активации кнопки с помощью клавиатуры.\n" +
            $"Активация кнопки с помощью клавиатуры зависит от состояний параметров {nameof(IsEnabledActivateKeyboard)} и {nameof(IsActivateKeyBasicEvent)}")]
        public Key ActivateKey
        {
            get => (Key)GetValue(ActivateKeyProperty);
            set => SetValue(ActivateKeyProperty, value);
        }
        #endregion

        #region Content
        /// <summary>
        /// Данные свойства <see cref="Content"/>
        /// </summary>
        public static readonly new DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(UIElement), typeof(IELButtonBase),
                new(ContentHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="Content"/>
        /// </summary>
        private static void ContentHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonBase Source && e.NewValue is UIElement SourceNewValue)
            {
                Source.Base_ButtonContentContainer.Content = SourceNewValue;
            }
        }

        /// <summary>
        /// Внутренний элемент объекта
        /// </summary>
        [Description("Контент элемента кнопки")]
        public new UIElement Content
        {
            get => (UIElement)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        #endregion

        #region PaddingButtonContent
        /// <summary>
        /// Данные свойства <see cref="PaddingButtonContent"/>
        /// </summary>
        public static readonly DependencyProperty PaddingButtonContentProperty =
            DependencyProperty.Register(nameof(PaddingButtonContent), typeof(Thickness), typeof(IELButtonBase),
                new(new Thickness(5d), PaddingButtonContentHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="PaddingButtonContent"/>
        /// </summary>
        private static void PaddingButtonContentHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonBase Source && e.NewValue is Thickness SourceNewValue)
            {
                Source.Base_ButtonContentContainer.Margin = SourceNewValue;
            }
        }

        /// <summary>
        /// Внутреннее смещение контента в кнопке
        /// </summary>
        [Description("Внутреннее смещение контента кнопки.")]
        public Thickness PaddingButtonContent
        {
            get => (Thickness)GetValue(PaddingButtonContentProperty);
            set => SetValue(PaddingButtonContentProperty, value);
        }
        #endregion

        #region GuidesCornerRadius
        /// <summary>
        /// Данные свойства <see cref="GuidesCornerRadius"/>
        /// </summary>
        public static readonly DependencyProperty GuidesCornerRadiusProperty =
            DependencyProperty.Register(nameof(GuidesCornerRadius), typeof(CornerRadius), typeof(IELButtonBase),
                new(new CornerRadius(0d), GuidesCornerRadiusHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="GuidesCornerRadius"/>
        /// </summary>
        private static void GuidesCornerRadiusHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonBase Source && e.NewValue is CornerRadius SourceNewValue)
            {
                Source.Base_LeftGuideContainer.CornerRadius = SourceNewValue;
                Source.Base_RightGuideContainer.CornerRadius = SourceNewValue;
            }
        }

        /// <summary>
        /// Скругление границ объектов направляющих кнопки
        /// </summary>
        [Description("Скругление границ направляющих для кнопки.\n" + DescriptionCommentGuide)]
        public CornerRadius GuidesCornerRadius
        {
            get => (CornerRadius)GetValue(GuidesCornerRadiusProperty);
            set => SetValue(GuidesCornerRadiusProperty, value);
        }
        #endregion

        #region GuidesBorderThickness
        /// <summary>
        /// Данные свойства <see cref="GuidesBorderThickness"/>
        /// </summary>
        public static readonly DependencyProperty GuidesBorderThicknessProperty =
            DependencyProperty.Register(nameof(GuidesBorderThickness), typeof(Thickness), typeof(IELButtonBase),
                new(new Thickness(2d), GuidesBorderThicknessHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="GuidesBorderThickness"/>
        /// </summary>
        private static void GuidesBorderThicknessHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonBase Source && e.NewValue is Thickness SourceNewValue)
            {
                Source.Base_LeftGuideContainer.BorderThickness = SourceNewValue;
                Source.Base_RightGuideContainer.BorderThickness = SourceNewValue;
            }
        }

        /// <summary>
        /// Толщина границ направляющих кнопки
        /// </summary>
        [Description("Толщина границ направляющих для кнопки.\n" + DescriptionCommentGuide)]
        public Thickness GuidesBorderThickness
        {
            get => (Thickness)GetValue(GuidesBorderThicknessProperty);
            set => SetValue(GuidesBorderThicknessProperty, value);
        }
        #endregion

        #region GuidesMargin
        /// <summary>
        /// Данные свойства <see cref="GuidesMargin"/>
        /// </summary>
        public static readonly DependencyProperty GuidesMarginProperty =
            DependencyProperty.Register(nameof(GuidesMargin), typeof(Thickness), typeof(IELButtonBase),
                new(new Thickness(2d), GuidesMarginHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="GuidesMargin"/>
        /// </summary>
        private static void GuidesMarginHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonBase Source && e.NewValue is Thickness SourceNewValue)
            {
                Source.Base_LeftGuideContainer.Margin = SourceNewValue;
                Source.Base_RightGuideContainer.Margin =
                    new Thickness(SourceNewValue.Right, SourceNewValue.Top, SourceNewValue.Left, SourceNewValue.Bottom);
            }
        }

        /// <summary>
        /// Смещение направляющих в кнопке
        /// </summary>
        [Description("Смещение направляющих внутри кнопки.\n" +
            "Смещение присваивается для обоих направляющих сразу, отзеркаливая его для правой направляющей!\n" + 
            DescriptionCommentGuide)]
        public Thickness GuidesMargin
        {
            get => (Thickness)GetValue(GuidesMarginProperty);
            set => SetValue(GuidesMarginProperty, value);
        }
        #endregion

        #region GuidesPadding
        /// <summary>
        /// Данные свойства <see cref="GuidesPadding"/>
        /// </summary>
        public static readonly DependencyProperty GuidesPaddingProperty =
            DependencyProperty.Register(nameof(GuidesPadding), typeof(Thickness), typeof(IELButtonBase),
                new(new Thickness(2d), GuidesPaddingHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="GuidesPadding"/>
        /// </summary>
        private static void GuidesPaddingHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonBase Source && e.NewValue is Thickness SourceNewValue)
            {
                Source.Base_LeftGuideContainer.Padding = SourceNewValue;
                Source.Base_RightGuideContainer.Padding =
                    new Thickness(SourceNewValue.Right, SourceNewValue.Top, SourceNewValue.Left, SourceNewValue.Bottom);
            }
        }

        /// <summary>
        /// Внутреннее смещение направляющих в кнопке
        /// </summary>
        [Description("Внутреннее смещение направляющих внутри собственного контейнера.\n" +
            "Смещение присваивается для обоих направляющих сразу, отзеркаливая его для правой направляющей!\n" +
            DescriptionCommentGuide)]
        public Thickness GuidesPadding
        {
            get => (Thickness)GetValue(GuidesPaddingProperty);
            set => SetValue(GuidesPaddingProperty, value);
        }
        #endregion

        #region GuidesStrokeThickness
        /// <summary>
        /// Данные свойства <see cref="GuidesStrokeThickness"/>
        /// </summary>
        public static readonly DependencyProperty GuidesStrokeThicknessProperty =
            DependencyProperty.Register(nameof(GuidesStrokeThickness), typeof(double), typeof(IELButtonBase),
                new(2d, GuidesStrokeThicknessHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="GuidesStrokeThickness"/>
        /// </summary>
        private static void GuidesStrokeThicknessHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonBase Source && e.NewValue is double SourceNewValue)
            {
                Source.Base_LeftGuideLine.StrokeThickness = SourceNewValue;
                Source.Base_LeftGuidePolyLine.StrokeThickness = SourceNewValue;

                Source.Base_RightGuideLine.StrokeThickness = SourceNewValue;
                Source.Base_RightGuidePolyLine.StrokeThickness = SourceNewValue;
            }
        }

        /// <summary>
        /// Толщина границ направляющих в кнопке
        /// </summary>
        [Description("Толщина границ направляющих внутри собственного контейнера.\n" + DescriptionCommentGuide)]
        public double GuidesStrokeThickness
        {
            get => (double)GetValue(GuidesStrokeThicknessProperty);
            set => SetValue(GuidesStrokeThicknessProperty, value);
        }
        #endregion

        #region GuidesOffsetMainLine
        /// <summary>
        /// Данные свойства <see cref="GuidesOffsetMainLine"/>
        /// </summary>
        public static readonly DependencyProperty GuidesOffsetMainLineProperty =
            DependencyProperty.Register(nameof(GuidesOffsetMainLine), typeof(double), typeof(IELButtonBase),
                new(3d, GuidesOffsetMainLineHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="GuidesOffsetMainLine"/>
        /// </summary>
        private static void GuidesOffsetMainLineHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonBase Source && e.NewValue is double SourceNewValue)
            {
                if (SourceNewValue < 0d) throw new InvalidOperationException("Невозможно присвоить значение ниже нуля.");
                Source.UpdateGuideVisual();
            }
        }

        /// <summary>
        /// Смещение размера главной линии относительно пересечения лепестков у направляющих
        /// </summary>
        [Description("Смещение размера главной линии относительно пересечения лепестков у направляющих внутри собственного контейнера.\n" +
            DescriptionCommentGuideChangeDouble)]
        public double GuidesOffsetMainLine
        {
            get => (double)GetValue(GuidesOffsetMainLineProperty);
            set => SetValue(GuidesOffsetMainLineProperty, value);
        }
        #endregion

        #region GuidesOffsetPetalLines
        /// <summary>
        /// Данные свойства <see cref="GuidesOffsetPetalLines"/>
        /// </summary>
        public static readonly DependencyProperty GuidesOffsetPetalLinesProperty =
            DependencyProperty.Register(nameof(GuidesOffsetPetalLines), typeof(double), typeof(IELButtonBase),
                new(4d, GuidesOffsetPetalLinesHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="GuidesOffsetPetalLines"/>
        /// </summary>
        private static void GuidesOffsetPetalLinesHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonBase Source && e.NewValue is double SourceNewValue)
            {
                if (SourceNewValue < 0d) throw new InvalidOperationException("Невозможно присвоить значение ниже нуля.");
                Source.UpdateGuideVisual();
            }
        }

        /// <summary>
        /// Смещение пересечения лепестков линий направляющих
        /// </summary>
        [Description("Смещение пересечения лепестков линий направляющих внутри собственного контейнера.\n" +
            DescriptionCommentGuideChangeDouble)]
        public double GuidesOffsetPetalLines
        {
            get => (double)GetValue(GuidesOffsetPetalLinesProperty);
            set => SetValue(GuidesOffsetPetalLinesProperty, value);
        }
        #endregion

        #region GuidesDistanceBetweenPetalLines
        /// <summary>
        /// Данные свойства <see cref="GuidesDistanceBetweenPetalLines"/>
        /// </summary>
        public static readonly DependencyProperty GuidesDistanceBetweenPetalLinesProperty =
            DependencyProperty.Register(nameof(GuidesDistanceBetweenPetalLines), typeof(double), typeof(IELButtonBase),
                new(8d, GuidesDistanceBetweenPetalLinesHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="GuidesDistanceBetweenPetalLines"/>
        /// </summary>
        private static void GuidesDistanceBetweenPetalLinesHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonBase Source && e.NewValue is double SourceNewValue)
            {
                if (SourceNewValue < 0d) throw new InvalidOperationException("Невозможно присвоить значение ниже нуля.");
                Source.UpdateGuideVisual();
            }
        }

        /// <summary>
        /// Расстояние между леместками у направляющих
        /// </summary>
        [Description("Расстояние между леместками у направляющих, внутри собственного контейнера.\n" +
            DescriptionCommentGuideChangeDouble)]
        public double GuidesDistanceBetweenPetalLines
        {
            get => (double)GetValue(GuidesDistanceBetweenPetalLinesProperty);
            set => SetValue(GuidesDistanceBetweenPetalLinesProperty, value);
        }
        #endregion

        #region GuideVisual
        /// <summary>
        /// Данные свойства <see cref="GuideVisual"/>
        /// </summary>
        public static readonly DependencyProperty GuideVisualProperty =
            DependencyProperty.Register(nameof(GuideVisual), typeof(StateVisualGuide), typeof(IELButtonBase),
                new(StateVisualGuide.Default, GuideVisualHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="GuideVisual"/>
        /// </summary>
        private static void GuideVisualHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonBase Source && e.NewValue is StateVisualGuide SourceNewValue)
            {
                Source.Base_HeadGridContentButton.ColumnDefinitions[0].Width = new(0d,
                    SourceNewValue == StateVisualGuide.LeftArrow || SourceNewValue == StateVisualGuide.DuoArrow ?
                    GridUnitType.Auto : GridUnitType.Pixel);
                Source.Base_HeadGridContentButton.ColumnDefinitions[2].Width = new(0d,
                    SourceNewValue == StateVisualGuide.RightArrow || SourceNewValue == StateVisualGuide.DuoArrow ?
                    GridUnitType.Auto : GridUnitType.Pixel);
                Source.VisualGuideChanged?.Invoke(Source, SourceNewValue);
            }
        }

        /// <summary>
        /// Состояние отображения направляющих кнопки
        /// </summary>
        [Description("Состояние отображения направляющих в элементе кнопки.\n" + DescriptionCommentGuide)]
        public StateVisualGuide GuideVisual
        {
            get => (StateVisualGuide)GetValue(GuideVisualProperty);
            set => SetValue(GuideVisualProperty, value);
        }

        /// <summary>
        /// Событие изменения состояния отображения направляющих кнопки
        /// </summary>
        [Description("Событие изменение визуализации направляющих.\n" + DescriptionCommentGuide)]
        protected event EventHandler<StateVisualGuide>? VisualGuideChanged;
        #endregion

        #region OverrideBorderThickness
        /// <summary>
        /// Расширенный обработчик события изменения свойства <see cref="IELContainerBase.BorderThickness"/>
        /// </summary>
        private static void OverrideBorderThicknessHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonBase Source && e.NewValue is Thickness SourceNewValue)
            {
                Source.Base_BorderKeyVisible.BorderThickness = new(0d, SourceNewValue.Top, SourceNewValue.Right, 0d);
            }
        }
        #endregion

        #region OverrideCornerRadius
        /// <summary>
        /// Расширенный обработчик события изменения свойства <see cref="IELContainerBase.CornerRadius"/>
        /// </summary>
        private static void OverrideCornerRadiusHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonBase Source && e.NewValue is CornerRadius SourceNewValue)
            {
                Source.Base_BorderKeyVisible.CornerRadius = new(0d, SourceNewValue.TopRight, 0d, SourceNewValue.BottomRight);
            }
        }
        #endregion

        #region ContextMenu TODO: (контектное меню для IELPanelAction)
        ///// <summary>
        ///// Данные конкретного свойства
        ///// </summary>
        //public static readonly new DependencyProperty ContextMenuProperty =
        //    DependencyProperty.Register("ContextMenu", typeof(StackPanel), typeof(IELButtonBase),
        //        new());

        ///// <summary>
        ///// Отображаемый объект контекстного меню
        ///// </summary>
        //public new StackPanel? ContextMenu
        //{
        //    get => (StackPanel?)GetValue(ContextMenuProperty);
        //    set => SetValue(ContextMenuProperty, value);
        //}

        ///// <summary>
        ///// Объект события активации контекстного меню
        ///// </summary>
        //public new event EventHandler<StackPanel>? ContextMenuOpening;

        ///// <summary>
        ///// Объект события активации закрытия контекстного меню
        ///// </summary>
        //public new event EventHandler<StackPanel>? ContextMenuClosing;
        #endregion

        #endregion

        /// <summary>
        /// Статический конструктор для динамического связывания параметров
        /// </summary>
        static IELButtonBase()
        {
            BorderThicknessProperty.OverrideMetadata(typeof(IELButtonBase), new(OverrideBorderThicknessHandler));
            CornerRadiusProperty.OverrideMetadata(typeof(IELButtonBase), new(OverrideCornerRadiusHandler));
        }

        /// <summary>
        /// Инициализация базового класса визуализации кнопки IEL
        /// </summary>
        protected IELButtonBase() : base()
        {
            Base_ViewBoxButton = new()
            {
                Stretch = Stretch.Uniform,
                StretchDirection = StretchDirection.DownOnly,
                Focusable = false,
            };

            Base_HeadGridButton = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Focusable = false,
            };

            #region KeyBorderIndicator
            Base_RunTextKey = new(string.Empty);

            Base_TextBlockKeyVisible = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = SourceForeground.SourceBrush,
                Padding = new(2d),
                FontSize = ActivateKeyFontSize,
                Focusable = false,
            };
            Base_TextBlockKeyVisible.Inlines.Add(Base_RunTextKey);

            Base_BorderKeyVisible = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                BorderBrush = SourceBorderBrush.SourceBrush,
                Background = SourceBackground.SourceBrush,
                BorderThickness = new(0d, BorderThickness.Top, BorderThickness.Right, 0d),
                CornerRadius = new(0d, CornerRadius.TopRight, 0d, CornerRadius.BottomRight),
                Child = Base_TextBlockKeyVisible,
                Visibility = Visibility.Hidden,
                Focusable = false,
            };
            Canvas.SetZIndex(Base_BorderKeyVisible, 1);
            Base_HeadGridButton.Children.Add(Base_BorderKeyVisible);
            #endregion

            Base_HeadGridContentButton = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Focusable = false,
            };
            Base_ViewBoxButton.Child = Base_HeadGridButton;
            Base_HeadGridButton.Children.Add(Base_HeadGridContentButton);

            #region LeftGuide < |=|
            Base_LeftGuideContainer = new()
            {
                Margin = GuidesMargin,
                Padding = GuidesPadding,
                CornerRadius = GuidesCornerRadius,
                BorderThickness = GuidesBorderThickness,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                BorderBrush = SourceBorderBrush.SourceBrush,
                Focusable = false,
            };
            Base_LeftGuideGrid = new()
            {
                Margin = new(0d),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Focusable = false,

            };

            #region <-
            Base_LeftGuideLine = new()
            {
                Margin = new(0d),
                X1 = 0d,
                X2 = 0d,
                StrokeThickness = GuidesStrokeThickness,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Stretch = Stretch.None,
                Stroke = SourceBorderBrush.SourceBrush,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,
                Focusable = false,
            };
            Base_LeftGuidePolyLine = new()
            {
                Margin = new(0d),
                StrokeThickness = GuidesStrokeThickness,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Stretch = Stretch.None,
                Stroke = SourceBorderBrush.SourceBrush,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,
                Focusable = false,
            };
            #endregion

            #endregion

            #region RightGuide |=| >
            Base_RightGuideContainer = new()
            {
                Margin = new(GuidesMargin.Right, GuidesMargin.Top, GuidesMargin.Left, GuidesMargin.Bottom),
                Padding = new(GuidesPadding.Right, GuidesPadding.Top, GuidesPadding.Left, GuidesPadding.Bottom),
                CornerRadius = GuidesCornerRadius,
                BorderThickness = GuidesBorderThickness,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                BorderBrush = SourceBorderBrush.SourceBrush,
                ClipToBounds = true,
                Focusable = false,
            };
            Base_RightGuideGrid = new()
            {
                Margin = new(0d),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Focusable = false,
            };

            #region ->
            Base_RightGuideLine = new()
            {
                Margin = new(0d),
                X1 = 0d,
                X2 = 0d,
                StrokeThickness = GuidesStrokeThickness,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Stretch = Stretch.None,
                Stroke = SourceBorderBrush.SourceBrush,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,
                Focusable = false,
            };
            Base_RightGuidePolyLine = new()
            {
                Margin = new(3.5d, 0d, 0d, 0d),
                StrokeThickness = GuidesStrokeThickness,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Stretch = Stretch.None,
                Stroke = SourceBorderBrush.SourceBrush,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,
                Focusable = false,
            };
            #endregion

            #endregion

            Base_ButtonContentContainer = new()
            {
                Margin = PaddingButtonContent,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Focusable = false,
            };

            Base_HeadGridContentButton.ColumnDefinitions.Add(new() { Width = new(0d, GridUnitType.Pixel) });
            Base_HeadGridContentButton.ColumnDefinitions.Add(new() { Width = new(1d, GridUnitType.Star) });
            Base_HeadGridContentButton.ColumnDefinitions.Add(new() { Width = new(0d, GridUnitType.Pixel) });

            #region LeftGuide < |=|
            Base_LeftGuideContainer.Child = Base_LeftGuideGrid;

            #region <-
            Base_LeftGuideGrid.Children.Add(Base_LeftGuideLine);

            Base_LeftGuidePolyLine.Points.Add(new(0d, 0d));
            Base_LeftGuidePolyLine.Points.Add(new(0d, 0d));
            Base_LeftGuidePolyLine.Points.Add(new(0d, 0d));
            Base_LeftGuideGrid.Children.Add(Base_LeftGuidePolyLine);
            #endregion

            Grid.SetColumn(Base_LeftGuideContainer, 0);
            Base_HeadGridContentButton.Children.Add(Base_LeftGuideContainer);
            #endregion

            #region RightGuide |=| >
            Base_RightGuideContainer.Child = Base_RightGuideGrid;

            #region ->
            Base_RightGuideGrid.Children.Add(Base_RightGuideLine);

            Base_RightGuidePolyLine.Points.Add(new(0d, 0d));
            Base_RightGuidePolyLine.Points.Add(new(0d, 0d));
            Base_RightGuidePolyLine.Points.Add(new(0d, 0d));
            Base_RightGuideGrid.Children.Add(Base_RightGuidePolyLine);
            #endregion

            Grid.SetColumn(Base_RightGuideContainer, 2);
            Base_HeadGridContentButton.Children.Add(Base_RightGuideContainer);
            #endregion

            Grid.SetColumn(Base_ButtonContentContainer, 1);
            Base_HeadGridContentButton.Children.Add(Base_ButtonContentContainer);

            #region MouseActivate
            Base_BorderContainer.MouseDown += (sender, e) =>
            {
                if (IsEnabled && IsEnabledActivateMouse &&
                (BasicActivate != null || AdditionalActivate != null) &&
                (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed))
                {
                    SetActiveSpecrum(SpectrumColor.Used);
                    SourceTimer.Stop();
                }
            };

            Base_BorderContainer.MouseLeftButtonUp += (sender, e) =>
            {
                if (IsEnabled && BasicActivate != null && IsEnabledActivateMouse)
                {
                    SetActiveSpecrum(SpectrumColor.Select);
                    BasicActivate.Invoke(this, e);
                }
            };

            Base_BorderContainer.MouseRightButtonUp += (sender, e) =>
            {
                if (IsEnabled && AdditionalActivate != null && IsEnabledActivateMouse)
                {
                    SetActiveSpecrum(SpectrumColor.Select);
                    AdditionalActivate.Invoke(this, e);
                }
            };
            #endregion

            #region KeyActivate
            base.PreviewKeyDown += (sender, e) =>
            {
                if (e.IsRepeat || (e.Key != Key.Enter && e.Key != ActivateKey)) return;
                else if (IsEnabled && IsEnabledActivateKeyboard && 
                ((BasicActivate != null && e.Key == Key.Enter) ||
                (AdditionalActivate != null && e.Key == ActivateKey)))
                {
                    SetActiveSpecrum(SpectrumColor.Used);
                    SourceTimer.Stop();
                    // (BasicActivate != null && IsActivateKeyBasicEvent) || (AdditionalActivate != null && !IsActivateKeyBasicEvent)
                }
            };

            base.PreviewKeyUp += (sender, e) =>
            {
                if (IsEnabled && IsEnabledActivateKeyboard && (e.Key == ActivateKey || e.Key == Key.Enter))
                {
                    SetActiveSpecrum(SpectrumColor.Select);
                    if (BasicActivate != null && e.Key == Key.Enter)
                        BasicActivate.Invoke(this, new(Mouse.PrimaryDevice, 0, MouseButton.Left));
                    else if (AdditionalActivate != null && e.Key == ActivateKey)
                        AdditionalActivate.Invoke(this, new(Mouse.PrimaryDevice, 0, MouseButton.Right));
                    //else
                    //{
                    //    if (BasicActivate != null && IsActivateKeyBasicEvent)
                    //        BasicActivate.Invoke(this, new(Mouse.PrimaryDevice, 0, MouseButton.Left));
                    //    else if (AdditionalActivate != null && !IsActivateKeyBasicEvent)
                    //        AdditionalActivate.Invoke(this, new(Mouse.PrimaryDevice, 0, MouseButton.Right));
                    //}
                }
            };
            #endregion

            GotFocus += (sender, e) =>
            {
                IsVisibleSetConst = IsVisibleActivateKey;
                if (!IsVisibleSetConst)
                    IsVisibleActivateKey = true;
                SetActiveSpecrum(SpectrumColor.Select);
                //Base_BorderContainer.Focus();
            };

            LostFocus += (sender, e) =>
            {
                if (!IsVisibleSetConst)
                    IsVisibleActivateKey = false;
                SetActiveSpecrum(SpectrumColor.Default);
            };

            Base_BorderContainer.ClipToBounds = true;
            base.SetValue(IELContainerBase.ContentProperty, Base_ViewBoxButton);
            UpdateGuideVisual();
        }

        /// <summary>
        /// Обновить отображение позиционирования обоих направляющих
        /// </summary>
        private void UpdateGuideVisual()
        {
            double CenterY = GuidesDistanceBetweenPetalLines / 2d;

            Base_LeftGuideLine.X1 = GuidesOffsetPetalLines + GuidesOffsetMainLine;
            Base_LeftGuidePolyLine.Points[0] = new(GuidesOffsetPetalLines, 0d);
            Base_LeftGuidePolyLine.Points[1] = new(0, CenterY);
            Base_LeftGuidePolyLine.Points[2] = new(GuidesOffsetPetalLines, GuidesDistanceBetweenPetalLines);

            Base_RightGuideLine.X2 = GuidesOffsetPetalLines + GuidesOffsetMainLine;
            Base_RightGuidePolyLine.Points[0] = new(0d, 0d);
            Base_RightGuidePolyLine.Points[1] = new(GuidesOffsetPetalLines, CenterY);
            Base_RightGuidePolyLine.Points[2] = new(0d, GuidesDistanceBetweenPetalLines);
        }
    }
}
