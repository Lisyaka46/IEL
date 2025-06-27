using IEL.CORE.Enums;
using IEL.Interfaces.Front;
using System.Windows.Controls;
using System.Windows.Media;

namespace IEL.CORE.Classes.ObjectSettings
{
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
        /// Значение прозрачности показания изображения действий элемента
        /// </summary>
        public double VisibleOpacityImageMouse { get; set; } = 0.4d;

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
            if (_VisibleMouseImaging)
            {
                Element.Source = ImageMouseButton(Button.OnActivateMouseLeft, Button.OnActivateMouseRight);
                Element.UpdateLayout();
            }
            UpdateVisibleMouseEvents(Element, Activate);
        }
        /// <summary>
        /// Обновить видимость событий мыши
        /// </summary>
        public void UpdateVisibleMouseEvents(Image Element, bool Activate)
        {
            Element.BeginAnimation(Image.OpacityProperty, ObjectAnimateSetting.GetAnimationDouble(Activate ? VisibleOpacityImageMouse : 0d));
        }
        #endregion

        #region ImagedEventsButton
        /// <summary>
        /// Данные изображений отображения событий нажатия
        /// </summary>
        private readonly ImageSource?[] EventImageSourceMouse = new ImageSource[4];

        /// <summary>
        /// Изображение отображения событий нажатия при отсутствии возможности нажатия
        /// </summary>
        public ImageSource? NotEventImageMouse
        {
            get => EventImageSourceMouse[(int)EventsMouse.Not];
            set => EventImageSourceMouse[(int)EventsMouse.Not] = value;
        }

        /// <summary>
        /// Изображение отображения событий нажатия только при левой возможности нажатия
        /// </summary>
        public ImageSource? OnlyLeftEventImageMouse
        {
            get => EventImageSourceMouse[(int)EventsMouse.Left];
            set => EventImageSourceMouse[(int)EventsMouse.Left] = value;
        }

        /// <summary>
        /// Изображение отображения событий нажатия только при правой возможности нажатия
        /// </summary>
        public ImageSource? OnlyRightEventImageMouse
        {
            get => EventImageSourceMouse[(int)EventsMouse.Right];
            set => EventImageSourceMouse[(int)EventsMouse.Right] = value;
        }

        /// <summary>
        /// Изображение отображения событий нажатия при двусторонней возможности нажатия
        /// </summary>
        public ImageSource? FullEventImageMouse
        {
            get => EventImageSourceMouse[(int)EventsMouse.Full];
            set => EventImageSourceMouse[(int)EventsMouse.Full] = value;
        }

        /// <summary>
        /// Узнать отображения действий над кнопкой
        /// </summary>
        /// <returns>Изображение мыши с действиями</returns>
        public ImageSource? ImageMouseButton(object? Left, object? Right) =>
            Task.FromResult(GetImageMouseEvents(
                Left != null ?
                    (Right != null ? EventsMouse.Full : EventsMouse.Left) :
                    (Right != null ? EventsMouse.Right : EventsMouse.Not)
            )).Result;

        /// <summary>
        /// Получить изображение событий по перечислению
        /// </summary>
        /// <param name="Event">Тип состояния событий нажатия</param>
        /// <returns>Возможное изображение отображения событий</returns>
        private ImageSource? GetImageMouseEvents(EventsMouse Event) => EventImageSourceMouse[(int)Event];
        #endregion

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
