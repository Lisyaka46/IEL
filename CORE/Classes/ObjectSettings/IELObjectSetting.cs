using IEL.CORE.Enums;
using IEL.Interfaces;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using Color = System.Windows.Media.Color;

namespace IEL.CORE.Classes.ObjectSettings
{
    /// <summary>
    /// Общий класс настроек объектов интерфейса
    /// </summary>
    public partial class IELObjectSetting : DependencyObject, IIELObjectSetting
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
    }
}
