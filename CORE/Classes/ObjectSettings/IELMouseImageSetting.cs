using IEL.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace IEL.CORE.Classes.ObjectSettings
{
    /// <summary>
    /// Класс настроек отображения возможных использований нажатия на элемент
    /// </summary>
    public class IELMouseImageSetting
    {
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

        #region OnlyRightEventImageMouse
        /// <summary>
        /// Изображение отображения событий нажатия только при правой возможности нажатия
        /// </summary>
        public ImageSource? OnlyRightEventImageMouse
        {
            get => EventImageSourceMouse[(int)EventsMouse.Right];
            set => EventImageSourceMouse[(int)EventsMouse.Right] = value;
        }
        #endregion

        /// <summary>
        /// Изображение отображения событий нажатия при двусторонней возможности нажатия
        /// </summary>
        public ImageSource? FullEventImageMouse
        {
            get => EventImageSourceMouse[(int)EventsMouse.Full];
            set => EventImageSourceMouse[(int)EventsMouse.Full] = value;
        }

        /// <summary>
        /// Получить изображение событий по перечислению
        /// </summary>
        /// <param name="Event">Тип состояния событий нажатия</param>
        /// <returns>Возможное изображение отображения событий</returns>
        internal ImageSource? GetImageMouseEvents(EventsMouse Event) => EventImageSourceMouse[(int)Event];
        #endregion
    }
}
