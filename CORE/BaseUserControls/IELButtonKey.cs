using IEL.CORE.Enums;
using IEL.Interfaces.Front;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IEL.CORE.BaseUserControls
{
    /// <summary>
    /// БАЗОВЫЙ КЛАСС для отображения кнопки IEL с возможностью манипуляции клавиатурой
    /// </summary>
    public class IELButtonKey : IELButton, IIELButtonKey
    {
        private bool _CharKeyKeyboardActivate = false;
        /// <summary>
        /// Активность видимости символа действия активации кнопки
        /// </summary>
        public bool CharKeyboardActivate
        {
            get => _CharKeyKeyboardActivate;
            set
            {
                //BorderButton.BeginAnimation(MarginProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationThickness(new(!value ? -24 : 0, 0, 0, 0)));
                //BorderButtonKey.BeginAnimation(OpacityProperty, IELSettingObject.ObjectAnimateSetting.GetAnimationDouble(value ? 1d : 0d));
                _CharKeyKeyboardActivate = value;
            }
        }

        private Key _CharKeyKeyboard = Key.A;
        /// <summary>
        /// Клавиша отвечающая за активацию кнопки
        /// </summary>
        public Key CharKeyKeyboard
        {
            get => _CharKeyKeyboard;
            set
            {
                _CharKeyKeyboard = value;
                //TextBlockKey.Text = IIELObject.KeyName(value).ToString();
            }
        }

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
