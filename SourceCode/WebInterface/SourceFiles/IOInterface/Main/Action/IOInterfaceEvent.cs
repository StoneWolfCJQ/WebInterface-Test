using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebInterface
{
    partial class IOInterface
    {
        private void AddEvent(DataGridView dgv)
        {
            dgv.CurrentCellDirtyStateChanged += Dgv_CurrentCellDirtyStateChanged;
            dgv.CellValueChanged += Dgv_CellValueChanged;
            dgv.DataBindingComplete += Dgv_DataBindingComplete;
            dgv.CellClick += Dgv_CellClick;
        }


        public void InitializeEvents()
        {
            this.FormClosing += IOInterface_FormClosing;
        }

        private void IOInterface_FormClosing(object sender, FormClosingEventArgs e)
        {
            suppressShowingError = true;
            updateError = true;
            ACSUpdater.ForceStopUpdate();
            try
            {
                UpdateUIThread.Abort();
                
            }
            catch
            {

            }
            try
            {
                UpdateErrorThread.Abort();
            }
            catch
            {

            }
            SaveDefaultFile();
        }

        private void childItemSaveIO2File_Click(object sender, EventArgs e)
        {
            SaveDefaultFile();
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            dgv.BeginEdit(false);
            if (e.ColumnIndex == dgv.Columns["CheckStatus"].Index)
            {
                ComboBox comboBox = (ComboBox)dgv.EditingControl;
                comboBox.DroppedDown = true;
            }
        }

        private void toggleListenButton_Click(object sender, EventArgs e)
        {
            ToggleListen(toggleType.NONE);
        }

        private void childItemPasteData_Click(object sender, EventArgs e)
        {
            PasteData();
            SaveDefaultFile();
        }

        private void menuItemAddController_Click(object sender, EventArgs e)
        {
            ControllerConfigWindow cc = new ControllerConfigWindow();
            cc.ShowDialog();
            RetrieveSelectController();
            SaveDefaultFile();
        }

        private void IOInterface_Load(object sender, EventArgs e)
        {

        }

        private void toggleConnectButton_Click(object sender, EventArgs e)
        {
            bool result;
            ToggleConnect(toggleType.NONE, out result);
            SaveDefaultFile();
        }

        private void ControllerDropList_SelectedValueChanged(object sender, EventArgs e)
        {
            RetrieveSelectController();
        }

        private void Dgv_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (!connected)
            {
                MessageBox.Show(this, "未连接！");
            }
            else if ((e.ColumnIndex == dgv.Columns["ONOFF"].Index) && (dgv == outputDGV))
            {
                String cn = controllerDropList.SelectedValue as String;
                String IOName = dgv.Rows[e.RowIndex].Cells["IOName"].Value as String;
                String ONOFF = ((String)dgv.Rows[e.RowIndex].Cells["ONOFF"].Value == "ON" ? "OFF" : "ON");
                ChangeOnOFF(cn, IOName, ONOFF);
            }
        }

        private void Dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            
        }

        private void Dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void Dgv_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (((DataGridView)sender).IsCurrentCellDirty)
            {
                
                if ((((DataGridView)sender).CurrentCell.ColumnIndex) != (((DataGridView)sender).Columns["IOAlias"].Index))
                {
                    ((DataGridView)sender).EndEdit();
                }
                // This fires the cell value changed handler below
                ((DataGridView)sender).CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void deleteRowButton_Click(object sender, EventArgs e)
        {
            DeleteRow();
        }


        private void clearTableButton_Click(object sender, EventArgs e)
        {
            ClearTable();
        }


        private void childItemLimitWizard_Click(object sender, EventArgs e)
        {
            ShowLimitTableWizard();
        }


        private void childItemImportData_Click(object sender, EventArgs e)
        {
            ImportData();
        }

        private void childItemExportData_Click(object sender, EventArgs e)
        {
            ExportData();
        }
    }
}
