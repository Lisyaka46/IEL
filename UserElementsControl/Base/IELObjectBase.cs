using LibraryIEL.CORE.Animation;
using LibraryIEL.CORE.Themes.Data;
using LibraryIEL.CORE.Themes.Palettes;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace LibraryIEL.UserElementsControl.Base
{
    /// <summary>
    /// <b>БАЗОВЫЙ КЛАСС</b><br/>
    /// UI-объект IEL
    /// </summary>
    public class IELObjectBase : ContentControl
    {
        #region Properties

        #region Palette
        /// <summary>
        /// Данные свойства <see cref="Palette"/>
        /// </summary>
        public static readonly DependencyProperty PaletteProperty =
            DependencyProperty.Register(nameof(Palette), typeof(PaletteData), typeof(IELObjectBase),
                new(PaletteHandler));

        /// <summary>
        /// Обработчик события изменения свойства <see cref="Palette"/>
        /// </summary>
        private static void PaletteHandler(DependencyObject Element, DependencyPropertyChangedEventArgs e)
        {
            if (Element is IELObjectBase Source && e.NewValue is PaletteData SourceNewValue)
            {
                Source.Background = SourceNewValue.BackGroundData;
                Source.BorderBrush = SourceNewValue.BorderGroundData;
                Source.Foreground = SourceNewValue.ForeGroundData;
            }
        }

        /// <summary>
        /// Данные спектра использования цветов
        /// </summary>
        [Description("Объект цветовой настройки всех видов отображения для Q-логики.\n" +
            "Реализует 3 состояния спектров цвета " +
            $"({nameof(Background)}, {nameof(BorderBrush)}, {nameof(Foreground)})\n" +
            "Спектры отвечают за наведение, нажатие и другие цветовые анимации.")]
        public PaletteData Palette
        {
            get => (PaletteData)GetValue(PaletteProperty);
            set => SetValue(PaletteProperty, value);
        }
        #endregion

        #region Background
        /// <summary>
        /// Объект настройки анимации отображения фона в объекте
        /// </summary>
        public PaletteSpectrum SourceBackground { get; } = new(QData.UnknownSpectrumBackGround);

        /// <summary>
        /// Объект настройки отображения фона 
        /// </summary>
        [Description("Объект цветовой настройки отображения фона объекта для Q-логики.\n" +
            "Реализует 4 спектра цветов, которые отвечают за наведение, нажатие и другие цветовые анимации.")]
        public virtual new QData Background
        {
            get => SourceBackground.GetData();
            set => SourceBackground.ChangeData(value, IsAnimatedSettingQ);
        }
        #endregion

        #region BorderBrush
        /// <summary>
        /// Объект настройки анимирования отображения границ в объекте
        /// </summary>
        public PaletteSpectrum SourceBorderBrush { get; } = new(QData.UnknownSpectrumBorderGround);

        /// <summary>
        /// Объект настройки отображения границ
        /// </summary>
        [Description("Объект цветовой настройки отображения границ объекта для Q-логики.\n" +
            "Реализует 4 спектра цветов, которые отвечают за наведение, нажатие и другие цветовые анимации.")]
        public virtual new QData BorderBrush
        {
            get => SourceBorderBrush.GetData();
            set => SourceBorderBrush.ChangeData(value, IsAnimatedSettingQ);
        }
        #endregion

        #region Foreground
        /// <summary>
        /// Объект настройки анимирования отображения текста в объекте
        /// </summary>
        public PaletteSpectrum SourceForeground { get; } = new(QData.UnknownSpectrumForeGround);


        /// <summary>
        /// Объект настройки отображения текста
        /// </summary>
        [Description("Объект цветовой настройки отображения текста для Q-логики.\n" +
            "Реализует 4 спектра цветов, которые отвечают за наведение, нажатие и другие цветовые анимации.")]
        public virtual new QData Foreground
        {
            get => SourceForeground.GetData();
            set => SourceForeground.ChangeData(value, IsAnimatedSettingQ);
        }
        #endregion

        #region IsEnabledSettingQ
        /// <summary>
        /// Данные свойства <see cref="IsEnabledSettingQ"/>
        /// </summary>
        public static readonly DependencyProperty IsEnabledSettingQProperty =
            DependencyProperty.Register(nameof(IsEnabledSettingQ), typeof(bool), typeof(IELObjectBase),
                new(true,
                    (sender, e) =>
                    {
                        ((IELObjectBase)sender).SetActiveSpecrum(SpectrumColor.Default);
                    }));

        /// <summary>
        /// Состояние использования настроек Q
        /// </summary>
        [Description("Состояние активности Q-логики.\n" +
            "Отвечает за использование данной цветовой логики наведения, нажатия и других анимаций.\n" +
            "При отключённом состоянии объект продолжит функционировать, лишь будет отсутствовать визуальная цветовая анимация Q-логики.")]
        public bool IsEnabledSettingQ
        {
            get => (bool)GetValue(IsEnabledSettingQProperty);
            set => SetValue(IsEnabledSettingQProperty, value);
        }
        #endregion

        #region IsAnimatedSettingQ
        /// <summary>
        /// Данные свойства <see cref="IsAnimatedSettingQ"/>
        /// </summary>
        public static readonly DependencyProperty IsAnimatedSettingQProperty =
            DependencyProperty.Register(nameof(IsAnimatedSettingQ), typeof(bool), typeof(IELObjectBase),
                new(true));

        /// <summary>
        /// Состояние анимирования настройки Q-логики
        /// </summary>
        /// <remarks>
        /// Данное свойство зависит от <see cref="IsEnabledSettingQ"/>, так как оно включает использование цветов
        /// </remarks>
        [Description("Состояние анимирования Q-логики.\n" +
            "Отвечает за плавтоне изменение цвета при наведении, нажатия и других визуальных цветовых анимаций.\n" +
            $"Этот параметр зависит от {nameof(IsEnabledSettingQ)}, который отвечает за активность данной настройки.")]
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
