using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebInterface
{
    public partial class ControllerConfigWindow
    {
        private bool CheckCellError()
        {
            if ((illegalRowIndex > 0))
            {
                controllerDGV.Rows[illegalRowIndex].Cells[illegalColumnIndex].Value = oldStr;
                return false;
            }
            return true;
        }

        private bool CheckControllerNameLegal(string cn, int rowIndex, int colIndex)
        {
            if (cn.Contains(' '))
            {
                MessageBox.Show("名称不能带有空格", "名称格式错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (IODataCollection.controllerDict.ContainsKey(IODataCollection.MergeTypeNameAndControllerName(selectControllerTypeName, cn)))
            {
                MessageBox.Show("名称重复！", "名称错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private bool CheckControllerIPLegal(string IP, int rowIndex, int colIndex, List<string> checkList)
        {
            if (IP == "")
            {
                return true;
            }

            IP = IP.Trim(' ');
            controllerDGV.CellValueChanged -= ControllerDGV_CellValueChanged;
            controllerDGV.Rows[rowIndex].Cells[colIndex].Value = IP;
            controllerDGV.CellValueChanged += ControllerDGV_CellValueChanged;
            if (IP == oldStr)
            {
                return true;
            }

            //Remove Duplication Warning
            /*if (checkList.FindAll(c=>c==IP).Count>1)
            {
                MessageBox.Show("IP重复！", "名称错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }*/

            List<string> s = IP.Split('.').ToList();
            if (s.Count != 4)
            {
                MessageBox.Show("IP错误");
                return false;
            }

            foreach (string ss in s)
            {
                bool r = int.TryParse(ss, out int i);
                if ((!r) || (i < 0) || (i > 255))
                {
                    MessageBox.Show("IP错误");
                    return false;
                }
            }
            return true;
        }

        bool CheckControllerNumberLegal(string number, int rowIndex, int colIndex, List<string> checkList)
        {
            if (number == "")
            {
                return true;
            }

            number = number.Trim(' ');
            controllerDGV.CellValueChanged -= ControllerDGV_CellValueChanged;
            controllerDGV.Rows[rowIndex].Cells[colIndex].Value = number;
            controllerDGV.CellValueChanged += ControllerDGV_CellValueChanged;
            if ( number == oldStr)
            {
                return true;
            }

            //Removed
            /*if (checkList.FindAll(c => c == number).Count > 1)
            {
                MessageBox.Show("站号重复！", "地址错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }*/

            int i;
            try
            {
                i = int.Parse(number);
            }
            catch
            {
                MessageBox.Show("站号不是整数！", "地址错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if ((i > gNumberCheckMax) || (i < gNumberCheckMin))
            {
                MessageBox.Show("站号大于64或者小于0！", "地址错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private bool CheckControllerName(string cn, int rowIndex, int colIndex, List<string> CheckList)
        {
            DataGridViewCellStyle st = new DataGridViewCellStyle();
            if ((cn.ToString() == "") && (oldStr != ""))
            {
                MessageBox.Show("已添加的控制器名称不能为空\n如果你要删除控制器，请用Remove按钮\n如果你不想监听此控制器，请将Update的勾取消掉或将IP设为空");
                controllerDGV.CellValueChanged -= ControllerDGV_CellValueChanged;
                controllerDGV.Rows[rowIndex].Cells["Controller Name"].Value = oldStr;
                controllerDGV.CellValueChanged += ControllerDGV_CellValueChanged;
                return false;
            }
            else if (cn.ToString() == "")
            {
                return false;
            }
            else if (cn == oldStr)
            {
                return false;
            }
            else if (!CheckControllerNameLegal(cn, rowIndex, colIndex))
            {
                illegalRowIndex = rowIndex;
                illegalColumnIndex = colIndex;
                st.BackColor = System.Drawing.Color.OrangeRed;
                controllerDGV.Rows[rowIndex].Cells["Controller Name"].Style = st;
                return false;
            }
            return true;
        }

        private bool CheckControllerAddress(string address, int rowIndex, int colIndex, List<string> checkList)
        {
            DataGridViewCellStyle st = new DataGridViewCellStyle();
            bool result;
            switch (GetCheckType())
            {
                case CheckType.IP:result = CheckControllerIPLegal(address, rowIndex, colIndex, checkList);break;
                case CheckType.Number:result = CheckControllerNumberLegal(address, rowIndex, colIndex, checkList);break;
                default:throw new NotImplementedException();
            }
            if (!result)
            {
                illegalRowIndex = rowIndex;
                illegalColumnIndex = colIndex;
                st.BackColor = System.Drawing.Color.OrangeRed;
                controllerDGV.Rows[rowIndex].Cells[GetAddressName()].Style = st;
                return false;
            }
            return true;
        }

        public void CheckRowValueLegal(int rowIndex, int colIndex)
        {
            string cn, address;

            List<string> checkList = (from DataGridViewRow dgvr in controllerDGV.Rows
                                      let s = dgvr.Cells[colIndex].Value as string
                                      where s != ""
                                      select s).ToList();

            cn = GetCellValueAndChange(rowIndex, "Controller Name");
            address = GetCellValueAndChange(rowIndex, GetAddressName());

            illegalRowIndex = -1;
            illegalColumnIndex = -1;

            if (colIndex == controllerDGV.Columns["Controller Name"].Index)
            {
                if (!CheckControllerName(cn, rowIndex, colIndex, checkList))
                {
                    return;
                }
            }
            else if (colIndex == controllerDGV.Columns[GetAddressName()].Index)
            {
                if (!CheckControllerAddress(address, rowIndex, colIndex, checkList))
                {
                    return;
                }
            }

            switch (editControllerType)
            {
                case EditType.Add:
                    IODataCollection.AddController(selectControllerTypeName, cn, address);
                    break;
                case EditType.UpdateName:
                    IODataCollection.UpdateControllerName(selectControllerTypeName, oldStr, cn);
                    break;
                case EditType.UpdateAddress:
                    IODataCollection.UpdateControllerAddress(selectControllerTypeName, address, cn);
                    break;
                default: break;
            }
        }

        private bool IsCellReadOnly(int rowIndex, int colIndex)
        {
            bool result = controllerDGV.Rows[rowIndex].Cells[colIndex].ReadOnly;
            return result;
        }

        private void UncheckBadRow()
        {
            int rowIndex;
            foreach (DataGridViewRow row in controllerDGV.Rows)
            {
                rowIndex = row.Index;
                if ((GetCellValueString(rowIndex, "Controller Name") == "") || (GetCellValueString(rowIndex, GetAddressName()) == ""))
                {
                    switch (selectControllerTypeName)
                    {
                        case "ACS":
                            if ((GetCellValueString(rowIndex, "Controller Name") != ACSBaseConfig.sim))
                            {
                                controllerDGV.Rows[rowIndex].Cells["Update"].Value = false;
                            };
                            break;
                        default: controllerDGV.Rows[rowIndex].Cells["Update"].Value = false; break;
                    }
                    
                    
                }
                
            }
        }
    }

    public partial class ControllerConfigWindow
    {

    }
}
