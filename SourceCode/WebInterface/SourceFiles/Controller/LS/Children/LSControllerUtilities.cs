using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using csLTDMC;

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
            LTDMC.dmc_board_close();
            LSList.Clear();
        }

        //queryList:when start with IN, only IN0.1~IN0.7 allowed, while others must be *(0~9).(0~7)
        //other IOs must start at 0 and be sequential
        //IN0-IN7 for internel IO, port0's 0-7, others for remaining port IOs
        //other port mapping rule: *x.y-- port:floor((x+1)/4), index:(x+1)*8+y
        public List<int> ReadDevice(string cname, List<string> queryList)
        {
            Dictionary<string, int> IOIndexList=new Dictionary<string, int>();
            uint[] result = new uint[queryList.Count];
            int i=0;
            foreach (string s in queryList)
            {
                if (Regex.IsMatch(s,@"^(?i)(\w+\\in0)$"))
                {
                    result[i]= ~(ControllerNames.LSdebug?
                               0x85F26B3E :
                               LTDMC.dmc_read_inport((ushort)LSList[cname], 0));
                }
                else if (Regex.IsMatch(s, @"^(?i)(\w+\\out0)$"))
                {
                    result[i] = ~(ControllerNames.LSdebug ?
                               0x85F26B3E :
                               LTDMC.dmc_read_outport((ushort)LSList[cname], 0));
                }
                else
                {
                    Match m = Regex.Match(s, @"\d+$");
                    int IOIndex=int.Parse(m.Value);
                   /* if (IOIndexList.Values.ToList().Exists(ss=>ss==IOIndex)){
                        throw new Exception($"IO Index重复！卡号：{LSList[cname]}，IO组：{IOIndex}");
                    }
                    else
                    {
                        IOIndexList.Add(s, IOIndex);
                    }*/
                    int portID=(IOIndex+1)/4;
                    uint temp = 0;
                    if (s.StartsWith(IODataCollection.dataTableType.Input.ToString() + '\\') ||
                        s.StartsWith(IODataCollection.dataTableType.Limit.ToString() + '\\'))
                    {
                        temp = LTDMC.dmc_read_inport((ushort)LSList[cname], (ushort)portID);
                    }
                    else if (s.StartsWith(IODataCollection.dataTableType.Output.ToString() + '\\'))
                    {
                        temp = LTDMC.dmc_read_outport((ushort)LSList[cname], (ushort)portID);
                    }
                    else throw new Exception("内部错误，雷赛查询字符串错误");

                    if (ControllerNames.LSdebug)
                    {
                        temp = 0x85F26B3E;
                    }
                    int shiftDist= (IOIndex - (4 * portID - 1)) * 8;
                    result[i]=((~temp)>>shiftDist)&0xFF;
                }
                i++;
            }

            return result.Select(s=>(int)s).ToList();
        }

        public int WriteDevice(string cname, string device, int value)
        {
            int bitNo;
            if (Regex.IsMatch(device, @"^(?i)(out0)\."))
            {
                bitNo=int.Parse(device.Substring(device.IndexOf('.')+1));           
            }
            else
            {
                Match m = Regex.Match(device, @"\d*\.");
                bitNo=int.Parse(m.Value.TrimEnd('.'))*8 + 8
                     + int.Parse(device.Substring(device.IndexOf('.')+1));
            }
            value = value == 0 ? 1 : 0;
            
            return LTDMC.dmc_write_outbit((ushort)LSList[cname], (ushort)bitNo, (ushort)value);
        }

        public void Connect(string cname, int CardID)
        {
           LSList.Add(cname, CardID);
        }

        // public void Open(ref ActUtlTypeClass api, int stationNumber, string cName)
        // {
        //     //
        // }

        public void StartUpdate(string cname, string queryStr)
        {

        }

    }

    public partial class LSControllerUtilities
{
        public Dictionary<string, int> LSList;
        private bool stopUpdate = false;
    }
}
