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
        private string GetCellValueString<T>(int rowIndex, T colIndex)
        {
            string rs = "";
            int ci = 0;
            if (typeof(T) == typeof(string))
            {
                string cs = (string)Convert.ChangeType(colIndex, typeof(string));
                ci = controllerDGV.Columns[cs].Index;
            }
            else
            {
                ci = (int)Convert.ChangeType(colIndex, typeof(int));
            }

            try
            {
                rs = (string)controllerDGV.Rows[rowIndex].Cells[ci].Value;
            }
            catch
            {
            }
            return rs;
        }

        private string GetCellValueAndChange<T>(int rowIndex, T colIndex)
        {
            string rs = "";
            int ci = 0;
            if (typeof(T) == typeof(string))
            {
                string cs = (string)Convert.ChangeType(colIndex, typeof(string));
                ci = controllerDGV.Columns[cs].Index;
            }
            else
            {
                ci = (int)Convert.ChangeType(colIndex, typeof(int));
            }

            try
            {
                rs = (string)controllerDGV.Rows[rowIndex].Cells[ci].Value;
            }
            catch
            {
                controllerDGV.CellValueChanged -= ControllerDGV_CellValueChanged;
                controllerDGV.Rows[rowIndex].Cells[ci].Value = rs;
                controllerDGV.CellValueChanged += ControllerDGV_CellValueChanged;
            }
            return rs;
        }
    }

    public partial class ControllerConfigWindow
    {
        
    }
}
