using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using WebInterface.DataUpdater;
using System.Threading;

namespace WebInterface
{
    partial class IOInterface
    {
        private void PasteData()
        {
            if (controllerDropList.Items.Count > 0)
            {
                String pasteText = Clipboard.GetText();
                String IOType = IOTab.SelectedTab.Text;
                IODataCollection.dataTableType dtt = (IODataCollection.dataTableType)Array.IndexOf(
                        Enum.GetNames(typeof(IODataCollection.dataTableType)), IOType);
                DataGridView dgv= new DataGridView();
                switch (dtt)
                {
                    case IODataCollection.dataTableType.Input:
                        dgv = inputDGV;
                        break;
                    case IODataCollection.dataTableType.Output:
                        dgv = outputDGV;
                        break;
                    case IODataCollection.dataTableType.Limit:
                        dgv = limitDGV;
                        break;
                }
                String controllerName = controllerDropList.SelectedValue as String;
                PasteFormPreviewWindow pfp = new PasteFormPreviewWindow(pasteText, dgv, dgvList, controllerName.Split('-')[0]);
                pfp.ShowDialog();
                if (pfp.result)
                {                    
                    DataTable ct = pfp.pTable;
                    IODataCollection.AddTable(dtt, controllerName, ct);
                    switch (dtt)
                    {
                        case IODataCollection.dataTableType.Input:
                            inputDGV.Sort(inputDGV.Columns["IOName"], ListSortDirection.Ascending);
                            break;
                        case IODataCollection.dataTableType.Output:
                            outputDGV.Sort(outputDGV.Columns["IOName"], ListSortDirection.Ascending);
                            break;
                        case IODataCollection.dataTableType.Limit:
                            limitDGV.Sort(limitDGV.Columns["IOName"], ListSortDirection.Ascending);
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("请先添加控制器", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                childItemManageControllers.PerformClick();
            }
        }

        private Thread LimitWizardThread = new Thread(()=> { });
        LimitTableWizard l ;
        private void ShowLimitTableWizard()
        {            
            string name = controllerDropList.SelectedValue as string;
            if (!String.IsNullOrEmpty(name))
            {
                if (LimitWizardThread.IsAlive)
                {
                    Invoke(new Action(() => { l.WindowState = FormWindowState.Normal; l.BringToFront(); }));
                    return;
                }
                l = new LimitTableWizard(controllerDropList);
                LimitWizardThread = new Thread(() => { l.ShowDialog(); })
                {
                    IsBackground = true
                };
                LimitWizardThread.SetApartmentState(ApartmentState.STA);
                LimitWizardThread.Start();
            }
            else
            {
                MessageBox.Show("请先选择一个控制器", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
