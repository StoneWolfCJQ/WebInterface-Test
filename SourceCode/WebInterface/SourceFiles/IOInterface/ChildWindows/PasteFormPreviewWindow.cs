using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace WebInterface
{
    public partial class PasteFormPreviewWindow : Form
    {
        public DataTable pTable= new DataTable();
        public String pText;
        public bool result = false;
        public List<String> IONameList = new List<String>();
        public List<String> DGVIONameList = new List<string>();
        private DataGridView dgv;
        private List<DataGridView> dgvList;
        string cType;
        public PasteFormPreviewWindow(String _pText, DataGridView _dgv, List<DataGridView> _dgvList, string cType)
        {            
            pText = _pText;
            InitializeComponent();
            confirmButton.Enabled = false;
            InitializeTable();
            dgv = _dgv;
            dgvList = _dgvList;
            InitializeDGVList();
            FormClosing += PasteFormPreviewWindow_FormClosing;
            pDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.cType = cType;
        }

        private void InitializeDGVList()
        {
            DGVIONameList = (from d in dgvList
                             where d.Name != dgv.Name
                             from DataGridViewRow r in d.Rows
                             let s = r.Cells["IOName"].Value as string
                             where ValidStr(s)
                             select s).ToList();
        }

        private void PasteFormPreviewWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            //
        }

        private void InitializeTable()
        {
            bool result;
            if (CheckText())
            {
                pTable = IODataCollection.CreateTableFromClipboardText(pText,out result);
                if (result)
                {
                    pDGV.DataSource = pTable;
                    //pDGV.ReadOnly = true;
                    pDGV.Columns[IODataCollection.colTypeDict[IODataCollection.colType.IOName]].ReadOnly = false;
                    pDGV.Columns[IODataCollection.colTypeDict[IODataCollection.colType.IOAlias]].ReadOnly = false;
                    pDGV.Columns[IODataCollection.colTypeDict[IODataCollection.colType.ONOFF]].ReadOnly = true;
                    pDGV.Columns[IODataCollection.colTypeDict[IODataCollection.colType.CheckStatus]].ReadOnly = true;
                    confirmButton.Enabled = true;
                    pDGV.CellValueChanged += PDGV_CellValueChanged;     
                }
                else
                {
                    MessageBox.Show("数据有错，请更正", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }       
            }
        }

        private void PDGV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //CheckTable();           
        }

        private bool CheckText()
        {
            return true;
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            if (CheckTable(false, out bool nonConflict))
            {
                result = true;
                RemoveBadRow();
                this.Close();
            }
            else
            {
                MessageBox.Show("数据有错，请更正", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Temperary Function To Check Table
        private bool CheckTable(bool allowNonConflictDup, out bool nonConflict)
        {
            bool presult = true;
            nonConflict = true;
            IONameList.Clear();

            if (pDGV.Rows.Count == 0)
            {
                presult = false;
                return presult;
            }

            if (allowNonConflictDup)
            {
                IONameList = (from DataGridViewRow ro in dgv.Rows
                              let s = ro.Cells["IOName"].Value as string
                              where ValidStr(s)
                              select s).ToList();
            }
            
            DataGridViewCellStyle style = new DataGridViewCellStyle();
           
            pDGV.DefaultCellStyle = style.Clone();
            
            foreach (DataGridViewRow row in pDGV.Rows)
            {
                if (!RegCheck(row) || !ConflictDuplicationCheck(row))
                {
                    style.BackColor = System.Drawing.Color.OrangeRed;                   
                    presult = false;
                }
                else
                {
                    if (!NonConflictDuplicationCheck(row))
                    {
                        if (allowNonConflictDup)
                        {
                            style.BackColor = System.Drawing.Color.AliceBlue;
                            nonConflict = false;
                        }
                        else
                        {
                            style.BackColor = System.Drawing.Color.OrangeRed;
                            presult = false;
                        }
                    }
                    else
                    {
                        style.BackColor = System.Drawing.Color.White;
                    }
                }
                row.Cells["IOName"].Style = style.Clone();
            }
            return presult;
        }

        private bool ConflictDuplicationCheck(DataGridViewRow row)
        {
            bool result = true;
            String str = row.Cells["IOName"].Value as string;
            if (DGVIONameList.Contains(str)) 
            {
                result = false;
            }

            return result;
        }

        private bool NonConflictDuplicationCheck(DataGridViewRow row)
        {
            bool result = true;
            String str = row.Cells["IOName"].Value as string;
            if (IONameList.Contains(str))
            {
                result = false;
            }
            else if (ValidStr(str))
            {
                IONameList.Add(str);
            }

            return result;
        }

        private bool RegCheck(int ri, int ci)
        {
            return RegCheck(GetString(pDGV.Rows[ri].Cells[ci].Value));
        }

        private bool RegCheck(String istr)
        {
            if (istr== "TheAbsolutelyCorrectIOName")
            {
                return true;
            }
            string regStr;
            switch (cType)
            {
                case "ACS": regStr = @"^(?i)([_a-z]([_a-z0-9]*)\.(30|31|([1-2]?[0-9])))$";break;
                case "QPLC":regStr = @"^[XY]([0-9A-F]+)$";break;
                case ControllerNames.LS: regStr = @"^(?i)((in0\.[0-7])|([_a-z]([_a-z0-9]*)\.[0-7]))$";break;
                default:throw new NotImplementedException();
            }
            return Regex.IsMatch(istr, regStr);
        }

        private bool RegCheck(DataGridViewRow row)
        {
            return RegCheck(GetString(row.Cells["IOName"].Value));
        }

        private String GetString(Object cv)
        {
            String s = "";
            try
            {
                s = (String)cv;
            }
            catch
            {
                
            }
            return !ValidStr(s) ? "TheAbsoluteCorrectIOName" : s;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            result = false;
            this.Close();
        }

        private void appendButton_Click(object sender, EventArgs e)
        {
            if (!CheckTable(true, out bool nonConflict))
            {
                MessageBox.Show("数据有错，请更正", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (!nonConflict)
                {
                    DialogResult dr = MessageBox.Show("存在重复数据，新数据将覆盖旧数据，是否继续", "提示", 
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (DialogResult.OK != dr)
                    {
                        return;                        
                    }
                }
                MergeTableAndRemoveOldDup();
                result = true;
                Close();
            }
        }

        void MergeTableAndRemoveOldDup()
        {
            pTable.Merge(dgv.DataSource as DataTable);
            RemoveBadRow();
        }

        void RemoveBadRow()
        {
            pTable = (from DataRow ro in pTable.Rows
                      let mer = (ro["IOName"] as string + ro["IOAlias"] as string).Trim(' ')
                      where ValidStr(mer)
                      group ro by ro["IOName"] as string into rog
                      select rog.First()).CopyToDataTable();
        }

        bool ValidStr(string s)
        {
            return !(String.IsNullOrWhiteSpace(s) || String.IsNullOrEmpty(s));
        }

        private void getCheckButton_Click(object sender, EventArgs e)
        {
            foreach (DataRow r in pTable.Rows)
            {
                string rs = r["IOName"] as string;
                foreach (DataGridViewRow dr in dgv.Rows)
                {
                    string drs = dr.Cells["IOName"].Value as string;
                    if (rs == drs)
                    {
                        r["CheckStatus"] = (IODataCollection.checkStatusType)dr.Cells["CheckStatus"].Value;
                    }
                }
            }
        }

        private void clearCheckButton_Click(object sender, EventArgs e)
        {
            foreach (DataRow r in pTable.Rows)
            {
                r["CheckStatus"] = IODataCollection.checkStatusType.Uncheck;
            }
        }

        private void AddRowButton_Click(object sender, EventArgs e)
        {
            pTable.Rows.Add("", "", "OFF", IODataCollection.checkStatusType.Uncheck);
        }
    }
}
