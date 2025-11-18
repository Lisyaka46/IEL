using IEL.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
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
        #region UIElements
        /// <summary>
        /// Базовый объект в котором реализуется наследуемый элемент
        /// </summary>
        protected Border Base_BorderContainer { get; private set; }
        #endregion

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
                        ((IELContainerBase)sender).Base_BorderContainer.Child = (UIElement)e.NewValue;
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
                        ((IELContainerBase)sender).Base_BorderContainer.CornerRadius = (CornerRadius)e.NewValue;
                    }));

        /// <summary>
        /// Скругление границ объекта
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)Base_BorderContainer.GetValue(CornerRadiusProperty);
            set => Base_BorderContainer.SetValue(CornerRadiusProperty, value);
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
                        ((IELContainerBase)sender).Base_BorderContainer.BorderThickness = (Thickness)e.NewValue;
                    }));

        /// <summary>
        /// Толщина границ объекта
        /// </summary>
        public new Thickness BorderThickness
        {
            get => (Thickness)Base_BorderContainer.GetValue(BorderThicknessProperty);
            set => Base_BorderContainer.SetValue(BorderThicknessProperty, value);
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
                        ((IELContainerBase)sender).Base_BorderContainer.Padding = (Thickness)e.NewValue;
                    }));

        /// <summary>
        /// Внутреннее смещение в объекте
        /// </summary>
        public new Thickness Padding
        {
            get => (Thickness)Base_BorderContainer.GetValue(PaddingProperty);
            set => Base_BorderContainer.SetValue(PaddingProperty, value);
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
                        ((IELContainerBase)sender).Base_BorderContainer.IsEnabled = (bool)e.NewValue;
                        ((IELContainerBase)sender).IsEnabledChanged.Invoke(sender, new(IsEnabledProperty, e.OldValue, e.NewValue));
                    }));

        /// <summary>
        /// Состояние активности элемента
        /// </summary>
        public new bool IsEnabled
        {
            get => (bool)Base_BorderContainer.GetValue(IsEnabledProperty);
            set => Base_BorderContainer.SetValue(IsEnabledProperty, value);
        }
        
        /// <summary>
        /// Событие изменения состояния активности элемента
        /// </summary>
        public new event DependencyPropertyChangedEventHandler IsEnabledChanged;
        #endregion

        #endregion

        protected IELContainerBase()
        {
            Base_BorderContainer = new()
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

            Base_BorderContainer.MouseEnter += (sender, e) =>
            {
                if (IsEnabled)
                {
                    SetActiveSpecrum(StateSpectrum.Select, true);
                    SourceTimer.Start();
                }
            };

            Base_BorderContainer.MouseLeave += (sender, e) =>
            {
                if (IsEnabled)
                {
                    SetActiveSpecrum(StateSpectrum.Default, true);
                    SourceTimer.Stop();
                }
            };

            IsEnabledChanged += (sender, e) =>
            {
                SourceTimer.Stop();
                StateSpectrum Value = (bool)e.NewValue ? StateSpectrum.Default : StateSpectrum.NotEnabled;
                SetActiveSpecrum(Value, true);
            };
            Base_BorderContainer.Background = SourceBackground.InicializeConnectedSolidColorBrush();
            Base_BorderContainer.BorderBrush = SourceBorderBrush.InicializeConnectedSolidColorBrush();
            SetValue(ContentControl.ContentProperty, Base_BorderContainer);
        }
    }
}
