using Key = System.Windows.Input.Key;

namespace IEL.CORE.Classes
{
    /// <summary>
    /// Класс конвертации клавиши
    /// </summary>
    public static class IELKeyConverter
    {
        /// <summary>
        /// Конвертировать значение клавиши в строку имени
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ConvertKeyToString(Key key) => key switch
        {
            Key.Escape => "ESC",
            Key.Oem3 => "~",
            Key.Tab => "TAB",
            Key.RightShift => "R-SHIFT",
            Key.LeftShift => "L-SHIFT",
            Key.RightAlt => "R-ALT",
            Key.LeftAlt => "L-ALT",
            Key.RightCtrl => "R-CTRL",
            Key.LeftCtrl => "L-CTRL",
            Key.Oem5 => "\\",
            Key.OemComma => "<",
            Key.OemPeriod => ">",
            Key.OemQuestion => "/",
            Key.Oem1 => ":",
            Key.OemOpenBrackets => "{",
            Key.OemCloseBrackets => "}",
            Key.Return => "ENTER",
            Key.Space => "SPACE",
            Key.OemMinus => "-",
            Key.OemPlus => "+",
            Key.OemQuotes => "'",
            Key.Up => "UP",
            Key.Down => "DOWN",
            Key.Left => "LEFT",
            Key.Right => "RIGHT",
            Key.D0 => "0",
            Key.D1 => "1",
            Key.D2 => "2",
            Key.D3 => "3",
            Key.D4 => "4",
            Key.D5 => "5",
            Key.D6 => "6",
            Key.D7 => "7",
            Key.D8 => "8",
            Key.D9 => "9",
            _ => key.ToString(),
        };
    }
}
