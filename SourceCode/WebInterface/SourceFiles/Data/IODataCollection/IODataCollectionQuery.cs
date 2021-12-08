using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace WebInterface
{
    static partial class IODataCollection
    {
        public static List<ControllerListSource> queryList = new List<ControllerListSource>();
    }

    static partial class IODataCollection
    {
        #region Unviersal Functions
        
        public static void DuplicateControllerTable(string cType)
        {
            for (int i = 0; i < 18; i++)
            {
                ControllerDataTableList.Find(t => t.TableName == cType).Rows.Add(false, "", "");
            }
        }
        public static void UpdateIOQueryList(string mergeName, List<int> tl)
        {
            ControllerListSource cls = queryList.Find(q => q.name == mergeName);
            if (!cls.IOResult.SequenceEqual(tl))
            {
                cls.IOResult = tl;
            }
        }
        public static void UpdateDataSetIOFromQueryList()
        {
            string IOName;
            int index, IOint, lindex;
            int ONOFF;
            List<int> IOResult;
            foreach (string controllerName in controllerNameList.Select(cl => cl.name))
            {
                foreach (DataTable dt in dataDict[controllerName].Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        IOName = (string)row["IOName"];
                        if (IOName != "")
                        {
                            if (IOName.Contains('.'))
                            {
                                index = int.Parse(IOName.Split('.')[1]);
                            }
                            else
                            {
                                index = 0;
                            }
                            IOName = GetIOGroupString(controllerName.Split('-')[0], dt.TableName, IOName);
                            ONOFF = ((string)row["ONOFF"]) == "ON" ? 1 : 0;
                            IOResult = queryList.Find(q => q.name == controllerName).IOResult;
                            lindex = queryList.Find(q => q.name == controllerName).IOList.FindIndex(s => s == IOName);
                            IOint = IOResult[lindex];
                            if ((ONOFF << index) != (IOint & (1 << index)))
                            {
                                row["ONOFF"] = ((string)row["ONOFF"]) == "ON" ? "OFF" : "ON";
                            }
                        }
                    }
                }
            }
        }
        public static void GenerateQueryList(string cType)
        {  
            queryList.AddRange((from s in dataDict
                         where s.Key.StartsWith(cType + "-")
                         where (controllerDict.ContainsKey(s.Key)) && (FindNameListItemByName(s.Key) != null)
                         let t = s.Value.Tables
                         select new
                         {
                             s.Key,
                             IP = controllerDict[s.Key],
                             ss = (from DataTable tt in t
                                   from DataRow rr in tt.Rows
                                   let rs = (string)rr["IOName"]
                                   where rs != ""
                                   let rss = GetIOGroupString(cType, tt.TableName, rs)
                                   group rss by rss into rsg
                                   select rsg.Key).ToList()
                         } into lg
                         where lg.ss.Count != 0
                         select new ControllerListSource(lg.Key, lg.IP, lg.ss)).ToList());
        }

        static string GetIOGroupString(string cType, string tType, string IOName)
        {
            switch (cType)
            {
                case ControllerNames.LS:return tType + '\\' + IOName.Split('.')[0];
                default:return IOName.Split('.')[0];
            }
        }
        #endregion

        #region ACS

        #endregion
    }
}
