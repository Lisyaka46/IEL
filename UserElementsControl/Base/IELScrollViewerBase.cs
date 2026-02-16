using IEL.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace IEL.UserElementsControl.Base
{
    /// <summary>
    /// Базовый класс объекта отображения стековых данных
    /// </summary>
    public partial class IELScrollViewerBase : ContentControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        [LibraryImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static partial bool SetCursorPos(int X, int Y);

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

        #region UIElements
        /// <summary>
        /// Главная сетка объекта
        /// </summary>
        protected internal Grid MainGrid;

        /// <summary>
        /// Сетка вертикального элемента скроллбара
        /// </summary>
        protected internal Grid GridVerticalScrollBorder;

        /// <summary>
        /// Сетка горизонтального элемента скроллбара
        /// </summary>
        protected internal Grid GridHorizontalScrollBorder;

        /// <summary>
        /// Элемент вертикального скроллбара
        /// </summary>
        protected internal Rectangle RectangleVerticalScrollBorder;

        /// <summary>
        /// Элемент горизонтального скроллбара
        /// </summary>
        protected internal Rectangle RectangleHorizontalScrollBorder;

        /// <summary>
        /// Главный элемент прокрутки
        /// </summary>
        protected internal Border MainBorderScrollViewer;

        #endregion

        #region Properties

        #region Content
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(FrameworkElement), typeof(IELScrollViewerBase),
                new(
                    (sender, e) =>
                    {
                        ((IELScrollViewerBase)sender).MainBorderScrollViewer.Child = (FrameworkElement)e.NewValue;
                    }));

        /// <summary>
        /// Внутренний элемент объекта
        /// </summary>
        public new FrameworkElement Content
        {
            get => (FrameworkElement)GetValue(ContentProperty);
            set
            {
                //if (value is not FrameworkElement) throw new ArgumentException($"Аргумент не является реализуемым типом от {typeof(FrameworkElement)}");
                if (MainBorderScrollViewer.Child != null)
                    ((FrameworkElement)MainBorderScrollViewer.Child).SizeChanged -= Value_SizeChanged;
                value.SizeChanged += Value_SizeChanged;
                SetValue(ContentProperty, value);
            }
        }

        /// <summary>
        /// Реализация события изменения размера контента
        /// </summary>
        /// <param name="sender">Объект</param>
        /// <param name="e">Объект управления событием</param>
        private void Value_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FrameworkElement Element = (FrameworkElement)e.Source;
            MainBorderScrollViewer.UpdateLayout();
            if (AutoUpdateVisibleHorizontalScroll)
            {
                if (Element.ActualWidth > MainGrid.ActualWidth)
                {
                    if (!_IsVisibleScrollBar.Horizontal)
                        ActivateHorizontalScrollBar();
                    UpdateWidthScrollBar();
                }
                else if (_IsVisibleScrollBar.Horizontal)
                    DiactivateHorizontalScrollBar();
            }
            if (AutoUpdateVisibleVerticalScroll)
            {
                if (Element.ActualHeight > MainGrid.ActualHeight)
                {
                    if (!_IsVisibleScrollBar.Vertical)
                        ActivateVerticalScrollBar();
                    UpdateHeightScrollBar();
                }
                else if (_IsVisibleScrollBar.Vertical)
                    DiactivateVerticalScrollBar();
            }
        }
        #endregion

        #region HorizontalScrollAligment
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty HorizontalScrollAligmentProperty =
            DependencyProperty.Register("HorizontalScrollAligment", typeof(HorizontalScrollAlignment), typeof(IELScrollViewerBase),
                new(HorizontalScrollAlignment.Up,
                    (sender, e) =>
                    {
                        HorizontalScrollAlignment Value = (HorizontalScrollAlignment)e.NewValue;
                        IELScrollViewerBase Element = (IELScrollViewerBase)sender;
                        Element.MainGrid.RowDefinitions[0].Height = new(Value == 0 ? 0d : 1d, (GridUnitType)(Value == 0 ? 0 : 2));
                        Element.MainGrid.RowDefinitions[1].Height = new(Value == 0 ? 1d : 0d, (GridUnitType)(Value == 0 ? 2 : 0));
                        Grid.SetRow(Element.GridHorizontalScrollBorder, (int)Value);
                        Grid.SetRow(Element.GridVerticalScrollBorder, 1 - (int)Value);

                        Grid.SetRow(Element.MainBorderScrollViewer, 1 - (int)Value);
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
            DependencyProperty.Register("VerticalScrollAligment", typeof(VerticalScrollAlignment), typeof(IELScrollViewerBase),
                new(VerticalScrollAlignment.Right,
                    (sender, e) =>
                    {
                        VerticalScrollAlignment Value = (VerticalScrollAlignment)e.NewValue;
                        IELScrollViewerBase Element = (IELScrollViewerBase)sender;
                        Element.MainGrid.ColumnDefinitions[0].Width = new(Value == 0 ? 0d : 1d, (GridUnitType)(Value == 0 ? 0 : 2));
                        Element.MainGrid.ColumnDefinitions[1].Width = new(Value == 0 ? 1d : 0d, (GridUnitType)(Value == 0 ? 2 : 0));
                        Grid.SetColumn(Element.GridVerticalScrollBorder, (int)Value);
                        Grid.SetColumn(Element.GridHorizontalScrollBorder, 1 - (int)Value);

                        Grid.SetColumn(Element.MainBorderScrollViewer, 1 - (int)Value);
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
            DependencyProperty.Register("ScrollForce", typeof(int), typeof(IELScrollViewerBase),
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

        #endregion

        /// <summary>
        /// Состояние активности отображения полосы прокрутки
        /// </summary>
        private (bool Horizontal, bool Vertical) _IsVisibleScrollBar;

        /// <summary>
        /// Узнать текущее отображение полосы прокрутки
        /// </summary>
        /// <returns></returns>
        public bool IsVisibleScrollBar(ScrollOrientation orientation) =>
            orientation == ScrollOrientation.Horizontal ? _IsVisibleScrollBar.Horizontal : _IsVisibleScrollBar.Vertical;

        /// <summary>
        /// Единица прокрутки
        /// </summary>
        private (double Horizontal, double Vertical) OnePositionScroll = (1d, 1d);

        /// <summary>
        /// Состояние активности фокуса курсора на полосу прокрутки
        /// </summary>
        private (bool Horizontal, bool Vertical) IsCursorUseBorderScroll = (false, false);

        /// <summary>
        /// Позиция курсора на которой была захвачена полоса прокрутки
        /// </summary>
        private (Point? Horizontal, Point? Vertical) CursorSelectPositionBorderScroll = (null, null);

        /// <summary>
        /// Смещение полосы прокрутки при захвате курсором
        /// </summary>
        private (double? Horizontal, double? Vertical) CursorSelectOffsetBorderScroll = (null, null);

        /// <summary>
        /// Коэффициент полосы прокрутки по умолчанию
        /// </summary>
        private (double Horizontal, double Vertical) DefaultCoefficientScroll = (3d, 3d);

        /// <summary>
        /// Текущий расчитываемый коэффициент полосы прокрутки
        /// </summary>
        private (double Horizontal, double Vertical) CoefficientScroll;

        /// <summary>
        /// Текущее значение размера разности мжду контентом
        /// </summary>
        private (double Horizontal, double Vertical) Scrollable = (0d, 0d);

        /// <summary>
        /// Количество прокрутки по вертикали
        /// </summary>
        public double VerticalOffset => -MainBorderScrollViewer.Margin.Top;

        /// <summary>
        /// Количество прокрутки по горизонтали
        /// </summary>
        public double HorizontalOffset => -MainBorderScrollViewer.Margin.Left;

        /// <summary>
        /// Количество прокрутки по вертикали
        /// </summary>
        public double ScrollableWidth => Scrollable.Vertical;

        /// <summary>
        /// Количество прокрутки по горизонтали
        /// </summary>
        public double ScrollableHeight => Scrollable.Horizontal;

        /// <summary>
        /// Автоматическое обновление активности горизонтальной прокрутки
        /// </summary>
        public bool AutoUpdateVisibleHorizontalScroll { get; set; } = true;

        /// <summary>
        /// Автоматическое обновление активности вертикальной прокрутки
        /// </summary>
        public bool AutoUpdateVisibleVerticalScroll { get; set; } = true;

        /// <summary>
        /// Инициализация базового класса объекта визуализации стековых данных
        /// </summary>
        public IELScrollViewerBase()
        {
            CoefficientScroll = DefaultCoefficientScroll;
            MainGrid = new()
            {
                ClipToBounds = true,
            };
            MainGrid.RowDefinitions.Add(new() { Height = new(0d, GridUnitType.Auto) });
            MainGrid.RowDefinitions.Add(new() { Height = new(1d, GridUnitType.Star) });

            MainGrid.ColumnDefinitions.Add(new() { Width = new(1d, GridUnitType.Star) });
            MainGrid.ColumnDefinitions.Add(new() { Width = new(0d, GridUnitType.Auto) });

            #region VertivalBorder
            GridVerticalScrollBorder = new()
            {
                Margin = new(0),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            Grid.SetColumn(GridVerticalScrollBorder, 1);
            Grid.SetRow(GridVerticalScrollBorder, 1);
            RectangleVerticalScrollBorder = new()
            {
                Margin = new(0),
                Width = 0d,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Fill = new SolidColorBrush(Colors.White),
                RadiusX = 5d,
                RadiusY = 5d,
                Opacity = 0d,
            };
            GridVerticalScrollBorder.Children.Add(RectangleVerticalScrollBorder);
            MainGrid.Children.Add(GridVerticalScrollBorder);
            #endregion

            #region HorizontalBorder
            GridHorizontalScrollBorder = new()
            {
                Margin = new(0),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            Grid.SetColumn(GridHorizontalScrollBorder, 0);
            Grid.SetRow(GridHorizontalScrollBorder, 0);
            RectangleHorizontalScrollBorder = new()
            {
                Margin = new(0),
                Height = 0d,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Fill = new SolidColorBrush(Colors.White),
                RadiusX = 5d,
                RadiusY = 5d,
                Opacity = 0d,
            };
            GridHorizontalScrollBorder.Children.Add(RectangleHorizontalScrollBorder);
            MainGrid.Children.Add(GridHorizontalScrollBorder);
            #endregion

            MainBorderScrollViewer = new()
            {
                Margin = new(0),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = new SolidColorBrush(Colors.Transparent),
                Focusable = false,
            };
            Grid.SetRow(MainBorderScrollViewer, 1);
            MainGrid.Children.Add(MainBorderScrollViewer);

            #region HorizontalScroll
            RectangleHorizontalScrollBorder.MouseDown += (sender, e) =>
            {
                IsCursorUseBorderScroll.Horizontal = true;
                CursorSelectPositionBorderScroll.Horizontal = PointToScreen(Mouse.GetPosition(this));
                CursorSelectOffsetBorderScroll.Horizontal = HorizontalOffset;
            };
            RectangleHorizontalScrollBorder.MouseUp += (sender, e) =>
            {
                IsCursorUseBorderScroll.Horizontal = false;
            };
            RectangleHorizontalScrollBorder.MouseMove += (sender, e) =>
            {
                if (IsCursorUseBorderScroll.Horizontal &&
                    CursorSelectPositionBorderScroll.Horizontal.HasValue && CursorSelectOffsetBorderScroll.Horizontal.HasValue)
                {
                    Point SourcePointCursor = PointToScreen(Mouse.GetPosition(this));
                    double ChangePos = CursorSelectOffsetBorderScroll.Horizontal.Value +
                        (SourcePointCursor.X - CursorSelectPositionBorderScroll.Horizontal.Value.X) * CoefficientScroll.Horizontal;
                    if (ChangePos >= 0d && ChangePos <= Scrollable.Horizontal)
                    {
                        ScrollToHorizontalOffset(ChangePos);
                        RectangleHorizontalScrollBorder.Margin =
                            new(HorizontalOffset * OnePositionScroll.Horizontal, 0, 0, 0);
                    }
                    SetCursorPos((int)SourcePointCursor.X, (int)CursorSelectPositionBorderScroll.Horizontal.Value.Y);
                }
            };
            RectangleHorizontalScrollBorder.MouseLeave += (sender, e) =>
            {
                if (IsCursorUseBorderScroll.Horizontal)
                    IsCursorUseBorderScroll.Horizontal = false;
            };

            MainBorderScrollViewer.MouseWheel += (sender, e) =>
            {
                if (_IsVisibleScrollBar.Vertical)
                {
                    if (e.Delta < 0) ScrollToVerticalDown();
                    else ScrollToVerticalUp();
                }
                else if (_IsVisibleScrollBar.Horizontal)
                {
                    if (e.Delta < 0) ScrollToHorizontalRight();
                    else ScrollToHorizontalLeft();
                }
            };
            MainBorderScrollViewer.SizeChanged += (sender, e) =>
            {
                if (Content is FrameworkElement Element && AutoUpdateVisibleHorizontalScroll)
                {
                    if (UpdateWidthScrollBar() > Element.ActualWidth)
                        DiactivateHorizontalScrollBar();
                    else if (!_IsVisibleScrollBar.Horizontal &&
                        Scrollable.Horizontal > 0d && Element.ActualWidth > MainBorderScrollViewer.ActualWidth)
                    {
                        ActivateHorizontalScrollBar();
                    }
                }
            };
            #endregion

            #region VerticalScroll
            RectangleVerticalScrollBorder.MouseDown += (sender, e) =>
            {
                IsCursorUseBorderScroll.Vertical = true;
                CursorSelectPositionBorderScroll.Vertical = PointToScreen(Mouse.GetPosition(this));
                CursorSelectOffsetBorderScroll.Vertical = VerticalOffset;
            };
            RectangleVerticalScrollBorder.MouseUp += (sender, e) =>
            {
                IsCursorUseBorderScroll.Vertical = false;
            };
            RectangleVerticalScrollBorder.MouseMove += (sender, e) =>
            {
                if (IsCursorUseBorderScroll.Vertical &&
                    CursorSelectPositionBorderScroll.Vertical.HasValue && CursorSelectOffsetBorderScroll.Vertical.HasValue)
                {
                    Point SourcePointCursor = PointToScreen(Mouse.GetPosition(this));
                    double ChangePos = CursorSelectOffsetBorderScroll.Vertical.Value -
                        (CursorSelectPositionBorderScroll.Vertical.Value.Y - SourcePointCursor.Y) * CoefficientScroll.Vertical;
                    if (ChangePos >= 0d && ChangePos <= Scrollable.Vertical)
                    {
                        ScrollToVerticalOffset(ChangePos);
                        RectangleVerticalScrollBorder.Margin =
                            new(0, VerticalOffset * OnePositionScroll.Vertical, 0, 0);
                    }
                    SetCursorPos((int)CursorSelectPositionBorderScroll.Vertical.Value.X, (int)SourcePointCursor.Y);
                }
            };
            RectangleVerticalScrollBorder.MouseLeave += (sender, e) =>
            {
                if (IsCursorUseBorderScroll.Vertical)
                    IsCursorUseBorderScroll.Vertical = false;
            };

            MainBorderScrollViewer.SizeChanged += (sender, e) =>
            {
                if (Content is FrameworkElement Element && AutoUpdateVisibleVerticalScroll)
                {
                    if (UpdateHeightScrollBar() > Element.ActualHeight)
                        DiactivateVerticalScrollBar();
                    else if (!_IsVisibleScrollBar.Vertical &&
                        Scrollable.Vertical > 0d && Element.ActualHeight > MainBorderScrollViewer.ActualHeight)
                    {
                        ActivateVerticalScrollBar();
                    }
                }
            };
            #endregion

            SetValue(UserControl.ContentProperty, MainGrid);
        }

        #region HorizontalScrollBar
        /// <summary>
        /// Обновить значение длинны скроллбара
        /// </summary>
        public double UpdateWidthScrollBar()
        {
            Scrollable.Horizontal = Math.Max(Content.ActualWidth - MainGrid.ActualWidth, 0d);
            if (_IsVisibleScrollBar.Horizontal && !IsCursorUseBorderScroll.Horizontal)
            {
                double WidthVisual = GridHorizontalScrollBorder.ActualWidth - Scrollable.Horizontal / CoefficientScroll.Horizontal;
                if (WidthVisual <= RectangleHorizontalScrollBorder.MinWidth || double.IsInfinity(WidthVisual))
                {
                    RectangleHorizontalScrollBorder.Width = RectangleHorizontalScrollBorder.MinWidth;
                    CoefficientScroll.Horizontal = Scrollable.Horizontal /
                        (GridHorizontalScrollBorder.ActualWidth - RectangleHorizontalScrollBorder.Width);
                }
                else
                {
                    CoefficientScroll.Horizontal = DefaultCoefficientScroll.Horizontal;
                    RectangleHorizontalScrollBorder.Width = WidthVisual;
                }
                if (HorizontalOffset > Scrollable.Horizontal)
                {
                    MainBorderScrollViewer.Margin = new(-Scrollable.Horizontal, MainBorderScrollViewer.Margin.Top, 0, 0);
                    //ThicknessAnimationType.To = new(-Scrollable.Horizontal, MainBorderScrollViewer.Margin.Top, 0, 0);
                    //MainBorderScrollViewer.BeginAnimation(MarginProperty, ThicknessAnimationType);
                }
                if (Scrollable.Horizontal > 0)
                {
                    WidthVisual = GridHorizontalScrollBorder.ActualWidth - RectangleHorizontalScrollBorder.Width;
                    OnePositionScroll.Horizontal = WidthVisual / Scrollable.Horizontal;
                    RectangleHorizontalScrollBorder.Margin = new(HorizontalOffset * OnePositionScroll.Horizontal, 0, 0, 0);
                }
                return RectangleHorizontalScrollBorder.Width;
            }
            return Scrollable.Horizontal;
        }

        /// <summary>
        /// Диактивировать полосу прокрутки визуализаторов консоли
        /// </summary>
        public void DiactivateHorizontalScrollBar()
        {
            _IsVisibleScrollBar.Horizontal = false;
            RectangleHorizontalScrollBorder.MinWidth = 0d;
            DoubleAnimationType.To = 0d;
            RectangleHorizontalScrollBorder.BeginAnimation(OpacityProperty, DoubleAnimationType, HandoffBehavior.SnapshotAndReplace);
            RectangleHorizontalScrollBorder.BeginAnimation(HeightProperty, DoubleAnimationType, HandoffBehavior.SnapshotAndReplace);
            ThicknessAnimationType.To = new(0d);
            GridHorizontalScrollBorder.BeginAnimation(MarginProperty, ThicknessAnimationType, HandoffBehavior.SnapshotAndReplace);
        }

        /// <summary>
        /// Активировать полосу прокрутки визуализаторов консоли
        /// </summary>
        public void ActivateHorizontalScrollBar()
        {
            _IsVisibleScrollBar.Horizontal = true;
            RectangleHorizontalScrollBorder.MinWidth = 50d;
            DoubleAnimationType.To = 1d;
            RectangleHorizontalScrollBorder.BeginAnimation(OpacityProperty, DoubleAnimationType, HandoffBehavior.SnapshotAndReplace);
            DoubleAnimationType.To = 10d;
            RectangleHorizontalScrollBorder.BeginAnimation(HeightProperty, DoubleAnimationType, HandoffBehavior.SnapshotAndReplace);
            ThicknessAnimationType.To = new(2d);
            GridHorizontalScrollBorder.BeginAnimation(MarginProperty, ThicknessAnimationType, HandoffBehavior.SnapshotAndReplace);
        }

        /// <summary>
        /// Осуществить прокрутку по горизонтали
        /// </summary>
        /// <param name="Offset">Смещение прокрутки по горизонтали</param>
        public void ScrollToHorizontalOffset(double Offset)
        {
            if (Offset > Scrollable.Horizontal)
                Offset = Scrollable.Horizontal;
            else if (Offset <= 0) Offset = 0;
            MainBorderScrollViewer.BeginAnimation(MarginProperty, null);
            MainBorderScrollViewer.Margin = new(-Offset, MainBorderScrollViewer.Margin.Top, 0, 0);
        }

        /// <summary>
        /// Осуществить прокрутку по поризонтали вправо относительно силы
        /// </summary>
        public void ScrollToHorizontalRight()
        {
            ScrollToHorizontalOffset(HorizontalOffset + ScrollForce);
            RectangleHorizontalScrollBorder.Margin =
                new(HorizontalOffset * OnePositionScroll.Horizontal, VerticalOffset, 0, 0);
        }

        /// <summary>
        /// Осуществить прокрутку по поризонтали влево относительно силы
        /// </summary>
        public void ScrollToHorizontalLeft()
        {
            ScrollToHorizontalOffset(HorizontalOffset - ScrollForce);
            RectangleHorizontalScrollBorder.Margin =
                new(HorizontalOffset * OnePositionScroll.Horizontal, VerticalOffset, 0, 0);
        }
        #endregion

        #region VerticalScrollBar
        /// <summary>
        /// Обновить значение длинны скроллбара
        /// </summary>
        public double UpdateHeightScrollBar()
        {
            Scrollable.Vertical = Math.Max(Content.ActualHeight - MainGrid.ActualHeight, 0d);
            if (_IsVisibleScrollBar.Vertical && !IsCursorUseBorderScroll.Vertical)
            {
                double HeightVisual = GridVerticalScrollBorder.ActualHeight - Scrollable.Vertical / CoefficientScroll.Vertical;
                if (HeightVisual <= RectangleVerticalScrollBorder.MinHeight || double.IsInfinity(HeightVisual))
                {
                    RectangleVerticalScrollBorder.Height = RectangleVerticalScrollBorder.MinHeight;
                    CoefficientScroll.Vertical = Scrollable.Vertical /
                        (GridVerticalScrollBorder.ActualHeight - RectangleVerticalScrollBorder.Height);
                }
                else
                {
                    CoefficientScroll.Vertical = DefaultCoefficientScroll.Vertical;
                    RectangleVerticalScrollBorder.Height = HeightVisual;
                }
                if (VerticalOffset > Scrollable.Vertical)
                {
                    MainBorderScrollViewer.Margin = new(MainBorderScrollViewer.Margin.Left, -Scrollable.Vertical, 0, 0);
                    //ThicknessAnimationType.To = new(MainBorderScrollViewer.Margin.Left, -Scrollable.Vertical, 0, 0);
                    //MainBorderScrollViewer.BeginAnimation(MarginProperty, ThicknessAnimationType);
                }
                if (Scrollable.Vertical > 0)
                {
                    HeightVisual = GridVerticalScrollBorder.ActualHeight - RectangleVerticalScrollBorder.Height;
                    OnePositionScroll.Vertical = HeightVisual / Scrollable.Vertical;
                    RectangleVerticalScrollBorder.Margin = new(0, VerticalOffset * OnePositionScroll.Vertical, 0, 0);
                }
                return RectangleVerticalScrollBorder.Height;
            }
            return Scrollable.Vertical;
        }

        /// <summary>
        /// Диактивировать полосу прокрутки визуализаторов консоли
        /// </summary>
        public void DiactivateVerticalScrollBar()
        {
            _IsVisibleScrollBar.Vertical = false;
            RectangleVerticalScrollBorder.MinHeight = 0d;
            DoubleAnimationType.To = 0d;
            RectangleVerticalScrollBorder.BeginAnimation(OpacityProperty, DoubleAnimationType, HandoffBehavior.SnapshotAndReplace);
            RectangleVerticalScrollBorder.BeginAnimation(WidthProperty, DoubleAnimationType, HandoffBehavior.SnapshotAndReplace);
            ThicknessAnimationType.To = new(0d);
            GridVerticalScrollBorder.BeginAnimation(MarginProperty, ThicknessAnimationType, HandoffBehavior.SnapshotAndReplace);
            MainBorderScrollViewer.BeginAnimation(MarginProperty, ThicknessAnimationType, HandoffBehavior.SnapshotAndReplace);
        }

        /// <summary>
        /// Активировать полосу прокрутки визуализаторов консоли
        /// </summary>
        public void ActivateVerticalScrollBar()
        {
            _IsVisibleScrollBar.Vertical = true;
            RectangleVerticalScrollBorder.MinHeight = 50d;
            DoubleAnimationType.To = 1d;
            RectangleVerticalScrollBorder.BeginAnimation(OpacityProperty, DoubleAnimationType, HandoffBehavior.SnapshotAndReplace);
            DoubleAnimationType.To = 10d;
            RectangleVerticalScrollBorder.BeginAnimation(WidthProperty, DoubleAnimationType, HandoffBehavior.SnapshotAndReplace);
            ThicknessAnimationType.To = new(2d);
            GridVerticalScrollBorder.BeginAnimation(MarginProperty, ThicknessAnimationType, HandoffBehavior.SnapshotAndReplace);
        }

        /// <summary>
        /// Осуществить прокрутку по вертикали
        /// </summary>
        /// <param name="Offset">Смещение прокрутки по вертикали</param>
        public void ScrollToVerticalOffset(double Offset)
        {
            if (Offset > Scrollable.Vertical)
                Offset = Scrollable.Vertical;
            else if (Offset <= 0) Offset = 0;
            MainBorderScrollViewer.BeginAnimation(MarginProperty, null);
            MainBorderScrollViewer.Margin = new(MainBorderScrollViewer.Margin.Left, -Offset, 0, 0);
        }

        /// <summary>
        /// Осуществить прокрутку по вертикали вниз относительно силы
        /// </summary>
        public void ScrollToVerticalDown()
        {
            ScrollToVerticalOffset(VerticalOffset + ScrollForce);
            RectangleVerticalScrollBorder.Margin =
                new(HorizontalOffset, VerticalOffset * OnePositionScroll.Vertical, 0, 0);
        }

        /// <summary>
        /// Осуществить прокрутку по вертикали вверх относительно силы
        /// </summary>
        public void ScrollToVerticalUp()
        {
            ScrollToVerticalOffset(VerticalOffset - ScrollForce);
            RectangleVerticalScrollBorder.Margin =
                new(HorizontalOffset, VerticalOffset * OnePositionScroll.Vertical, 0, 0);
        }
        #endregion
    }
}
