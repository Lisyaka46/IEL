using IEL.CORE.Enums;
using LibraryIEL.CORE.Themes.Palettes;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace IEL.UserElementsControl.Base
{
    /// <summary>
    /// <b>БАЗОВЫЙ КЛАСС</b><br/>
    /// Кнопка IEL
    /// </summary>
    public class IELButtonBase : IELContainerBase
    {
        #region UIElements
        /// <summary>
        /// Главный контейнер кнопки
        /// </summary>
        private Grid Base_HeadGridButton;

        /// <summary>
        /// Левый элемент отображения направляющей
        /// </summary>
        private Border Base_LeftBorderGuideButton; // < |=|

        /// <summary>
        /// Правый элемент отображения направляющей
        /// </summary>
        private Border Base_RightBorderGuideButton; // |=| >

        /// <summary>
        /// Главный объект отображения содержимого кнопки
        /// </summary>
        protected Viewbox Base_ViewBoxButton;
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
                Source.Base_ViewBoxButton.Child = SourceNewValue;
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
                new(new Thickness(5), PaddingButtonContentHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="PaddingButtonContent"/>
        /// </summary>
        private static void PaddingButtonContentHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonBase Source && e.NewValue is Thickness SourceNewValue)
            {
                Source.Base_ViewBoxButton.Margin = SourceNewValue;
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

        #region CornerRadiusGuides
        /// <summary>
        /// Данные свойства <see cref="CornerRadiusGuides"/>
        /// </summary>
        public static readonly DependencyProperty CornerRadiusGuidesProperty =
            DependencyProperty.Register(nameof(CornerRadiusGuides), typeof(CornerRadius), typeof(IELButtonBase),
                new(new CornerRadius(0), CornerRadiusGuidesHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="CornerRadiusGuides"/>
        /// </summary>
        private static void CornerRadiusGuidesHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonBase Source && e.NewValue is CornerRadius SourceNewValue)
            {
                Source.Base_LeftBorderGuideButton.CornerRadius = SourceNewValue;
                Source.Base_RightBorderGuideButton.CornerRadius = SourceNewValue;
            }
        }

        /// <summary>
        /// Скругление границ объектов направляющих кнопки
        /// </summary>
        [Description("Скругление границ направляющих для кнопки.\n" +
            "Направляющие представляют собой индикаторы векторных стрелок, которые являются лишь визуальной состовляющей кнопки.\n" +
            $"Направляющие НЕ являются частью свойства {nameof(Content)}")]
        public CornerRadius CornerRadiusGuides
        {
            get => (CornerRadius)GetValue(CornerRadiusGuidesProperty);
            set => SetValue(CornerRadiusGuidesProperty, value);
        }
        #endregion

        #region BorderThicknessGuides
        /// <summary>
        /// Данные свойства <see cref="BorderThicknessGuides"/>
        /// </summary>
        public static readonly DependencyProperty BorderThicknessGuidesProperty =
            DependencyProperty.Register(nameof(BorderThicknessGuides), typeof(Thickness), typeof(IELButtonBase),
                new(new Thickness(2), BorderThicknessGuidesHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="BorderThicknessGuides"/>
        /// </summary>
        private static void BorderThicknessGuidesHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELButtonBase Source && e.NewValue is Thickness SourceNewValue)
            {
                Source.Base_LeftBorderGuideButton.BorderThickness = SourceNewValue;
                Source.Base_RightBorderGuideButton.BorderThickness = SourceNewValue;
            }
        }

        /// <summary>
        /// Толщина границ направляющих кнопки
        /// </summary>
        [Description("Толщина границ направляющих для кнопки.\n" +
            "Направляющие представляют собой индикаторы векторных стрелок, которые являются лишь визуальной состовляющей кнопки.\n" +
            $"Направляющие НЕ являются частью свойства {nameof(Content)}")]
        public Thickness BorderThicknessGuides
        {
            get => (Thickness)GetValue(BorderThicknessGuidesProperty);
            set => SetValue(BorderThicknessGuidesProperty, value);
        }
        #endregion

        #region VisualGuide
        /// <summary>
        /// Данные свойства <see cref="VisualGuide"/>
        /// </summary>
        public static readonly DependencyProperty VisualGuideProperty =
            DependencyProperty.Register(nameof(VisualGuide), typeof(StateVisualGuide), typeof(IELButtonBase),
                new(StateVisualGuide.Default, VisualGuideHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="VisualGuide"/>
        /// </summary>
        private static void VisualGuideHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
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
        [Description("Состояние отображения направляющих в элементе кнопки.\n" +
            "Направляющие представляют собой индикаторы векторных стрелок, которые являются лишь визуальной состовляющей кнопки.\n" +
            $"Направляющие НЕ являются частью свойства {nameof(Content)}")]
        public StateVisualGuide VisualGuide
        {
            get => (StateVisualGuide)GetValue(VisualGuideProperty);
            set => SetValue(VisualGuideProperty, value);
        }

        /// <summary>
        /// Событие изменения состояния отображения направляющих кнопки
        /// </summary>
        [Description("Событие изменение визуализации направляющих")]
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
            Base_HeadGridButton = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            Base_HeadGridButton.ColumnDefinitions.Add(new() { Width = new(0d, GridUnitType.Pixel) });
            Base_HeadGridButton.ColumnDefinitions.Add(new() { Width = new(1d, GridUnitType.Star) });
            Base_HeadGridButton.ColumnDefinitions.Add(new() { Width = new(0d, GridUnitType.Pixel) });

            Base_LeftBorderGuideButton = new() // < |=|
            {
                Width = 20d,
                Height = 20d,
                Margin = new(2),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                CornerRadius = new(0),
                BorderThickness = new(2),
                BorderBrush = SourceBorderBrush.SourceBrush,
                Child = new Viewbox()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Child = new TextBlock()
                    {
                        Padding = new(0d, 0.7d, 1d, 0d),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Foreground = SourceForeground.SourceBrush,
                        Text = "<",
                        FontFamily = new("Arial Black"),
                    }
                }
            };
            Base_RightBorderGuideButton = new() // |=| >
            {
                Width = 20d,
                Height = 20d,
                Margin = new(2),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                CornerRadius = new(0),
                BorderThickness = new(2),
                BorderBrush = SourceBorderBrush.SourceBrush,
                Child = new Viewbox()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Child = new TextBlock()
                    {
                        Padding = new(1d, 0.7d, 0d, 0d),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Foreground = SourceForeground.SourceBrush,
                        Text = ">",
                        FontFamily = new("Arial Black"),
                    }
                }
            };

            Base_ViewBoxButton = new()
            {
                Margin = new(5),
                Stretch = System.Windows.Media.Stretch.Uniform,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            Grid.SetColumn(Base_LeftBorderGuideButton, 0);
            Grid.SetColumn(Base_ViewBoxButton, 1);
            Grid.SetColumn(Base_RightBorderGuideButton, 2);
            Base_HeadGridButton.Children.Add(Base_LeftBorderGuideButton);
            Base_HeadGridButton.Children.Add(Base_ViewBoxButton);
            Base_HeadGridButton.Children.Add(Base_RightBorderGuideButton);


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

            base.SetValue(IELContainerBase.ContentProperty, Base_HeadGridButton);
        }
    }
}
