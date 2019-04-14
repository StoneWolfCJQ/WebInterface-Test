using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInterface.QueryUtilities
{
    public static partial class ACSQueryUtilities
    {
        public static String GenACSSimQueryStr(List<String> IOList)
        {
            String queryStr;
            List<String> ls;
            String[] la;

            ls = new List<string>(IOList);
            ls.Insert(0, "?");
            la = ls.ToArray();
            queryStr = String.Join(",", la).Remove(1, 1);

            return queryStr;
        }

        public static List<int> ConvertACSTransStr(String transStr, out bool result)
        {
            char splitChar = '\r';
            int[] tr = new int[0];
            transStr = transStr.Trim('\r');
            result = true;
            try
            {
                tr = transStr.Split(splitChar).Select(s => int.Parse(s)).ToArray();
            }
            catch
            {
                result = false;
            }           

            return tr.ToList();
        }
    }

    public static partial class QueryUtilities
    {
    }
}
