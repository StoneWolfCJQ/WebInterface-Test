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
        private String GetCellValueString<T>(int rowIndex, T colIndex)
        {
            String rs = "";
            int ci = 0;
            if (typeof(T) == typeof(String))
            {
                String cs = (String)Convert.ChangeType(colIndex, typeof(String));
                ci = controllerDGV.Columns[cs].Index;
            }
            else
            {
                ci = (int)Convert.ChangeType(colIndex, typeof(int));
            }

            try
            {
                rs = (String)controllerDGV.Rows[rowIndex].Cells[ci].Value;
            }
            catch
            {
            }
            return rs;
        }

        private String GetCellValueAndChange<T>(int rowIndex, T colIndex)
        {
            String rs = "";
            int ci = 0;
            if (typeof(T) == typeof(String))
            {
                String cs = (String)Convert.ChangeType(colIndex, typeof(String));
                ci = controllerDGV.Columns[cs].Index;
            }
            else
            {
                ci = (int)Convert.ChangeType(colIndex, typeof(int));
            }

            try
            {
                rs = (String)controllerDGV.Rows[rowIndex].Cells[ci].Value;
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
