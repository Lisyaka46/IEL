using IEL.CORE.Enums;
using System.Windows.Media;

namespace IEL.CORE.Classes
{
    /// <summary>
    /// Класс настройки поведения цвета при разных состояниях объекта
    /// </summary>
    public class BrushSettingQ : ICloneable
    {
        private Action<StateSpectrum, Color, bool>? ActionColorChange { get; set; }

        /// <summary>
        /// Установить значение действия воспроизводимое для взаимодействия объекта и настроек цвета
        /// </summary>
        /// <param name="action">Параметр действия</param>
        /// <returns>Состояние добавления действия к настройке</returns>
        public bool SetActionColorChanged(in Action<StateSpectrum, Color, bool> action)
        {
            ActionColorChange = action.Clone() as Action<StateSpectrum, Color, bool>;
            ActionColorChange?.Invoke(StateSpectrum.Default, Default, false);
            return true;
        }

        /// <summary>
        /// Массив данных цвета
        /// </summary>
        public QData ColorData { get; private set; }

        /// <summary>
        /// Последний изменяемый спектр цыета
        /// </summary>
        private StateSpectrum LastChange { get; set; }

        /// <summary>
        /// Изменить напрямую стиль отображения объекта
        /// </summary>
        /// <param name="Spectrum">Стиль придаваемый использоваемому объекту</param>
        /// <param name="Animated">Анимируются ли свойства цвета</param>
        public void InvokeObjectUsedStateColor(StateSpectrum Spectrum, bool Animated = true)
        {
            ActionColorChange?.Invoke(Spectrum, ColorData.GetIndexingColor(Spectrum), Animated);
        }

        #region UsedState
        /// <summary>
        /// Состояние навигации использования
        /// </summary>
        /// <remarks>
        /// При включённом состоянии цвет обычного состояния становится использованным, а использованный обычным
        /// <code></code>
        /// <b>Default <![CDATA[<]]>=<![CDATA[>]]> Used</b>
        /// </remarks>
        private bool UsedState;

        /// <summary>
        /// Узнать состояние использования
        /// </summary>
        /// <returns>Текущее состояние использования</returns>
        public bool GetUsedState() => UsedState;

        /// <summary>
        /// Установить новое значение использованию цвета
        /// </summary>
        /// <param name="NewValue">Новое значение</param>
        public void SetUsedState(bool NewValue)
        {
            UsedState = NewValue;
            try
            {
                LastChange = StateSpectrum.Default;
                ActionColorChange?.Invoke(StateSpectrum.Default, ColorData.GetIndexingColor(NewValue ? StateSpectrum.Used : StateSpectrum.Default), true);
            }
            catch { }
        }
        #endregion

        #region Default
        /// <summary>
        /// Цвет отключённого состояния состояния
        /// </summary>
        public Color Default
        {
            get => ColorData.GetIndexingColor(UsedState ? StateSpectrum.Used : StateSpectrum.Default);
            set
            {
                LastChange = StateSpectrum.Default;
                ColorData.SetIndexingColor(StateSpectrum.Default, value);
                ActionColorChange?.Invoke(StateSpectrum.Default, value, true);
            }
        }
        #endregion

        #region NotEnabled
        /// <summary>
        /// Цвет отключённого состояния состояния
        /// </summary>
        public Color NotEnabled
        {
            get => ColorData.GetIndexingColor(StateSpectrum.NotEnabled);
            set
            {
                LastChange = StateSpectrum.NotEnabled;
                ColorData.SetIndexingColor(StateSpectrum.NotEnabled, value);
                ActionColorChange?.Invoke(StateSpectrum.NotEnabled, value, true);
            }
        }
        #endregion

        #region Select
        /// <summary>
        /// Цвет выделенного состояния
        /// </summary>
        public Color Select
        {
            get => ColorData.GetIndexingColor(StateSpectrum.Select);
            set
            {
                LastChange = StateSpectrum.Select;
                ColorData.SetIndexingColor(StateSpectrum.Select, value);
            }
        }

        #endregion

        #region Used
        /// <summary>
        /// Цвет нажатого или использованого состояния
        /// </summary>
        public Color Used
        {
            get => ColorData.GetIndexingColor(UsedState ? StateSpectrum.Default : StateSpectrum.Used);
            set
            {
                LastChange = StateSpectrum.Used;
                ColorData.SetIndexingColor(StateSpectrum.Used, value);
            }
        }
        #endregion

        public BrushSettingQ()
        {
            UsedState = false;
            ColorData = new();
        }

        /// <summary>
        /// Default -> Select -> Used -> NotEnabled
        /// </summary>
        /// <param name="ByteColorData"></param>
        public BrushSettingQ(byte[,] ByteColorData)
        {
            UsedState = false;
            ColorData = new(ByteColorData);
        }

        /// <summary>
        /// Source QData
        /// </summary>
        /// <param name="ByteColorData"></param>
        public BrushSettingQ(QData ByteColorData)
        {
            UsedState = false;
            ColorData = (QData)ByteColorData.Clone();
        }

        /// <summary>
        /// Клонировать объект политры
        /// </summary>
        /// <returns>Объект палитры</returns>
        public object Clone()
        {
            BrushSettingQ Clone = new(ColorData)
            {
                ActionColorChange = ActionColorChange
            };
            Clone.ActionColorChange?.Invoke(LastChange, Clone.ColorData.GetIndexingColor(LastChange), true);
            return Clone;
        }
    }
}
