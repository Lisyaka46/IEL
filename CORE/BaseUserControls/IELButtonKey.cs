using IEL.CORE.Enums;

namespace IEL.CORE.BaseUserControls
{
    /// <summary>
    /// БАЗОВЫЙ КЛАСС для отображения кнопки IEL с возможностью манипуляции клавиатурой
    /// </summary>
    public class IELButtonKey : IELButton
    {
        /// <summary>
        /// Инициализировать <b>БАЗОВОЕ ПРЕДСТАВЛЕНИЕ</b> кнопки IEL с позможностью управления клавиатурой
        /// </summary>
        public IELButtonKey()
        {
            //InitializeComponent();
        }

        /// <summary>
        /// Анимация мерцания
        /// </summary>
        [MTAThread()]
        public void BlinkAnimation()
        {
            SetActiveSpecrum(StateSpectrum.Used, false);

            SetActiveSpecrum(StateSpectrum.Select, true);
            //IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, this, true);
        }

        /// <summary>
        /// Анимация мерцания
        /// </summary>
        [MTAThread()]
        public void UnfocusAnimation()
        {
            SetActiveSpecrum(StateSpectrum.Default, true);
            //IELSettingObject.UpdateVisibleMouseEvents(ImageMouseButtonsUse, false);
        }
    }
}
