#if DEBUG
using LibraryIEL.CORE.Debug;
#endif
using LibraryIEL.CORE.Animation;
using LibraryIEL.CORE.Classes;
using LibraryIEL.UserElementsControl.Base;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media;

namespace LibraryIEL.UserElementsControl
{
    /// <summary>
    /// Логика взаимодействия для IELTextBox.xaml
    /// </summary>
    public partial class IELTextBox : IELContainerBase
    {
        #region Properties

        #region DigitOnly
        /// <summary>
        /// Данные свойства <see cref="DigitOnly"/>
        /// </summary>
        public static readonly DependencyProperty DigitOnlyProperty =
            DependencyProperty.Register(nameof(DigitOnly), typeof(bool), typeof(IELTextBox),
                new(false));

        /// <summary>
        /// Состояние активности обработки ввода исключительно цифр
        /// </summary>
        [Description("Состояние отвечающее что последующая обработка будет производиться исключительно цифр.\n" +
            "Текст, который был введён до изменения состояния этого параметра не будет удалён")]
        [Category(Category_Text)]
        public bool DigitOnly
        {
            get => (bool)GetValue(DigitOnlyProperty);
            set => SetValue(DigitOnlyProperty, value);
        }
        #endregion

        #region Text
        /// <summary>
        /// Данные свойства <see cref="Text"/>
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(IELTextBox),
                new(string.Empty, TextHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="Text"/>
        /// </summary>
        private static void TextHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELTextBox Source && e.NewValue is string SourceNewValue)
            {
                Source.RunElementText.Text = SourceNewValue;
            }
        }

        /// <summary>
        /// Отображаемый текст в элементе
        /// </summary>
        [Description("Отображаеммый вводимый текст в элементе")]
        [Category(Category_Text)]
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        #endregion

        #region FontSize
        /// <summary>
        /// Данные свойства <see cref="FontSize"/>
        /// </summary>
        public static readonly new DependencyProperty FontSizeProperty =
            DependencyProperty.Register(nameof(FontSize), typeof(double), typeof(IELTextBox),
                new(14d, FontSizeHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="FontSize"/>
        /// </summary>
        private static void FontSizeHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELTextBox Source && e.NewValue is double SourceNewValue)
            {
                Source.TextBlockVisualText.FontSize = SourceNewValue;
            }
        }

        /// <summary>
        /// Размер текста
        /// </summary>
        [Description("Размер вводимого текста в элементе")]
        [Category(Category_Text)]
        public new double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }
        #endregion

        #region MaxLength
        /// <summary>
        /// Данные свойства <see cref="MaxLength"/>
        /// </summary>
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register(nameof(MaxLength), typeof(uint), typeof(IELTextBox),
                new(32u, MaxLengthHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="MaxLength"/>
        /// </summary>
        private static void MaxLengthHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            //if (Element is IELTextBox Source && e.NewValue is uint SourceNewValue)
            //{
            //    Source.TextBlockVisualText.FontSize = SourceNewValue;
            //}
        }

        /// <summary>
        /// Максимальный размер текста
        /// </summary>
        [Description("Максимальная поддерживаемая длинна вводимого текста")]
        [Category(Category_Text)]
        public uint MaxLength
        {
            get => (uint)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }
        #endregion

        #endregion

        #region Caret
        /// <summary>
        /// Индекс позиции каретки
        /// </summary>
        /// <remarks>
        /// Индекс отображает позицию каретки в тексте, где -1 позиция каретки без символа позади неё<br/>
        /// Большие индексы отображают позицию каретки после символа, где <b>n</b> - после <b>n+1</b> символа<br/>
        /// </remarks>
        private int CaretIndex = -1;

        /// <summary>
        /// Предыдущее значение смещения каретки по контейнеру относительно оси X
        /// </summary>
        private double BackOffsetCaret = 0d;

        /// <summary>
        /// Индекс начала выделения текста по индексу каретки
        /// </summary>
        private int CaretSelectStart = 0;

        /// <summary>
        /// Количество выделенных символов
        /// </summary>
        private int CaretSelectCount = 0;

        /// <summary>
        /// Состояние указывающе на текущую активность выделения текста
        /// </summary>
        private bool SelectActivate = false;

        /// <summary>
        /// Состояние указывающе на текущую активность отображения каретки
        /// </summary>
        private bool CaretActivate = false;

        /// <summary>
        /// Объект анимации каретки
        /// </summary>
        private readonly DoubleAnimation DoubleAnimationCaret = new()
        {
            EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseOut, Exponent = 1.4d },
            Duration = TimeSpan.FromMilliseconds(300d),
            From = 0d,
            To = 1d,
            RepeatBehavior = RepeatBehavior.Forever,
        };

        /// <summary>
        /// Объект анимации позиции каретки
        /// </summary>
        private readonly ThicknessAnimation ThicknessAnimationCaret = new()
        {
            EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseOut, Exponent = 1.4d },
            Duration = TimeSpan.FromMilliseconds(100d),
        };
        #endregion

        #region DebugContainer
