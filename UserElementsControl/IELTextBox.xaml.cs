using IEL.UserElementsControl.Base;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace IEL.UserElementsControl
{
    /// <summary>
    /// Логика взаимодействия для IELTextBox.xaml
    /// </summary>
    public partial class IELTextBox : IELContainerBase
    {
        #region Properties

        #region DigitOnly
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty DigitOnlyProperty =
            DependencyProperty.Register(nameof(DigitOnly), typeof(bool), typeof(IELTextBox),
                new(false,
                    (sender, e) =>
                    {
                    }));

        /// <summary>
        /// Состояние активности обработки ввода исключительно цифр
        /// </summary>
        public bool DigitOnly
        {
            get => (bool)GetValue(DigitOnlyProperty);
            set
            {
                SetValue(DigitOnlyProperty, value);
            }
        }
        #endregion

        #region Text
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(IELTextBox),
                new(string.Empty, OnPropertyChangedText));

        /// <summary>
        /// Обработчик события изменения свочтва
        /// </summary>
        private static void OnPropertyChangedText(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            IELTextBox Element = (IELTextBox)sender;
            Element.TextBlockVisualText.Text = (string)e.NewValue;
        }

        /// <summary>
        /// Отображаемый текст в элементе
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set
            {
                SetValue(TextProperty, value);
            }
        }
        #endregion

        #region FontSize
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty FontSizeProperty =
            DependencyProperty.Register(nameof(FontSize), typeof(double), typeof(IELTextBox),
                new(14d, OnPropertyChangedFontSize));

        /// <summary>
        /// Обработчик события изменения свочтва
        /// </summary>
        private static void OnPropertyChangedFontSize(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            IELTextBox Element = (IELTextBox)sender;
            Element.TextBlockVisualText.FontSize = (double)e.NewValue;
        }

        /// <summary>
        /// Размер текста
        /// </summary>
        public new double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }
        #endregion

        #endregion

        /// <summary>
        /// Текущее состояние фокусировки на элементе
        /// </summary>
        public bool IsFocus { get; private set; } = false;

        #region Caret
        /// <summary>
        /// Индекс позиции каретки
        /// </summary>
        /// <remarks>
        /// Индекс отображает позицию каретки в тексте, где -1 позиция каретки без символа позади неё<br/>
        /// Большие индексы отображают позицию каретки после символа, где 0+ - после n+1 символа<br/>
        /// </remarks>
        private int CaretIndex = -1;

        /// <summary>
        /// Объект анимации каретки
        /// </summary>
        private DoubleAnimation DoubleAnimationCaret = new()
        {
            EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseOut, Exponent = 1.4d },
            Duration = TimeSpan.FromMilliseconds(150d),
            To = 1d,
        };

        /// <summary>
        /// Объект анимации позиции каретки
        /// </summary>
        private ThicknessAnimation ThicknessAnimationCaret = new()
        {
            EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseOut, Exponent = 1.4d },
            Duration = TimeSpan.FromMilliseconds(100d),
        };

        /// <summary>
        /// Таймер отображения каретки
        /// </summary>
        private readonly DispatcherTimer CaretVisualTimer = new()
        {
            Interval = TimeSpan.FromMilliseconds(180d),
        };
        #endregion

        /// <summary>
        /// Элемент конвертера вводимого символа с клавиатуры
        /// </summary>
        private static readonly KeyConverter ConverterInputKey = new();

        /// <summary>
        /// Инициализировать объект интерфейса. Текстовый ввод
        /// </summary>
        public IELTextBox()
        {
            InitializeComponent();
            #region Foreground
            TextBlockVisualText.Foreground = SourceForeground.SourceBrush;
            CaretLine.Stroke = SourceBorderBrush.SourceBrush;
            #endregion
            CaretLine.Opacity = 0d;
            TextBlockVisualText.Text = string.Empty;

            CaretVisualTimer.Tick += CaretVisualTimerTickHandler;
            GotFocus += GotFocusHandler;
            LostFocus += LostFocusHandler;
            KeyDown += KeyDownHandler;

            //PreviewKeyDown += (sender, e) =>
            //{
            //    if (DigitOnly)
            //        e.Handled = (int)e.Key < 34 || (int)e.Key > 43;
            //};
        }

        /// <summary>
        /// Взять прямоугольник символа по индексу в тексте
        /// </summary>
        public Rect GetCharacterRect(int index)
        {
            if (string.IsNullOrEmpty(TextBlockVisualText.Text) || index < 0 || index >= TextBlockVisualText.Text.Length)
                return Rect.Empty;

            TextPointer startPointer = TextBlockVisualText.ContentStart.GetPositionAtOffset(index);

            return startPointer.GetCharacterRect(LogicalDirection.Forward);
        }

        #region Handlers

        #region Focused
        /// <summary>
        /// Обработчик события фокусировки на элемент
        /// </summary>
        private void GotFocusHandler(object sender, RoutedEventArgs e)
        {
            if (IsFocus) return;
            IsFocus = true;
            SourceBackground.UsedState = true;
            Keyboard.Focus(this);
            CaretVisualTimer.Start();
        }

        /// <summary>
        /// Обработчик события потери фокуса на элемент
        /// </summary>
        private void LostFocusHandler(object sender, RoutedEventArgs e)
        {
            if (!IsFocus) return;
            IsFocus = false;
            SourceBackground.UsedState = false;
            Keyboard.ClearFocus();
            CaretVisualTimer.Stop();
        }
        #endregion

        #region Caret
        /// <summary>
        /// Обработчик события потери фокуса на элемент
        /// </summary>
        private void CaretVisualTimerTickHandler(object? sender, EventArgs e)
        {
            DoubleAnimationCaret.To = DoubleAnimationCaret.To == 0d ? 1d : 0d;
            CaretLine.BeginAnimation(OpacityProperty, DoubleAnimationCaret);
        }
        #endregion

        /// <summary>
        /// Обработчик события нажатия клавиши на элементе
        /// </summary>
        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            Rect RectSymbol;
            char? NewSymbol;
            switch (e.Key)
            {
                case Key.Left:
                    if (CaretIndex > 0) CaretIndex--;
                    break;
                case Key.Right:
                    if (CaretIndex < TextBlockVisualText.Text.Length - 1) CaretIndex++;
                    break;
                case Key.Back:
                    if (CaretIndex == -1) return;
                    else if (CaretIndex == TextBlockVisualText.Text.Length)
                    {
                        TextBlockVisualText.Text = CaretIndex > -1 ? TextBlockVisualText.Text.Remove(CaretIndex, 1) : string.Empty;
                    }
                    else
                    {
                        TextBlockVisualText.Text = TextBlockVisualText.Text.Remove(CaretIndex, 1);
                    }
                    CaretIndex--;
                    TextBlockVisualText.UpdateLayout();
                    break;
                default:
                    NewSymbol = GetCharFromKey(e.Key, Keyboard.Modifiers.HasFlag(ModifierKeys.Shift), Console.CapsLock);
                    if (NewSymbol == null) return;
                    TextBlockVisualText.Text += NewSymbol;
                    TextBlockVisualText.UpdateLayout();
                    CaretIndex++;
                    break;
            }

            RectSymbol = CaretIndex > -1 && CaretIndex < TextBlockVisualText.Text.Length - 1 ? GetCharacterRect(CaretIndex) :
                new(new Point(1, 0), new Size(TextBlockVisualText.ActualWidth, TextBlockVisualText.ActualHeight));
            ThicknessAnimationCaret.To = new(RectSymbol.Right, 0d, 0d, 0d);
            CaretLine.BeginAnimation(Line.MarginProperty, ThicknessAnimationCaret);
        }
        #endregion

        /// <summary>
        /// Получает символ, который будет введен с учетом Shift и CapsLock
        /// </summary>
        public static char? GetCharFromKey(Key key, bool shiftPressed = false, bool capsLockOn = false)
        {
            // Используем встроенный KeyConverter
            string keyString = ConverterInputKey.ConvertToString(key) ?? string.Empty;
            // Обрабатываем буквы
            if (key >= Key.A && key <= Key.Z)
            {
                char c = keyString.ToLower()[0];

                // Учитываем Shift и CapsLock
                if (shiftPressed ^ capsLockOn)
                {
                    c = char.ToUpper(c);
                }

                return c;
            }

            if (key >= Key.D0 && key <= Key.D9)
            {
                char c = keyString[0];
                if (shiftPressed)
                {
                    char[] shiftChars = [')', '!', '@', '#', '$', '%', '^', '&', '*', '('];
                    int index = key - Key.D0;
                    return shiftChars[index];
                }
                return c;
            }

            // Обрабатываем специальные символы
            return key switch
            {
                Key.Space => ' ',
                Key.OemPeriod => shiftPressed ? '>' : '.',
                Key.OemComma => shiftPressed ? '<' : ',',
                Key.OemQuestion => shiftPressed ? '?' : '/',
                Key.OemSemicolon => shiftPressed ? ':' : ';',
                Key.OemQuotes => shiftPressed ? '"' : '\'',
                Key.OemMinus => shiftPressed ? '_' : '-',
                Key.OemPlus => shiftPressed ? '+' : '=',
                Key.OemTilde => shiftPressed ? '~' : '`',
                Key.OemOpenBrackets => shiftPressed ? '{' : '[',
                Key.OemCloseBrackets => shiftPressed ? '}' : ']',
                Key.OemBackslash => shiftPressed ? '|' : '\\',
                Key.OemPipe => shiftPressed ? '|' : '\\',
                // NumPad
                Key.NumPad0 => '0',
                Key.NumPad1 => '1',
                Key.NumPad2 => '2',
                Key.NumPad3 => '3',
                Key.NumPad4 => '4',
                Key.NumPad5 => '5',
                Key.NumPad6 => '6',
                Key.NumPad7 => '7',
                Key.NumPad8 => '8',
                Key.NumPad9 => '9',
                Key.Add => '+',
                Key.Subtract => '-',
                Key.Multiply => '*',
                Key.Divide => '/',
                Key.Decimal => '.',
                _ => null,
            };
        }
    }
}
