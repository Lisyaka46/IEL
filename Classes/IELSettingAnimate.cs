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
        public BrushSettingDNSU BackgroundDNSU { get; set; }

        /// <summary>
        /// Объект управления цветом границ
        /// </summary>
        public BrushSettingDNSU BorderBrushDNSU { get; set; }

        /// <summary>
        /// Объект управления цветом текста
        /// </summary>
        public BrushSettingDNSU ForegroundDNSU { get; set; }

        public IELSettingAnimate()
        {
            TimeSpan MainDuration = TimeSpan.FromMilliseconds(80d);
            BackgroundDNSU = new(MainDuration);
            BorderBrushDNSU = new(MainDuration);
            ForegroundDNSU = new(MainDuration);
        }

        public IELSettingAnimate(TimeSpan BackgroundDuration, TimeSpan BorderBrushDuration, TimeSpan ForegroundDuration)
        {
            BackgroundDNSU = new(BackgroundDuration);
            BorderBrushDNSU = new(BorderBrushDuration);
            ForegroundDNSU = new(ForegroundDuration);
        }

        public IELSettingAnimate(BrushSettingDNSU BG, BrushSettingDNSU BR, BrushSettingDNSU FG)
        {
            BackgroundDNSU = BG;
            BorderBrushDNSU = BR;
            ForegroundDNSU = FG;
        }
    }
}
