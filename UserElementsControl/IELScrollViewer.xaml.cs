using IEL.CORE.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace IEL.UserElementsControl
{
    /// <summary>
    /// Базовый класс объекта отображения стековых данных
    /// </summary>
    public partial class IELScrollViewer : UserControl
    {
        /// <summary>
        /// Объект анимации Double
        /// </summary>
        private static DoubleAnimation DoubleAnimationType =
            new(0, TimeSpan.FromMilliseconds(400d))
            {
                DecelerationRatio = 0.2d,
                EasingFunction = new QuinticEase() { EasingMode = EasingMode.EaseOut },
                From = null
            };

        /// <summary>
        /// Объект анимации Thickness
        /// </summary>
        private static ThicknessAnimation ThicknessAnimationType =
            new(new(0), TimeSpan.FromMilliseconds(400d))
            {
                DecelerationRatio = 0.2d,
                EasingFunction = new QuinticEase() { EasingMode = EasingMode.EaseOut },
                From = null
            };

        #region Properties

        #region Content
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(FrameworkElement), typeof(IELScrollViewer),
                new(null,
                    (sender, e) =>
                    {
                        FrameworkElement? Value = (FrameworkElement?)e.NewValue;
                        IELScrollViewer Element = (IELScrollViewer)sender;
                        Element.UserControlContent.Child = Value;
                        ((FrameworkElement?)e.OldValue)?.SizeChanged -= Element.Value_SizeChanged;
                        if (Value != null)
                        {
                            Value.SizeChanged += Element.Value_SizeChanged;
                            Element.RectangleVerticalScroll.VerticalAlignment =
                                Value.VerticalAlignment is VerticalAlignment.Top or VerticalAlignment.Bottom ?
                                Value.VerticalAlignment : VerticalAlignment.Top;
                            Element.RectangleHorizontalScroll.HorizontalAlignment =
                                Value.HorizontalAlignment is HorizontalAlignment.Left or HorizontalAlignment.Right ?
                                Value.HorizontalAlignment : HorizontalAlignment.Left;
                        }
                    }));

        private void Value_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.HeightChanged)
            {
                if (MaxHeightVerticalScroll > 0d && UserControlContent.ActualHeight > 0d)
                {
                    if (RectangleVerticalScroll.Width is 0d or double.NaN)
                        RectangleVerticalScroll.Width = ThicknessScroll;
                    RectangleVerticalScroll.Height = Math.Max(MinScrollSize, ActualHeight - MaxHeightVerticalScroll / ScrollForce);
                    if (ActualVerticalOffset > 0d && Content?.VerticalAlignment == VerticalAlignment.Bottom)
                        ScrollToVerticalOffset(ActualVerticalOffset + (e.NewSize.Height - e.PreviousSize.Height) / ScrollForce);
                }
                else RectangleVerticalScroll.Width = 0d;
            }

            if (e.WidthChanged)
            {
                if (MaxWidthHorizontalScroll > 0d && UserControlContent.ActualWidth > 0d)
                {
                    if (RectangleHorizontalScroll.Height is 0d or double.NaN)
                        RectangleHorizontalScroll.Height = ThicknessScroll;
                    RectangleHorizontalScroll.Width = Math.Max(MinScrollSize, ActualWidth - MaxWidthHorizontalScroll / ScrollForce);
                    if (ActualHorizontalOffset > 0d && Content?.HorizontalAlignment == HorizontalAlignment.Right)
                        ScrollToHorizontalOffset(ActualHorizontalOffset + (e.NewSize.Width - e.PreviousSize.Width) / ScrollForce);
                }
                else RectangleHorizontalScroll.Height = 0d;
            }
        }

        /// <summary>
        /// Внутренний элемент объекта
        /// </summary>
        public new FrameworkElement? Content
        {
            get => (FrameworkElement?)GetValue(ContentProperty);
            set
            {
                SetValue(ContentProperty, value);
            }
        }
        #endregion

        #region HorizontalScrollAligment
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty HorizontalScrollAligmentProperty =
            DependencyProperty.Register("HorizontalScrollAligment", typeof(HorizontalScrollAlignment), typeof(IELScrollViewer),
                new(HorizontalScrollAlignment.Up,
                    (sender, e) =>
                    {
                        HorizontalScrollAlignment Value = (HorizontalScrollAlignment)e.NewValue;
                        IELScrollViewer Element = (IELScrollViewer)sender;
                        Element.MainGrid.RowDefinitions[0].Height = new(Value == 0 ? 0d : 1d, (GridUnitType)(Value == 0 ? 0 : 2));
                        Element.MainGrid.RowDefinitions[1].Height = new(Value == 0 ? 1d : 0d, (GridUnitType)(Value == 0 ? 2 : 0));
                        Grid.SetRow(Element.RectangleHorizontalScroll, (int)Value);
                        Grid.SetRow(Element.RectangleVerticalScroll, 1 - (int)Value);

                        Grid.SetRow(Element.UserControlContent, 1 - (int)Value);
                    }));

        /// <summary>
        /// Ориентация позиционирования горизонтального скроллбара в объекте
        /// </summary>
        public HorizontalScrollAlignment HorizontalScrollAligment
        {
            get => (HorizontalScrollAlignment)GetValue(HorizontalScrollAligmentProperty);
            set => SetValue(HorizontalScrollAligmentProperty, value);
        }
        #endregion

        #region VerticalScrollAligment
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty VerticalScrollAligmentProperty =
            DependencyProperty.Register("VerticalScrollAligment", typeof(VerticalScrollAlignment), typeof(IELScrollViewer),
                new(VerticalScrollAlignment.Right,
                    (sender, e) =>
                    {
                        VerticalScrollAlignment Value = (VerticalScrollAlignment)e.NewValue;
                        IELScrollViewer Element = (IELScrollViewer)sender;
                        Element.MainGrid.ColumnDefinitions[0].Width = new(Value == 0 ? 0d : 1d, (GridUnitType)(Value == 0 ? 0 : 2));
                        Element.MainGrid.ColumnDefinitions[1].Width = new(Value == 0 ? 1d : 0d, (GridUnitType)(Value == 0 ? 2 : 0));
                        Grid.SetColumn(Element.RectangleVerticalScroll, (int)Value);
                        Grid.SetColumn(Element.RectangleHorizontalScroll, 1 - (int)Value);

                        Grid.SetColumn(Element.UserControlContent, 1 - (int)Value);
                    }));

        /// <summary>
        /// Ориентация позиционирования вертикального скроллбара в объекте
        /// </summary>
        public VerticalScrollAlignment VerticalScrollAligment
        {
            get => (VerticalScrollAlignment)GetValue(VerticalScrollAligmentProperty);
            set
            {
                SetValue(VerticalScrollAligmentProperty, value);
            }
        }
        #endregion

        #region ScrollForce
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty ScrollForceProperty =
            DependencyProperty.Register("ScrollForce", typeof(int), typeof(IELScrollViewer),
                new(5,
                    (sender, e) =>
                    {

                    }));

        /// <summary>
        /// Сила прокрутки от 1 позиции мыши
        /// </summary>
        public int ScrollForce
        {
            get => (int)GetValue(ScrollForceProperty);
            set
            {
                SetValue(ScrollForceProperty, value);
            }
        }
        #endregion

        #region MinScrollSize
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty MinScrollSizeProperty =
            DependencyProperty.Register("MinScrollSize", typeof(int), typeof(IELScrollViewer),
                new(50,
                    (sender, e) =>
                    {

                    }));

        /// <summary>
        /// Минимальный размер для отображения полосы прокрутки
        /// </summary>
        public int MinScrollSize
        {
            get => (int)GetValue(MinScrollSizeProperty);
            set
            {
                SetValue(MinScrollSizeProperty, value);
            }
        }
        #endregion

        #region ThicknessScroll
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty ThicknessScrollProperty =
            DependencyProperty.Register("ThicknessScroll", typeof(double), typeof(IELScrollViewer),
                new(10d,
                    (sender, e) =>
                    {

                    }));

        /// <summary>
        /// Толщина полосы прокрутки
        /// </summary>
        public double ThicknessScroll
        {
            get => (double)GetValue(ThicknessScrollProperty);
            set
            {
                SetValue(ThicknessScrollProperty, value);
            }
        }
        #endregion

        #endregion

        #region Horizontal Control
        /// <summary>
        /// Состояние использования прокрутки с помощью мыши
        /// </summary>
        private bool IsCursorHorizontalUse = false;

        /// <summary>
        /// Начальная позиция курсора прокрутки по горизонтали
        /// </summary>
        private double StartHorizontalOffset = 0d;

        /// <summary>
        /// Начальная позиция элемента скроллбара по горизонтали
        /// </summary>
        private double StartLeftHorizontal = 0d;

        /// <summary>
        /// Текущее значение прокрутки по горизонтали
        /// </summary>
        public double ActualHorizontalOffset =>
            Content?.HorizontalAlignment == HorizontalAlignment.Right ?
            RectangleHorizontalScroll.Margin.Right : RectangleHorizontalScroll.Margin.Left;

        /// <summary>
        /// Максимальное значение смещение барьера по горизонтали
        /// </summary>
        private double MaxHorizontalOffset =>
            Content?.ActualWidth > ActualWidth && RectangleHorizontalScroll.Height > 0d ?
            ActualWidth - RectangleHorizontalScroll.Width : 0d;

        /// <summary>
        /// Максимальное значение смещение контента по горизонтали
        /// </summary>
        private double MaxWidthHorizontalScroll => (Content?.ActualWidth - ActualWidth) ?? 0d;
        #endregion

        #region Vertical Control
        /// <summary>
        /// Состояние использования прокрутки с помощью мыши
        /// </summary>
        private bool IsCursorVerticalUse = false;

        /// <summary>
        /// Начальная позиция курсора прокрутки по вертикали
        /// </summary>
        private double StartVerticalOffset = 0d;

        /// <summary>
        /// Начальная позиция элемента скроллбара по вертикали
        /// </summary>
        private double StartTopVertical = 0d;

        /// <summary>
        /// Текущее значение прокрутки по вертикали
        /// </summary>
        public double ActualVerticalOffset =>
            Content?.VerticalAlignment == VerticalAlignment.Bottom ?
            RectangleVerticalScroll.Margin.Bottom : RectangleVerticalScroll.Margin.Top;

        /// <summary>
        /// Максимальное значение прокрутки по вертикали
        /// </summary>
        private double MaxVerticalOffset =>
            Content?.ActualHeight > ActualHeight && RectangleVerticalScroll.Width > 0d ?
            ActualHeight - RectangleVerticalScroll.Height : 0d;

        /// <summary>
        /// Максимальное значение прокрутки контента по вертикали
        /// </summary>
        private double MaxHeightVerticalScroll => (Content?.ActualHeight - ActualHeight) ?? 0d;
        #endregion

        /// <summary>
        /// Инициализация базового класса объекта визуализации стековых данных
        /// </summary>
        public IELScrollViewer()
        {
            InitializeComponent();

            #region HorizontalScroll
            RectangleHorizontalScroll.MouseLeftButtonDown += (sender, e) =>
            {
                Rectangle Element = (Rectangle)sender;
                StartHorizontalOffset = ActualHorizontalOffset;
                StartLeftHorizontal = e.GetPosition(this).X;
                IsCursorHorizontalUse = true;
                Element.CaptureMouse();
            };
            RectangleHorizontalScroll.MouseLeftButtonUp += (sender, e) =>
            {
                Rectangle Element = (Rectangle)sender;
                Element.ReleaseMouseCapture();
                IsCursorHorizontalUse = false;
            };
            RectangleHorizontalScroll.MouseMove += (sender, e) =>
            {
                if (IsCursorHorizontalUse && Content != null)
                {
                    Rectangle Element = (Rectangle)sender;
                    double DeltaX = e.GetPosition(this).X - StartLeftHorizontal;
                    if (Content.HorizontalAlignment == HorizontalAlignment.Right) DeltaX = -DeltaX;
                    ScrollToHorizontalOffset(StartHorizontalOffset + DeltaX);
                }
            };
            #endregion

            #region VerticalScroll
            RectangleVerticalScroll.MouseLeftButtonDown += (sender, e) =>
            {
                Rectangle Element = (Rectangle)sender;
                StartVerticalOffset = ActualVerticalOffset;
                StartTopVertical = e.GetPosition(this).Y;
                IsCursorVerticalUse = true;
                Element.CaptureMouse();
            };
            RectangleVerticalScroll.MouseLeftButtonUp += (sender, e) =>
            {
                Rectangle Element = (Rectangle)sender;
                Element.ReleaseMouseCapture();
                IsCursorVerticalUse = false;
            };
            RectangleVerticalScroll.MouseMove += (sender, e) =>
            {
                if (IsCursorVerticalUse && Content != null)
                {
                    Rectangle Element = (Rectangle)sender;
                    double DeltaY = e.GetPosition(this).Y - StartTopVertical;
                    if (Content.VerticalAlignment == VerticalAlignment.Bottom) DeltaY = -DeltaY;
                    ScrollToVerticalOffset(StartVerticalOffset + DeltaY);
                }
            };
            #endregion

            MainGrid.Background = new SolidColorBrush(Colors.Transparent);
            MainGrid.MouseWheel += (sender, e) =>
            {
                bool DeltaAligment;
                if (MaxVerticalOffset > 0d)
                {
                    DeltaAligment = Content?.VerticalAlignment == VerticalAlignment.Top ? e.Delta < 0d : e.Delta > 0d;
                    ScrollToVerticalOffset(ActualVerticalOffset + (DeltaAligment ? ScrollForce : -ScrollForce));
                }
                else if (MaxHorizontalOffset > 0d)
                {
                    DeltaAligment = Content?.HorizontalAlignment == HorizontalAlignment.Left ? e.Delta < 0d : e.Delta > 0d;
                    ScrollToHorizontalOffset(ActualHorizontalOffset + (DeltaAligment ? ScrollForce : -ScrollForce));
                }
            };

            SizeChanged += Value_SizeChanged;
        }

        #region HorizontalScrollBar
        /// <summary>
        /// Осуществить прокрутку по горизонтали
        /// </summary>
        /// <param name="Offset">Смещение прокрутки по горизонтали</param>
        public void ScrollToHorizontalOffset(double Offset)
        {
            if (double.IsNaN(MaxHorizontalOffset) || ActualHorizontalOffset == Offset || Content == null) return;
            Offset = Math.Clamp(Offset, 0d, MaxHorizontalOffset);
            RectangleHorizontalScroll.Margin = new(
                Content.HorizontalAlignment == HorizontalAlignment.Left ? Offset : 0d,
                RectangleHorizontalScroll.Margin.Top,
                Content.HorizontalAlignment == HorizontalAlignment.Right ? Offset : 0d,
                RectangleHorizontalScroll.Margin.Bottom);
            Offset = -MaxWidthHorizontalScroll / MaxHorizontalOffset * Offset;
            UserControlContent.Margin = new(
                Content.HorizontalAlignment == HorizontalAlignment.Left ? Offset : 0d,
                UserControlContent.Margin.Top,
                Content.HorizontalAlignment == HorizontalAlignment.Right ? Offset : 0d,
                UserControlContent.Margin.Bottom);
        }
        #endregion

        #region VerticalScrollBar

        /// <summary>
        /// Осуществить прокрутку по вертикали
        /// </summary>
        /// <param name="Offset">Смещение прокрутки по вертикали</param>
        public void ScrollToVerticalOffset(double Offset)
        {
            if (double.IsNaN(MaxVerticalOffset) || ActualVerticalOffset == Offset || Content == null) return;
            Offset = Math.Clamp(Offset, 0d, MaxVerticalOffset);
            RectangleVerticalScroll.Margin = new(
                RectangleVerticalScroll.Margin.Left,
                Content.VerticalAlignment == VerticalAlignment.Top ? Offset : 0d,
                RectangleVerticalScroll.Margin.Right,
                Content.VerticalAlignment == VerticalAlignment.Bottom ? Offset : 0d);
            Offset = -MaxHeightVerticalScroll / MaxVerticalOffset * Offset;
            UserControlContent.Margin = new(
                UserControlContent.Margin.Left,
                Content.VerticalAlignment == VerticalAlignment.Top ? Offset : 0d,
                UserControlContent.Margin.Right,
                Content.VerticalAlignment == VerticalAlignment.Bottom ? Offset : 0d);
        }
        #endregion
    }
}
