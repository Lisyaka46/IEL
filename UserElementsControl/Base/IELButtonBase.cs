using IEL.CORE.Enums;
using LibraryIEL.CORE.Themes.Palettes;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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

        #region UIElements
        /// <summary>
        /// Главный объект отображения содержимого кнопки
        /// </summary>
        private Viewbox Base_ViewBoxButton;

        /// <summary>
        /// Главный контейнер кнопки
        /// </summary>
        private Grid Base_HeadGridButton;

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

        #region OnActivateMouseEvents
        /// <summary>
        /// Объект события активации левым щелчком мыши
        /// </summary>
        public event MouseButtonEventHandler? OnActivateMouseLeft;

        /// <summary>
        /// Объект события активации правым щелчком мыши
        /// </summary>
        public event MouseButtonEventHandler? OnActivateMouseRight;
        #endregion

        #region Properties

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
                Source.Base_HeadGridButton.ColumnDefinitions[0].Width = new(0d,
                    SourceNewValue == StateVisualGuide.LeftArrow || SourceNewValue == StateVisualGuide.DuoArrow ?
                    GridUnitType.Auto : GridUnitType.Pixel);
                Source.Base_HeadGridButton.ColumnDefinitions[2].Width = new(0d,
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
        /// Инициализация базового класса визуализации кнопки IEL
        /// </summary>
        protected IELButtonBase() : base()
        {
            Base_ViewBoxButton = new()
            {
                Stretch = Stretch.Uniform,
                StretchDirection = StretchDirection.DownOnly,
            };
            Base_HeadGridButton = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            Base_HeadGridButton.ColumnDefinitions.Add(new() { Width = new(0d, GridUnitType.Pixel) });
            Base_HeadGridButton.ColumnDefinitions.Add(new() { Width = new(1d, GridUnitType.Star) });
            Base_HeadGridButton.ColumnDefinitions.Add(new() { Width = new(0d, GridUnitType.Pixel) });
            Base_ViewBoxButton.Child = Base_HeadGridButton;

            #region LeftGuide < |=|
            Base_LeftGuideContainer = new()
            {
                Margin = new(2d),
                Padding = new(2d),
                CornerRadius = new(0d),
                BorderThickness = new(2d),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                BorderBrush = SourceBorderBrush.SourceBrush,
            };
            Base_LeftGuideGrid = new()
            {
                Margin = new(0d),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,

            };
            Base_LeftGuideContainer.Child = Base_LeftGuideGrid;

            #region <-
            Base_LeftGuideLine = new()
            {
                Margin = new(0d),
                X1 = 8d,
                X2 = 0d,
                StrokeThickness = 2d,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Stretch = Stretch.None,
                Stroke = SourceBorderBrush.SourceBrush,
            };
            Base_LeftGuideGrid.Children.Add(Base_LeftGuideLine);
            Base_LeftGuidePolyLine = new()
            {
                Margin = new(0d),
                StrokeThickness = 2d,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Stretch = Stretch.None,
                Stroke = SourceBorderBrush.SourceBrush,
            };
            Base_LeftGuidePolyLine.Points.Add(new(8d, 0d));
            Base_LeftGuidePolyLine.Points.Add(new(0d, 5d));
            Base_LeftGuidePolyLine.Points.Add(new(8d, 10d));
            Base_LeftGuideGrid.Children.Add(Base_LeftGuidePolyLine);
            #endregion

            Grid.SetColumn(Base_LeftGuideContainer, 0);
            Base_HeadGridButton.Children.Add(Base_LeftGuideContainer);
            #endregion

            #region RightGuide |=| >
            Base_RightGuideContainer = new()
            {
                Margin = new(2d),
                Padding = new(2d),
                CornerRadius = new(0d),
                BorderThickness = new(2d),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                BorderBrush = SourceBorderBrush.SourceBrush,
                ClipToBounds = true,
            };
            Base_RightGuideGrid = new()
            {
                Margin = new(0d),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,

            };
            Base_RightGuideContainer.Child = Base_RightGuideGrid;

            #region ->
            Base_RightGuideLine = new()
            {
                Margin = new(0d),
                X1 = 0d,
                X2 = 8d,
                StrokeThickness = 2d,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Stretch = Stretch.None,
                Stroke = SourceBorderBrush.SourceBrush,
            };
            Base_RightGuideGrid.Children.Add(Base_RightGuideLine);
            Base_RightGuidePolyLine = new()
            {
                Margin = new(3.5d, 0d, 0d, 0d),
                StrokeThickness = 2d,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Stretch = Stretch.None,
                Stroke = SourceBorderBrush.SourceBrush,
            };
            Base_RightGuidePolyLine.Points.Add(new(0d, 0d));
            Base_RightGuidePolyLine.Points.Add(new(8d, 5d));
            Base_RightGuidePolyLine.Points.Add(new(0d, 10d));
            Base_RightGuideGrid.Children.Add(Base_RightGuidePolyLine);
            #endregion

            Grid.SetColumn(Base_RightGuideContainer, 2);
            Base_HeadGridButton.Children.Add(Base_RightGuideContainer);
            #endregion

            Base_ButtonContentContainer = new()
            {
                Margin = new(5),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            Grid.SetColumn(Base_ButtonContentContainer, 1);
            Base_HeadGridButton.Children.Add(Base_ButtonContentContainer);


            Base_BorderContainer.MouseDown += (sender, e) =>
            {
                if (IsEnabled)
                {
                    if (
                    (e.LeftButton == MouseButtonState.Pressed && OnActivateMouseLeft != null) ||
                    (e.RightButton == MouseButtonState.Pressed && OnActivateMouseRight != null))
                    {
                        SetActiveSpecrum(SpectrumColor.Used);
                        SourceTimer.Stop();
                    }
                }
            };

            Base_BorderContainer.MouseLeftButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseLeft != null)
                {
                    SetActiveSpecrum(SpectrumColor.Select);
                    OnActivateMouseLeft.Invoke(this, e);
                }
            };

            Base_BorderContainer.MouseRightButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    SetActiveSpecrum(SpectrumColor.Select);
                    OnActivateMouseRight.Invoke(this, e);
                }
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

            Base_LeftGuideLine.X1 = GuidesOffsetPetalLines + Math.PI;
            Base_LeftGuidePolyLine.Points[0] = new(GuidesOffsetPetalLines, 0d);
            Base_LeftGuidePolyLine.Points[1] = new(0, CenterY);
            Base_LeftGuidePolyLine.Points[2] = new(GuidesOffsetPetalLines, GuidesDistanceBetweenPetalLines);

            Base_RightGuideLine.X2 = GuidesOffsetPetalLines + Math.PI;
            Base_RightGuidePolyLine.Points[0] = new(0d, 0d);
            Base_RightGuidePolyLine.Points[1] = new(GuidesOffsetPetalLines, CenterY);
            Base_RightGuidePolyLine.Points[2] = new(0d, GuidesDistanceBetweenPetalLines);
        }
    }
}
