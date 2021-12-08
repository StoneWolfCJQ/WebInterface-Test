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
        public  Dictionary<string, DataSet> dataDict;
        public  Dictionary<string, string> controllerDict;
        public  DataTable ACSControllerTable { get; set; }
        public  BindingList<ControllerListSource> controllerNameList;
    }
    class TestAction
    {
        public TestStoreClass jsonStorer;
        //Store Data with json
        public TestAction(string path)
        {
            Save2File(path);
        }

        private void Save2File(string path)
        {
            FillStorer();
            string s = CreateJSON();
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

        private string CreateJSON()
        {
            string s = "";
            s = JsonConvert.SerializeObject(jsonStorer, Formatting.Indented);

            return s;
        }
    }
}
