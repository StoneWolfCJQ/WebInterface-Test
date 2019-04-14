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
            foreach (String s in controllerNameList.Select(cl=>cl.name))
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
            foreach (String s in controllerNameList.Select(cl => cl.name))
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
            
            String IOName = e.Row["IOName"] as String;
            if (IOName != "")
            {
                DataTable dt = sender as DataTable;
                String cn = dt.DataSet.DataSetName;
                String cln = e.Column.ColumnName;
                String content = "";
                if ("CheckStatus" == cln)
                {
                    checkStatusType ct = (checkStatusType)e.Row[e.Column.ColumnName];
                    content = Enum.GetName(typeof(checkStatusType), ct);
                }
                else
                {
                    content = e.Row[e.Column.ColumnName] as String;
                }
                DateTime now = DateTime.UtcNow;
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
                     where DateTime.UtcNow.Subtract(d.time) > intervalRemoveOld
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
        private static TimeSpan intervalRemoveOld = new TimeSpan(1, 0, 0, 10);
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
        public String cname { get; set; }
        public String type { get; set; }
        public String iname { get; set; }
        public String value { get; set; }
        public String cvalue { get; set; }

        public IODataChangeContainer(DateTime _time, String _cname, String _type, String _iname, String _value)
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
