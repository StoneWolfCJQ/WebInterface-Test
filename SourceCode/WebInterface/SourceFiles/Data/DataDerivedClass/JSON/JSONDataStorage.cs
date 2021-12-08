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
            string cp = Directory.GetCurrentDirectory();
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

            string s = ReadFile<string>();
            result = jb.FillObjectFromInput<string>(s);

            return result;
        }

        public override T ReadFile<T>()
        {
            string fs = File.ReadAllText(defaultPath);

            return (T)Convert.ChangeType(fs, typeof(string));
        }

        public override void Save2File()
        {
            jb.FillStorer();
            string s = jb.ConvertStorer<string>();
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
