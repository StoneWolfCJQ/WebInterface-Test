using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebInterface
{
    partial class IOInterface
    {
        public void SaveDefaultFile()
        {
            lock(cm)
                {
                DataManager dm = cm.dm;
                try
                {
                    dm.SaveFile();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"保存文件错误，错误信息:\"\n{e.Message}\"\n请检查文件可读性或设置本程序管理员权限");
                }
            }            
        }

        private void ExportData()
        {
            
        }

        private void ImportData()
        {

        }

        private void DataManagementInitialize()
        {
            cm = new ACSMaster();
            DataManager dm = cm.dm;
            try
            {
                if (dm.HasStorage())
                {
                    dm.FillData();
                    hasStorageData = true;
                }
                else
                {
                    hasStorageData = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                hasStorageData = false;
            }
            IODataCollection.RebuildControllerTableList();
        }
    }
}
