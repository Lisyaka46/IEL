using IEL.CORE.Classes;
using IEL.CORE.Classes.ObjectSettings;
using System.Windows.Input;

namespace IEL.Interfaces.Front
{
    /// <summary>
    /// Интерфейс объекта IEL
    /// </summary>
    public interface IIELObject
    {
        #region Color Setting
        /// <summary>
        /// Объект настройки состояний фона
        /// </summary>
        public BrushSettingQ QBackground { get; protected set; }

        /// <summary>
        /// Объект настройки состояний границы
        /// </summary>
        public BrushSettingQ QBorderBrush { get; protected set; }

        /// <summary>
        /// Объект настройки состояний текста
        /// </summary>
        public BrushSettingQ QForeground { get; protected set; }
        #endregion

        /// <summary>
        /// Статус активности объекта
        /// </summary>
        public bool IsEnabled { get; }

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
