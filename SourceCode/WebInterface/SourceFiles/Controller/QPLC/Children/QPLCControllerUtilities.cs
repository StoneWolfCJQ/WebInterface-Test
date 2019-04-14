using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using ActUtlTypeLib;

namespace WebInterface
{
    public partial class QPLCControllerUtilities
    {
        public QPLCControllerUtilities()
        {
            qList = new Dictionary<string, ActUtlTypeClass>();
        }

        public QPLCControllerUtilities(QPLCControllerUtilities _qcsu)
        {
            qList = new Dictionary<string, ActUtlTypeClass>(_qcsu.qList);
        }

        ~QPLCControllerUtilities()
        {
            qList = null;
        }

        public void CloseComm()
        {
            foreach (ActUtlTypeClass a in qList.Values)
            {
                try
                {
                    a.Close();
                }
                catch
                {

                }
                
            }
            qList.Clear();
        }

        public List<int> ReadDevice(String cname, List<string> queryList)
        {
            string queryList2 = String.Join("\n", queryList);
            short[] result = new short[queryList.Count];
            int iReturnCode;
            iReturnCode = qList[cname].ReadDeviceRandom2(queryList2, queryList.Count, out result[0]);
            
            if (iReturnCode != 0)
            {
                throw new Exception(String.Format("站号{1:d}变量读取错误：0x{0:x8}", iReturnCode, qList[cname].ActLogicalStationNumber));
            }

            return result.Select(s=>(int)s).ToList();
        }

        public int WriteDevice(string cname, string device, int value)
        {
            return qList[cname].SetDevice(device, value);
        }

        public void Connect(String cname, int stationNumber)
        {
            ActUtlTypeClass tapi = new ActUtlTypeClass();
            Open(ref tapi, stationNumber, cname);
            qList.Add(cname, tapi);
        }

        public void Open(ref ActUtlTypeClass api, int stationNumber, string cName)
        {
            api.ActLogicalStationNumber = stationNumber;
            int iReturnCode;
            iReturnCode = api.Open();
            if (iReturnCode!=0)
            {
                throw new Exception(String.Format("打开{2:s}站号{0:d}错误 0x{1:x8}", stationNumber, iReturnCode, cName));
            }
        }

        public void StartUpdate(String cname, String queryStr)
        {

        }

    }

    public partial class QPLCControllerUtilities
{
        public Dictionary<String, ActUtlTypeClass> qList;
        public ActUtlTypeClass simApi;
        private bool stopUpdate = false;
    }
}
