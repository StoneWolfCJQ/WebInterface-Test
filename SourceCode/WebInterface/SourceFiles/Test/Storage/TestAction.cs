using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using System.ComponentModel;

namespace WebInterface
{
    class TestStoreClass
    {
        public  Dictionary<String, DataSet> dataDict;
        public  Dictionary<String, String> controllerDict;
        public  DataTable ACSControllerTable { get; set; }
        public  BindingList<ControllerListSource> controllerNameList;
    }
    class TestAction
    {
        public TestStoreClass jsonStorer;
        //Store Data with json
        public TestAction(String path)
        {
            Save2File(path);
        }

        private void Save2File(String path)
        {
            FillStorer();
            String s = CreateJSON();
            File.WriteAllText(path, s);
        }

        private void FillStorer()
        {
            jsonStorer = new TestStoreClass();
            jsonStorer.controllerDict = IODataCollection.controllerDict;
            jsonStorer.controllerNameList = IODataCollection.controllerNameList;
            jsonStorer.dataDict = IODataCollection.dataDict;
            jsonStorer.ACSControllerTable = IODataCollection.ACSControllerTable;
        }

        private String CreateJSON()
        {
            String s = "";
            s = JsonConvert.SerializeObject(jsonStorer, Formatting.Indented);

            return s;
        }
    }
}
