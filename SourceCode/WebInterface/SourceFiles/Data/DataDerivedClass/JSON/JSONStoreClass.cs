using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;

namespace WebInterface
{
    class JSONStoreClass : StoreClass
    {
        public Dictionary<string, DataSet> dataDict;
        public Dictionary<string, string> controllerDict;
        public DataTable QPLCControllerTable { get; set; }
        public DataTable ACSControllerTable { get; set; }
        public DataTable ControllerTypeTable { get; set; }
        public BindingList<ControllerListSource> controllerNameList;
        public DataTable LSControllerTable { get; set; }
    }
}
