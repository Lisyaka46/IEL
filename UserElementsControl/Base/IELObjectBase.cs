using LibraryIEL.CORE.Themes.Data;
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
        /// Данные спектра использования цветов
        /// </summary>
        public PaletteData Palette
        {
            get => new(Background, BorderBrush, Foreground);
            set
            {
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
        public PaletteSpectrum SourceBackground { get; } = PaletteSpectrum.UnknownSpectrumBackGround;

        /// <summary>
        /// Объект настройки отображения фона 
        /// </summary>
        public new QData Background
        {
            get => SourceBackground.GetData();
            set => SourceBackground.ChangeData(value, IsAnimatedSettingQ);
        }
        #endregion

        #region BorderBrush
        /// <summary>
        /// Объект настройки анимирования отображения границ в объекте
        /// </summary>
        public PaletteSpectrum SourceBorderBrush { get; } = PaletteSpectrum.UnknownSpectrumBorderGround;

        /// <summary>
        /// Объект настройки отображения границ
        /// </summary>
        public new QData BorderBrush
        {
            get => SourceBorderBrush.GetData();
            set => SourceBorderBrush.ChangeData(value, IsAnimatedSettingQ);
        }
        #endregion

        #region Foreground
        /// <summary>
        /// Объект настройки анимирования отображения текста в объекте
        /// </summary>
        public PaletteSpectrum SourceForeground { get; } = PaletteSpectrum.UnknownSpectrumForeGround;


        /// <summary>
        /// Объект настройки отображения текста
        /// </summary>
        public new QData Foreground
        {
            get => SourceForeground.GetData();
            set => SourceForeground.ChangeData(value, IsAnimatedSettingQ);
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
                        ((IELObjectBase)sender).SetActiveSpecrum(SpectrumColor.Default);
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
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty IsAnimatedSettingQProperty =
            DependencyProperty.Register("IsAnimatedSettingQ", typeof(bool), typeof(IELObjectBase),
                new(true));

        /// <summary>
        /// Состояние анимирования настройки Q-логики
        /// </summary>
        /// <remarks>
        /// Данное свойство зависит от <see cref="IsEnabledSettingQ"/>, так как оно включает использование цветов
        /// </remarks>
        public bool IsAnimatedSettingQ
        {
            get => (bool)GetValue(IsAnimatedSettingQProperty) && IsEnabledSettingQ;
            set
            {
                SourceBackground.SetActiveSpecrum(SpectrumColor.Default, true);
                SourceBorderBrush.SetActiveSpecrum(SpectrumColor.Default, true);
                SourceForeground.SetActiveSpecrum(SpectrumColor.Default, true);
                SetValue(IsAnimatedSettingQProperty, value);
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// Активировать визуализацию спектра для всех Q сегментов
        /// </summary>
        /// <param name="Spectrum">Устанавливаемый спектр</param>
        public void SetActiveSpecrum(SpectrumColor Spectrum)
        {
            if (!IsEnabledSettingQ) return;
            SourceBackground.SetActiveSpecrum(Spectrum, IsAnimatedSettingQ);
            SourceBorderBrush.SetActiveSpecrum(Spectrum, IsAnimatedSettingQ);
            SourceForeground.SetActiveSpecrum(Spectrum, IsAnimatedSettingQ);
        }
    }
}
