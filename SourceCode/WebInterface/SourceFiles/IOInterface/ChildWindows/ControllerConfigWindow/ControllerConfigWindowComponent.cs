using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace WebInterface
{
    public partial class ControllerConfigWindow
    {
        void GetControllerTable(int selectedCTypeRow)
        {
            selectControllerTypeName = GetControllerTypeName(selectedCTypeRow);
            switch (selectControllerTypeName)
            {
                case "ACS": controllerDGV.DataSource = IODataCollection.ACSControllerTable; break;
                case "QPLC": controllerDGV.DataSource = IODataCollection.QPLCControllerTable; break;
                default: throw new NotImplementedException();
            }
        }

        private void RemoveController(String cname)
        {
            IODataCollection.RemoveController(selectControllerTypeName, cname);
        }

        private void CellBeginEditHandler(int rrowIndex, int ccolIndex)
        {
            controllerDGV.CellEndEdit += ControllerDGV_CellEndEdit;
            int rowIndex = rrowIndex;
            int colIndex = ccolIndex;

            String oldName = "";
            String oldAddress = "";
            editControllerType = EditType.None;

            if ((illegalRowIndex >= 0))
            {
                if (ccolIndex== controllerDGV.Columns["Controller Name"].Index)
                {
                    oldName = oldStr;
                }
            }
            else
            {
                oldName = GetCellValueAndChange(rowIndex, "Controller Name");
                oldAddress = GetCellValueAndChange(rowIndex, GetAddressName());
                
                if (ccolIndex == controllerDGV.Columns["Controller Name"].Index)
                {
                    oldStr = oldName;
                }
                else
                {
                    oldStr = oldAddress;
                }
            }

            if (colIndex == controllerDGV.Columns["Controller Name"].Index)
            {
                if (String.IsNullOrEmpty(oldName))
                {
                    editControllerType = EditType.Add;
                }
                else
                {
                    editControllerType = EditType.UpdateName;
                }
            }
            else if (!String.IsNullOrEmpty(oldName))
            {
                editControllerType = EditType.UpdateAddress; 
            }
        }

        private void RemoveControllerFromSelect()
        {
            if (!CheckCellError())
            {
                MessageBox.Show("有错误未修正，错误的行已恢复上一次有效值", "提示", MessageBoxButtons.OK);
            }
            String cname;
            if (controllerDGV.SelectedCells.Count > 1)
            {
                DialogResult dr = MessageBox.Show("将删除多个控制器，是否继续", "警告", MessageBoxButtons.YesNo);
                if (DialogResult.Yes != dr)
                {
                    return;
                }
            }

            foreach (DataGridViewCell dgvc in controllerDGV.SelectedCells)
            {
                DataGridViewRow dgr = dgvc.OwningRow;
                cname = GetCellValueString(dgr.Index, "Controller Name");                
                if (("" != cname) && (ACSBaseConfig.sim != cname)) 
                {
                    controllerDGV.CellValueChanged -= ControllerDGV_CellValueChanged;
                    dgr.SetValues(false, "", "");
                    controllerDGV.CellValueChanged += ControllerDGV_CellValueChanged;
                    RemoveController(cname);
                }
            }
        }


        private void SetReadOnly()
        {
            var dcs = from DataGridViewRow d in controllerDGV.Rows
                      where GetCellValueString(d.Index, "Controller Name") == ACSBaseConfig.sim
                      select new
                      {
                          nc = d.Cells["Controller Name"],                          
                          ic = d.Cells[GetAddressName()]
                      };

            foreach (var cell in dcs)
            {
                cell.nc.ReadOnly = true;
                switch (GetControllerTypeName(selectControllerTypeRow))
                {
                    case "ACS": cell.ic.ReadOnly = true;break;
                    case "QPLC":break;
                    default:throw new NotImplementedException();
                }
                
            }
        }

        bool CheckIllegalCellBeforeSwitch()
        {
            if (illegalRowIndex >= 0)
            {
                DialogResult dr = MessageBox.Show("有错误未更正，是否继续，错误的值不会被保存", "错误", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    controllerDGV.CellValueChanged -= ControllerDGV_CellValueChanged;
                    controllerDGV.Rows[illegalRowIndex].Cells[illegalColumnIndex].Value = oldStr;
                    controllerDGV.CellValueChanged += ControllerDGV_CellValueChanged;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        string GetControllerTypeName(int selectedCTypeRow)
        {            
            return (string)ControllerTypeDGV.Rows[selectedCTypeRow].Cells["Controller Type"].Value ;
        }

        string GetAddressName()
        {
            string s = GetControllerTypeName(selectControllerTypeRow);
            switch (s)
            {
                case "ACS": return "Controller IP";
                case "QPLC":return "Station Number";
                default:throw new NotImplementedException();
            }
        }

        string GetAddressName(string cType)
        {
            string s = cType;
            switch (s)
            {
                case "ACS": return "Controller IP";
                case "QPLC": return "Station Number";
                default: throw new NotImplementedException();
            }
        }

        CheckType GetCheckType()
        {
            return controllerCheckType[selectControllerTypeName];
        }

        private void RebuildBindingList()
        {
            UncheckBadRow();
            Dictionary<String, String> d = new Dictionary<String, String>();
            d = (from DataRow dr1 in IODataCollection.ControllerTypeTable.Rows
                 where (bool)dr1["Update"] == true
                 let ct = (string)dr1["Controller Type"]
                 select new { key = ct, Rows = IODataCollection.ControllerDataTableList.Find(t => t.TableName == ct).Rows } into dts
                 from DataRow dr in dts.Rows
                 // from DataGridViewRow dgv in controllerDGV.Rows
                 where dr["Update"] != null
                 where (bool)dr["Update"] == true
                 select new { key = dts.key + "-" + (string)dr["Controller Name"], value = (string)dr[GetAddressName(dts.key)] }).ToDictionary(k => k.key, k => k.value);
            Invoke(new Action(() => { IODataCollection.RebuildBindList(d); }));
        }
    }

    public partial class ControllerConfigWindow
    {
        int illegalRowIndex = -1;
        int illegalColumnIndex = -1;
        //static bool firstStart = true;
        int selectControllerTypeRow = 0;
        string selectControllerTypeName;
        EditType editControllerType;
        String oldStr;
        enum EditType
        { Add, UpdateName, UpdateAddress, None };
        enum CheckType
        { IP, Number}
        Dictionary<string, CheckType> controllerCheckType;
        int gNumberCheckMax = 64;
        int gNumberCheckMin = 0;
    }
}
