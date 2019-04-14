using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInterface
{
    partial class DataManager
    {
        public virtual bool HasStorage()
        {
            return true;
        }

        public virtual void FillData()
        {

        }

        public virtual void SaveFile()
        {

        }
    }

    partial class DataManager
    {
        public DataStorage gds;
    }
}
