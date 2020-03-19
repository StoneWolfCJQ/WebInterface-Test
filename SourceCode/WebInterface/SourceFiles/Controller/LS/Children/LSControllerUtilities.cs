using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;

namespace WebInterface
{
    public partial class LSControllerUtilities
    {
        public LSControllerUtilities()
        {
            LSList = new Dictionary<string, int>();
        }

        public LSControllerUtilities(LSControllerUtilities _ls)
        {
            LSList = new Dictionary<string, int>(_ls.LSList);
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

        //queryList:when start with IN, only IN0.1~IN0.7 allowed, while others must be *(0~9).(0~7)
        //other IOs must start at 0 and be sequential
        //IN0-IN7 for internel IO, port0's 0-7, others for remaining port IOs
        //other port mapping rule: *x.y-- port:floor((x+1)/4), index:(x+1)*8+y
        public List<int> ReadDevice(String cname, List<string> queryList)
        {
            Dictionary<string, int> IOIndexList=new Dictionary<string, int>();
            short[] result = new short[queryList.Count];
            int i=0;
            foreach (string s in queryList)
            {
                if (Regex.IsMatch(s,@"^(?i)(in0)$"))
                {
                    result[i]=LTDMC.dmc_read_inport(LSList[cname], 0);
                }
                else
                {
                    Match m = Regex.Match(s, @"([0-9]|([1-9][0-9]*))$");
                    int IOIndex=int.Parse(m.Value);
                    if (IOIndexList.Values.ToList().Exists(ss=>ss==IOIndex)){
                        throw new Exception(string.Format("IO Index重复！卡号：{0:d}，IO组：{1:s}",LSList[cname],s));
                    }
                    else
                    {
                        IOIndexList.Add(s, IOIndex);
                    }
                    int portID=(IOIndex+1)/4;
                    result[i]=LTDMC.dmc_read_inport(LSList[cname], portID);
                }
                i++;
            }

            return result.Select(s=>(int)s).ToList();
        }

        public int WriteDevice(string cname, string device, int value)
        {
            Match m = Regex.Match(device, @"[0-9]|([1-9][0-9]*)\.");
            int portID=(int.Parse(m.Value.TrimEnd())+1)/4;
            return LTDMC.dmc_write_outbit(LSList[cname], portID, value);
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
        public Dictionary<String, int> LSList;
        private bool stopUpdate = false;
    }
}
