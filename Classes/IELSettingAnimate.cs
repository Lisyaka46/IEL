using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEL.Classes
{
    public class IELSettingAnimate
    {

        /// <summary>
        /// Объект управления цветом фона
        /// </summary>
        public BrushSettingSUN BackgroundSUN { get; set; }

        /// <summary>
        /// Объект управления цветом границ
        /// </summary>
        public BrushSettingSUN BorderBrushSUN { get; set; }

        /// <summary>
        /// Объект управления цветом текста
        /// </summary>
        public BrushSettingSUN ForegroundSUN { get; set; }


        public IELSettingAnimate()
        {
            BackgroundSUN = new();
            BorderBrushSUN = new();
            ForegroundSUN = new();
        }

        public IELSettingAnimate(BrushSettingSUN Background, BrushSettingSUN BorderBrush, BrushSettingSUN Foreground)
        {
            BackgroundSUN = Background;
            BorderBrushSUN = BorderBrush;
            ForegroundSUN = Foreground;
        }
    }
}
