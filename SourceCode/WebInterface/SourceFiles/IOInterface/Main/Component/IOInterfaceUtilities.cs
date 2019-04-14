using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Threading;

namespace WebInterface
{
    partial class IOInterface
    {        
    }

    partial class IOInterface
    {
        SocketListener _socketListener;
        bool listening = false;
        bool connected = false;
        public enum styleEnum
        { Good, Nogood, Error, Problem, None };

        private DataTable testDT;
        private bool init = false;
        private bool updatedIO = false;
        private List<DataGridView> dgvList;
        private DataGridView templateDGV;
        private DataGridViewButtonColumn templateButtonCol;
        private DataGridViewComboBoxColumn templateCBCol;
        private Dictionary<styleEnum, DataGridViewCellStyle> dgvStyleDict;
        private bool tableCheckResult;
        private Dictionary<String, Dictionary<IODataCollection.dataTableType, DataGridViewCell>> errorTableDict;
        private Thread UpdateUIThread=new Thread(()=> { });
    }
}
