using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IEL.Interfaces.Front
{
    public interface IIELEventsVision
    {
        /// <summary>
        /// Перечисление возможных отображений мыши
        /// </summary>
        private enum EventMouse
        {
            /// <summary>
            /// Нет отображения
            /// </summary>
            Not = 0,

            /// <summary>
            /// Левое отображение
            /// </summary>
            Left = 1,

            /// <summary>
            /// Правое отображение
            /// </summary>
            Right = 2,

            /// <summary>
            /// Полное отображение
            /// </summary>
            Full = 3,
        }

        /// <summary>
        /// Изображение отображения событий нажатия при отсутствии возможности нажатия
        /// </summary>
        public ImageSource? NotEventImageMouse { get; set; }

        /// <summary>
        /// Изображение отображения событий нажатия только при левой возможности нажатия
        /// </summary>
        public ImageSource? OnlyLeftEventImageMouse { get; set; }

        /// <summary>
        /// Изображение отображения событий нажатия только при правой возможности нажатия
        /// </summary>
        public ImageSource? OnlyRightEventImageMouse { get; set; }

        /// <summary>
        /// Изображение отображения событий нажатия при двусторонней возможности нажатия
        /// </summary>
        public ImageSource? FullEventImageMouse { get; set; }

        /// <summary>
        /// Узнать отображения действий над кнопкой
        /// </summary>
        /// <returns>Изображение мыши с действиями</returns>
        public virtual ImageSource? ImageMouseButton(IIELButtonKey Button, IIELEventsVision Vision) =>
            Task.FromResult(GetImageMouseEvents(
                Button.OnActivateMouseLeft != null ?
                    (Button.OnActivateMouseRight != null ? EventMouse.Full : EventMouse.Left) :
                    (Button.OnActivateMouseRight != null ? EventMouse.Right : EventMouse.Not), Vision
            )).Result;

        /// <summary>
        /// Узнать отображения действий над кнопкой
        /// </summary>
        /// <returns>Изображение мыши с действиями</returns>
        public virtual ImageSource? ImageMouseButton(IIELButtonDefault Button, IIELEventsVision Vision) =>
            Task.FromResult(GetImageMouseEvents(
                Button.OnActivateMouseLeft != null ? 
                    (Button.OnActivateMouseRight != null ? EventMouse.Full : EventMouse.Left) :
                    (Button.OnActivateMouseRight != null ? EventMouse.Right : EventMouse.Not), Vision
            )).Result;

        /// <summary>
        /// Получить изображение событий по перечислению
        /// </summary>
        /// <param name="Event">Тип состояния событий нажатия</param>
        /// <returns>Возможное изображение отображения событий</returns>
        private ImageSource? GetImageMouseEvents(EventMouse Event, IIELEventsVision Vision)
        {
            return Event switch
            {
                EventMouse.Not => NotEventImageMouse,
                EventMouse.Left => OnlyLeftEventImageMouse,
                EventMouse.Right => OnlyRightEventImageMouse,
                EventMouse.Full => FullEventImageMouse,
                _ => null,
            };
        }
    }
}
