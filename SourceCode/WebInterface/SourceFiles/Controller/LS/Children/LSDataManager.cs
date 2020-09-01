using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WebInterface
{
    partial class LSDataManager : DataManager
    {
        public LSDataManager()
        {        
            gds = new JSONDataStorage();//TEMP USE　JSON
        }

        public override bool HasStorage()
        {
            try
            {
                return gds.HasStorage();
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public override void FillData()
        {
            gds.FillData();
        }

        public override void SaveFile()
        {
            gds.Save2File();
        }
    }

    partial class LSDataManager : DataManager
    {
        
    }
}
