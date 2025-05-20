using IEL.CORE.Enums;
using IEL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IEL.CORE.Classes.ObjectSettings
{
    public class IELObjectSetting : IIELObjectSetting
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
        private BrushSettingQ _BackgroundSetting = new()
        {
            Default = Colors.White,
            Select = Colors.Gray,
            Used = Colors.LightGray,
            NotEnabled = Colors.Red,
        };
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
                    EVENT_BackgroundQChanged?.Invoke(NewValue);
                };
                _BackgroundSetting = value;
                EVENT_BackgroundQChanged?.Invoke(value.Default);
            }
        }

        private BrushSettingQ _BorderBrushSetting = new()
        {
            Default = Colors.Black,
            Select = Colors.DarkGray,
            Used = Colors.Gray,
            NotEnabled = Colors.DarkRed,
        };
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
                    EVENT_BorderBrushQChanged?.Invoke(NewValue);
                };
                _BorderBrushSetting = value;
                EVENT_BorderBrushQChanged?.Invoke(value.Default);
            }
        }

        private BrushSettingQ _ForegroundSetting = new()
        {
            Default = Colors.Black,
            Select = Colors.DarkGray,
            Used = Colors.DarkGray,
            NotEnabled = Colors.Black,
        };
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
                    EVENT_ForegroundQChanged?.Invoke(NewValue);
                };
                _ForegroundSetting = value;
                EVENT_ForegroundQChanged?.Invoke(value.Default);
            }
        }
        #region Event Change Color
        /// <summary>
        /// Делегат события изменения объекта настройки цвета
        /// </summary>
        /// <param name="NewValue">Новое значение обычного цвета в новом объекте настройки</param>
        public delegate void BrushSettingQElementChanged(Color NewValue);

        private event BrushSettingQElementChanged? EVENT_BackgroundQChanged;
        /// <summary>
        /// Обект события изменения цвета обычного состояния фона
        /// </summary>
        public event BrushSettingQElementChanged? BackgroundQChanged
        {
            add
            {
                value?.Invoke(BackgroundSetting.Default);
                EVENT_BackgroundQChanged += value;
            }
            remove => EVENT_BackgroundQChanged -= value;
        }

        private event BrushSettingQElementChanged? EVENT_BorderBrushQChanged;
        /// <summary>
        /// Обект события изменения цвета обычного состояния границы
        /// </summary>
        public event BrushSettingQElementChanged? BorderBrushQChanged
        {
            add
            {
                value?.Invoke(BorderBrushSetting.Default);
                EVENT_BorderBrushQChanged += value;
            }
            remove => EVENT_BorderBrushQChanged -= value;
        }


        private event BrushSettingQElementChanged? EVENT_ForegroundQChanged;
        /// <summary>
        /// Обект события изменения цвета обычного состояния текста
        /// </summary>
        public event BrushSettingQElementChanged? ForegroundQChanged
        {
            add
            {
                value?.Invoke(ForegroundSetting.Default);
                EVENT_ForegroundQChanged += value;
            }
            remove => EVENT_ForegroundQChanged -= value;
        }
        #endregion
        #endregion

        /// <summary>
        /// Инициализировать класс настроек объекта
        /// </summary>
        public IELObjectSetting()
        {
            AnimationMillisecond = 200d;
        }
    }
}
