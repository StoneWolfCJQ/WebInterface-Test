using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInterface.QueryUtilities
{
    public static partial class ACSQueryUtilities
    {
        public static string GenACSSimQueryStr(List<string> IOList)
        {
            string queryStr;
            List<string> ls;
            string[] la;

            ls = new List<string>(IOList);
            ls.Insert(0, "?");
            la = ls.ToArray();
            queryStr = string.Join(",", la).Remove(1, 1);

            return queryStr;
        }

        public static List<int> ConvertACSTransStr(string transStr, out bool result)
        {
            char splitChar = '\r';
            int[] tr = new int[0];
            transStr = transStr.Trim('\r');
            result = true;
            try
            {
                tr = transStr.Split(splitChar).Select(s => ACSTransParse(s)).ToArray();
            }
            catch
            {
                result = false;
            }           

            return tr.ToList();
        }

        static int ACSTransParse(string s)
        {
            if (s.Contains(","))
            {
                return Convert.ToInt32(s.Replace(",", "").Trim(' '),2);
            }
            else
            {
                return int.Parse(s);
            }
        }
    }

    public static partial class QueryUtilities
    {
    }
}