#if DEBUG
        /// <summary>
        /// Контейнер диагностической информации
        /// </summary>
        public readonly DebugContainer Debug = new();

        /// <summary>
        /// Обновить отображаемую отладочную информацию
        /// </summary>
        private void DebugUpdateInfo()
        {
            Debug[nameof(CaretIndex)].Text = $"{nameof(CaretIndex)}: {CaretIndex}";
            Debug[nameof(CaretSelectStart)].Text = $"{nameof(CaretSelectStart)}: {CaretSelectStart}";
            Debug[nameof(CaretSelectCount)].Text = $"{nameof(CaretSelectCount)}: {CaretSelectCount}";
            Debug[nameof(SelectActivate)].Text = $"{nameof(SelectActivate)}: {SelectActivate.ToString().ToUpper()}";
        }
#endif
        #endregion

        /// <summary>
        /// Инициализировать объект интерфейса. Текстовый ввод
        /// </summary>
        public IELTextBox()
        {
            InitializeComponent();
            FocusVisualStyle = null;

            #region Brush
            TextBlockVisualText.Foreground = SourceForeground.SourceBrush;
            CaretLine.Stroke = SourceBorderBrush.SourceBrush;
            RectangleSelect.Stroke = SourceBorderBrush.SourceBrush;
            RectangleSelect.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            #endregion

#if DEBUG
            Debug.Title = nameof(IELTextBox);
            Debug.AddSegment(nameof(CaretIndex));
            Debug.AddSegment(nameof(CaretSelectStart));
            Debug.AddSegment(nameof(CaretSelectCount));
            Debug.AddSegment(nameof(SelectActivate));
#endif

            CaretLine.Opacity = 0d;
            RectangleSelect.Opacity = 0d;

            GotFocus += GotFocusHandler;
            LostFocus += LostFocusHandler;
            PreviewKeyDown += PreviewKeyDownHandler;
            KeyDown += KeyDownHandler;
            MouseDown += (sender, e) =>
            {
                if (!SourceBackground.UsedState)
                    SetActiveSpecrum(CORE.Themes.Palettes.SpectrumColor.Used);
                else
                {
                    SourceBorderBrush.SetActiveSpecrum(CORE.Themes.Palettes.SpectrumColor.Used, true);
                    if (SelectActivate)
                    {
                        OffAnimateSelect(false);
                        OnAnimateCaret();
                    }
                }
                e.Handled = true;
            };
            MouseUp += (sender, e) =>
            {
                if (!SourceBackground.UsedState)
                    SetActiveSpecrum(CORE.Themes.Palettes.SpectrumColor.Default);
                else
                {
                    SourceBorderBrush.SetActiveSpecrum(CORE.Themes.Palettes.SpectrumColor.Default, true);
                }

                if (RunElementText.Text.Length == 0) return;
                Point MousePoint = e.GetPosition(this);
                MousePoint.X -= TextBlockVisualText.Margin.Left;
                TextPointer? SourcePointer = TextBlockVisualText.GetPositionFromPoint(MousePoint, true);
                if (SourcePointer == null) CaretIndex = RunElementText.Text.Length - 1;
                else
                {
                    // Индекс символа относительно начала Run
                    CaretIndex = RunElementText.ContentStart.GetOffsetToPosition(SourcePointer) - 1;

                }
                Point PosCaret = GetCharacterRect(CaretIndex);
                ThicknessAnimationCaret.To = new(Math.Min(PosCaret.X, GridMain.ActualWidth - 2),
                    CaretLine.Margin.Top, CaretLine.Margin.Right, CaretLine.Margin.Bottom);
                CaretLine.BeginAnimation(Line.MarginProperty, ThicknessAnimationCaret);
                BackOffsetCaret = ThicknessAnimationCaret.To.Value.Left;
            };
        }

        /// <summary>
        /// Взять прямоугольник символа по индексу в тексте, относительно <see cref="Grid"/>
        /// </summary>
        private Point GetCharacterRect(int index)
        {
            if (index < 0 || index >= RunElementText.Text.Length) return new(TextBlockVisualText.Margin.Left + TextBlockVisualText.Padding.Left, 0d);
            TextPointer? Pointer = RunElementText.ContentStart.GetPositionAtOffset(index + 1, LogicalDirection.Forward);
            if (Pointer == null) return default;

            Rect Result = Pointer.GetCharacterRect(LogicalDirection.Forward);
            return TextBlockVisualText.TranslatePoint(Result.Location, GridMain);
        }

        #region Events

        #region Focused
        /// <summary>
        /// Обработчик события фокусировки на элемент
        /// </summary>
        private void GotFocusHandler(object sender, RoutedEventArgs e)
        {
            SourceBackground.UsedState = true;
            Keyboard.Focus(this);
            OnAnimateCaret();
        }

        /// <summary>
        /// Обработчик события потери фокуса на элемент
        /// </summary>
        private void LostFocusHandler(object sender, RoutedEventArgs e)
        {
            SourceBackground.UsedState = false;
            Keyboard.ClearFocus();
            if (SelectActivate)
                OffAnimateSelect(false);
            else 
                OffAnimateCaret();
        }
        #endregion

        #region KeyDown
        /// <summary>
        /// Обработчик события нажатия клавиши на элементе
        /// </summary>
        private void PreviewKeyDownHandler(object sender, KeyEventArgs e)
        {
            e.Handled = Keyboard.Modifiers == ModifierKeys.Control;
            if (e.Handled && e.IsRepeat) return;
            switch (e.Key)
            {
                case Key.A:
                    if (Keyboard.Modifiers == ModifierKeys.Control &&
                        (!SelectActivate || CaretSelectCount < RunElementText.Text.Length))
                    {
                        CaretIndex = -1;
                        if (!SelectActivate)
                        {
                            OnAnimateSelect(true);
                            OffAnimateCaret();
                        }
                        else if (CaretSelectCount < 0) SetAlginSelectAnimate(true); 
                        CaretSelectStart = 0;
                        CaretSelectCount = RunElementText.Text.Length + 1;
                        UpdateSelect(Key.None);
                    }
                    break;
                case Key.X:
                case Key.C:
                    if (!SelectActivate || Keyboard.Modifiers != ModifierKeys.Control) return;
                    try
                    {
                        Clipboard.SetDataObject(RunElementText.Text.Substring(CaretSelectStart + (CaretSelectCount < 0 ? CaretSelectCount : 0),
                            Math.Abs(CaretSelectCount)));
                    } catch { }
                    if (e.Key is Key.X)
                    {
                        DeleteSymbolsFromSelect();
                        UpdatePositionFromCaretIndex(e.Key);
                    }
                    else
                    {
                        ManagerAnimation.AnimateTakingZeroFromTo(RectangleSelect.Fill, SolidColorBrush.ColorProperty,
                            SourceBackground.GetData().Select, Color.FromArgb(0, 0, 0, 0),
                            TimeSpan.FromMilliseconds(700d));
                    }
                    break;
                case Key.V:
                    if (Keyboard.Modifiers == ModifierKeys.Control && Clipboard.ContainsText())
                    {
                        if (SelectActivate) DeleteSymbolsFromSelect();

                        string pasteText = Clipboard.GetText();
                        int availableSpace = (int)MaxLength - RunElementText.Text.Length;

                        if (availableSpace <= 0) return;
                        else if (pasteText.Length > availableSpace)
                            pasteText = pasteText[..availableSpace];

                        RunElementText.Text = RunElementText.Text.Insert(CaretIndex + 1, pasteText);
                        CaretIndex += pasteText.Length;
                        UpdatePositionFromCaretIndex(e.Key);
                    }
                    break;
                default:
                    e.Handled = false;
                    break;
            }
        }

        /// <summary>
        /// Обработчик события нажатия клавиши на элементе
        /// </summary>
        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (!(CaretActivate || SelectActivate) &&
                e.Key is Key.Enter or Key.Space)
            {
                OnAnimateCaret();
                return;
            }
            else if (e.Key is Key.LeftShift or Key.RightShift or
                              Key.LeftCtrl  or Key.RightCtrl  or
                              Key.LeftAlt   or Key.RightAlt   or
                              Key.LWin      or Key.RWin       or
                              Key.Enter     or
                              (>= Key.F1 and <= Key.F12) ||
                              !(CaretActivate || SelectActivate))
            {
                return; // Не обрабатываем
            }
            string NewContent;

            #region Switch
            switch (e.Key)
            {
                case Key.Escape:
                    OffAnimateCaret();
                    if (SelectActivate) OffAnimateSelect(false);
                    return;
                case Key.Home:
                    CaretIndex = -1;
                    TextBlockVisualText.Margin = new(0d,
                        TextBlockVisualText.Margin.Top, TextBlockVisualText.Margin.Right, TextBlockVisualText.Margin.Bottom);
                    break;
                case Key.End:
                    CaretIndex = RunElementText.Text.Length - 1;
                    break;
                case Key.Left:
                    UpdateSelect(e.Key);
                    if (SelectActivate) break;
                    else if (CaretIndex >= 0) CaretIndex--;
                    break;
                case Key.Right:
                    UpdateSelect(e.Key);
                    if (SelectActivate) break;
                    else if (CaretIndex < RunElementText.Text.Length - 1) CaretIndex++;
                    break;
                case Key.Back:
                    if (SelectActivate)
                    {
                        DeleteSymbolsFromSelect();
                        break;
                    }
                    if (CaretIndex == -1) return;
                    else if (CaretIndex == RunElementText.Text.Length)
                    {
                        RunElementText.Text = CaretIndex > -1 ? RunElementText.Text.Remove(CaretIndex, 1) : string.Empty;
                    }
                    else
                    {
                        RunElementText.Text = RunElementText.Text.Remove(CaretIndex, 1);
                    }
                    CaretIndex--;
                    TextBlockVisualText.UpdateLayout();
                    break;
                case Key.Delete:
                    if (SelectActivate)
                    {
                        DeleteSymbolsFromSelect();
                        break;
                    }
                    if (CaretIndex == RunElementText.Text.Length - 1) return;
                    else if (CaretIndex < RunElementText.Text.Length - 1)
                    {
                        RunElementText.Text = RunElementText.Text.Remove(CaretIndex + 1, 1);
                    }
                    break;
                default:
                    if (DigitOnly && (int)e.Key < 34 || (int)e.Key > 43) return;
                    if (SelectActivate) DeleteSymbolsFromSelect();
                    if (RunElementText.Text.Length == MaxLength) return;
                    NewContent = IELKeyConverter.GetStringFromKey(e.Key);
                    if (NewContent.Length == 0) return;
                    else if (CaretIndex < RunElementText.Text.Length - 1)
                        RunElementText.Text = RunElementText.Text.Insert(Math.Max(0, CaretIndex + 1), NewContent);
                    else RunElementText.Text += NewContent;
                    TextBlockVisualText.UpdateLayout();
                    CaretIndex++;
                    break;
            }
            #endregion

            UpdatePositionFromCaretIndex(e.Key);
        }
        #endregion

        #endregion

        /// <summary>
        /// Обновить отображение позиции каретки по её текущему индексу
        /// </summary>
        /// <param name="SourceKey">Клавиша, которая была до этого нажата</param>
        private void UpdatePositionFromCaretIndex(Key SourceKey)
        {
            Point PosCaret = GetCharacterRect(CaretIndex);
            ThicknessAnimationCaret.To = new(PosCaret.X + (BackOffsetCaret - PosCaret.X),
                    CaretLine.Margin.Top, CaretLine.Margin.Right, CaretLine.Margin.Bottom);
            if (TextBlockVisualText.Margin.Left + BackOffsetCaret - PosCaret.X >= 0d)
            {
                TextBlockVisualText.Margin = new(0d,
                    TextBlockVisualText.Margin.Top, TextBlockVisualText.Margin.Right, TextBlockVisualText.Margin.Bottom);
                PosCaret = GetCharacterRect(CaretIndex);
                ThicknessAnimationCaret.To = new(Math.Min(PosCaret.X, GridMain.ActualWidth - 2),
                    CaretLine.Margin.Top, CaretLine.Margin.Right, CaretLine.Margin.Bottom);
            }
            else if (SourceKey == Key.End)
            {
                if (TextBlockVisualText.ActualWidth > GridMain.ActualWidth - 2)
                {
                    TextBlockVisualText.Margin = new(-(TextBlockVisualText.ActualWidth - (GridMain.ActualWidth - 2)),
                    TextBlockVisualText.Margin.Top, TextBlockVisualText.Margin.Right, TextBlockVisualText.Margin.Bottom);
                    PosCaret = GetCharacterRect(CaretIndex);
                }
                ThicknessAnimationCaret.To = new(Math.Min(PosCaret.X, GridMain.ActualWidth - 2),
                    CaretLine.Margin.Top, CaretLine.Margin.Right, CaretLine.Margin.Bottom);
            }
            else if ((ThicknessAnimationCaret.To.Value.Left < 60 && (SourceKey == Key.Left || SourceKey == Key.Back)) ||
                (ThicknessAnimationCaret.To.Value.Left >= (GridMain.ActualWidth - 2) - 10 && (SourceKey != Key.Left)) ||
                (CaretIndex > -1 && CaretIndex < RunElementText.Text.Length && SourceKey == Key.Back) || SourceKey == Key.End)
                TextBlockVisualText.Margin = new(TextBlockVisualText.Margin.Left + BackOffsetCaret - PosCaret.X,
                    TextBlockVisualText.Margin.Top, TextBlockVisualText.Margin.Right, TextBlockVisualText.Margin.Bottom);
            else
            {
                ThicknessAnimationCaret.To = new(Math.Min(PosCaret.X, GridMain.ActualWidth - 2),
                    CaretLine.Margin.Top, CaretLine.Margin.Right, CaretLine.Margin.Bottom);
            }
            if (ThicknessAnimationCaret.To.Value.Left == 0d)
                ThicknessAnimationCaret.To = new(TextBlockVisualText.Padding.Left,
                    CaretLine.Margin.Top, CaretLine.Margin.Right, CaretLine.Margin.Bottom);
            CaretLine.BeginAnimation(Line.MarginProperty, ThicknessAnimationCaret);
            BackOffsetCaret = ThicknessAnimationCaret.To.Value.Left;
#if DEBUG
            DebugUpdateInfo();
#endif
        }

        /// <summary>
        /// Обновить позицию выделения по клавише
        /// </summary>
        /// <param name="SourceKey">
        /// Клавиша, которая изменяет поведение выделения<br/>
        /// <see cref="Key.Left"/> влево<br/>
        /// <see cref="Key.Right"/> вправо
        /// </param>
        private void UpdateSelect(Key SourceKey)
        {
            if (Keyboard.Modifiers is ModifierKeys.Shift || SourceKey is Key.None)
            {
                if ((SourceKey is Key.Right && CaretIndex < RunElementText.Text.Length) ||
                    SourceKey is Key.Left or Key.None)
                {
                    if (SelectActivate)
                    {
                        if (SourceKey is Key.Right)
                        {
                            if (CaretSelectStart + CaretSelectCount == RunElementText.Text.Length ||
                                (CaretSelectCount < 0 && CaretSelectStart + CaretSelectCount >= RunElementText.Text.Length - 1)) return;
                        }
                        else if (SourceKey is Key.Left)
                        {
                            if (CaretSelectStart + CaretSelectCount == 0 ||
                                (CaretSelectCount > 0 && CaretSelectStart + CaretSelectCount <= 1)) return;
                        }
                    }

                    Point PosCaretStart, PosCaretEnd;
                    if (!SelectActivate)
                    {
                        OnAnimateSelect(SourceKey == Key.Right);
                        OffAnimateCaret();
                        PosCaretStart = GetCharacterRect(CaretSelectStart - 1);
                        PosCaretEnd = GetCharacterRect(CaretSelectStart + CaretSelectCount - 1);

                        RectangleSelect.BeginAnimation(Rectangle.WidthProperty, null);
                        RectangleSelect.BeginAnimation(Rectangle.MarginProperty, null);
                        RectangleSelect.Width = 0d;
                        RectangleSelect.Margin = new(PosCaretStart.X,
                            RectangleSelect.Margin.Top, RectangleSelect.Margin.Right, RectangleSelect.Margin.Bottom);
                    }
                    else
                    {
                        CaretSelectCount += SourceKey is Key.Right ? 1 : -1;
                        if (CaretSelectCount == 0)
                        {
                            CaretSelectCount = SourceKey is Key.Right ? 1 : -1;
                            SetAlginSelectAnimate(CaretSelectCount > 0);
                        }
                        PosCaretStart = GetCharacterRect(CaretSelectStart - 1);
                        PosCaretEnd = GetCharacterRect(CaretSelectStart + CaretSelectCount - 1);
                    }
                    ManagerAnimation.AnimateTakingZeroTo(RectangleSelect, Rectangle.WidthProperty,
                            Math.Abs(PosCaretEnd.X - PosCaretStart.X),
                            TimeSpan.FromMilliseconds(500d));
                    ManagerAnimation.AnimateTakingZeroTo(RectangleSelect, Rectangle.MarginProperty,
                        new Thickness(Math.Min(PosCaretStart.X, PosCaretEnd.X),
                        RectangleSelect.Margin.Top, RectangleSelect.Margin.Right, RectangleSelect.Margin.Bottom),
                        TimeSpan.FromMilliseconds(500d));
                }
            }
            else if (SelectActivate)
            {
                if (SourceKey is Key.Right && CaretSelectCount > 0 ||
                    SourceKey is Key.Left && CaretSelectCount < 0)
                    CaretIndex += SourceKey is Key.Right ? CaretSelectCount - 1 : CaretSelectCount + 1;
                OffAnimateSelect(SourceKey is Key.Right);
                OnAnimateCaret();
            }
#if DEBUG
            DebugUpdateInfo();
#endif
        }

        /// <summary>
        /// Удалить выделенные символы в поле ввода
        /// </summary>
        /// <remarks>
        /// Также переключить видимость выделения и каретки
        /// </remarks>
        private void DeleteSymbolsFromSelect()
        {
            if (!SelectActivate) return;
            else if (CaretSelectCount < 0)
                CaretIndex += CaretSelectCount;
            RunElementText.Text = RunElementText.Text.Remove(CaretSelectStart + (CaretSelectCount < 0 ? CaretSelectCount : 0),
                Math.Abs(CaretSelectCount));
            OffAnimateSelect(CaretSelectCount < 0);
            OnAnimateCaret();
        }

        #region Animate

        #region Caret
        /// <summary>
        /// Начать анимирование каретки
        /// </summary>
        private void OnAnimateCaret()
        {
            CaretActivate = true;
            if (ManagerAnimation.IsEnabled)
            {
                DoubleAnimationCaret.AutoReverse = true;
                DoubleAnimationCaret.From = 0d;
                DoubleAnimationCaret.To = 1d;
                CaretLine.BeginAnimation(OpacityProperty, DoubleAnimationCaret);
            }
            else CaretLine.Opacity = 1d;
        }

        /// <summary>
        /// Закончить анимирование каретки
        /// </summary>
        private void OffAnimateCaret()
        {
            CaretActivate = false;
            if (ManagerAnimation.IsEnabled)
                CaretLine.BeginAnimation(OpacityProperty, null);
            CaretLine.Opacity = 0d;
        }
        #endregion

        #region Select
        /// <summary>
        /// Начать анимирование объекта выделения
        /// </summary>
        /// <param name="RightAlign">Состояние отвечающее за направленность анимирования границы выделения</param>
        private void OnAnimateSelect(bool RightAlign)
        {
            SelectActivate = true;
            CaretSelectStart = CaretIndex + 1;
            CaretSelectCount = RightAlign ? 1 : -1;
            ManagerAnimation.AnimateTakingZeroTo(RectangleSelect, Rectangle.OpacityProperty,
                1d, TimeSpan.FromMilliseconds(500d));
            RectangleSelect.BeginAnimation(Rectangle.WidthProperty, null);
            RectangleSelect.BeginAnimation(Rectangle.MarginProperty, null);


            SetAlginSelectAnimate(RightAlign);
        }

        /// <summary>
        /// Изменить направление анимирования выделенного текста
        /// </summary>
        /// <param name="RightAlign">Состояние отвечающее за направленность анимирования границы выделения</param>
        private void SetAlginSelectAnimate(bool RightAlign)
        {
            if (ManagerAnimation.IsEnabled)
            {
                DoubleAnimationCaret.AutoReverse = false;
                DoubleAnimationCaret.From = 0d;
                DoubleAnimationCaret.To = RightAlign ? -1.9d : 1.9d;
                RectangleSelect.BeginAnimation(Rectangle.StrokeDashOffsetProperty, DoubleAnimationCaret);
            }
        }

        /// <summary>
        /// Закончить анимирование объекта выделения
        /// </summary>
        /// <param name="RightAlign">Состояние отвечающее за направленность анимирования границы выделения</param>
        private void OffAnimateSelect(bool RightAlign)
        {
            SelectActivate = false;
            CaretSelectStart = 0;
            CaretSelectCount = 0;
            ManagerAnimation.AnimateTakingZeroTo(RectangleSelect, Rectangle.OpacityProperty,
                0d, TimeSpan.FromMilliseconds(200d));

            ManagerAnimation.AnimateTakingZeroTo(RectangleSelect, Rectangle.WidthProperty,
                0d, TimeSpan.FromMilliseconds(200d));

            if (RightAlign)
                ManagerAnimation.AnimateTakingZeroTo(RectangleSelect, Rectangle.MarginProperty,
                    new Thickness(RectangleSelect.Margin.Left + RectangleSelect.Width,
                    RectangleSelect.Margin.Top, RectangleSelect.Margin.Right, RectangleSelect.Margin.Bottom),
                    TimeSpan.FromMilliseconds(200d));

            if (ManagerAnimation.IsEnabled)
            {
                RectangleSelect.BeginAnimation(Rectangle.StrokeDashOffsetProperty, null);
                CaretLine.Opacity = 0d;
            }
        }
        #endregion

        #endregion
    }
}
