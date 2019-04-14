using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace WebInterface
{
    public partial class IOInterface : Form
    {
        public IOInterface()
        {
            InitializeComponent();
            InitializeCustomComponent();
            InitializeEvents();
        }

        ~IOInterface()
        {
            Dispose();           
        }

        private void inputDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }

    
}
