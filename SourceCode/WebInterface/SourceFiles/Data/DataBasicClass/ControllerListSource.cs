using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInterface
{
    public class ControllerListSource
    {
        public String name { get; set; }
        public String IP { get; set; }
        public List<String> IOList { get; set; }
        public List<int> IOResult { get; set; }
        public ControllerListSource(String _name, String _IP, List<String> _IOLIst = null)
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
