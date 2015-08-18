using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.Helpers
{
    public class AdaptiveTriggerConsts
    {
        public const int PhoneMinimumWindowWidth = 0;
        public int PhoneMinimumWidth { get { return PhoneMinimumWindowWidth; } }

        public const int DesktopMinimumWindowWidth = 720;
        public int DesktopMinimumWidth { get { return DesktopMinimumWindowWidth; } }

        public AdaptiveTriggerConsts()
        {

        }
    }
}
