using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WebInterface
{
    public partial class ControllerConfigWindow : Form
    {
        public ControllerConfigWindow()
        {
            InitializeComponent();
            InitializeControllerType();
            InitializeControllerList();            
            this.FormClosing += ControllerConfigWindow_FormClosing;
            this.Shown += ControllerConfigWindow_Shown;
        }

        private void ControllerConfigWindow_Shown(object sender, EventArgs e)
        {
            SetReadOnly();
        }

        private void ControllerConfigWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            controllerDGV.EndEdit();
            Thread.Sleep(1);
            if (!CheckIllegalCellBeforeSwitch())
            {
                e.Cancel = true;
            }
            RebuildBindingList();
        }
        
        private void InitializeControllerList()
        {
            GetControllerTable(selectControllerTypeRow);
            controllerDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            controllerDGV.Columns["Update"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            controllerDGV.CellBeginEdit += ControllerDGV_CellBeginEdit;
            controllerDGV.CellValueChanged += ControllerDGV_CellValueChanged;
            controllerDGV.Sorted += ControllerDGV_Sorted;
            controllerDGV.CellClick += ControllerDGV_CellClick;
            foreach (DataGridViewColumn col in controllerDGV.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void InitializeControllerType()
        {
            ControllerTypeDGV.DataSource = IODataCollection.ControllerTypeTable;
            ControllerTypeDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ControllerTypeDGV.Rows[selectControllerTypeRow].Selected = true;
            //ControllerTypeDGV.Columns["Update"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            ControllerTypeDGV.CellValueChanged += ControllerTypeDGV_CellValueChanged;
            ControllerTypeDGV.SelectionChanged += ControllerTypeDGV_SelectionChanged;
            ControllerTypeDGV.Columns["Controller Type"].ReadOnly = true;            
            foreach (DataGridViewColumn col in ControllerTypeDGV.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            controllerCheckType = new Dictionary<string, CheckType>
            {
                { "ACS", CheckType.IP },
                { "QPLC", CheckType.Number},
                {ControllerNames.LS,CheckType.Number }
            };
        }

        private void ControllerTypeDGV_SelectionChanged(object sender, EventArgs e)
        {
            if (!CheckIllegalCellBeforeSwitch())
            {
                ControllerTypeDGV.SelectionChanged -= ControllerTypeDGV_SelectionChanged;
                ControllerTypeDGV.Rows[selectControllerTypeRow].Selected = true;
                ControllerTypeDGV.SelectionChanged += ControllerTypeDGV_SelectionChanged;
                return;                
            }
            else
            {
                illegalColumnIndex = -1;
                illegalRowIndex = -1;
            }
            int i;
            try
            {
                 i = ControllerTypeDGV.Rows.IndexOf(ControllerTypeDGV.SelectedRows[0]);
            }
            catch
            {
                i = selectControllerTypeRow;
            }
            UncheckBadRow();
            selectControllerTypeRow = i;
            GetControllerTable(i);
            SetReadOnly();
        }

        private void ControllerTypeDGV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ControllerDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            dgv.BeginEdit(false);
        }

        private void ControllerDGV_Sorted(object sender, EventArgs e)
        {
            
        }

        private void ControllerDGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (illegalRowIndex >= 0)
            {
                DataGridViewCellStyle st = new DataGridViewCellStyle();
                MessageBox.Show("请修正错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                st.BackColor = System.Drawing.Color.OrangeRed;
                controllerDGV.Rows[illegalRowIndex].Cells[illegalColumnIndex].Style = st;                
            }
            controllerDGV.CellEndEdit -= ControllerDGV_CellEndEdit;
        }

        private void ControllerDGV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex != controllerDGV.Columns["Update"].Index) && (e.RowIndex >= 0)) 
            {
                controllerDGV.CellEndEdit -= ControllerDGV_CellEndEdit;
                CheckRowValueLegal(e.RowIndex, e.ColumnIndex);
            }
        }

        private void ControllerDGV_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if ((!IsCellReadOnly(e.RowIndex, e.ColumnIndex)) && (e.RowIndex >= 0)) 
            {
                bool result;
                CheckIllegalKeyCorrect(e.RowIndex, e.ColumnIndex, out result);
                if (!result)
                {
                    e.Cancel = true;
                    return;
                }
                if (e.ColumnIndex != controllerDGV.Columns["Update"].Index)
                {
                    CellBeginEditHandler(e.RowIndex, e.ColumnIndex);
                }
            }
            if ((illegalRowIndex >= 0) && (e.RowIndex < 0))
            {
                e.Cancel = true;
            }
        }
       
        public void CheckIllegalKeyCorrect(int rowIndex, int colIndex, out bool result)
        {
            result = true;
            DataGridViewCellStyle st = new DataGridViewCellStyle();
            if ((rowIndex == illegalRowIndex) && (illegalRowIndex >= 0) 
                && (colIndex == illegalColumnIndex))
            {                
                st.BackColor = System.Drawing.Color.White;
                controllerDGV.Rows[illegalRowIndex].Cells[illegalColumnIndex].Style = st;
            }
            else if (illegalRowIndex>=0)
            {
                MessageBox.Show("请修正错误后再编辑", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                st.BackColor = System.Drawing.Color.OrangeRed;
                result = false;
                controllerDGV.Rows[illegalRowIndex].Cells[illegalColumnIndex].Style = st;
            }           
        }

        private void closeWindowsButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void controllerRemoveButton_Click(object sender, EventArgs e)
        {
            RemoveControllerFromSelect();
        }
    }
}
