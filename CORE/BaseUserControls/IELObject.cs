using IEL.CORE.Classes;
using IEL.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace IEL.CORE.BaseUserControls
{
    /// <summary>
    /// БАЗОВЫЙ КЛАСС для отображения элемента IEL
    /// </summary>
    public class IELObject : ContentControl
    {
        #region Properties

        #region Background
        /// <summary>
        /// Объект настройки анимации отображения фона в объекте
        /// </summary>
        public BrushSettingQ SourceBackground { get; } = new();

        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(QData), typeof(IELObject),
                new(new QData(),
                    (sender, e) =>
                    {
                        ((IELObject)sender).SourceBackground.SetQData((QData)e.NewValue);
                    }));

        /// <summary>
        /// Объект настройки отображения фона 
        /// </summary>
        public new QData Background
        {
            get => (QData)GetValue(BackgroundProperty);
            set
            {
                SetValue(BackgroundProperty, value);
            }
        }
        #endregion

        #region BorderBrush
        /// <summary>
        /// Объект настройки анимирования отображения границ в объекте
        /// </summary>
        public BrushSettingQ SourceBorderBrush { get; } = new();

        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty BorderBrushProperty =
            DependencyProperty.Register("BorderBrush", typeof(QData), typeof(IELObject),
                new(new QData(),
                    (sender, e) =>
                    {
                        ((IELObject)sender).SourceBorderBrush.SetQData((QData)e.NewValue);
                    }));

        /// <summary>
        /// Объект настройки отображения границ
        /// </summary>
        public new QData BorderBrush
        {
            get => (QData)GetValue(BorderBrushProperty);
            set
            {
                SetValue(BorderBrushProperty, value);
            }
        }
        #endregion

        #region Foreground
        /// <summary>
        /// Объект настройки анимирования отображения текста в объекте
        /// </summary>
        public BrushSettingQ SourceForeground { get; } = new();

        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(QData), typeof(IELObject),
                new(new QData(),
                    (sender, e) =>
                    {
                        ((IELObject)sender).SourceForeground.SetQData((QData)e.NewValue);
                    }));

        /// <summary>
        /// Объект настройки отображения текста
        /// </summary>
        public new QData Foreground
        {
            get => (QData)GetValue(ForegroundProperty);
            set
            {
                SetValue(ForegroundProperty, value);
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// Активировать визуализацию спектра для всех Q сегментов
        /// </summary>
        /// <param name="Spectrum">Устанавливаемый спектр</param>
        /// <param name="Animated">Состояние анимирования изменения</param>
        public void SetActiveSpecrum(StateSpectrum Spectrum, bool Animated)
        {
            SourceBackground.SetActiveSpecrum(Spectrum, Animated);
            SourceBorderBrush.SetActiveSpecrum(Spectrum, Animated);
            SourceForeground.SetActiveSpecrum(Spectrum, Animated);
        }
    }
}
