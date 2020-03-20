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
        private void ToggleListen(toggleType type)
        {
            if ((!listening) && ((toggleType.NONE == type) || (toggleType.OPEN == type)))
            {
                StartListen();
            }
            else if (listening && ((toggleType.NONE == type) || (toggleType.CLOSE == type)))
            {
                StopListen();
            }
        }

        private void StartListen()
        {
            if (!connected)
            {
                ToggleConnect(toggleType.OPEN, out bool result);
                if (!result)
                {
                    return;
                }
            }
            String IPAddressStr = IPTextBox.Text;
            String PortStr = portTextBox.Text;
            try
            {
                IPEndPoint IPAddressProg = new IPEndPoint(IPAddress.Parse(IPAddressStr), int.Parse(PortStr));
                _socketListener = new SocketListener(IPAddressProg);
                HTTPRequestHandler.IO = this;
                _socketListener.StartListen();
                listening = !listening;
                toggleListenButton.Text = "Stop Listen";
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
                StopListen();
            }
        }

        private void StopListen()
        {
            try
            {
                _socketListener.StopListen();
            }
            catch
            {

            }
            IODataUpdater.RemoveChangeEventForIODataCollection();
            listening = !listening;
            toggleListenButton.Text = "Start Listen!";
        }

        private void ToggleConnect(toggleType type, out bool result)
        {
            result = false;
            if ((!connected) && ((toggleType.NONE == type) || (toggleType.OPEN == type)))
            {
                StartConnect(out result);
            }
            else if (connected && ((toggleType.NONE == type) || (toggleType.CLOSE == type)))
            {
                StopConnect();
            }
        }

        private void StartConnect(out bool result)
        {
            result = true;
            IODataCollection.queryList.Clear();
            if (!ACSUpdater.CreateQuery() || !QPLCUpdater.CreateQuery() || !LSUpdater.CreateQuery() ||
            IODataCollection.controllerNameList.Count==0)
            {
                MessageBox.Show("无控制器或有的控制器无IO", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                result = false;
                return;
            }
            
            DisableMenu();
            CancellationTokenSource ts = new CancellationTokenSource();
            CancellationToken tk = ts.Token;

            Task[] tasks = new Task[3]
            {
                Task.Run(() => ACSUpdater.Connect(tk)),
                Task.Run(() => QPLCUpdater.Connect(tk)),
                Task.Run(() => LSUpdater.Connect(tk)),
            };

            bool connected = false, hasErr = false;
            try
            {
                Task.WaitAll(tasks);
                connected = true;
            }
            catch (AggregateException ae)
            {
                hasErr = true;
                ts.Cancel();
                try
                {
                    Task.WaitAll(tasks);
                }
                catch
                {

                }
                Invoke(new Action(() =>
                MessageBox.Show(ae.InnerException.Message.ToString())));
            }

            while (!connected && !hasErr)
            {
                Thread.Sleep(100);                
            }

            if (connected)
            {
                try
                {
                    ACSUpdater.StartUpdate();
                    QPLCUpdater.StartUpdate();
                    LSUpdater.StartUpdate();
                    CheckUpdateErrorThread();
                    connected = !connected;
                    toggleConnectButton.Text = "Disconnect";
                    ThreadUpdateUI();
                    if (LimitWizardThread.IsAlive)
                    {
                        LimitWizardThread.Abort();
                    }
                    this.connected = true;
                }
                catch (Exception e)
                {
                    hasErr = true;
                    MessageBox.Show(e.Message);
                }
            }

            if (hasErr)
            {
                result = false;
                StopConnect();
            }
        }

        private void StopConnect()
        {
            ACSUpdater.StopUpdate();
            QPLCUpdater.StopUpdate();
            LSUpdater.StopUpdate();
            ToggleListen(toggleType.CLOSE);
            connected = false;
            toggleConnectButton.Text = "Connect!";
            UpdateUIThread.Abort();
            EnableMenu();
        }

        private void DisableMenu()
        {
            this.childItemPasteData.Enabled = false;
            this.childItemManageControllers.Enabled = false;
            childItemExportImport.Enabled = false;
            childItemLimitWizard.Enabled = false;
            deleteRowButton.Enabled = false;
            clearTableButton.Enabled = false;
        }

        private void EnableMenu()
        {
            this.childItemPasteData.Enabled = true;
            this.childItemManageControllers.Enabled = true;
            childItemExportImport.Enabled = true;
            childItemLimitWizard.Enabled = true;
            deleteRowButton.Enabled = true ;
            clearTableButton.Enabled = true;
        }
    }

    public partial class IOInterface
    {
        enum toggleType { OPEN, CLOSE, NONE};
    }
}
