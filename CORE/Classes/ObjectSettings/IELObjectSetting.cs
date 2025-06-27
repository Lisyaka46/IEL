using IEL.CORE.Enums;
using IEL.Interfaces;
using OperPage_les.UI.Dialogs;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace IEL.CORE.Classes.ObjectSettings
{
    public partial class IELObjectSetting : IIELObjectSetting
    {
        /// <summary>
        /// Делегат события изменения поля значения
        /// </summary>
        /// <typeparam name="T_Value">Входной тип изменяемого значения</typeparam>
        /// <param name="NewValue">Новое значение</param>
        public delegate void IELSettingValueChangedHandler<T_Value>(T_Value NewValue);

        /// <summary>
        /// Объект настройки анимации объектов
        /// </summary>
        internal readonly AnimateSetting ObjectAnimateSetting = new();

        /// <summary>
        /// Длительность анимации
        /// </summary>
        public double AnimationMillisecond
        {
            get => ObjectAnimateSetting.AnimationMillisecond;
            set => ObjectAnimateSetting.AnimationMillisecond = value;
        }

        #region Color Setting
        /// <summary>
        /// Объект настройки состояний фона
        /// </summary>
        public BrushSettingQ BackgroundSetting { get; set; } = new(new byte[,]
                        {
                        { 255, 0, 0, 0 },
                        { 255, 100, 100, 100 },
                        { 255, 168, 168, 168 },
                        { 255, 255, 0, 0 },
                        });

        /// <summary>
        /// Объект настройки состояний границы
        /// </summary>
        public BrushSettingQ BorderBrushSetting { get; set; } = new(new byte[,]
                        {
                        { 255, 0, 0, 0 },
                        { 255, 60, 60, 60 },
                        { 255, 100, 100, 100 },
                        { 255, 156, 0, 0 },
                        });

        /// <summary>
        /// Объект настройки состояний текста
        /// </summary>
        public BrushSettingQ ForegroundSetting { get; set; } = new(new byte[,]
                        {
                        { 255, 0, 0, 0 },
                        { 255, 60, 60, 60 },
                        { 255, 60, 60, 60 },
                        { 255, 0, 0, 0 },
                        });
        #endregion

        /// <summary>
        /// Подключить использование текущей настройки по событиям
        /// </summary>
        public void UseActiveQSetting(StateSpectrum ActivateSpectrum, bool Animated = true)
        {
            BackgroundSetting.InvokeObjectUsedStateColor(ActivateSpectrum, Animated);
            BorderBrushSetting.InvokeObjectUsedStateColor(ActivateSpectrum, Animated);
            ForegroundSetting.InvokeObjectUsedStateColor(ActivateSpectrum, Animated);
        }

        /// <summary>
        /// Инициализировать класс настроек объекта
        /// </summary>
        public IELObjectSetting()
        {
            AnimationMillisecond = 200d;
        }

        #region Key
        /// <summary>
        /// Главная директория ресурсов проекта
        /// </summary>
        internal static readonly string MainDirectoryLibrary = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/IEL/";

        /// <summary>
        /// Главная директория ресурсов проекта
        /// </summary>
        internal static readonly string DirectoryValidKeyLibrary = MainDirectoryLibrary + @"Key";

        /// <summary>
        /// Установить ключ для библиотеки интерфейса на прямую
        /// </summary>
        /// <param name="SourcePathFileKey">Директория файла ключа</param>
        /// <returns></returns>
        public static bool SetFileKey(string SourcePathFileKey)
        {
            if (File.Exists(DirectoryValidKeyLibrary)) if (CheckFileKey()) return true;
            if (!File.Exists(SourcePathFileKey) && Path.GetFileName(SourcePathFileKey).Equals("Key")) return false;
            Directory.CreateDirectory(MainDirectoryLibrary);
            File.Copy(SourcePathFileKey, SourcePathFileKey + "_COPY");
            File.Move(SourcePathFileKey + "_COPY", DirectoryValidKeyLibrary);
            return true;
        }

        /// <summary>
        /// Установить ключ для библиотеки интерфейса через интерфейс
        /// </summary>
        /// <returns></returns>
        public static ResultSet SetFileKeyInWindow()
        {
            WindowProgramKey WprogramKey = new();
            WprogramKey.SetKeyValid(MainDirectoryLibrary);
            return WprogramKey.Cancel ? (WprogramKey.CloseProject ? ResultSet.Close : ResultSet.NotSet) : ResultSet.Complete;
        }

        /// <summary>
        /// Проверить установленный ключ валидности
        /// </summary>
        /// <returns></returns>
        internal static bool CheckFileKey()
        {
            if (!File.Exists(DirectoryValidKeyLibrary)) return false;
            bool InitKeyValid;
            try
            {
                string MainPackAndValidKey = File.ReadAllText(DirectoryValidKeyLibrary);
                string UUID = RegexPackValidKey().Match(MainPackAndValidKey).Value;
                MainPackAndValidKey = MainPackAndValidKey[(UUID.Length + 1)..];
                string Pack = RegexPackValidKey().Match(MainPackAndValidKey).Value;
                string Key = MainPackAndValidKey[(Pack.Length + 1)..];
                InitKeyValid = ConsoleManipulateKey.CORE.Manipulate.CheckKeyValid(Pack, Key) && UUID.Equals(ConsoleManipulateKey.CORE.Manipulate.GetCodeUUID());
            }
            catch
            {
                InitKeyValid = false;
            }
            if (!InitKeyValid) MessageBox.Show("Установленный валидный ключ не подходит", "Предупреждение",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return InitKeyValid;
        }

        /// <summary>
        /// Использовать проверку установки ключа валидности
        /// </summary>
        /// <exception cref="Exception">Отказ на предоставление ключа</exception>
        internal static void GlobalSetValidKey()
        {
            if (!CheckFileKey())
                while (true)
                {
                    ResultSet result = SetFileKeyInWindow();
                    if (result == ResultSet.Close) throw new Exception("Отказ о предоставлении ключа валидности.");
                    else if (result == ResultSet.Complete) break;
                }
        }

        #region Regex
        [GeneratedRegex(@"[^ ]+")]
        private static partial Regex RegexPackValidKey();
        #endregion
        #endregion
    }
}
