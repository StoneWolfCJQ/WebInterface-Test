using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInterface
{
    partial class DataBase
    {
        public DataBase()
        {
        }

        virtual public T ConvertStorer<T>()
        {
            return (T)new object();
        }

        virtual public void FillStorer()
        {

        }

        virtual public bool FillObjectFromInput<T>(T io)
        {
            return true;
        }

        virtual public void FillData()
        {

        }
    }

    partial class DataBase
    {

    }
}
