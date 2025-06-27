using ConsoleManipulateKey.CORE;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace OperPage_les.UI.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для WindowInputProgramKey.xaml
    /// </summary>
    public partial class WindowProgramKey : Window
    {
        internal bool Cancel { get; private set; } = true;

        internal bool CloseProject { get; private set; } = false;

        /// <summary>
        /// Объект анимации для управления Color значением
        /// </summary>
        private static readonly ColorAnimation ColorAnimate = new(Colors.Black, TimeSpan.FromMilliseconds(250d))
        {
            DecelerationRatio = 0.2d,
            EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseOut },
            From = null
        };

        private string PathSaveKey = string.Empty;

        public WindowProgramKey()
        {
            InitializeComponent();
            TextBlockPack.Foreground = new SolidColorBrush(Colors.Black);
            TextBoxKey.Background = new SolidColorBrush(Color.FromArgb(0, 255, 0, 0));

        }

        /// <summary>
        /// Открыть окно добавления ключа
        /// </summary>
        /// <returns>Состояние успешности проверки валидности ключа</returns>
        internal bool SetKeyValid(string PathSaveFileKey)
        {
            if (Directory.Exists(PathSaveFileKey)) throw new Exception($"Данная директория не доступка для сохранения файла ключа \"{PathSaveFileKey}\"");
            PathSaveKey = PathSaveFileKey;
            TextBlockPack.Text = Manipulate.GenerateKeyPack();
            #region SetEvents
            ButtonCopyPack.Click += (sender, e) =>
            {
                System.Windows.Clipboard.SetText(TextBlockPack.Text);
                ColorAnimation animation = ColorAnimate.Clone();
                animation.From = Color.FromArgb(255, 0, 255, 0);
                animation.To = Colors.Black;
                animation.Duration = TimeSpan.FromMilliseconds(1000d);
                TextBlockPack.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            };
            ButtonUpdatePack.Click += (sender, e) =>
            {
                TextBlockPack.Text = Manipulate.GenerateKeyPack();
            };
            ButtonCancel.Click += (sender, e) =>
            {
                MessageBoxResult Result = MessageBox.Show("При отмене ввода ключа приложение закроется.\nВы уперены что хотите выйти?", "Подтверждение действия",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
                Cancel = true;
                CloseProject = true;
                if (Result == MessageBoxResult.Yes) Close();
            };
            ButtonInput.Click += (sender, e) =>
            {
                Check();
            };
            TextBoxKey.KeyUp += (sender, e) =>
            {
                switch (e.Key)
                {
                    case Key.Enter:
                        Check();
                        break;
                    case Key.Escape:
                        Keyboard.ClearFocus();
                        break;
                }
            };
            #endregion
            if (!CloseProject) ShowDialog();
            return !Cancel;
        }

        /// <summary>
        /// Произвести попытку проверки валидности ключа
        /// </summary>
        private void Check()
        {
            try
            {
                Cancel = !Manipulate.CheckKeyValid(TextBlockPack.Text, TextBoxKey.Text);
            }
            catch
            {
                Cancel = true;
            }
            if (!Cancel)
            {
                System.IO.File.WriteAllText(PathSaveKey + @"/Key", $"{Manipulate.GetCodeUUID()} {TextBlockPack.Text} {TextBoxKey.Text}");
                Close();
            }
            else
            {
                ColorAnimation animation = ColorAnimate.Clone();
                animation.From = Color.FromArgb(255, 255, 0, 0);
                animation.To = Color.FromArgb(0, 255, 0, 0);
                animation.Duration = TimeSpan.FromMilliseconds(1000d);
                TextBoxKey.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
        }
    }
}
