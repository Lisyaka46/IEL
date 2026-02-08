using IEL.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

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
        /// Главная сетка объектов стека
        /// </summary>
        protected internal Grid MainGridElements;

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
        protected internal ScrollViewer MainScrollViewer;
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
                        ((IELScrollViewerBase)sender).MainGridElements.Children.Add((UIElement)e.NewValue);
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
                if (MainGridElements.Children.Count > 0)
                    ((FrameworkElement)MainGridElements.Children[0]).SizeChanged -= Value_SizeChanged;
                MainGridElements.Children.Clear();
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
            if (AutoUpdateVisibleHorizontalScroll)
            {
                if (MainScrollViewer.ScrollableWidth > 0d && Element.ActualWidth > MainGrid.ActualWidth)
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
                if (MainScrollViewer.ScrollableHeight > 0d && Element.ActualHeight > MainGrid.ActualHeight)
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
        /// Автоматическое обновление активности горизонтальной прокрутки
        /// </summary>
        public bool AutoUpdateVisibleHorizontalScroll = true;

        /// <summary>
        /// Автоматическое обновление активности вертикальной прокрутки
        /// </summary>
        public bool AutoUpdateVisibleVerticalScroll = true;

        /// <summary>
        /// Инициализация базового класса объекта визуализации стековых данных
        /// </summary>
        public IELScrollViewerBase()
        {
            CoefficientScroll = DefaultCoefficientScroll;
            MainGrid = new();
            MainGrid.RowDefinitions.Add(new() { Height = new(0d, GridUnitType.Auto) });
            MainGrid.RowDefinitions.Add(new() { Height = new(1d, GridUnitType.Star) });

            MainGrid.ColumnDefinitions.Add(new() { Width = new(1d, GridUnitType.Star) });
            MainGrid.ColumnDefinitions.Add(new() { Width = new(0d, GridUnitType.Auto) });

            GridVerticalScrollBorder = new Grid()
            {
                Margin = new(0 ),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            Grid.SetColumn(GridVerticalScrollBorder, 1);
            Grid.SetRow(GridVerticalScrollBorder, 1);
            RectangleVerticalScrollBorder = new()
            {
                Width = 0d,
                MinHeight = 50d,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Fill = new SolidColorBrush(Colors.White),
                RadiusX = 5d,
                RadiusY = 5d,
                Opacity = 0d,
            };
            GridVerticalScrollBorder.Children.Add(RectangleVerticalScrollBorder);
            MainGrid.Children.Add(GridVerticalScrollBorder);

            GridHorizontalScrollBorder = new Grid()
            {
                Margin = new(0),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            Grid.SetColumn(GridHorizontalScrollBorder, 0);
            Grid.SetRow(GridHorizontalScrollBorder, 0);
            RectangleHorizontalScrollBorder = new()
            {
                Height = 0d,
                MinWidth = 50d,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Fill = new SolidColorBrush(Colors.White),
                RadiusX = 5d,
                RadiusY = 5d,
                Opacity = 0d,
            };
            GridHorizontalScrollBorder.Children.Add(RectangleHorizontalScrollBorder);
            MainGrid.Children.Add(GridHorizontalScrollBorder);

            MainScrollViewer = new()
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                //PanningMode = PanningMode.Both,
                Background = new SolidColorBrush(Colors.Transparent),
                Focusable = false,
            };
            Grid.SetRow(MainScrollViewer, 1);
            MainGrid.Children.Add(MainScrollViewer);

            MainGridElements = new()
            {
                
            };
            MainScrollViewer.Content = MainGridElements;

            #region HorizontalScroll
            RectangleHorizontalScrollBorder.MouseDown += (sender, e) =>
            {
                IsCursorUseBorderScroll.Horizontal = true;
                CursorSelectPositionBorderScroll.Horizontal = PointToScreen(Mouse.GetPosition(this));
                CursorSelectOffsetBorderScroll.Horizontal = MainScrollViewer.HorizontalOffset;
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
                    if (ChangePos >= 0d && ChangePos <= MainScrollViewer.ScrollableWidth)
                        MainScrollViewer.ScrollToHorizontalOffset(ChangePos);
                    SetCursorPos((int)SourcePointCursor.X, (int)CursorSelectPositionBorderScroll.Horizontal.Value.Y);
                }
            };
            RectangleHorizontalScrollBorder.MouseLeave += (sender, e) =>
            {
                if (IsCursorUseBorderScroll.Horizontal)
                    IsCursorUseBorderScroll.Horizontal = false;
            };

            MainScrollViewer.ScrollChanged += (sender, e) =>
            {
                if (_IsVisibleScrollBar.Horizontal)
                    RectangleHorizontalScrollBorder.Margin = new(MainScrollViewer.HorizontalOffset * OnePositionScroll.Horizontal, 0, 0, 0);
            };
            MainScrollViewer.SizeChanged += (sender, e) =>
            {
                if (Content is FrameworkElement Element && AutoUpdateVisibleHorizontalScroll)
                {
                    if (UpdateWidthScrollBar() > Element.ActualWidth)
                        DiactivateHorizontalScrollBar();
                    else if (!_IsVisibleScrollBar.Horizontal &&
                        MainScrollViewer.ScrollableWidth > 0d && Element.ActualWidth > MainGrid.ActualWidth)
                    {
                        ActivateHorizontalScrollBar();
                    }
                }
                MainGridElements.MaxWidth = MainScrollViewer.ActualWidth;
            };
            #endregion

            #region VerticalScroll
            RectangleVerticalScrollBorder.MouseDown += (sender, e) =>
            {
                IsCursorUseBorderScroll.Vertical = true;
                CursorSelectPositionBorderScroll.Vertical = PointToScreen(Mouse.GetPosition(this));
                CursorSelectOffsetBorderScroll.Vertical = MainScrollViewer.VerticalOffset;
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
                    if (ChangePos >= 0d && ChangePos <= MainScrollViewer.ScrollableHeight)
                        MainScrollViewer.ScrollToVerticalOffset(ChangePos);
                    SetCursorPos((int)CursorSelectPositionBorderScroll.Vertical.Value.X, (int)SourcePointCursor.Y);
                }
            };
            RectangleVerticalScrollBorder.MouseLeave += (sender, e) =>
            {
                if (IsCursorUseBorderScroll.Vertical)
                    IsCursorUseBorderScroll.Vertical = false;
            };

            MainScrollViewer.ScrollChanged += (sender, e) =>
            {
                if (_IsVisibleScrollBar.Vertical)
                    RectangleVerticalScrollBorder.Margin = new(0, MainScrollViewer.VerticalOffset * OnePositionScroll.Vertical, 0, 0);
            };
            MainScrollViewer.SizeChanged += (sender, e) =>
            {
                if (Content is FrameworkElement Element && AutoUpdateVisibleVerticalScroll)
                {
                    if (UpdateHeightScrollBar() > Element.ActualHeight)
                        DiactivateVerticalScrollBar();
                    else if (!_IsVisibleScrollBar.Vertical &&
                        MainScrollViewer.ScrollableHeight > 0d && Element.ActualHeight > MainGrid.ActualHeight)
                    {
                        ActivateVerticalScrollBar();
                    }
                }
            };
            #endregion

            MainScrollViewer.MouseWheel += (sender, e) =>
            {
                if (_IsVisibleScrollBar.Horizontal)
                {
                    if (_IsVisibleScrollBar.Vertical)
                    {
                        if (MainScrollViewer.VerticalOffset + e.Delta <= MainScrollViewer.ScrollableHeight)
                            MainScrollViewer.ScrollToVerticalOffset(MainScrollViewer.VerticalOffset + e.Delta);
                        else
                            MainScrollViewer.ScrollToVerticalOffset(MainScrollViewer.ScrollableHeight);
                        return;
                    }
                    else if (MainScrollViewer.HorizontalOffset - e.Delta <= MainScrollViewer.ScrollableWidth)
                        MainScrollViewer.ScrollToHorizontalOffset(MainScrollViewer.HorizontalOffset - e.Delta);
                    else
                        MainScrollViewer.ScrollToHorizontalOffset(MainScrollViewer.ScrollableWidth);
                }
            };

            SetValue(UserControl.ContentProperty, MainGrid);
        }

        #region HorizontalScrollBar
        /// <summary>
        /// Обновить значение длинны скроллбара
        /// </summary>
        public double UpdateWidthScrollBar()
        {
            if (_IsVisibleScrollBar.Horizontal && !IsCursorUseBorderScroll.Horizontal)
            {
                double WidthVisual = GridHorizontalScrollBorder.ActualWidth - MainScrollViewer.ScrollableWidth / CoefficientScroll.Horizontal;
                if (WidthVisual <= RectangleVerticalScrollBorder.MinWidth)
                {
                    RectangleHorizontalScrollBorder.Width = RectangleHorizontalScrollBorder.MinWidth;
                    CoefficientScroll.Horizontal = MainScrollViewer.ScrollableWidth /
                        (GridHorizontalScrollBorder.ActualWidth - RectangleHorizontalScrollBorder.Width);
                }
                else
                {
                    CoefficientScroll.Horizontal = DefaultCoefficientScroll.Horizontal;
                    RectangleHorizontalScrollBorder.Width = WidthVisual;
                }
                if (MainScrollViewer.ScrollableWidth > 0)
                {
                    WidthVisual = GridHorizontalScrollBorder.ActualWidth - RectangleHorizontalScrollBorder.Width;
                    OnePositionScroll.Horizontal = WidthVisual / MainScrollViewer.ScrollableWidth;
                    RectangleHorizontalScrollBorder.Margin = new(WidthVisual, 0, 0, 0);
                }
                return RectangleHorizontalScrollBorder.Width;
            }
            return 0d;
        }

        /// <summary>
        /// Диактивировать полосу прокрутки визуализаторов консоли
        /// </summary>
        public void DiactivateHorizontalScrollBar()
        {
            _IsVisibleScrollBar.Horizontal = false;
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
            DoubleAnimationType.To = 1d;
            RectangleHorizontalScrollBorder.BeginAnimation(OpacityProperty, DoubleAnimationType, HandoffBehavior.SnapshotAndReplace);
            DoubleAnimationType.To = 10d;
            RectangleHorizontalScrollBorder.BeginAnimation(HeightProperty, DoubleAnimationType, HandoffBehavior.SnapshotAndReplace);
            ThicknessAnimationType.To = new(2d);
            GridHorizontalScrollBorder.BeginAnimation(MarginProperty, ThicknessAnimationType, HandoffBehavior.SnapshotAndReplace);
        }
        #endregion

        #region VerticalScrollBar
        /// <summary>
        /// Обновить значение длинны скроллбара
        /// </summary>
        public double UpdateHeightScrollBar()
        {
            if (_IsVisibleScrollBar.Vertical && !IsCursorUseBorderScroll.Vertical)
            {
                double HeightVisual = GridVerticalScrollBorder.ActualHeight - MainScrollViewer.ScrollableHeight / CoefficientScroll.Vertical;
                if (HeightVisual <= RectangleVerticalScrollBorder.MinHeight || double.IsInfinity(HeightVisual))
                {
                    RectangleVerticalScrollBorder.Height = RectangleVerticalScrollBorder.MinHeight;
                    CoefficientScroll.Vertical = MainScrollViewer.ScrollableHeight /
                        (GridVerticalScrollBorder.ActualHeight - RectangleVerticalScrollBorder.Height);
                }
                else
                {
                    CoefficientScroll.Vertical = DefaultCoefficientScroll.Vertical;
                    RectangleVerticalScrollBorder.Height = HeightVisual;
                }
                if (MainScrollViewer.ScrollableHeight > 0)
                {
                    HeightVisual = GridVerticalScrollBorder.ActualHeight - RectangleVerticalScrollBorder.Height;
                    OnePositionScroll.Vertical = HeightVisual / MainScrollViewer.ScrollableHeight;
                    RectangleVerticalScrollBorder.Margin = new(0, HeightVisual, 0, 0);
                }
                return RectangleVerticalScrollBorder.Height;
            }
            return 0d;
        }

        /// <summary>
        /// Диактивировать полосу прокрутки визуализаторов консоли
        /// </summary>
        public void DiactivateVerticalScrollBar()
        {
            _IsVisibleScrollBar.Vertical = false;
            DoubleAnimationType.To = 0d;
            RectangleVerticalScrollBorder.BeginAnimation(OpacityProperty, DoubleAnimationType, HandoffBehavior.SnapshotAndReplace);
            RectangleVerticalScrollBorder.BeginAnimation(WidthProperty, DoubleAnimationType, HandoffBehavior.SnapshotAndReplace);
            ThicknessAnimationType.To = new(0d);
            GridVerticalScrollBorder.BeginAnimation(MarginProperty, ThicknessAnimationType, HandoffBehavior.SnapshotAndReplace);
        }

        /// <summary>
        /// Активировать полосу прокрутки визуализаторов консоли
        /// </summary>
        public void ActivateVerticalScrollBar()
        {
            _IsVisibleScrollBar.Vertical = true;
            DoubleAnimationType.To = 1d;
            RectangleVerticalScrollBorder.BeginAnimation(OpacityProperty, DoubleAnimationType, HandoffBehavior.SnapshotAndReplace);
            DoubleAnimationType.To = 10d;
            RectangleVerticalScrollBorder.BeginAnimation(WidthProperty, DoubleAnimationType, HandoffBehavior.SnapshotAndReplace);
            ThicknessAnimationType.To = new(2d);
            GridVerticalScrollBorder.BeginAnimation(MarginProperty, ThicknessAnimationType, HandoffBehavior.SnapshotAndReplace);
        }
        #endregion
    }
}
