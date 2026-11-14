using LibraryIEL.CORE.Themes.Palettes;
using LibraryIEL.Interfaces;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace LibraryIEL.UserElementsControl.Base
{
    /// <summary>
    /// <b>БАЗОВЫЙ КЛАСС</b><br/>
    /// Контейнер IEL
    /// </summary>
    public class IELContainerBase : IELObjectBase, IIELObjectUsingMessage
    {
        #region UIElements
        /// <summary>
        /// Базовый объект в котором реализуется наследуемый элемент
        /// </summary>
        protected Border Base_BorderContainer { get; private set; }
        #endregion

        /// <summary>
        /// Состояние указывающее на вход курсора в границы визуального объекта
        /// </summary>
        protected bool IsEnter { get; private set; }

        #region Properties

        #region IntervalHover
        /// <summary>
        /// Таймер срабатывания задержки
        /// </summary>
        protected DispatcherTimer SourceTimer { get; private set; }

        /// <summary>
        /// Данные свойства <see cref="IntervalHover"/>
        /// </summary>
        public static readonly DependencyProperty IntervalHoverProperty =
            DependencyProperty.Register(nameof(IntervalHover), typeof(TimeSpan), typeof(IELContainerBase),
                new(TimeSpan.FromMilliseconds(1000d), IntervalHoverHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="IntervalHover"/>
        /// </summary>
        private static void IntervalHoverHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELContainerBase Source && e.NewValue is TimeSpan SourceNewValue)
            {
                Source.SourceTimer.Interval = SourceNewValue;
            }
        }

        /// <summary>
        /// Время задержки перед срабатыванием события удержания мыши в кнопке
        /// <see cref="MouseHover"/>
        /// </summary>
        [Description("Временной промежуток, отвечающий за срабатывание события задержки курсора мыши на UI-объекте.\n" +
            $"Событие задержки курсора: {nameof(MouseHover)}")]
        [Category(Category_Management)]
        public TimeSpan IntervalHover
        {
            get => (TimeSpan)GetValue(IntervalHoverProperty);
            set => SetValue(IntervalHoverProperty, value);
        }

        /// <summary>
        /// Событие задержки курсора на элементе
        /// </summary>
        public event EventHandler? MouseHover;
        #endregion

        #region Content
        /// <summary>
        /// Данные свойства <see cref="Content"/>
        /// </summary>
        public static readonly new DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(UIElement), typeof(IELContainerBase),
                new(ContentHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="Content"/>
        /// </summary>
        private static void ContentHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELContainerBase Source && e.NewValue is UIElement SourceNewValue)
            {
                Source.Base_BorderContainer.Child = SourceNewValue;
            }
        }

        /// <summary>
        /// Внутренний элемент объекта
        /// </summary>
        [Description("Контент элемента внутри границ барьера")]
        [Category(Category_Main)]
        public new UIElement Content
        {
            get => (UIElement)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        #endregion

        #region CornerRadius
        /// <summary>
        /// Данные свойства <see cref="CornerRadius"/>
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(IELContainerBase),
                new(new CornerRadius(0), CornerRadiusHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="CornerRadius"/>
        /// </summary>
        private static void CornerRadiusHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELContainerBase Source && e.NewValue is CornerRadius SourceNewValue)
            {
                Source.Base_BorderContainer.CornerRadius = SourceNewValue;
            }
        }

        /// <summary>
        /// Скругление границ объекта
        /// </summary>
        [Description("Скругление границ главного контейнера элемента")]
        [Category(Category_Visual)]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        #endregion

        #region BorderThickness
        /// <summary>
        /// Обработчик события изменения свойства <see cref="BorderThickness"/>
        /// </summary>
        private static void BorderThicknessHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELContainerBase Source && e.NewValue is Thickness SourceNewValue)
            {
                Source.Base_BorderContainer.BorderThickness = SourceNewValue;
            }
        }

        /// <summary>
        /// Толщина границ объекта
        /// </summary>
        [Description("Толщина границ главного контейнера элемента")]
        [Category(Category_Visual)]
        public new Thickness BorderThickness
        {
            get => (Thickness)Base_BorderContainer.GetValue(BorderThicknessProperty);
            set => Base_BorderContainer.SetValue(BorderThicknessProperty, value);
        }
        #endregion

        #region Padding
        /// <summary>
        /// Обработчик события изменения свойства <see cref="Padding"/>
        /// </summary>
        private static void PaddingHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELContainerBase Source && e.NewValue is Thickness SourceNewValue)
            {
                Source.Base_BorderContainer.Padding = SourceNewValue;
            }
        }

        /// <summary>
        /// Внутреннее смещение в объекте
        /// </summary>
        [Description("Смещение контента внутри границ элемента")]
        [Category(Category_Visual)]
        public new Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }
        #endregion

        #region Message
        /// <summary>
        /// Данные свойства <see cref="Message"/>
        /// </summary>
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(IELContainerBase),
                new(string.Empty));

        /// <summary>
        /// Сообщение для отображения сводки об объекте через объект <see cref="IELBlockMessage"/>
        /// </summary>
        [Description($"Сообщение, отображаемое с помощью \"{nameof(IELBlockMessage)}\"")]
        [Category(Category_Message)]
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }
        #endregion

        #region IsEnabled
        /// <summary>
        /// Обработчик события изменения свойства <see cref="IsEnabled"/>
        /// </summary>
        private static void IsEnabledHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
            {
                if (Element is IELContainerBase Source && e.NewValue is bool SourceNewValue)
                {
                    Source.Base_BorderContainer.IsEnabled = SourceNewValue;
                }
            }

        /// <summary>
        /// Состояние активности элемента
        /// </summary>
        [Description("Состояние активности элемента.\n" +
            $"Управляется Q-логикой ({nameof(Palette)})")]
        [Category(Category_Main)]
        public new bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }
        #endregion

        #endregion
        
        /// <summary>
        /// Статический конструктор для переопределения свойств
        /// </summary>
        static IELContainerBase()
        {
            BorderThicknessProperty.OverrideMetadata(typeof(IELContainerBase), new FrameworkPropertyMetadata(new Thickness(2), BorderThicknessHandler));
            PaddingProperty.OverrideMetadata(typeof(IELContainerBase), new FrameworkPropertyMetadata(new Thickness(0), PaddingHandler));
            IsEnabledProperty.OverrideMetadata(typeof(IELContainerBase), new FrameworkPropertyMetadata(true, IsEnabledHandler));
        }

        /// <summary>
        /// Инициализировать базовый класс визуализации контейнера
        /// </summary>
        protected IELContainerBase() : base()
        {
            Focusable = true;
            Base_BorderContainer = new()
            {
                BorderThickness = new(2),
                Background = SourceBackground.SourceBrush,
                BorderBrush = SourceBorderBrush.SourceBrush,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Focusable = false
            };
            SourceTimer = new()
            {
                Interval = IntervalHover,
            };
            SourceTimer.Tick += (sender, e) =>
            {
                MouseHover?.Invoke(this, e);
                SourceTimer.Stop();
            };

            MouseEnter += (sender, e) =>
            {
                if (IsEnabled)
                {
                    if (!IsFocused) SetActiveSpecrum(SpectrumColor.Select);
                    IsEnter = true;
                    SourceTimer.Start();
                }
            };

            MouseLeave += (sender, e) =>
            {
                if (IsEnabled)
                {
                    if (!IsFocused) SetActiveSpecrum(SpectrumColor.Default);
                    IsEnter = false;
                    SourceTimer.Stop();
                }
            };

            IsEnabledChanged += (sender, e) =>
            {
                bool Value = (bool)e.NewValue;
                SourceTimer.Stop();
                Cursor = Value ? Cursors.Hand : Cursors.No;
                SetActiveSpecrum(Value ? SpectrumColor.Default : SpectrumColor.NotEnabled);
            };
            SetValue(ContentControl.ContentProperty, Base_BorderContainer);
        }

        /// <summary>
        /// Узнать позицию и размер объекта
        /// </summary>
        /// <param name="RelativeTo">Относительно какого элемента расчитывается позиция</param>
        public Rect GetRectFrom(UIElement RelativeTo) =>
            new(TranslatePoint(TransformToVisual(this).Transform(default), RelativeTo), new Size(ActualWidth, ActualHeight));
    }
}
