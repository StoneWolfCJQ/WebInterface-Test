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
        private void InitializeDGVStyle()
        {
            DataGridViewCellStyle s = new DataGridViewCellStyle();
            dgvStyleDict = new Dictionary<styleEnum, DataGridViewCellStyle>();
            s.BackColor = System.Drawing.Color.ForestGreen;
            dgvStyleDict.Add(styleEnum.Good, s.Clone());
            s.BackColor = System.Drawing.Color.OrangeRed;
            dgvStyleDict.Add(styleEnum.Nogood, s.Clone());
            s.BackColor = System.Drawing.Color.Goldenrod;
            dgvStyleDict.Add(styleEnum.Problem, s.Clone());
            s.BackColor = System.Drawing.Color.OrangeRed;
            dgvStyleDict.Add(styleEnum.Error, s.Clone());
            s.BackColor = System.Drawing.SystemColors.Control;
            dgvStyleDict.Add(styleEnum.None, s.Clone());
        }

        private void InitializeAllDGV()
        {
            dgvList = new List<DataGridView> { inputDGV, outputDGV, limitDGV };
            InitializeDGV();
        }

        private void InitializeDGV()
        {
            InitializeButtonColumn();
            InitializeCBColumn();
            foreach (DataGridView dgv in dgvList)
            {
                dgv.DataSource = IODataCollection.templateEmptyTable.Copy();
                dgv.Columns.Remove(dgv.Columns["ONOFF"]);
                dgv.Columns.Remove(dgv.Columns["CheckStatus"]);
                dgv.Columns.AddRange((DataGridViewButtonColumn)templateButtonCol.Clone(),
                    (DataGridViewComboBoxColumn)templateCBCol.Clone());
                dgv.Columns["ONOFF"].DefaultCellStyle = dgvStyleDict[styleEnum.Good];
                dgv.Columns["CheckStatus"].DefaultCellStyle = dgvStyleDict[styleEnum.None];
                dgv.ReadOnly = true;
                dgv.CellContentDoubleClick += Dgv_CellContentDoubleClick;                
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv.RowHeadersVisible = false;
                dgv.AllowUserToResizeRows = false;
                dgv.AllowUserToOrderColumns = false;
            }
        }

        
        private void InitializeButtonColumn()
        {
            templateButtonCol = new DataGridViewButtonColumn();
            templateButtonCol.FlatStyle = FlatStyle.Flat;
            templateButtonCol.UseColumnTextForButtonValue = false;
            templateButtonCol.HeaderText = "ON/OFF";
            templateButtonCol.Text = "OFF";
            templateButtonCol.Name = "ONOFF";
            templateButtonCol.ValueType = typeof(string);
            templateButtonCol.DataPropertyName = "ONOFF";
        }

        private void InitializeCBColumn()
        {
            var cboSrc = Enum.GetNames(typeof(IODataCollection.checkStatusType)).
                    Select(x => new
                    {
                        Check = x,
                        Index = (int)Enum.Parse(typeof(IODataCollection.checkStatusType), x)
                    }
                           ).ToList();

            templateCBCol = new DataGridViewComboBoxColumn();
            templateCBCol.DataSource = cboSrc;
            templateCBCol.DisplayMember = "Check";
            templateCBCol.ValueMember = "Index";
            templateCBCol.HeaderText = "CheckeStatus";
            templateCBCol.DataPropertyName = "CheckStatus";
            templateCBCol.ValueType = typeof(int);
            templateCBCol.Name = "CheckStatus";
            templateCBCol.FlatStyle = FlatStyle.Flat;
        }

        private void RetrieveSelectController()
        {
            String controllerName = controllerDropList.SelectedValue as String;
            if (String.IsNullOrEmpty(controllerName))
            {
                foreach (DataGridView dgv in dgvList)
                {
                    dgv.DataSource = IODataCollection.templateEmptyTable.Copy();                   
                }
                return;
            }

            if (controllerDropList.Items.Count != 0)
            {
                inputDGV.DataSource = IODataCollection.dataDict[controllerName]
                    .Tables[IODataCollection.dataTableType.Input.ToString()];
                outputDGV.DataSource = IODataCollection.dataDict[controllerName]
                    .Tables[IODataCollection.dataTableType.Output.ToString()];
                limitDGV.DataSource = IODataCollection.dataDict[controllerName]
                    .Tables[IODataCollection.dataTableType.Limit.ToString()];
                foreach (DataGridView dgv in dgvList)
                {
                    dgv.Columns.Remove(dgv.Columns["ONOFF"]);
                    dgv.Columns.Remove(dgv.Columns["CheckStatus"]);
                    dgv.Columns.AddRange((DataGridViewButtonColumn)templateButtonCol.Clone(),
                        (DataGridViewComboBoxColumn)templateCBCol.Clone());
                    dgv.Columns["ONOFF"].DefaultCellStyle = dgvStyleDict[styleEnum.Good];
                    dgv.Columns["CheckStatus"].DefaultCellStyle = dgvStyleDict[styleEnum.None];
                    dgv.ReadOnly = false;
                    dgv.Columns["IOName"].ReadOnly = true;
                    AddEvent(dgv);
                }
            }
        }

        public void ChangeCheckStatus(String cName, String IOName, String checkStatus)
        {
            Invoke(new Action(()=> { IODataCollection.ChangeCheckStatus(cName, IOName, checkStatus) ; }));
        }

        public void ChangeOnOFF(String cName, String IOName, String onoff)
        {
            int i = onoff == "ON" ? 1 : 0;
            String command = $"{IOName}={i}";
            string cType = cName.Split('-')[0];
            try
            {
                switch (cType)
                {
                    case "ACS": String s = ACSUpdater.acsu.Transaction(cName, command);break;
                    case "QPLC":
                        int iReturnCode;
                        iReturnCode = QPLCUpdater.qplcu.WriteDevice(cName, IOName, i);
                        if (iReturnCode!=0)
                        {
                            throw new Exception(String.Format("0xf{0:x8}", iReturnCode));
                        }
                        break;
                }
                    
            }
            catch(Exception e)
            {
                MessageBox.Show($"错误，无法更改状态：{cName}@{command}\n错误说明：{e.Message}");
            }
        }

        private void CheckErrorCell(IODataCollection.dataTableType dtt, String controllerName)
        {

        }

        void DeleteRow()
        {
            foreach (DataGridView dgv in dgvList)
            {
                if (dgv.Visible)
                {
                    DialogResult dr = MessageBox.Show("确定删除？", "提示",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (DialogResult.Yes == dr)
                    {
                        List<DataGridViewRow> rl = new List<DataGridViewRow>();
                        foreach (DataGridViewCell cell in dgv.SelectedCells)
                        {
                            DataGridViewRow row = cell.OwningRow;
                            string s = row.Cells["IOName"].Value as string;
                            rl.Add(row);                            
                        }
                        for (int i = 0; i < rl.Count; i++)
                        {
                            dgv.Rows.Remove(rl[i]);
                        }
                        string cname = controllerDropList.SelectedValue as string;
                        String IOType = IOTab.SelectedTab.Text;
                        IODataCollection.dataTableType dtt = (IODataCollection.dataTableType)Array.IndexOf(
                                Enum.GetNames(typeof(IODataCollection.dataTableType)), IOType);
                        if (dgv.Rows.Count <= 0)
                        {
                            IODataCollection.FillEmptyTable(cname, dtt);
                        }
                        System.Data.DataTable table = dgv.DataSource as System.Data.DataTable;
                        table.AcceptChanges();
                    }
                    
                }
            }            
        }

        void ClearTable()
        {
            foreach (DataGridView dgv in dgvList)
            {
                if (dgv.Visible)
                {
                    DialogResult dr = MessageBox.Show("确定清空？", "提示",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (DialogResult.Yes == dr)
                    {
                        System.Data.DataTable table = dgv.DataSource as System.Data.DataTable;
                        table.Clear();
                        string cname = controllerDropList.SelectedValue as string;
                        String IOType = IOTab.SelectedTab.Text;
                        IODataCollection.dataTableType dtt = (IODataCollection.dataTableType)Array.IndexOf(
                                Enum.GetNames(typeof(IODataCollection.dataTableType)), IOType);
                        IODataCollection.FillEmptyTable(cname, dtt);
                    }
                }
            }
        }
    }
}
