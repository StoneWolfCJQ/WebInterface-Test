using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInterface
{
    class LSMaster : ControllerMaster
    {
        public LSMaster()
        {
            dm = new LSDataManager();
        }
    }
}
