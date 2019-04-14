using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WebInterface
{
    //Base class for data storage management
    partial class DataStorage
    {
        public DataStorage()
        {
            
        }

        public virtual bool HasStorage()
        {
            return true;
        }

        public virtual bool ReadStorageOK()
        {
            return true;
        }

        public virtual T ReadFile<T>()
        {
            return (T)new object();
        }

        public virtual void Save2File()
        {

        }

        public virtual void FillData()
        {

        }
    }

    partial class DataStorage
    {
        public DataBase db;
        public JSONBase jb;

        public String defaultFileName;
        public String defaultDir;
        public String defaultPath;
    }
}
