using System.Reflection;
using System.Windows.Input;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;

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
