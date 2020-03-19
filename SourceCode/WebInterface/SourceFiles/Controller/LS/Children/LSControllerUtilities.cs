using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting.Messaging;

namespace WebInterface
{
    public partial class LSControllerUtilities
    {
        public LSControllerUtilities()
        {
            LSList = new Dictionary<string, LTDMC>();
        }

        public LSControllerUtilities(LSControllerUtilities _ls)
        {
            LSList = new Dictionary<string, ActUtlTypeClass>(_ls.LSList);
        }

        ~LSControllerUtilities()
        {
            LSList = null;
        }

        public void CloseComm()
        {
            foreach (LTDMC a in LSList.Values)
            {
                try
                {
                    a.dmc_board_close();
                }
                catch
                {

                }
                
            }
            LSList.Clear();
        }

        //queryList rule:only format same as INx.y[0<y<7] allowed, map between controller IO and this is
        //controller:software---portid:floor(x/4),index:x*8+y
        public List<int> ReadDevice(String cname, List<string> queryList)
        {
            foreach (string s in queryList)
            {
                queryList
            }
            //below are useless, for reference only
            string queryList2 = String.Join("\n", queryList);
            short[] result = new short[queryList.Count];
            int iReturnCode;
            iReturnCode = LSList[cname].ReadDeviceRandom2(queryList2, queryList.Count, out result[0]);
            
            if (iReturnCode != 0)
            {
                throw new Exception(String.Format("站号{1:d}变量读取错误：0x{0:x8}", iReturnCode, LSList[cname].ActLogicalStationNumber));
            }

            return result.Select(s=>(int)s).ToList();
        }

        public int WriteDevice(string cname, string device, int value)
        {
            return LSList[cname].SetDevice(device, value);
        }

        public void Connect(String cname, int stationNumber)
        {
            ActUtlTypeClass tapi = new ActUtlTypeClass();
            Open(ref tapi, stationNumber, cname);
            LSList.Add(cname, tapi);
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

    public partial class LSControllerUtilities
{
        public Dictionary<String, LTDMC> LSList;
        private bool stopUpdate = false;
    }
}
