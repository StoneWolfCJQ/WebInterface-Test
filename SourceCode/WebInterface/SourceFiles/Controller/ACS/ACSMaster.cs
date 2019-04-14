using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInterface
{
    class ACSMaster : ControllerMaster
    {
        public ACSMaster()
        {
            dm = new ACSDataManager();
        }
    }
}
