using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInterface.QueryUtilities
{
    public static partial class Utilities
    {
        public static String ConvertDateTimeUTCToHTTPGMT(DateTime utctime)
        {
            return utctime.ToString("R");            
        }

        public static DateTime ConvertHTTPGMTTODateTimeUTC(String utctimes)
        {
            return DateTime.ParseExact(utctimes, "R", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static String ConvertDateTimeUTCNowToHTTPGMT()
        {
            return DateTime.UtcNow.ToString("R");
        }

        public static String ConvertDateTimeMinUTCToHTTP()
        {
            return DateTime.MinValue.ToString("R");
        }
    }

    public static partial class Query
    {

    }
}
