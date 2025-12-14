using IEL.UserElementsControl.Base;
using System.Windows;
using System.Windows.Controls;

namespace IEL.UserElementsControl
{
    /// <summary>
    /// Логика взаимодействия для IELTextBox.xaml
    /// </summary>
    public partial class IELTextBox : IELContainerBase
    {
        /// <summary>
        /// Вертикальная ориентация контента элемента
        /// </summary>
        public VerticalAlignment TextBoxVerticalAlignment
        {
            get => TextBoxMain.VerticalAlignment;
            set => TextBoxMain.VerticalAlignment = value;
        }

        /// <summary>
        /// Горизонтальная ориентация контента элемента
        /// </summary>
        public HorizontalAlignment TextBoxHorizontalAlignment
        {
            get => TextBoxMain.HorizontalAlignment;
            set => TextBoxMain.HorizontalAlignment = value;
        }

        /// <summary>
        /// Доступна или нет печать в объекте
        /// </summary>
        public bool IsReadOnly
        {
            get => TextBoxMain.IsReadOnly;
            set => TextBoxMain.IsReadOnly = value;
        }

        #region FontSize
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty FontSizeProperty =
            DependencyProperty.Register(nameof(FontSize), typeof(double), typeof(IELTextBox),
                new(14d,
                    (sender, e) =>
                    {
                        ((IELTextBox)sender).TextBoxMain.FontSize = (double)e.NewValue;
                    }));

        /// <summary>
        /// Размер текста
        /// </summary>
        public new double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }
        #endregion

        #region Text
        /// <summary>
        /// Текст отображаемый в панели ввода
        /// </summary>
        public string Text
        {
            get => (string)TextBoxMain.GetValue(TextBox.TextProperty);
            set
            {
                TextBoxMain.SetValue(TextBox.TextProperty, value);
                TextBoxMain.SelectionStart = TextBoxMain.Text.Length;
            }
        }
        #endregion

        /// <summary>
        /// Сделать фокус на элементе
        /// </summary>
        public new void Focus()
        {
            TextBoxMain.Focus();
        }

        /// <summary>
        /// Событие изменения текста
        /// </summary>
        public event TextChangedEventHandler? TextChanged
        {
            add => TextBoxMain.TextChanged += value;
            remove => TextBoxMain.TextChanged -= value;
        }

        /// <summary>
        /// Фокусировка текста
        /// </summary>
        public bool IsFocus { get; private set; } = false;

        /// <summary>
        /// Инициализировать объект интерфейса. Текстовый ввод
        /// </summary>
        public IELTextBox()
        {
            InitializeComponent();
            #region Foreground
            TextBoxMain.Foreground = SourceForeground.SourceBrush;
            #endregion

            TextBoxMain.ContextMenu = null;

            GotKeyboardFocus += (sender, e) =>
            {
                IsFocus = true;
                SourceBackground.SetUsedState(true);
            };
            TextBoxMain.LostKeyboardFocus += (sender, e) =>
            {
                IsFocus = false;
                SourceBackground.SetUsedState(false);
            };

            MouseDown += (sender, e) =>
            {
                TextBoxMain.Focus();
            };
        }
    }
}
