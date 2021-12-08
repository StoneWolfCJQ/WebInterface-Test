using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data;

namespace WebInterface
{
    partial class JSONBase : DataBase
    {
        public JSONBase()
        {
            
        }

        public override T ConvertStorer<T>()
        {
            return (T)Convert.ChangeType(JsonConvert.SerializeObject(jsonStorer, Formatting.Indented), typeof(T));
        }

        public override void FillStorer()
        {
            jsonStorer.ACSControllerTable = IODataCollection.ACSControllerTable;
            jsonStorer.controllerDict = IODataCollection.controllerDict;
            jsonStorer.QPLCControllerTable = IODataCollection.QPLCControllerTable;
            jsonStorer.LSControllerTable = IODataCollection.LSControllerTable;
            jsonStorer.controllerNameList = IODataCollection.controllerNameList;
            jsonStorer.dataDict = IODataCollection.dataDict;
            jsonStorer.ControllerTypeTable = IODataCollection.ControllerTypeTable;
        }

        public override bool FillObjectFromInput<T>(T io)
        {
            string s = (string)Convert.ChangeType(io, typeof(string));
            jsonStorer = JsonConvert.DeserializeObject<JSONStoreClass>(s);
            
            ConvertCheckStatusType();
            return true;
        }

        public override void FillData()
        {
            IODataCollection.LSControllerTable = jsonStorer.LSControllerTable;
            IODataCollection.ACSControllerTable = jsonStorer.ACSControllerTable;
            IODataCollection.QPLCControllerTable = jsonStorer.QPLCControllerTable;
            IODataCollection.controllerDict = jsonStorer.controllerDict;
            IODataCollection.controllerNameList = jsonStorer.controllerNameList;
            IODataCollection.dataDict = jsonStorer.dataDict;
            IODataCollection.ControllerTypeTable = jsonStorer.ControllerTypeTable;
        }

        public void ConvertCheckStatusType()
        {            
            foreach (KeyValuePair<string, DataSet> o in jsonStorer.dataDict)
            {
                DataSet ds = o.Value;
                ds.DataSetName = o.Key;
                List<DataTable> adts = new List<DataTable>();
                List<DataTable> rdts = new List<DataTable>();
                foreach (DataTable dt in ds.Tables)
                {
                    DataTable dtc = dt.Clone();
                    dtc.Columns["CheckStatus"].DataType = typeof(IODataCollection.checkStatusType);
                    foreach (DataRow row in dt.Rows)
                    {
                        row["ONOFF"] = "OFF";
                        dtc.ImportRow(row);
                    }
                    adts.Add(dtc.Copy());
                    rdts.Add(dt);
                }
                int i =0, l= rdts.Count;
                while (i < l)
                {
                    
                    ds.Tables.Remove(rdts[i]);
                    ds.Tables.Add(adts[i]);
                    i++;
                }
            }
        }
    }

    partial class JSONBase:DataBase
    {
        private JSONStoreClass jsonStorer = new JSONStoreClass();
    }
}
