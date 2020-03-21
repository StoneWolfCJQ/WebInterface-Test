using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Data;
using System.IO;
using System.Windows.Forms;
using WebInterface.DataUpdater;

namespace WebInterface
{
    public class CustomCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            int s = x.Length-y.Length;
            if (s != 0)
            {
                s = s / Math.Abs(s);
            }
            else
            {
                s = String.Compare(x, y);
            }
            return s;
        }
    }

    partial class JSONHandler
    {
        public static String SendInitialJSONFromDataCollection(out DateTime lastModifiedTimeutc)
        {
            Dictionary<String, DataSet> tdd = new Dictionary<string, DataSet>(IODataCollection.dataDict);
            lastModifiedTimeutc = DateTime.Now;
            IODataUpdater.AddChangeEventForIODataCollection();

            String JSON = "";
            JObject o = new JObject(from tt in tdd
                                    where IODataCollection.FindNameListItemByName(tt.Key) != null
                                    let ds = tt.Value.Tables
                                    select new JProperty(tt.Key,
                                    new JObject(from DataTable table in ds
                                                select new JProperty(table.TableName,
                                                new JObject((from DataRow rr in table.Rows
                                                            let rs = rr["IOName"] as string
                                                            where rs != ""
                                                            let rss = GetIOName(tt.Key, rs)
                                                            group rss by rss into rsg
                                                            orderby rsg.Key ascending
                                                            select new JProperty(rsg.Key,
                                                             GenJSONJArrayFromDataCollection(tdd, tt.Key, table.TableName, rsg.Key)
                                                             )).OrderBy(k=>k.Name, new CustomCompare()))))));
            JSON = o.ToString();
            return JSON;
        }

        private static JArray GenJSONJArrayFromDataCollection(Dictionary<String, DataSet> tdd, String cname, String tname, String iname)
        {
            var ja = from d in tdd
                     where d.Key == cname
                     select d.Value.Tables into d1
                     from DataTable table in d1
                     where table.TableName == tname
                     select table.Rows into r
                     from DataRow rs in r
                     let rn = (String)rs["IOName"]
                     where rn != ""
                     let rnn = GetIOName(cname, rn)
                     where rnn == iname
                     let index = GetIndex(cname, rn)
                     let onoff = ((String)rs["ONOFF"]) == "ON" ? 1 : 0
                     let check = (int)rs["CheckStatus"]
                     let alias = rs["IOAlias"] as string
                     orderby index ascending
                     select new { index, onoff, check, alias };

            int jindex = 0, jonoff = 0, jcheck = 0;
            JArray jalias = new JArray();
            foreach (var j in ja)
            {
                jindex += (1 << j.index);
                jonoff += (j.onoff << j.index);
                jcheck += (j.check << (2 * j.index));
                jalias.Add(new JValue(j.alias));
            }

            return new JArray( new JValue(jindex), new JValue(jonoff), new JValue(jcheck), jalias);
        }

        public static String SendChangeJSONFromDataCollection(DateTime lastModifiedSinceutc, out DateTime lastModifiedTimeutc)
        {
            String JSON = "";
            List<IODataChangeContainer> tp;
            lock (IODataCollection.dataUpdateList)
            {
                tp = new List<IODataChangeContainer>(IODataCollection.dataUpdateList);
            }

            lastModifiedTimeutc = DateTime.Now;
            DateTime tempDate = lastModifiedTimeutc;

            JObject o = new JObject(from er in tp
                                    where er.time.Subtract(lastModifiedSinceutc) > IODataCollection.intervalSendNew
                                    group er by er.type into g
                                    select new JProperty(g.Key, new JObject(from gi in g
                                                                            group gi by gi.cname into gs
                                                                            select new JProperty(gs.Key, new JArray(from gv in gs
                                                                                                                    select new JValue(gv.cvalue)))
                                                                            )));
            if (o.Count > 0)
            {
                int a;
            }
            JSON = o.ToString();
            return JSON;
        }

        static int GetIndex(string mergeName, string IOName)
        {
            string cType = mergeName.Split('-')[0];
            switch (cType)
            {
                case "ACS":
                case ControllerNames.LS: return int.Parse(IOName.Split('.')[1]);
                case "QPLC": return int.Parse(IOName.Substring(IOName.Length-1),System.Globalization.NumberStyles.HexNumber);
                default: throw new NotImplementedException();
            }
        }

        static string GetIOName(string mergeName, string IOName)
        {
            string cType = mergeName.Split('-')[0];
            switch (cType)
            {
                case "ACS":
                case ControllerNames.LS: return IOName.Split('.')[0];
                case "QPLC": return $"{IOName.Substring(0, IOName.Length - 1)}0-{IOName.Substring(0, IOName.Length - 1)}F";
                default:throw new NotImplementedException();
            }
        }
    }

    public partial class JSONHandler
    {
        public bool JSONIsInit = false;
        public String InitJSONStr;
        public String lastJSON;
    }
}
