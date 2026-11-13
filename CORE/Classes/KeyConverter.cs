using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using Key = System.Windows.Input.Key;

namespace LibraryIEL.CORE.Classes
{
    /// <summary>
    /// Класс конвертации клавиши
    /// </summary>
    public static class IELKeyConverter
    {
        /// <summary>
        /// Контейнер для опредения сиволов клавиши
        /// </summary>
        private static StringBuilder SourceStringContainer = new();

        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int ToUnicodeEx(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags,
            IntPtr dwhkl);

        [DllImport("user32.dll")]
        private static extern bool GetKeyboardState(byte[] lpKeyState);

        /// <summary>
        /// Вернуть строку распознанную по клавише клавиатуры с учётом выбранного языка
        /// </summary>
        /// <param name="key">Клавиша клавиатуры</param>
        public static string GetStringFromKey(Key key)
        {
            SourceStringContainer.Clear();
            uint VirtualCode = (uint)KeyInterop.VirtualKeyFromKey(key);
            uint ScanCode = MapVirtualKey(VirtualCode, 0);
            byte[] State = new byte[256];
            GetKeyboardState(State);
            int result = ToUnicodeEx(VirtualCode, ScanCode, State, SourceStringContainer, SourceStringContainer.Capacity, 0, GetKeyboardLayout(0));
            return result > 0 ? SourceStringContainer.ToString() : string.Empty;
        }

        /// <summary>
        /// Конвертировать значение клавиши в строку имени
        /// </summary>
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
