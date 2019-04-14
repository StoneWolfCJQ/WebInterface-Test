using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace WebInterface
{
    public partial class IOInterface
    {
        public static void ShowError(String err)
        {
            if (!suppressShowingError)
            {
                MessageBox.Show(err, "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }
    }

    public partial class IOInterface
    {
        public static bool updateError = false;
        public static bool suppressShowingError = false;
    }    
}
