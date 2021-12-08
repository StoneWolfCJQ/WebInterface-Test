using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInterface
{
    public class ControllerListSource
    {
        public string name { get; set; }
        public string IP { get; set; }
        public List<string> IOList { get; set; }
        public List<int> IOResult { get; set; }
        public ControllerListSource(string _name, string _IP, List<string> _IOLIst = null)
        {
            name = _name;
            IP = _IP;
            IOList = _IOLIst;
            if (IOList != null)
            {
                IOResult = new List<int>(from i in IOList select 0);
            }
        }
    }
}
