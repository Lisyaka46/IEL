using IEL.CORE.Enums;
using IEL.Interfaces;
using System.Windows;

namespace IEL.CORE.Classes.ObjectSettings
{
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
        private BrushSettingQ _BackgroundSetting = new(new byte[,]
                        {
                        { 255, 0, 0, 0 },
                        { 255, 100, 100, 100 },
                        { 255, 168, 168, 168 },
                        { 255, 255, 0, 0 },
                        });
        /// <summary>
        /// Объект настройки состояний фона
        /// </summary>
        public BrushSettingQ BackgroundSetting
        {
            get => _BackgroundSetting;
            set
            {
                value.ColorChanged += (Spectrum, NewValue) =>
                {
                    if ((Spectrum == StateSpectrum.Default && !_BackgroundSetting.IsEnabled) || (Spectrum == StateSpectrum.NotEnabled && _BackgroundSetting.IsEnabled)) return;
                    BackgroundQChanged?.Invoke(NewValue);
                };
                _BackgroundSetting = value;
                BackgroundQChanged?.Invoke(value.Default);
            }
        }

        private BrushSettingQ _BorderBrushSetting = new(new byte[,]
                        {
                        { 255, 0, 0, 0 },
                        { 255, 60, 60, 60 },
                        { 255, 100, 100, 100 },
                        { 255, 156, 0, 0 },
                        });
        /// <summary>
        /// Объект настройки состояний границы
        /// </summary>
        public BrushSettingQ BorderBrushSetting
        {
            get => _BorderBrushSetting;
            set
            {
                value.ColorChanged += (Spectrum, NewValue) =>
                {
                    if ((Spectrum == StateSpectrum.Default && !_BorderBrushSetting.IsEnabled) || (Spectrum == StateSpectrum.NotEnabled && _BorderBrushSetting.IsEnabled)) return;
                    BorderBrushQChanged?.Invoke(NewValue);
                };
                _BorderBrushSetting = value;
                BorderBrushQChanged?.Invoke(value.Default);
            }
        }

        private BrushSettingQ _ForegroundSetting = new(new byte[,]
                        {
                        { 255, 0, 0, 0 },
                        { 255, 60, 60, 60 },
                        { 255, 60, 60, 60 },
                        { 255, 0, 0, 0 },
                        });
        /// <summary>
        /// Объект настройки состояний текста
        /// </summary>
        public BrushSettingQ ForegroundSetting
        {
            get => _ForegroundSetting;
            set
            {
                value.ColorChanged += (Spectrum, NewValue) =>
                {
                    if ((Spectrum == StateSpectrum.Default && !_ForegroundSetting.IsEnabled) || (Spectrum == StateSpectrum.NotEnabled && _ForegroundSetting.IsEnabled)) return;
                    ForegroundQChanged?.Invoke(NewValue);
                };
                _ForegroundSetting = value;
                ForegroundQChanged?.Invoke(value.Default);
            }
        }
        #region Event Change Color
        /// <summary>
        /// Делегат события изменения объекта настройки цвета
        /// </summary>
        /// <param name="NewValue">Новое значение обычного цвета в новом объекте настройки</param>
        public delegate void BrushSettingQElementChanged(Color NewValue);

        /// <summary>
        /// Обект события изменения цвета обычного состояния фона
        /// </summary>
        public event BrushSettingQElementChanged? BackgroundQChanged;

        /// <summary>
        /// Обект события изменения цвета обычного состояния границы
        /// </summary>
        public event BrushSettingQElementChanged? BorderBrushQChanged;

        /// <summary>
        /// Обект события изменения цвета обычного состояния текста
        /// </summary>
        public event BrushSettingQElementChanged? ForegroundQChanged;
        #endregion
        #endregion

        /// <summary>
        /// Подключить использование текущей настройки по событиям
        /// </summary>
        public void UseActiveQSetting()
        {
            BackgroundQChanged?.Invoke(BackgroundSetting.Default);
            BorderBrushQChanged?.Invoke(BorderBrushSetting.Default);
            ForegroundQChanged?.Invoke(ForegroundSetting.Default);
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
