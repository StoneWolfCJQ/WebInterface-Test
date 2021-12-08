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
        public static void AddChangeEvent()
        {
            ClearChangeDict();
            foreach (string s in controllerNameList.Select(cl=>cl.name))
            {
                DataSet ds = dataDict[s];
                foreach (DataTable dt in ds.Tables)
                {
                    dt.ColumnChanged += DataSetTableColumnChanged;
                }
            }
        }

        public static void RemoveChangeEvent()
        {
            foreach (string s in controllerNameList.Select(cl => cl.name))
            {
                DataSet ds = dataDict[s];
                foreach (DataTable dt in ds.Tables)
                {
                    dt.ColumnChanged -= DataSetTableColumnChanged;
                }
            }
        }

        private static void DataSetTableColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            new Thread(() =>
            {
                if (e.Column.ColumnName != "ONOFF")
                {
                    lock (SaveFile)
                    {
                        SaveFile.SaveFile = 1;
                    }
                }
            }).Start();
            
            string IOName = e.Row["IOName"] as string;
            if (IOName != "")
            {
                DataTable dt = sender as DataTable;
                string cn = dt.DataSet.DataSetName;
                string cln = e.Column.ColumnName;
                string content = "";
                if ("CheckStatus" == cln)
                {
                    checkStatusType ct = (checkStatusType)e.Row[e.Column.ColumnName];
                    content = Enum.GetName(typeof(checkStatusType), ct);
                }
                else
                {
                    content = e.Row[e.Column.ColumnName] as string;
                }
                DateTime now = DateTime.Now;
                IODataChangeContainer id = new IODataChangeContainer(now, cn, cln, IOName, content);
                AddChangeDict(id);
            }
        }

        private static void AddChangeDict(IODataChangeContainer id)
        {
            RemoveDuplication(id);
            lock(dataUpdateList)
            {
                dataUpdateList.Add(id);
            }
        }

        private static void RemoveDuplication(IODataChangeContainer id)
        {
            List<IODataChangeContainer> tp;
            lock (dataUpdateList)
            {
                tp = new List<IODataChangeContainer>(dataUpdateList);
            }
            var d = from dd in tp
                    where (dd.cname == id.cname) && (dd.type == id.type) && (dd.iname == id.iname)
                    select dd.time;

            lock (dataUpdateList)
            {
                foreach (DateTime dt in d)
                {
                    dataUpdateList.Remove(dataUpdateList.Find(o => o.time == dt));
                }
            }
        }

        public static void RemoveOldFromChangeDict()
        {
            List<IODataChangeContainer> tp;
            lock (dataUpdateList)
            {
               tp = new List<IODataChangeContainer>(dataUpdateList);
            }
            
            var od = from d in tp
                     where DateTime.Now.Subtract(d.time) > intervalRemoveOld
                     select d.time;

            lock (dataUpdateList)
            {
                foreach (DateTime dt in od)
                {
                    dataUpdateList.Remove(dataUpdateList.Find(o => o.time == dt));
                }
            }
        }

        public static void ClearChangeDict()
        {
            dataUpdateList.Clear();
        }

    }

    static partial class IODataCollection
    {
        public static List<IODataChangeContainer> dataUpdateList = new List<IODataChangeContainer>();
        private static TimeSpan intervalRemoveOld = new TimeSpan(0, 0, 0, 10);
        public static TimeSpan intervalSendNew = new TimeSpan(0, 0, 0, 0, -500);
        public static bool initJsonSent = false;
        public static SaveFileFlag SaveFile=new SaveFileFlag(0);
    }

    public class SaveFileFlag
    {
        public SaveFileFlag(int i)
        {
            SaveFile = i;
        }

        public int SaveFile;
    }

    public class IODataChangeContainer
    {
        public DateTime time { get; set; }
        public string cname { get; set; }
        public string type { get; set; }
        public string iname { get; set; }
        public string value { get; set; }
        public string cvalue { get; set; }

        public IODataChangeContainer(DateTime _time, string _cname, string _type, string _iname, string _value)
        {
            time = _time;
            cname = _cname;
            type = _type;
            iname = _iname;
            value = _value;
            cvalue = iname + '=' + value;
        }


    }
}
