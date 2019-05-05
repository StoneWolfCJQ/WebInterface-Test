using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WebInterface
{
    //Base class for data storage management
    partial class JSONDataStorage : DataStorage
    {
        public JSONDataStorage()
        {
            jb = new JSONBase();
            defaultFileName = "ACSJson.json";
            String cp = Directory.GetCurrentDirectory();
            defaultDir = cp + @"\json\";
            defaultPath = defaultDir + defaultFileName;
        }

        public override bool HasStorage()
        {
            bool result = false;
            if (File.Exists(defaultPath) && ReadStorageOK())
            {
                result = true;
            }
            return result;
        }

        public override bool ReadStorageOK()
        {
            bool result;

            String s = ReadFile<String>();
            result = jb.FillObjectFromInput<String>(s);

            return result;
        }

        public override T ReadFile<T>()
        {
            String fs = File.ReadAllText(defaultPath);

            return (T)Convert.ChangeType(fs, typeof(String));
        }

        public override void Save2File()
        {
            jb.FillStorer();
            String s = jb.ConvertStorer<String>();
            if (!Directory.Exists(defaultDir))
            {
                Directory.CreateDirectory(defaultDir);
            }
            File.WriteAllText(defaultPath, s);
        }

        public override void FillData()
        {
            jb.FillData();
        }
    }

    partial class JSONDataStorage : DataStorage
    {

    }
}
