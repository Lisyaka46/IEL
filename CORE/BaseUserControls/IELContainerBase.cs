using IEL.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace IEL.CORE.BaseUserControls
{
    /// <summary>
    /// БАЗОВЫЙ КЛАСС для отображения контейнера IEL
    /// </summary>
    public class IELContainerBase : IELObject
    {
        /// <summary>
        /// Базовый объект в котором реализуется наследуемый элемент
        /// </summary>
        protected Border Base_BorderButton { get; private set; }

        #region Properties

        #region IntervalHover
        /// <summary>
        /// Таймер срабатывания задержки
        /// </summary>
        protected DispatcherTimer SourceTimer { get; private set; }

        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty IntervalHoverProperty =
            DependencyProperty.Register("IntervalHover", typeof(TimeSpan), typeof(IELContainerBase),
                new(TimeSpan.FromMilliseconds(1000d),
                    (sender, e) =>
                    {
                        ((IELContainerBase)sender).SourceTimer.Interval = (TimeSpan)e.NewValue;
                    }));

        /// <summary>
        /// Время задержки перед срабатыванием события удержания мыши в кнопке
        /// </summary>
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
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(UIElement), typeof(IELContainerBase),
                new(
                    (sender, e) =>
                    {
                        ((IELContainerBase)sender).Base_BorderButton.Child = (UIElement)e.NewValue;
                    }));

        /// <summary>
        /// Внутренний элемент объекта
        /// </summary>
        public new UIElement Content
        {
            get => (UIElement)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        #endregion

        #region CornerRadius
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(IELContainerBase),
                new(new CornerRadius(0),
                    (sender, e) =>
                    {
                        ((IELContainerBase)sender).Base_BorderButton.CornerRadius = (CornerRadius)e.NewValue;
                    }));

        /// <summary>
        /// Скругление границ объекта
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)Base_BorderButton.GetValue(CornerRadiusProperty);
            set => Base_BorderButton.SetValue(CornerRadiusProperty, value);
        }
        #endregion

        #region BorderThickness
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(IELContainerBase),
                new(new Thickness(2),
                    (sender, e) =>
                    {
                        ((IELContainerBase)sender).Base_BorderButton.BorderThickness = (Thickness)e.NewValue;
                    }));

        /// <summary>
        /// Толщина границ объекта
        /// </summary>
        public new Thickness BorderThickness
        {
            get => (Thickness)Base_BorderButton.GetValue(BorderThicknessProperty);
            set => Base_BorderButton.SetValue(BorderThicknessProperty, value);
        }
        #endregion

        #region Padding
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty PaddingProperty =
            DependencyProperty.Register("Padding", typeof(Thickness), typeof(IELContainerBase),
                new(new Thickness(0),
                    (sender, e) =>
                    {
                        ((IELContainerBase)sender).Base_BorderButton.Padding = (Thickness)e.NewValue;
                    }));

        /// <summary>
        /// Внутреннее смещение в объекте
        /// </summary>
        public new Thickness Padding
        {
            get => (Thickness)Base_BorderButton.GetValue(PaddingProperty);
            set => Base_BorderButton.SetValue(PaddingProperty, value);
        }
        #endregion

        #region IsEnabled
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty IsEnabledProperty =
            DependencyProperty.Register("IsEnabled", typeof(bool), typeof(IELContainerBase),
                new(true,
                    (sender, e) =>
                    {
                        ((IELContainerBase)sender).Base_BorderButton.IsEnabled = (bool)e.NewValue;
                    }));

        /// <summary>
        /// Состояние активности элемента
        /// </summary>
        public new bool IsEnabled
        {
            get => (bool)Base_BorderButton.GetValue(IsEnabledProperty);
            set => Base_BorderButton.SetValue(IsEnabledProperty, value);
        }
        #endregion

        #endregion

        protected IELContainerBase()
        {
            Base_BorderButton = new()
            {
                BorderThickness = new(2),
            };
            SourceTimer = new()
            {
                Interval = TimeSpan.FromMilliseconds(1300d),
            };
            SourceTimer.Tick += (sender, e) =>
            {
                MouseHover?.Invoke(this, e);
                SourceTimer.Stop();
            };

            Base_BorderButton.MouseEnter += (sender, e) =>
            {
                if (IsEnabled)
                {
                    SetActiveSpecrum(StateSpectrum.Select, true);
                    SourceTimer.Start();
                }
            };

            Base_BorderButton.MouseLeave += (sender, e) =>
            {
                if (IsEnabled)
                {
                    SetActiveSpecrum(StateSpectrum.Default, true);
                    SourceTimer.Stop();
                }
            };

            Base_BorderButton.IsEnabledChanged += (sender, e) =>
            {
                SourceTimer.Stop();
                StateSpectrum Value = (bool)e.NewValue ? StateSpectrum.Default : StateSpectrum.NotEnabled;
                SetActiveSpecrum(Value, true);
            };
            Base_BorderButton.Background = QBackground.InicializeConnectedSolidColorBrush();
            Base_BorderButton.BorderBrush = QBorderBrush.InicializeConnectedSolidColorBrush();
            SetValue(ContentControl.ContentProperty, Base_BorderButton);
        }
    }
}
