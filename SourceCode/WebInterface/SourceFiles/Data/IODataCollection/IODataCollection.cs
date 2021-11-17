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
        static IODataCollection()
        {
            dataDict = new Dictionary<String, DataSet>();
            controllerDict = new Dictionary<String, String>();
            controllerNameList = new BindingList<ControllerListSource>();
            controllerNameList.AllowEdit = true;
            controllerNameList.AllowRemove = true;
            InitializeOthers();
            InitializeTemplateEmptyTable();
            InitializeControllerTable();
            InitializeControllerTypeTable();
        }

        private static void InitializeOthers()
        {
            var d = from tt in Enum.GetNames(typeof(colType))
                    select new { type = (colType)Enum.Parse(typeof(colType), tt), tt};
            colTypeDict = new Dictionary<colType, String>();
            colTypeDict = d.ToDictionary(t => t.type, t => t.tt);
        }

        private static void InitializeControllerTable()
        {
            //ACS
            ACSControllerTable = new DataTable();
            DataColumn aupdate = new DataColumn("Update", typeof(bool));
            DataColumn cname = new DataColumn("Controller Name", typeof(String));
            DataColumn cAddress = new DataColumn("Controller IP", typeof(String));
            ACSControllerTable.Columns.AddRange(new[] { aupdate, cname, cAddress });
            ACSControllerTable.TableName = "ACS";
            //QPLC
            QPLCControllerTable = new DataTable();
            aupdate = new DataColumn("Update", typeof(bool));
            cname = new DataColumn("Controller Name", typeof(String));
            cAddress = new DataColumn("Station Number", typeof(string));
            QPLCControllerTable.Columns.AddRange(new[] { aupdate, cname, cAddress });
            QPLCControllerTable.TableName = "QPLC";
            //LS
            LSControllerTable = new DataTable();
            aupdate = new DataColumn("Update", typeof(bool));
            cname = new DataColumn("Controller Name", typeof(String));
            cAddress = new DataColumn("Card Number", typeof(string));
            LSControllerTable.Columns.AddRange(new[] { aupdate, cname, cAddress });
            LSControllerTable.TableName = "LS";
            //DataSet
            ControllerDataTableList = new List<DataTable>();
            ControllerDataTableList.Add(ACSControllerTable);
            ControllerDataTableList.Add(QPLCControllerTable);
            ControllerDataTableList.Add(LSControllerTable);
        }

        
        private static void InitializeTemplateEmptyTable()
        {
            DataColumn nd = new DataColumn(colTypeDict[colType.IOName], typeof(String));
            DataColumn na = new DataColumn(colTypeDict[colType.IOAlias], typeof(String));
            DataColumn of = new DataColumn(colTypeDict[colType.ONOFF], typeof(String));
            DataColumn cl = new DataColumn(colTypeDict[colType.CheckStatus], typeof(checkStatusType));
            templateEmptyTable.Columns.AddRange(new DataColumn[] { nd, na, of, cl });

            for (int i = 0; i < 20; i++)
            {
                templateEmptyTable.Rows.Add("", "", "OFF", checkStatusType.Uncheck);
            }
        }

        static void InitializeControllerTypeTable()
        {
            ControllerTypeTable = new DataTable();
            DataColumn aupdate = new DataColumn("Update", typeof(bool));
            DataColumn cType = new DataColumn("Controller Type", typeof(String));
            ControllerTypeTable.Columns.AddRange(new[] { aupdate, cType });
            ControllerTypeTable.Rows.Add(true, "ACS");
            ControllerTypeTable.Rows.Add(false, "QPLC");
            ControllerTypeTable.Rows.Add(false, "LS");
        }
        
    }

    static partial class IODataCollection
    {
        public static Dictionary<String,DataSet> dataDict;
        public static Dictionary<String, String> controllerDict;
        public static DataTable ACSControllerTable { get;set; }
        public static DataTable QPLCControllerTable { get; set; }
        public static DataTable LSControllerTable { get; set; }
        public static List<DataTable> ControllerDataTableList { get; set; }
        public static DataTable ControllerTypeTable { get; set; }
        public static DataTable templateEmptyTable=new DataTable();
        public static BindingList<ControllerListSource> controllerNameList;//for drop list binding
        public enum dataTableType { Input, Output, Limit };
        public enum checkStatusType { Uncheck, Checked, Error, Problem };
        public enum colType { IOName, IOAlias, ONOFF, CheckStatus};
        public static Dictionary<colType, String> colTypeDict;
        public static DataTable errorTable;
    }
}
