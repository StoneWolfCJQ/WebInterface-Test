using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;

namespace WebInterface
{
    partial class LimitTableWizard
    {
        #region Event

        private void ScriptTextBox_TextChanged(object sender, EventArgs e)
        {
            string s = scriptTextBox.Text;
            if (s.Contains("WHILE"))
            {
                copyScriptButton.Enabled = true;
            }
            else if (string.IsNullOrEmpty(s))
            {
                copyScriptButton.Enabled = false;
            }
        }


        private void SourceCB_SelectedValueChanged(object sender, EventArgs e)
        {
            GetCBString();
            EnableButton();
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            FillTable();
            GenerateScript();
        }

        private void copyScriptButton_Click(object sender, EventArgs e)
        {
            string script = scriptTextBox.Text;
            Clipboard.SetText(script);
            MessageBox.Show("已复制", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }

        private void tableAddButton_Click(object sender, EventArgs e)
        {
            if (CheckTableEmpty())
            {
                MessageBox.Show("没有限位定义", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
            else
            {
                DialogResult r = MessageBox.Show("将覆盖原有IO表，是否继续", "提示"
                    , MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (DialogResult.Yes == r)
                {
                    DataTable table = limitDGV.DataSource as DataTable;
                    table.AcceptChanges();
                    Invoke(new Action(() => { IODataCollection.AddTable(IODataCollection.dataTableType.Limit, controllerName, table); }));
                }
            }
        }

        private void getTableButton_Click(object sender, EventArgs e)
        {
            GetTable();
        }

        private void DescriptionTextBox_TextChanged(object sender, EventArgs e)
        {
            GetDescriptionTextBox();
        }
        #endregion

        #region EventCall
        void GetDescriptionTextBox()
        {
            string text = descriptionTextBox.Text;
            String[][] textLines = GetArrayFromDescription(text);
            bool result;
            int i;
            int max = 1;
            axisDesp.Clear();
            foreach (string[] sl in textLines)
            {
                result = int.TryParse(sl[0], out i);
                if (result)
                {
                    if (i + 1 > max)
                    {
                        max = i + 1;
                    }
                }
                else
                {
                    continue;
                }

                string s;
                if (sl.Length > 1)
                {
                    s = sl[1];
                }
                else
                {
                    s = String.Empty;
                }

                if (axisDesp.ContainsKey(i))
                {
                    axisDesp[i] = s;
                }
                else
                {
                    axisDesp.Add(i, s);
                }
            }
            axisDesp.OrderBy(a => a.Key);
            axisNumberBox.Value = max;
        }

        string[][] GetArrayFromDescription(string s)
        {
            s = s.Replace("\r\n", "\n").Replace('\t', ' ');
            return s.Split(new string[] { "\n" }, StringSplitOptions.None)
                .Select(ss => ss.Split(' ')).ToArray();
        }

        bool CheckTableEmpty()
        {
            DataTable table = limitDGV.DataSource as DataTable;
            string name;
            foreach (DataRow row in table.Rows)
            {
                name = row["IOName"] as string;
                if (!(String.IsNullOrEmpty(name)|| String.IsNullOrWhiteSpace(name)))
                {
                    return false;
                }
            }
            return true;
        }

        void FillTable()
        {
            DataTable table = limitDGV.DataSource as DataTable;
            table.RejectChanges();
            Dictionary<string, string> ta = new Dictionary<string, string>();
            int max = Convert.ToInt32(axisNumberBox.Value) - 1;
            int i, j;
            for (j = 0; j <= max; j++)
            {
                if (axisDesp.ContainsKey(j))
                {
                    for (i = 0; i < 2; i++)
                    {
                        ta.Add(preFix + j + '.' + i, axisDesp[j] + sChar + LimitStr[i]);
                    }
                }
                else
                {
                    for (i = 0; i < 2; i++)
                    {
                        ta.Add(preFix + j + '.' + i,"$$$ " + j + "号轴" + sChar + LimitStr[i]);
                    }
                }
            }
            
            string name;
            foreach (DataRow row in table.Rows)
            {
                name = row["IOName"] as string;
                if (ta.ContainsKey(name))
                {
                    if (!ta[name].StartsWith("$$$"))
                    {
                        row["IOAlias"] = ta[name];
                    }
                    else if (IsStringEmpty(row["IOAlias"] as string))
                    {
                        row["IOAlias"] = ta[name].TrimStart('$',' ');
                    }
                    ta.Remove(name);
                }
            }

            foreach (var v in ta)
            {
                table.Rows.Add(v.Key, v.Value.TrimStart('$', ' '), "OFF", IODataCollection.checkStatusType.Uncheck);
            }

            IODataCollection.ClearEmptyRowFromTable(table);
            table.DefaultView.Sort = "IOName ASC";            
        }

        bool IsStringEmpty(string s)
        {
            return (String.IsNullOrEmpty(s) || String.IsNullOrWhiteSpace(s));
        }

        bool IsStringValid(string s)
        {
            string reg = String.Format(@"^{0}[0-9]+\.[0-1]$", preFix);
            return Regex.IsMatch(s, reg);
        }

        void GenerateScript()
        {
            string head = "GLOBAL INT";
            string body = "AxisLimitMapForWebInterface:\r\nWHILE 1";
            string end = "END\r\nSTOP";
            List<int> il = new List<int>();
            int i, j;
            DataTable table = limitDGV.DataSource as DataTable;            
            foreach (DataRow row in table.Rows)
            {
                if (IsStringValid(row["IOName"] as string))
                {
                    i = GetAxisNumber(row);
                    j = int.Parse((row["IOName"] as string).Split('.')[1]);
                    if (!il.Exists(ii => ii == i))
                    {
                        il.Add(i);
                        head += ' ' +preFix + i + ',';
                    }
                    body += "\r\n" + row["IOName"] + $"=FAULT({i}).{1 - j}";
                }                
            }
            head = head.TrimEnd(',');
            scriptTextBox.Text = String.Join("\r\n", head, body, end);
        }

        int GetAxisNumber(DataRow row)
        {
            string s = row["IOName"] as string;

            return GetAxisNumber(s);
        }

        int GetAxisNumber(string s)
        {
            s = s.Substring(preFix.Length, s.IndexOf('.') - preFix.Length);
            if (int.TryParse(s, out int i))
            {
                return i;
            }

            return -1;
        }

        string GetAxisAlias(DataRow row)
        {
            string s = row["IOAlais"] as string;
            return GetAxisAlias(s);
        }

        string GetAxisAlias(string s)
        {
            string[] sl = s.Split(sChar);
            if (sl.Length <= 1)
            {
                return String.Empty;
            }
            sl[sl.Length - 1] = "";
            return String.Join(sChar.ToString(), sl);
        }

        #endregion

    }
}
