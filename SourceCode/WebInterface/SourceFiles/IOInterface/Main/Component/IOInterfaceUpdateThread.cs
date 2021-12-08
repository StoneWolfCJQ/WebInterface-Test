using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace WebInterface
{
    partial class IOInterface
    {
        Thread UpdateErrorThread { get; set; }
        private void CheckUpdateErrorThread()
        {
            UpdateErrorThread = new Thread(CheckUpdateError);
            UpdateErrorThread.IsBackground = true;
            if (!suppressShowingError)
            {
                UpdateErrorThread.Start();
            }
            else if (UpdateErrorThread.IsAlive)
            {
                UpdateErrorThread.Abort();
            }
        }

        void CheckUpdateError()
        {
            while (!updateError)
            {
                Thread.Sleep(100);
            }
            Thread.Sleep(10);
            updateError = false;
            try
            {
                Invoke(new Action(() => { ToggleListen(toggleType.CLOSE); ToggleConnect(toggleType.CLOSE, out bool result);  }));
            }
            catch
            {

            }
            if (!suppressShowingError)
            {
                MessageBox.Show("出现错误，已停止所有连接");
            }

        }

        private void ThreadUpdateUI()
        {
            UpdateUIThread = new Thread(UpdateUI);
            UpdateUIThread.IsBackground = true;
            UpdateUIThread.Start();
        }

        
        private void UpdateUI()
        {
            while ((!this.Created) || (controllerDropList.Items.Count < 1))
            {
                Thread.Sleep(100);
            }            

            while (true)
            {
                Thread.Sleep(20);
                foreach (DataGridView dgv in dgvList)
                {
                    if (dgv.Visible)
                    {
                        UpdateButtonAndCB(dgv);
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
        }

        private delegate void ThreadSafeUpateButtonAndCB(DataGridView dgv);
        private void UpdateButtonAndCB(DataGridView dgv)
        {
            if (dgv.InvokeRequired)
            {
                ThreadSafeUpateButtonAndCB ts = new ThreadSafeUpateButtonAndCB(UpdateButtonAndCB);
                try
                {
                    dgv.Invoke(ts, new object[] { dgv });                    
                }
                catch
                {

                }
            }
            else
            {
                string ONOFF;
                IODataCollection.checkStatusType cst;
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    ONOFF = (string)row.Cells["ONOFF"].Value;
                    cst = (IODataCollection.checkStatusType)row.Cells["CheckStatus"].Value;
                    row.Cells["ONOFF"].Style = ONOFF == "ON" ? dgvStyleDict[styleEnum.Nogood] : dgvStyleDict[styleEnum.Good];
                    switch (cst)
                    {
                        case IODataCollection.checkStatusType.Checked:
                            row.Cells["CheckStatus"].Style = dgvStyleDict[styleEnum.Good];
                            break;
                        case IODataCollection.checkStatusType.Uncheck:
                            row.Cells["CheckStatus"].Style = dgvStyleDict[styleEnum.None];
                            break;
                        case IODataCollection.checkStatusType.Error:
                            row.Cells["CheckStatus"].Style = dgvStyleDict[styleEnum.Error];
                            break;
                        case IODataCollection.checkStatusType.Problem:
                            row.Cells["CheckStatus"].Style = dgvStyleDict[styleEnum.Problem];
                            break;
                        default: break;
                    }
                }
            }
        }
    }
}
