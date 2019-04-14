using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Threading;

namespace WebInterface
{    
    static partial class IODataCollection
    {
        public static void ChangeCheckStatus(String cName, String IOName, String checkStatus)
        {
            DataRow row = (from DataTable t in dataDict[cName].Tables
                          from DataRow rs in t.Rows
                          where (String)rs["IOName"] == IOName
                          select rs).First();

            row["CheckStatus"] = (IODataCollection.checkStatusType)Enum.Parse(typeof(IODataCollection.checkStatusType), checkStatus);
        }

        public static string MergeTypeNameAndControllerName(string type, string name)
        {
            return type + "-" + name;
        }

        public static string GetControllerName(string mergeName)
        {
            return mergeName.Split('-')[1];
        }
    }

    static partial class IODataCollection
    {
       
    }
}
