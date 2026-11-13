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
        private static void FontSizeHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            IELTextBox Element = (IELTextBox)sender;
            Element.TextBlockVisualText.FontSize = (double)e.NewValue;
        }

        /// <summary>
        /// Размер текста
        /// </summary>
        [Description("Размер вводимого текста в элементе")]
        public new double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
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
                        OffAnimateSelect();
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

            //PreviewKeyDown += (sender, e) =>
            //{
            //    if (DigitOnly)
            //        e.Handled = (int)e.Key < 34 || (int)e.Key > 43;
            //};
        }

        /// <summary>
        /// Взять прямоугольник символа по индексу в тексте, относительно <see cref="Grid"/>
        /// </summary>
        public Point GetCharacterRect(int index)
        {
            if (index < 0 || index >= RunElementText.Text.Length) return default;
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
                OffAnimateSelect();
            else 
                OffAnimateCaret();
        }
        #endregion

        /// <summary>
        /// Обработчик события нажатия клавиши на элементе
        /// </summary>
        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            string NewContent;
            switch (e.Key)
            {
                case Key.Home:
                    CaretIndex = -1;
                    TextBlockVisualText.Margin = new(0d,
                        TextBlockVisualText.Margin.Top, TextBlockVisualText.Margin.Right, TextBlockVisualText.Margin.Bottom);
                    break;
                case Key.End:
                    CaretIndex = TextBlockVisualText.Text.Length - 1;
                    break;
                case Key.Left:
                    UpdateSelect(e.Key);
                    if (SelectActivate) break;
                    else if (CaretIndex >= 0) CaretIndex--;
                    break;
                case Key.Right:
                    UpdateSelect(e.Key);
                    if (SelectActivate) break;
                    else if (CaretIndex < TextBlockVisualText.Text.Length - 1) CaretIndex++;
                    break;
                case Key.Back:
                    if (SelectActivate)
                    {
                        if (CaretSelectCount < 0)
                            CaretIndex += CaretSelectCount;
                        RunElementText.Text = RunElementText.Text.Remove(CaretSelectStart + (CaretSelectCount < 0 ? CaretSelectCount : 0),
                            Math.Abs(CaretSelectCount));
                        OffAnimateSelect();
                        OnAnimateCaret();
                        break;
                    }
                    if (CaretIndex == -1) return;
                    else if (CaretIndex == TextBlockVisualText.Text.Length)
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
                        if (CaretSelectCount < 0)
                            CaretIndex += CaretSelectCount;
                        RunElementText.Text = RunElementText.Text.Remove(CaretSelectStart + (CaretSelectCount < 0 ? CaretSelectCount : 0),
                            Math.Abs(CaretSelectCount));
                        OffAnimateSelect();
                        OnAnimateCaret();
                        break;
                    }
                    if (CaretIndex == TextBlockVisualText.Text.Length - 1) return;
                    else if (CaretIndex < TextBlockVisualText.Text.Length - 1)
                    {
                        RunElementText.Text = RunElementText.Text.Remove(CaretIndex + 1, 1);
                    }
                    break;
                default:
                    if (SelectActivate)
                    {
                        OffAnimateSelect();
                        OnAnimateCaret();
                    }
                    NewContent = IELKeyConverter.GetStringFromKey(e.Key);
                    if (NewContent.Length == 0) return;
                    else if (CaretIndex < RunElementText.Text.Length - 1)
                        RunElementText.Text = RunElementText.Text.Insert(Math.Max(0, CaretIndex + 1), NewContent);
                    else RunElementText.Text += NewContent;
                    TextBlockVisualText.UpdateLayout();
                    CaretIndex++;
                    break;
            }

            UpdatePositionFromCaretIndex(e.Key);
#if DEBUG
            DebugUpdateInfo();
#endif
        }
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
            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                if ((SourceKey == Key.Right && CaretIndex < TextBlockVisualText.Text.Length - 1) ||
                    (SourceKey == Key.Left && CaretIndex >= 0))
                {
                    if (SelectActivate && ((SourceKey == Key.Right && CaretSelectStart + CaretSelectCount == RunElementText.Text.Length) ||
                        (SourceKey == Key.Left && CaretSelectStart + CaretSelectCount == 0))) return;

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
                        CaretSelectCount += SourceKey == Key.Right ? 1 : -1;
                        if (CaretSelectCount == 0)
                        {
                            CaretSelectCount = SourceKey == Key.Right ? 1 : -1;
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
                OffAnimateSelect();
                OnAnimateCaret();
            }
        }

        #region Animate

        #region Caret
        /// <summary>
        /// Начать анимирование каретки
        /// </summary>
        private void OnAnimateCaret()
        {
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
        private void OffAnimateSelect()
        {
            SelectActivate = false;
            CaretSelectStart = 0;
            CaretSelectCount = 0;
            ManagerAnimation.AnimateTakingZeroTo(RectangleSelect, Rectangle.OpacityProperty,
                0d, TimeSpan.FromMilliseconds(200d));

            ManagerAnimation.AnimateTakingZeroTo(RectangleSelect, Rectangle.WidthProperty,
                0d, TimeSpan.FromMilliseconds(200d));

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
