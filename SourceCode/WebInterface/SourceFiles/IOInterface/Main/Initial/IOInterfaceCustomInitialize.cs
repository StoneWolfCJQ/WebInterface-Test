using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using WebInterface.DataUpdater;

namespace WebInterface
{   
    partial class IOInterface
    {        
        private void InitializeCustomComponent()
        {            
            InitializeDGVStyle();            
            InitializeAllDGV();
            InitializeStorageData();
            InitializeControllerList();
            InitializeControllerTable();
        }

        private void InitializeStorageData()
        {
            DataManagementInitialize();
        }

        private void InitializeControllerList()
        {
            controllerDropList.DataSource = IODataCollection.controllerNameList;
            controllerDropList.DisplayMember = "name";
            controllerDropList.ValueMember = "name";
            controllerDropList.SelectedValueChanged += ControllerDropList_SelectedValueChanged;
        }

        private void InitializeControllerTable()
        {
            if (!hasStorageData)
            {
                ACSBaseConfig.dac = ACSBaseConfig.DefaultACSChoice.Simulator;
                IODataCollection.AddController("ACS", ACSBaseConfig.sim, "", ACSBaseConfig.dac == ACSBaseConfig.DefaultACSChoice.Simulator);
                IODataCollection.AddController("ACS",  "Default", "10.0.0.100", ACSBaseConfig.dac == ACSBaseConfig.DefaultACSChoice.Default);
                IODataCollection.AddControllerTable("ACS", ACSBaseConfig.dac == ACSBaseConfig.DefaultACSChoice.Simulator, ACSBaseConfig.sim, "");
                IODataCollection.AddControllerTable("ACS", ACSBaseConfig.dac == ACSBaseConfig.DefaultACSChoice.Default, "Default", "10.0.0.100");
                IODataCollection.DuplicateControllerTable("ACS");
                IODataCollection.AddController("QPLC", ACSBaseConfig.sim, "");
                IODataCollection.AddControllerTable("QPLC", false, ACSBaseConfig.sim, "");
                IODataCollection.DuplicateControllerTable("QPLC");
            }

            RetrieveSelectController();
        }
    }

    partial class IOInterface
    {
        private bool hasStorageData = false;
        private ControllerMaster cm;
    }
}
