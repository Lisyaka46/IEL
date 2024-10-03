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
        public BrushSettingDNSU? BackgroundDNSU { get; set; }

        /// <summary>
        /// Объект управления цветом границ
        /// </summary>
        public BrushSettingDNSU? BorderBrushDNSU { get; set; }

        /// <summary>
        /// Объект управления цветом текста
        /// </summary>
        public BrushSettingDNSU? ForegroundDNSU { get; set; }

        public IELSettingAnimate(BrushSettingDNSU? BG = null, BrushSettingDNSU? BR = null, BrushSettingDNSU? FG = null)
        {
            BackgroundDNSU = BG;
            BorderBrushDNSU = BR;
            ForegroundDNSU = FG;
        }
    }
}
