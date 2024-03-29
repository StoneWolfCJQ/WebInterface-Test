﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using WebInterface.QueryUtilities;
using csLTDMC;

namespace WebInterface
{
    public static partial class LSUpdater 
    {    
        public static bool CreateQuery()
        {
            IODataCollection.GenerateQueryList(_name);
            if ((IODataCollection.queryList.FindAll(c => c.name.StartsWith(_name+'-')).Count == 0) && 
            (IODataCollection.controllerNameList.ToList().FindAll(c => c.name.StartsWith(_name+'-')).Count() != 0))
            {
                return false;
            }
            return true;
        }

        public static void Connect(CancellationToken ct)
        {
            if (ControllerNames.LSdebug)
            {
                foreach (ControllerListSource cls in
                IODataCollection.controllerNameList.Where(c => c.name.StartsWith(_name + '-')))
                {
                    LSu.Connect(cls.name, int.Parse(cls.IP));
                }
            }
            else
            {
                foreach (ControllerListSource cls in
                IODataCollection.controllerNameList.Where(c => c.name.StartsWith(_name + '-')))
                {
                    try
                    {
                        LSConnect(ct);
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                    break;
                }
            }

        }
        
        public static void StartUpdate()
        {
            
            StartThreadLSQuery();
        }

        public static void StopUpdate()
        {
            new Thread(()=> { StopThreadLSQuery(); }).Start();
            //StopThreadIODataUpdate();
        }

        public static void ForceStopUpdate()
        {
            if (LSQueryThread .IsAlive)
            {
                LSQueryThread .Abort();
            }
           
            LSu.CloseComm();
            /*if (IODataUpdateThread.IsAlive)
            {
                IODataUpdateThread.Abort();
            }*/
            
        }

        private static void StartThreadLSQuery()
        {
            LSQueryThread  = new Thread(LSQuery);
            LSQueryThread.IsBackground = true;
            LSQueryThread.Start();
        }

        // not in use, use acs's as master 
        // private static void StatThreadIODataUpdate()
        // {
        //     IODataUpdateThread = new Thread(IODataUpdate);
        //     IODataUpdateThread.IsBackground = true;
        //     IODataUpdateThread.Start();
        // }

        private static void LSQuery()
        {
            foreach (ControllerListSource cls in IODataCollection.queryList.Where(c => c.name.StartsWith(ControllerNames.LS+'-')))
            {
                Thread t = new Thread(() => { Update(cls.name, cls.IOList); });
                t.IsBackground = true;
                t.Start();
                RunningTransaction.Add(cls.name);
            }
        }

        private static void Update(string cname, List<string> queryList)
        {
            while (true)
            {
                
                try
                {
                    Thread.Sleep(50);
                    List<int> tl = LSu.ReadDevice(cname, queryList);
                    IODataCollection.UpdateIOQueryList(cname, tl);
                }
                catch(Exception e)
                {
                    string queryStr = string.Join(",", queryList);
                    string err = $"{e.Message} {cname}@{queryStr}";                    
                    IOInterface.updateError = true;
                    RunningTransaction.Remove(cname);
                    if (RunningTransaction.Count == 0)
                    {
                        LSThreadAborted = true;
                    }

                    IOInterface.ShowError(err);
                    return;
                }

                if (LSThreadAbort)
                {
                    RunningTransaction.Remove(cname);
                    if (RunningTransaction.Count == 0)
                    {
                        LSThreadAborted = true;
                    }
                    return;
                }
            }            
        }

        private static void LSConnect(CancellationToken ct)
        {
            LSConnectTask(ct);            
        }

        private static void LSConnectTask(CancellationToken ct)
        {
            bool exception = false;
            string eMessage = "";
            int i = 0;
            ManualResetEvent mre = new ManualResetEvent(false);
            Thread t = new Thread(() => {
                try
                {
                    LSConnectDetail();
                }
                catch (Exception e)
                {
                    exception = true;
                    eMessage = e.Message;
                }
                mre.Set();
            }
            );
            t.IsBackground = true;
            t.Start();
            while (!mre.WaitOne(100))
            {
                i++;
                if (i > 50 || ct.IsCancellationRequested)
                {
                    t.Abort();
                    if (i > 50)
                    {
                        throw new Exception($"雷赛卡连接超时");
                    }
                    return;
                }
            }
            if (exception)
            {
                throw new Exception(eMessage);
            }
        }

        static void LSConnectDetail()
        {
            short i = LTDMC.dmc_board_init();
            if (i <= 0 || i > 8)
            {
                throw new Exception($"雷赛卡初始化错误，返回值：{i}");
            }
            else
            {
                ushort _num = 0;
                ushort[] cardIDs = new ushort[8];
                uint[] cardTypes = new uint[8];
                short res = LTDMC.dmc_get_CardInfList(ref _num, cardTypes, cardIDs);

                if (res != 0)
                {
                    throw new Exception($"雷赛卡无法获取卡信息，错误代码：{res}");
                }

                foreach (ControllerListSource cls in
                IODataCollection.controllerNameList.Where(c => c.name.StartsWith(_name + '-')))
                {
                    string s = string.Join("，", cardIDs).TrimEnd('，');
                    if ((Array.IndexOf(cardIDs, ushort.Parse(cls.IP)) == -1) ||
                        ((int.Parse(cls.IP) == 0) && (cardIDs[0] != 0)))
                    {
                        throw new Exception($"雷赛卡卡号不存在：{cls.IP}，卡号列表:{s}");
                    }
                    LSu.Connect(cls.name, int.Parse(cls.IP));
                }
            }
        }

        // not in use, use acs's as master 
        // private static void IODataUpdate()
        // {
        //     while (true)
        //     {
        //         Thread.Sleep(10);
        //         IODataCollection.UpdateDataSetIOFromQueryList();
        //         IODataCollection.RemoveOldFromChangeDict();
        //     }
        // }

        private static void StopThreadLSQuery()
        {
            LSThreadAbort = true;
            int i = 0;
            while ((!LSThreadAborted) && (i < 10)) 
            {
                Thread.Sleep(100);
                i++;
            };
            try
            {
                LSQueryThread.Abort();
            }
            catch
            {

            }
            
            LSu.CloseComm();
            LSThreadAbort = false;
            LSThreadAborted = false;
        }        

        private static void StopThreadIODataUpdate()
        {
            IODataCollection.initJsonSent = false;            
            IODataCollection.ClearChangeDict();
            IODataUpdateThread.Abort();
        }
    }

    public static partial class LSUpdater
    {
        private static Thread LSQueryThread =new Thread(()=> { }), IODataUpdateThread=new Thread(()=> { });
        private static bool LSThreadAbort = false, LSThreadAborted = false;
        private static List<string> RunningTransaction = new List<string>();
        public static LSControllerUtilities LSu = new LSControllerUtilities();
        public const string _name = ControllerNames.LS;
    }
}
