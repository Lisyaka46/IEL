using IEL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEL.Interfaces.Front
{
    public interface IIELControl
    {
        #region Color Setting
        /// <summary>
        /// Объект обычного состояния фона
        /// </summary>
        public BrushSettingQ BackgroundSetting { get; }

        /// <summary>
        /// Объект обычного состояния границы
        /// </summary>
        public BrushSettingQ BorderBrushSetting { get; }

        /// <summary>
        /// Объект обычного состояния текста
        /// </summary>
        public BrushSettingQ ForegroundSetting { get; }
        #endregion

        /// <summary>
        /// Длительность анимации в миллисекундах
        /// </summary>
        public int AnimationMillisecond { get; }
    }
}
