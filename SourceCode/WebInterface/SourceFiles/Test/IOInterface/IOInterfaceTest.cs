using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInterface
{
    partial class IOInterface
    {
        #region test


        //private async void UpdateIO()
        //{
        //    while (!this.Created)
        //    await Task.Run(()=>_UpdateIO());
        //}

        //private void _UpdateIO()
        //{
        //    string old1="11111111", old2="11111111";
        //    string newIOX0, newIOX1;
        //    try
        //    {
        //        while (true)
        //        {
        //            Thread.Sleep(1000);                    
        //            //ControllerListSource ss = new ControllerListSource("dd", "dd");
        //            //Invoke(new Action(() => { binding.Add(ss); }));
        //            //ss.name = "vv";
        //            //int sss = binding.IndexOf(ss);
        //            if (init)
        //            {
        //                newIOX0 = X0TextBox.Text;
        //                newIOX1 = X1TextBox.Text;



        //                if (newIOX0 != old1)
        //                {
        //                    int i = 0;
        //                    for (i = 0; i < newIOX0.Length; i++)
        //                    {
        //                        if (newIOX0[i] != old1[i])
        //                        {
        //                            testDGV.Rows[7 - i].Cells["Button"].Value = newIOX0[i] == '1' ? "ON" : "OFF";
        //                            DataGridViewCellStyle style = new DataGridViewCellStyle();
        //                            style.BackColor = newIOX0[i] == '1' ?  System.Drawing.Color.OrangeRed: System.Drawing.Color.ForestGreen;
        //                            testDGV.Rows[7 - i].Cells["Button"].Style = style;
        //                        }
        //                    }
        //                    old1 = newIOX0;
        //                };
        //                if (newIOX1 != old2)
        //                {
        //                    int i = 0;
        //                    for (i = 0; i < newIOX1.Length; i++)
        //                    {
        //                        if (newIOX1[i] != old2[i])
        //                        {
        //                            testDGV.Rows[15 - i].Cells["Button"].Value = newIOX1[i] == '1' ? "ON" : "OFF";
        //                            DataGridViewCellStyle style = new DataGridViewCellStyle();
        //                            style.BackColor = newIOX1[i] == '1' ? System.Drawing.Color.OrangeRed : System.Drawing.Color.ForestGreen;
        //                            testDGV.Rows[15 - i].Cells["Button"].Style = style;
        //                        }
        //                    }
        //                    old2 = newIOX1;
        //                };   
        //                updatedIO = true;
        //            }

        //        }

        //    }
        //    catch(Exception e)
        //    {

        //    }

        //}

        //private async void TestDataTable()
        //{
        //    testDT = new DataTable();
        //    if (this.Created)
        //    await Task.Run(() => FillTable());
        //}

        //private void FillTable()
        //{

        //    DataColumn testCol1 = new DataColumn("input",typeof(string));
        //    DataColumn testCol2 = new DataColumn("input alias", typeof(string));
        //    DataColumn testCol3 = new DataColumn("Check", typeof(structureType));
        //    DataColumn testCol4 = new DataColumn("Button", typeof(string));
        //    testCol1.ReadOnly = true;
        //    testCol2.ReadOnly = true;
        //    DataGridViewButtonColumn testcool = new DataGridViewButtonColumn();
        //    testcool.FlatStyle = FlatStyle.Flat;
        //    testcool.UseColumnTextForButtonValue = false;
        //    testcool.HeaderText = "Status";
        //    testcool.Text = "OFF";
        //    testcool.Name = "Button";
        //    testcool.ValueType = typeof(string);

        //    testcool.DataPropertyName = "Button";
        //    DataTable testbu = new DataTable();
        //    testDT.Columns.AddRange(new DataColumn[]{ testCol1, testCol2, testCol3, testCol4 });

        //    Invoke(new Action(()=>{ testDGV.DataSource = testDT; }) );

        //    var cboSrc = Enum.GetNames(typeof(structureType)).
        //            Select(x => new {
        //                Name = x,
        //                Value = (int)Enum.Parse(typeof(structureType), x)
        //            }
        //                   ).ToList();


        //    DataGridViewComboBoxColumn testCCol = new DataGridViewComboBoxColumn();
        //    testCCol.ValueType = typeof(structureType);
        //    testCCol.DataSource = cboSrc;
        //    testCCol.DisplayMember = "Name";
        //    testCCol.ValueMember = "Value";
        //    testCCol.HeaderText = "CheckeStatus";
        //    testCCol.DataPropertyName = "Check";
        //    testCCol.Name = "CheckStatus";
        //    testCCol.FlatStyle = FlatStyle.Flat;   

        //    Invoke(new Action(() => {                
        //        testDGV.Columns.Remove(testDGV.Columns["Check"]);
        //        testDGV.Columns.Remove(testDGV.Columns["Button"]);
        //        testDGV.Columns.Add(testcool);
        //        testDGV.Columns.Add(testCCol);                
        //        string inputName, inputAlias;
        //        structureType isCheck;

        //        for (int i=0;i<16;i++)
        //        {
        //            inputName = "X" + ((int)(i / 8)).ToString() + '.' +((int)(i % 8)).ToString();
        //            inputAlias = inputName + "Alias";
        //            isCheck = structureType.Uncheck;
        //            testDT.Rows.Add(inputName, inputAlias, isCheck);
        //            testDGV.Rows[i].Cells["Button"].Value = "ON";
        //            DataGridViewCellStyle style = new DataGridViewCellStyle();
        //            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
        //            style.BackColor = System.Drawing.SystemColors.Control;                    
        //            testDGV.Rows[i].Cells["CheckStatus"].Style = style;
        //            style2.BackColor = System.Drawing.Color.OrangeRed;
        //            testDGV.Rows[i].Cells["Button"].Style = style2;

        //        }
        //        testDGV.Columns.GetLastColumn(DataGridViewElementStates.Displayed, DataGridViewElementStates.None).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        //        testDGV.BackgroundColor = System.Drawing.SystemColors.Control;
        //        testDGV.CellValueChanged += new DataGridViewCellEventHandler(TestDGV_CellValueChanged);
        //        testDGV.CurrentCellDirtyStateChanged +=
        //     new EventHandler(dataGridView1_CurrentCellDirtyStateChanged);
        //        testDGV.CellDoubleClick += TestDGV_CellClick;
        //    }));
        //    Invoke(new Action(() => { init=true; }));           
        //}

        //private void TestDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if ((e.ColumnIndex==testDGV.Columns["Button"].Index) && (e.RowIndex>=0))
        //    {
        //        ChangeOnOFF(e.RowIndex);
        //    }
        //}

        //public void ChangeOnOFF(int index, string onoff="")
        //{
        //    Invoke(new Action(() =>
        //    {                
        //        testDGV.Rows[index].Cells["Button"].Value = onoff == "" ? ((String)testDGV.Rows[index].Cells["Button"].Value == "OFF" ? "ON" : "OFF") : onoff;
        //        DataGridViewCellStyle style = new DataGridViewCellStyle();
        //        style.BackColor = (String)testDGV.Rows[index].Cells["Button"].Value == "ON" ? System.Drawing.Color.OrangeRed : System.Drawing.Color.ForestGreen;
        //        testDGV.Rows[index].Cells["Button"].Style = style;
        //        if (index <= 7)
        //        {
        //            char[] old = X0TextBox.Text.ToCharArray();
        //            old[7 - index] = (String)testDGV.Rows[index].Cells["Button"].Value == "OFF" ? '0' : '1';
        //           X0TextBox.Text = new String(old);
        //        }
        //        else
        //        {
        //            char[] old = X1TextBox.Text.ToCharArray();
        //            old[15 - index] = (String)testDGV.Rows[index].Cells["Button"].Value == "OFF" ? '0' : '1';
        //            X1TextBox.Text = new String(old);
        //        }
        //    }));
        //}

        //public int GetRowIndex(String IOName)
        //{
        //    int index=-1;
        //    foreach (DataGridViewRow row in testDGV.Rows)
        //    {
        //        if (((String)row.Cells["input"].Value)==IOName)
        //        {
        //            index = testDGV.Rows.IndexOf(row);
        //        }
        //    }
        //    return index;
        //}

        //private void dataGridView1_CurrentCellDirtyStateChanged(object sender,
        //EventArgs e)
        //{
        //    if (testDGV.IsCurrentCellDirty)
        //    {
        //        // This fires the cell value changed handler below
        //        testDGV.CommitEdit(DataGridViewDataErrorContexts.Commit);
        //    }
        //}

        ////Question: testDT changed upon exec this function
        //private void TestDGV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex==testDGV.Columns["CheckStatus"].Index)
        //    {
        //        int rowIndex = e.RowIndex;
        //        int i = (int)testDGV.Rows[rowIndex].Cells["CheckStatus"].Value;

        //        DataGridViewButtonCell ddd = testDGV.Rows[rowIndex].Cells["Button"] as DataGridViewButtonCell;


        //        testDT.Rows[rowIndex]["Check"] = (structureType)i; //(structureType)testDGV.Rows[rowIndex].Cells["CheckStatus"].Value;
        //        //ddd.Value = testDT.Rows[rowIndex]["Check"].ToString();
        //    }
        //}

        //private void ResponseThread()
        //{
        //    responseThread = new Thread(ResponseHTTPQuery);
        //    responseThread.IsBackground = true;
        //    responseThread.Start();
        //}

        //private void ResponseHTTPQuery()
        //{
        //    String[] _actionArr;
        //    while (true)
        //    {
        //        Thread.Sleep(10);
        //        _actionArr = HTTPRequestHandler.action;                
        //        if (_actionArr.Length != 0)
        //        {
        //            HTTPRequestHandler.action = new String[0];
        //            try
        //            {
        //                String IOName = _actionArr[0];
        //                String action = _actionArr[1];
        //                switch (action)
        //                {
        //                    case "Check":
        //                        //testButton.BackColor = System.Drawing.Color.Red;
        //                        break;
        //                    case "Uncheck":
        //                        //testButton.BackColor = System.Drawing.Color.Blue;
        //                        break;
        //                    case "ON":
        //                        //testButton.BackColor = System.Drawing.Color.Green;
        //                        break;
        //                    case "OFF":
        //                        //testButton.BackColor = System.Drawing.Color.Black;
        //                        break;
        //                    default: break;
        //                }

        //            }
        //            catch(Exception e)
        //            {
        //            }                    
        //        }
        //    }
        //}
        #endregion
    }
}
