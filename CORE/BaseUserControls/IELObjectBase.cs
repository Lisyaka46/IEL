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
using static IEL.CORE.Classes.QData;

namespace IEL.CORE.BaseUserControls
{
    /// <summary>
    /// БАЗОВЫЙ КЛАСС для отображения элемента IEL
    /// </summary>
    public class IELObjectBase : ContentControl
    {
        #region Properties

        #region SourcePaletteElement
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty PaletteElementProperty =
            DependencyProperty.Register("PaletteElement", typeof(PaletteSpectrum), typeof(IELObjectBase),
                new(new PaletteSpectrum(),
                    (sender, e) =>
                    {
                        PaletteSpectrum palette = (PaletteSpectrum)e.NewValue;
                        ((IELObjectBase)sender).SourceBackground.Source = palette.BG;
                        ((IELObjectBase)sender).SourceBorderBrush.Source = palette.BB;
                        ((IELObjectBase)sender).SourceForeground.Source = palette.FG;
                    }));

        /// <summary>
        /// Объект палитры
        /// </summary>
        public PaletteSpectrum PaletteElement
        {
            get => (PaletteSpectrum)GetValue(PaletteElementProperty);
            set
            {
                SetValue(PaletteElementProperty, value);
            }
        }
        #endregion

        #region Background
        /// <summary>
        /// Объект настройки анимации отображения фона в объекте
        /// </summary>
        public BrushSettingQ SourceBackground { get; } = new();

        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(QData), typeof(IELObjectBase),
                new(new QData(),
                    (sender, e) =>
                    {
                        ((IELObjectBase)sender).SourceBackground.ChangeSourceQData((QData)e.NewValue);
                    }));

        /// <summary>
        /// Объект настройки отображения фона 
        /// </summary>
        public new QData Background
        {
            get => PaletteElement.BG;
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
            DependencyProperty.Register("BorderBrush", typeof(QData), typeof(IELObjectBase),
                new(new QData(),
                    (sender, e) =>
                    {
                        ((IELObjectBase)sender).SourceBorderBrush.ChangeSourceQData((QData)e.NewValue);
                    }));

        /// <summary>
        /// Объект настройки отображения границ
        /// </summary>
        public new QData BorderBrush
        {
            get => PaletteElement.BB;
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
            DependencyProperty.Register("Foreground", typeof(QData), typeof(IELObjectBase),
                new(new QData(),
                    (sender, e) =>
                    {
                        ((IELObjectBase)sender).SourceForeground.ChangeSourceQData((QData)e.NewValue);
                    }));

        /// <summary>
        /// Объект настройки отображения текста
        /// </summary>
        public new QData Foreground
        {
            get => PaletteElement.FG;
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
