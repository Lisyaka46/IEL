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
        public BrushSettingDNSU BackgroundDNSU { get; internal set; }

        /// <summary>
        /// Объект управления цветом границ
        /// </summary>
        public BrushSettingDNSU BorderBrushDNSU { get; internal set; }

        /// <summary>
        /// Объект управления цветом текста
        /// </summary>
        public BrushSettingDNSU ForegroundDNSU { get; internal set; }


        internal IELSettingAnimate()
        {
            BackgroundDNSU = new();
            BorderBrushDNSU = new();
            ForegroundDNSU = new();
        }

        internal IELSettingAnimate(BrushSettingDNSU BG, BrushSettingDNSU BR, BrushSettingDNSU FG)
        {
            BackgroundDNSU = BG;
            BorderBrushDNSU = BR;
            ForegroundDNSU = FG;
        }
    }
}
