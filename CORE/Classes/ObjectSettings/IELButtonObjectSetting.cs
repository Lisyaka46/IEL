using IEL.CORE.Enums;
using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IEL.CORE.Classes.ObjectSettings
{
    /// <summary>
    /// Класс настроек всех кнопочных видов объектов
    /// </summary>
    public class IELButtonObjectSetting : IELUsingObjectSetting
    {
        #region StateVisualizationButton
        private bool _VisibleMouseImaging;
        /// <summary>
        /// Состояние активности отображения действий на кнопке
        /// </summary>
        /// <remarks>
        /// При включённом состоянии отображает изображение действий производимое над кнопкой.
        /// <code></code>
        /// <b>Изображение поменять нельзя.</b>
        /// </remarks>
        public bool VisibleMouseImaging
        {
            get => _VisibleMouseImaging;
            set
            {
                _VisibleMouseImaging = value;
                VisibleMouseImagingChanged?.Invoke(value);
            }
        }

        private StateVisualButton _StateVisualizationButton;
        /// <summary>
        /// Состояние отображения направления
        /// </summary>
        public StateVisualButton StateVisualizationButton
        {
            get => _StateVisualizationButton;
            set
            {
                _StateVisualizationButton = value;
                EVENT_VisualizationButtonChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// Событие изменения значения стиля отображения объекта
        /// </summary>
        public event IELSettingValueChangedHandler<bool>? VisibleMouseImagingChanged;

        private event IELSettingValueChangedHandler<StateVisualButton>? EVENT_VisualizationButtonChanged;
        /// <summary>
        /// Событие изменения значения стиля отображения объекта
        /// </summary>
        public event IELSettingValueChangedHandler<StateVisualButton>? VisualizationButtonChanged
        {
            add
            {
                EVENT_VisualizationButtonChanged += value;
                EVENT_VisualizationButtonChanged?.Invoke(_StateVisualizationButton);
            }
            remove { VisualizationButtonChanged -= value; }
        }

        /// <summary>
        /// Обновить видимость и отображение событий мыши
        /// </summary>
        public void UpdateVisibleMouseEvents(Image Element, IIELButton Button, bool Activate)
        {
            if (SettingMouseImage == null) return;
            if (_VisibleMouseImaging)
            {
                Element.Source = SettingMouseImage.GetImageMouseEvents(Button.GetSourceEventMouse());
                Element.UpdateLayout();
            }
            UpdateVisibleMouseEvents(Element, Activate);
        }

        /// <summary>
        /// Обновить видимость событий мыши
        /// </summary>
        public void UpdateVisibleMouseEvents(Image Element, bool Activate)
        {
            Element.BeginAnimation(Image.OpacityProperty,
                ObjectAnimateSetting.GetAnimationDouble(Activate ? SettingMouseImage?.VisibleOpacityImageMouse ?? 0d : 0d));
        }
        #endregion

        /// <summary>
        /// Объект настройки отображения изображений нажатий мыши
        /// </summary>
        public IELMouseImageSetting? SettingMouseImage { get; set; }

        /// <summary>
        /// Инициализировать класс настроек объекта
        /// </summary>
        public IELButtonObjectSetting()
        {
            StateVisualizationButton = StateVisualButton.Default;
            VisibleMouseImaging = false;
            AnimationMillisecond = 200d;
        }
    }
}
