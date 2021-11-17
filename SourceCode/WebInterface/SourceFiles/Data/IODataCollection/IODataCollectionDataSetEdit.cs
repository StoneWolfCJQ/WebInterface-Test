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
        public static void AddController(string controllerType, String controllerName, String controllerAddress, bool bindingAdd = false)
        {
            string mergeName = MergeTypeNameAndControllerName(controllerType, controllerName);
            controllerDict.Add(mergeName, controllerAddress);
            //Add new dataset to its data dict and initialize three tables
            DataSet ds = new DataSet(mergeName);
            dataDict.Add(mergeName, ds);
            InitialControllerDataSet(mergeName);
            if (bindingAdd)
            {
                ControllerListSource ncs = new ControllerListSource(mergeName, controllerAddress);
                controllerNameList.Add(ncs);
            }
        }

        public static void AddControllerTable(string cType, bool check, String controllerName, String controllerAddress)
        {
            ControllerDataTableList.Find(t=>t.TableName==cType).Rows.Add(check, controllerName, controllerAddress);
        }

        public static void InitialControllerDataSet(String controllerName)
        {
            foreach (dataTableType dtt in Enum.GetValues(typeof(dataTableType)))
            {
                AddTable(dtt, controllerName, templateEmptyTable);
            }
        }

        public static void UpdateControllerName(string cType, String oldKey, String newKey, bool updateBind = false)
        {
            string newMergeName = MergeTypeNameAndControllerName(cType, newKey);
            string oldMergeName = MergeTypeNameAndControllerName(cType, oldKey);
            controllerDict.Add(newMergeName, controllerDict[oldMergeName]);
            controllerDict.Remove(oldMergeName);
            dataDict.Add(newMergeName, dataDict[oldMergeName]);
            dataDict.Remove(oldMergeName);
            dataDict[newMergeName].DataSetName = newMergeName;
            if (updateBind)
            {
                ControllerListSource cls = FindNameListItemByName(oldMergeName);
                int index = controllerNameList.IndexOf(cls);
                ControllerListSource ncl = new ControllerListSource(newMergeName, cls.IP);
                controllerNameList.Insert(index, ncl);
                controllerNameList.Remove(cls);
            }
        }

        public static void UpdateControllerAddress(string cType, String newIP, String controllerName)
        {
            string oldMergeName = MergeTypeNameAndControllerName(cType, controllerName);
            controllerDict[oldMergeName] = newIP;
            ControllerListSource cls = FindNameListItemByName(oldMergeName);
            if (cls != null)
            {
                cls.IP = newIP;
            }
        }

        public static void AddTable(dataTableType DTType, String controllerName, DataTable table)
        {
            DataTable copyTable = table.Copy();
            if (dataDict[controllerName].Tables.Contains(DTType.ToString()))
            {
                DataTable sourceTable = dataDict[controllerName].Tables[DTType.ToString()];
                sourceTable.Clear();
                sourceTable.Merge(copyTable);
                sourceTable.AcceptChanges();
            }
            else
            {
                dataDict[controllerName].Tables.Add(new DataTable(DTType.ToString()));
                dataDict[controllerName].Tables[DTType.ToString()].Merge(copyTable);
            }
        }

        public static void AppendToTable(dataTableType DTType, String controllerName, DataTable table)
        {
            DataTable copyTable = table.Copy();
            DataTable sourceTable = dataDict[controllerName].Tables[DTType.ToString()];
            sourceTable.Merge(copyTable);
        }

        public static void ClearEmptyRowFromTable(DataTable table)
        {
            string name, alias;
            List<DataRow> rr = new List<DataRow>();
            foreach (DataRow row in table.Rows)
            {
                name = row["IOName"] as string;
                alias = row["IOAlias"] as string;
                if (String.IsNullOrEmpty(name + alias) || String.IsNullOrWhiteSpace(name + alias))
                {
                    rr.Add(row);
                }
            }

            for(int i = 0; i < rr.Count; i++)
            {
                table.Rows.Remove(rr[i]);
            }
        }

        public static void RemoveController(string cType, String controllerName, bool removeBind = false)
        {
            string oldMergeName = MergeTypeNameAndControllerName(cType, controllerName);
            controllerDict.Remove(oldMergeName);
            dataDict.Remove(oldMergeName);
            if (removeBind)
            {
                ControllerListSource cls = FindNameListItemByName(oldMergeName);
                controllerNameList.Remove(cls);
            }
            /*ACSControllerTable.Rows.Remove((from DataRow r in ACSControllerTable.Rows
                                        where (String)r["Controller Name"] == controllerName
                                        select r).First());
            ACSControllerTable.Rows.Add(false, "", "");*/
        }

        public static DataTable CreateTableFromClipboardText(String clipBoardText, out bool result)
        {
            DataTable table;
            result = true;
            table = templateEmptyTable.Copy();
            table.Clear();

            try
            {
                String[][] textLines = clipBoardText.Split(new string[] { "\r\n" }, StringSplitOptions.None)
                .Select(s => s.Split('\t')).ToArray();

                String[] cstr = Enum.GetNames(typeof(checkStatusType));

                foreach (String[] textStr in textLines)
                {
                    if ((textStr.Count() > 1))
                    {
                        table.Rows.Add(textStr[0], textStr[1], "OFF", checkStatusType.Uncheck);
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
            }

            return table;
        }

        public static bool ValidStr(string s)
        {
            return !(String.IsNullOrWhiteSpace(s) || String.IsNullOrEmpty(s));
        }


        public static void CreateTableFromClipboardHTML(String clipBoardHTML)
        {

        }

        public static ControllerListSource FindNameListItemByName(String controllerName)
        {
            var s = from c in controllerNameList
                    where c.name == controllerName
                    select c;

            if (s.Count() == 0)
            {
                return null;
            }
            return s.First();
        }

        public static void RefreshErrorTable()
        {

        }

        public static void RebuildBindList(Dictionary<String, String> d)
        {
            controllerNameList.Clear();
            List<ControllerListSource> c = (from dp in d
                                           select new ControllerListSource(dp.Key, dp.Value)).ToList();
            foreach (ControllerListSource cs in c)
            {
                controllerNameList.Add(cs);
            }
        }

        public static void FillEmptyTable(string cname, dataTableType dtt)
        {
            AddTable(dtt, cname, templateEmptyTable);
        }

        public static void RebuildControllerTableList()
        {
            ACSControllerTable.TableName = "ACS";
            QPLCControllerTable.TableName = "QPLC";
            LSControllerTable.TableName = "LS";
            ControllerDataTableList.Clear();
            ControllerDataTableList.Add(ACSControllerTable);
            ControllerDataTableList.Add(QPLCControllerTable);
            ControllerDataTableList.Add(LSControllerTable);
        }
    }
}
