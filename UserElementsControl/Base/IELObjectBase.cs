using LibraryIEL.CORE.Themes;
using LibraryIEL.CORE.Themes.Data;
using LibraryIEL.CORE.Themes.Palette;
using LibraryIEL.CORE.Themes.Palettes;
using System.Windows;
using System.Windows.Controls;

namespace IEL.UserElementsControl.Base
{
    /// <summary>
    /// БАЗОВЫЙ КЛАСС для отображения элемента IEL
    /// </summary>
    public class IELObjectBase : ContentControl
    {
        #region Properties

        #region Palette
        /// <summary>
        /// Данные палитры использования цветов
        /// </summary>
        private byte[] PaletteData;

        /// <summary>
        /// Данные спектра использования цветов
        /// </summary>
        public PaletteSpectrumData Palette
        {
            get => new(PaletteData);
            set
            {
                PaletteData = value.GetSourceBytes();
                SourceBackground.ChangeData(value.BackGroundData, IsAnimatedSettingQ);
                SourceBorderBrush.ChangeData(value.BorderGroundData, IsAnimatedSettingQ);
                SourceForeground.ChangeData(value.ForeGroundData, IsAnimatedSettingQ);
            }
        }
        #endregion

        #region Background
        /// <summary>
        /// Объект настройки анимации отображения фона в объекте
        /// </summary>
        public PaletteSpectrum SourceBackground { get; } = new(PaletteSpectrum.DefaultBG);

        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(QData), typeof(IELObjectBase),
                new(PaletteSpectrum.DefaultBG,
                    (sender, e) =>
                    {
                        ((IELObjectBase)sender).SourceBackground.ChangeSourceQData((QData)e.NewValue);
                    }));

        /// <summary>
        /// Объект настройки отображения фона 
        /// </summary>
        public new QData Background
        {
            get => Palette.BG;
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
        public PaletteSpectrum SourceBorderBrush { get; } = new(PaletteSpectrum.DefaultBB);

        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty BorderBrushProperty =
            DependencyProperty.Register("BorderBrush", typeof(QData), typeof(IELObjectBase),
                new(PaletteSpectrum.DefaultBB,
                    (sender, e) =>
                    {
                        ((IELObjectBase)sender).SourceBorderBrush.ChangeSourceQData((QData)e.NewValue);
                    }));

        /// <summary>
        /// Объект настройки отображения границ
        /// </summary>
        public new QData BorderBrush
        {
            get => Palette.BB;
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
        public PaletteSpectrum SourceForeground { get; } = new(PaletteSpectrum.DefaultFG);

        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(QData), typeof(IELObjectBase),
                new(PaletteSpectrum.DefaultFG,
                    (sender, e) =>
                    {
                        ((IELObjectBase)sender).SourceForeground.ChangeSourceQData((QData)e.NewValue);
                    }));

        /// <summary>
        /// Объект настройки отображения текста
        /// </summary>
        public new QData Foreground
        {
            get => Palette.FG;
            set
            {
                SetValue(ForegroundProperty, value);
            }
        }
        #endregion

        #region IsEnabledSettingQ
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty IsEnabledSettingQProperty =
            DependencyProperty.Register("IsEnabledSettingQ", typeof(bool), typeof(IELObjectBase),
                new(true,
                    (sender, e) =>
                    {
                        ((IELObjectBase)sender).SetActiveSpecrum(SpectrumColor.Default, false);
                    }));

        /// <summary>
        /// Состояние использования настроек Q
        /// </summary>
        public bool IsEnabledSettingQ
        {
            get => (bool)GetValue(IsEnabledSettingQProperty);
            set => SetValue(IsEnabledSettingQProperty, value);
        }
        #endregion

        #region IsAnimatedSettingQ
        /// <summary>
        /// Значение активности анимирования объекта
        /// </summary>
        private bool _IsAnimatedSettingQ = true;

        /// <summary>
        /// Состояние анимирования настройки Q-логики
        /// </summary>
        public bool IsAnimatedSettingQ
        {
            get => _IsAnimatedSettingQ;
            set
            {
                SourceBackground.SetActiveSpecrum(SpectrumColor.Default, true);
                SourceBorderBrush.SetActiveSpecrum(SpectrumColor.Default, true);
                SourceForeground.SetActiveSpecrum(SpectrumColor.Default, true);
                _IsAnimatedSettingQ = value;
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// Активировать визуализацию спектра для всех Q сегментов
        /// </summary>
        /// <param name="Spectrum">Устанавливаемый спектр</param>
        /// <param name="Animated">Состояние анимирования изменения</param>
        public void SetActiveSpecrum(SpectrumColor Spectrum, bool Animated)
        {
            if (!IsEnabledSettingQ) return;
            SourceBackground.SetActiveSpecrum(Spectrum, Animated);
            SourceBorderBrush.SetActiveSpecrum(Spectrum, Animated);
            SourceForeground.SetActiveSpecrum(Spectrum, Animated);
        }

        /// <summary>
        /// Активировать визуализацию спектра для всех Q сегментов в зависимости от настройки анимирования объекта
        /// </summary>
        /// <param name="Spectrum">Устанавливаемый спектр</param>
        public void SetActiveSpecrum(SpectrumColor Spectrum) => SetActiveSpecrum(Spectrum, _IsAnimatedSettingQ);
    }
}
