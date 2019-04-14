using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebInterface
{
    public partial class LimitTableWizard : Form
    {
        public ComboBox sourceCB;
        string controllerName;
        string preFix = "Axis";
        char sChar = '-';
        string[] LimitStr = { "负限位", "正限位" };
        Dictionary<int, string> axisDesp = new Dictionary<int, string>();

        public LimitTableWizard(ComboBox cb)
        {
            InitializeComponent();
            sourceCB=cb;
            InitializeCustomComponent();
        }

        ~LimitTableWizard()
        {
            Dispose(true);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        void InitializeCustomComponent()
        {
            sourceCB.SelectedValueChanged += SourceCB_SelectedValueChanged;
            descriptionTextBox.TextChanged += DescriptionTextBox_TextChanged;
            scriptTextBox.TextChanged += ScriptTextBox_TextChanged;
            GetCBString();
            EnableButton();
            GetTable();
            limitDGV.Columns["IOName"].ReadOnly = true;
            limitDGV.Columns["ONOFF"].ReadOnly = true;
            limitDGV.Columns["CheckStatus"].ReadOnly = true;
            copyScriptButton.Enabled = false;
            limitDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        void EnableButton()
        {            
            if (CBIsNull())
            {
                if (tableAddButton.InvokeRequired)
                {
                    Invoke(new Action(()=> { tableAddButton.Enabled = false; }));
                }
                else
                {
                    tableAddButton.Enabled = false;
                }
            }
            else
            {
                if (tableAddButton.InvokeRequired)
                {
                    Invoke(new Action(() => { tableAddButton.Enabled = true; }));
                }
                else
                {
                    tableAddButton.Enabled = true;
                }
            }
        }

        void GetCBString()
        {
            controllerName = sourceCB.SelectedValue as string;
        }

        bool CBIsNull()
        {
            return String.IsNullOrEmpty(controllerName);
        }

        void GetTable()
        {
            if (CBIsNull())
            {
                return;
            }
            DataTable table = IODataCollection.dataDict[controllerName]
                .Tables[IODataCollection.dataTableType.Limit.ToString()].Copy();
            table.AcceptChanges();
            limitDGV.DataSource = table;
        }
    }
}
