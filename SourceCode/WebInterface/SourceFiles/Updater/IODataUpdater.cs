using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using WebInterface.QueryUtilities;

namespace WebInterface.DataUpdater
{
    public partial class IODataUpdater : BaseUpdater
    {    
        public static void AddChangeEventForIODataCollection()
        {
            if (!IODataCollection.initJsonSent)
            {
                IODataCollection.AddChangeEvent();
                IODataCollection.initJsonSent = true;
            }
        }

        public static void RemoveChangeEventForIODataCollection()
        {
            IODataCollection.RemoveChangeEvent();
            IODataCollection.initJsonSent = false;
        }
    }

    public partial class IODataUpdater : BaseUpdater
    {
       
    }
}
