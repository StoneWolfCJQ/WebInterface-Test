using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInterface.QueryUtilities
{
    public static partial class Utilities
    {
        public static String ConvertDateTimeUTCToRoundTripTime(DateTime utctime)
        {
            return utctime.ToString("O");            
        }

        public static DateTime ConvertRoundTripTimeDateTimeUTC(String utctimes)
        {
            return DateTime.ParseExact(utctimes, "O", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static String ConvertDateTimeUTCNowToHTTPGMT()
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
        public const string LS="雷赛";
    }
}
