using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInterface
{
    public static partial class ACSBaseConfig
    {
        public static DefaultACSChoice dac = DefaultACSChoice.Simulator;
        public static String sim = "Simulator";
    }

    public static partial class ACSBaseConfig
    {
        public enum DefaultACSChoice
        { Simulator, Default, Custom };
    }
}
