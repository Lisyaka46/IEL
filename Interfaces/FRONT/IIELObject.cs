using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Input;

namespace IEL.Interfaces.Front
{
    /// <summary>
    /// Интерфейс объекта IEL
    /// </summary>
    public interface IIELObject
    {
        /// <summary>
        /// Статус активности объекта
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Узнать отображения действий над кнопкой
        /// </summary>
        /// <returns>Изображение мыши с действиями</returns>
        internal static sealed BitmapImage? ImageMouseButton(bool Left, bool Right)
        {
            if (Left)
            {
                if (Right) return new(new Uri("/Windows/WindowsImages/DoubleMouseButton.png", UriKind.Relative));
                else return new(new Uri("/Windows/WindowsImages/LeftMouseButton.png", UriKind.Relative));
            }
            else if (Right) return new(new Uri("/Windows/WindiwsImages/RightMouseButton.png", UriKind.Relative));
            else return null;
        }

        /// <summary>
        /// Узнать символ клавиши по коду клавиши
        /// </summary>
        /// <param name="key">Код клавиши</param>
        /// <returns>Символ клавиши</returns>
        internal static sealed char KeyName(Key? key)
        {
            return (key switch
            {
                Key.Oem3 => '~',
                Key.OemMinus => '-',
                Key.OemPlus => '+',
                Key.OemComma => '<',
                Key.OemPeriod => '>',
                Key.Oem2 => '/',
                Key.Oem4 => '[',
                Key.Oem6 => ']',
                Key.OemPipe => '\\',
                _ => key?.ToString()[^1]
            }) ?? '\0';
        }
    }
}
