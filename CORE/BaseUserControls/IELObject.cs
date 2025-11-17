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

namespace IEL.CORE.BaseUserControls
{
    /// <summary>
    /// БАЗОВЫЙ КЛАСС для отображения элемента IEL
    /// </summary>
    public class IELObject : ContentControl
    {
        /// <summary>
        /// Устаревшее свойство
        /// </summary>
        [Obsolete("Для установки используются стили - Перегружено под свойством объекта IELObject (QBackground)", true)]
        protected new Brush? Background;

        /// <summary>
        /// Устаревшее свойство
        /// </summary>
        [Obsolete("Для установки используются стили - Перегружено под свойством объекта IELObject (QBorderBrush)", true)]
        protected new Brush? BorderBrush;

        /// <summary>
        /// Устаревшее свойство
        /// </summary>
        [Obsolete("Для установки используются стили - Перегружено под свойством объекта IELObject (QForeground)", true)]
        protected new Brush? Foreground;

        #region Color Setting
        /// <summary>
        /// Ресурсный объект настройки состояний фона
        /// </summary>
        private readonly BrushSettingQ _QBackground = new();
        /// <summary>
        /// Объект настройки состояний фона
        /// </summary>
        public BrushSettingQ QBackground
        {
            get => _QBackground;
            set
            {
                _QBackground.SetQData(value.Clone());
            }
        }

        /// <summary>
        /// Ресурсный объект настройки состояний границы
        /// </summary>
        private readonly BrushSettingQ _QBorderBrush = new();
        /// <summary>
        /// Объект настройки состояний границы
        /// </summary>
        public BrushSettingQ QBorderBrush
        {
            get => _QBorderBrush;
            set
            {
                _QBorderBrush.SetQData(value.Clone());
            }
        }

        /// <summary>
        /// Ресурсный объект настройки состояний текста
        /// </summary>
        private readonly BrushSettingQ _QForeground = new();
        /// <summary>
        /// Объект настройки состояний текста
        /// </summary>
        public BrushSettingQ QForeground
        {
            get => _QForeground;
            set
            {
                _QForeground.SetQData(value.Clone());
            }
        }
        #endregion

        /// <summary>
        /// Активировать визуализацию спектра для всех Q сегментов
        /// </summary>
        /// <param name="Spectrum">Устанавливаемый спектр</param>
        /// <param name="Animated">Состояние анимирования изменения</param>
        public void SetActiveSpecrum(StateSpectrum Spectrum, bool Animated)
        {
            QBackground.SetActiveSpecrum(Spectrum, Animated);
            QBorderBrush.SetActiveSpecrum(Spectrum, Animated);
            QForeground.SetActiveSpecrum(Spectrum, Animated);
        }
    }
}
