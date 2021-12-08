using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInterface.QueryUtilities
{
    public static partial class Utilities
    {
        public static string ConvertDateTimeUTCToRoundTripTime(DateTime utctime)
        {
            return utctime.ToString("O");            
        }

        public static DateTime ConvertRoundTripTimeDateTimeUTC(string utctimes)
        {
            return DateTime.ParseExact(utctimes, "O", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string ConvertDateTimeUTCNowToHTTPGMT()
        {
            return DateTime.UtcNow.ToString("O");
        }
    }

    public static partial class Query
    {

    }
}

namespace WebInterface
{    
    public static class ControllerNames
    {
        public const string LS="LS";
        public static bool LSdebug = false;
    }
}
