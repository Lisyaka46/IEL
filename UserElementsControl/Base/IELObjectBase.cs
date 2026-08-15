using IEL.CORE.Animation;
using IEL.Interfaces;
using IEL.CORE.Themes.Data;
using IEL.CORE.Themes.Palettes;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace IEL.UserElementsControl.Base
{
    /// <summary>
    /// <b>БАЗОВЫЙ КЛАСС</b><br/>
    /// UI-объект IEL
    /// </summary>
    public class IELObjectBase : ContentControl, IIELAnimate
    {
        #region Properties

        #region Palette
        /// <summary>
        /// Данные спектра использования цветов
        /// </summary>
        [Description("Объект цветовой настройки всех видов отображения для Q-логики.\n" +
            "Реализует 3 состояния спектров цвета " +
            $"({nameof(Background)}, {nameof(BorderBrush)}, {nameof(Foreground)})\n" +
            "Спектры отвечают за наведение, нажатие и другие цветовые анимации.")]
        public virtual PaletteData Palette
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
        /// Объект менеджера анимаций настроек
        /// </summary>
        [Description("Менеджер анимаций используемый элементом.\n" +
            "С его помощью в объекте реализуются анимации и плавные переходы.\n" +
            "Если менеджер анимаций отсутствует, то анимирование не будет происходить, лишь резкое изменение.\n" +
            "Желательно использовать один общий менеджер анимаций для всех UI-объектов.")]
        public virtual AnimationManager? ManagerAnimation { get; set; }

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
