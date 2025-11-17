using IEL.CORE.Classes.ObjectSettings;
using IEL.CORE.Enums;
using IEL.GUI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace IEL.CORE.BaseUserControls
{
    /// <summary>
    /// БАЗОВЫЙ КЛАСС для отображения кнопки IEL
    /// </summary>
    public class IELButton : IELContainerBase
    {
        /// <summary>
        /// Делегат события изменения поля значения
        /// </summary>
        /// <typeparam name="T_Value">Входной тип изменяемого значения</typeparam>
        /// <param name="NewValue">Новое значение</param>
        public delegate void IELSettingValueChangedHandler<T_Value>(T_Value NewValue);

        /// <summary>
        /// Объект настройки отображения изображений нажатий мыши
        /// </summary>
        public IELMouseImageSetting? SettingMouseVisualEvents { get; set; }

        /// <summary>
        /// Объект изображения в котором отображаются возможные нажатия на кнопку
        /// </summary>
        protected Image? VisualElementMouseEvents { get; set; }

        #region OnActivateMouse
        /// <summary>
        /// Делегат события активации
        /// </summary>
        public delegate void ActivateHandler(object Source, MouseButtonEventArgs eventArgs, bool KeyActivate = false);

        /// <summary>
        /// Объект события активации левым щелчком мыши
        /// </summary>
        public ActivateHandler? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации правым щелчком мыши
        /// </summary>
        public ActivateHandler? OnActivateMouseRight { get; set; }
        #endregion

        #region Properties

        #region OpacityVisibleVisualImageMouseEvents
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty OpacityVisibleVisualImageMouseEventsProperty =
            DependencyProperty.Register("OpacityVisibleVisualImageMouseEvents", typeof(double), typeof(IELButton),
                new(0.6d,
                    (sender, e) =>
                    {
                        if ((double)e.NewValue < (double)e.OldValue)
                        {
                            ((IELButton)sender).SetValue(OpacityVisibleVisualImageMouseEventsProperty, 0d);
                        }
                        else if ((double)e.NewValue > (double)e.OldValue)
                        {
                            ((IELButton)sender).SetValue(OpacityVisibleVisualImageMouseEventsProperty, 1d);
                        }
                        else return;
                    }));

        /// <summary>
        /// Значение прозрачности видимости изображение отображения возможные нажатия на кнопку
        /// </summary>
        public double OpacityVisibleVisualImageMouseEvents
        {
            get => (double)GetValue(OpacityVisibleVisualImageMouseEventsProperty);
            set => SetValue(OpacityVisibleVisualImageMouseEventsProperty, value);
        }
        #endregion

        #region VisualGuide
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty VisualGuideProperty =
            DependencyProperty.Register("VisualGuide", typeof(StateVisualGuide), typeof(IELButton),
                new(
                    (sender, e) =>
                    {
                        ((IELButton)sender).VisualGuideChanged?.Invoke((StateVisualGuide)e.NewValue);
                    }));

        /// <summary>
        /// Состояние отображения направляющих кнопки
        /// </summary>
        public StateVisualGuide VisualGuide
        {
            get => (StateVisualGuide)GetValue(VisualGuideProperty);
            set => SetValue(VisualGuideProperty, value);
        }

        /// <summary>
        /// Событие изменения состояния отображения направляющих кнопки
        /// </summary>
        protected event IELSettingValueChangedHandler<StateVisualGuide>? VisualGuideChanged;
        #endregion

        #region IsVisibleGuide
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty IsVisibleGuideProperty =
            DependencyProperty.Register("IsVisibleGuide", typeof(bool), typeof(IELButton),
                new(false,
                    (sender, e) =>
                    {
                        ((IELButton)sender).IsVisibleGuideChanged?.Invoke((bool)e.NewValue);
                    }));

        /// <summary>
        /// Состояние видимости направляющих кнопки
        /// </summary>
        public bool IsVisibleGuide
        {
            get => (bool)GetValue(IsVisibleGuideProperty);
            set => SetValue(IsVisibleGuideProperty, value);
        }

        /// <summary>
        /// Событие изменения видимости направляющих кнопки
        /// </summary>
        protected event IELSettingValueChangedHandler<bool>? IsVisibleGuideChanged;
        #endregion

        #endregion

        protected IELButton()
        {
            Base_BorderButton.MouseEnter += (sender, e) =>
            {
                if (IsEnabled)
                {
                    UpdateVisibleMouseEvents(true);
                }
            };

            Base_BorderButton.MouseLeave += (sender, e) =>
            {
                if (IsEnabled)
                {
                    UpdateVisibleMouseEvents(false);
                }
            };

            Base_BorderButton.MouseDown += (sender, e) =>
            {
                if (IsEnabled)
                {
                    if (
                    (e.LeftButton == MouseButtonState.Pressed && OnActivateMouseLeft != null) ||
                    (e.RightButton == MouseButtonState.Pressed && OnActivateMouseRight != null))
                    {
                        SetActiveSpecrum(StateSpectrum.Used, false);
                        SourceTimer.Stop();
                    }
                }
            };

            Base_BorderButton.MouseLeftButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseLeft != null)
                {
                    SetActiveSpecrum(StateSpectrum.Select, true);
                    OnActivateMouseLeft.Invoke(this, e);
                }
            };

            Base_BorderButton.MouseRightButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    SetActiveSpecrum(StateSpectrum.Select, true);
                    OnActivateMouseRight.Invoke(this, e);
                }
            };

            Base_BorderButton.IsEnabledChanged += (sender, e) =>
            {
                if (VisualElementMouseEvents != null)
                    VisualElementMouseEvents.Opacity = 0d;
            };
        }

        /// <summary>
        /// Узнать тип доступных нажатий на элемент
        /// </summary>
        /// <returns>Объект пересичления возможных нажатий</returns>
        private EventsMouse GetSourceEventMouse()
        {
            if (OnActivateMouseLeft != null)
            {
                if (OnActivateMouseRight != null) return EventsMouse.Full;
                return EventsMouse.Left;
            }
            else if (OnActivateMouseRight != null) return EventsMouse.Right;
            return EventsMouse.Not;
        }

        /// <summary>
        /// Обновить видимость и отображение событий мыши
        /// </summary>
        private void UpdateVisibleMouseEvents(bool Activate)
        {
            if (SettingMouseVisualEvents == null || VisualElementMouseEvents == null) return;
            VisualElementMouseEvents.Source = SettingMouseVisualEvents.GetImageMouseEvents(GetSourceEventMouse());
            VisualElementMouseEvents.UpdateLayout();
            VisualElementMouseEvents.Opacity = Activate ? OpacityVisibleVisualImageMouseEvents : 0d;
        }
    }
}
