using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using WebInterface.QueryUtilities;

namespace WebInterface
{
    public static partial class QPLCUpdater 
    {    
        public static bool CreateQuery()
        {
            IODataCollection.GenerateQueryList("QPLC");
            if ((IODataCollection.queryList.FindAll(c => c.name.StartsWith("QPLC-")).Count == 0) && (IODataCollection.controllerNameList.ToList().FindAll(c => c.name.StartsWith("QPLC-")).Count() != 0))
            {
                return false;
            }
            return true;
        }

        public static void StartUpdate()
        {
            try
            {
                QPLCConnect();
            }
            catch (Exception e)
            {
                qplcu.CloseComm();
                throw new Exception(e.Message);
            }

            StartThreadQPLCQuery();
        }

        public static void StopUpdate()
        {
            new Thread(()=> { StopThreadQPLCQuery(); }).Start();
            //StopThreadIODataUpdate();
        }

        public static void ForceStopUpdate()
        {
            if (QPLCQueryThread .IsAlive)
            {
                QPLCQueryThread .Abort();
            }
           
            qplcu.CloseComm();
            /*if (IODataUpdateThread.IsAlive)
            {
                IODataUpdateThread.Abort();
            }*/
            
        }

        private static void StartThreadQPLCQuery()
        {
            QPLCQueryThread  = new Thread(QPLCQuery);
            QPLCQueryThread.IsBackground = true;
            QPLCQueryThread.Start();
        }

        // not in use, use acs's as master 
        // private static void StatThreadIODataUpdate()
        // {
        //     IODataUpdateThread = new Thread(IODataUpdate);
        //     IODataUpdateThread.IsBackground = true;
        //     IODataUpdateThread.Start();
        // }

        private static void QPLCQuery()
        {
            foreach (ControllerListSource cls in IODataCollection.queryList.Where(c => c.name.StartsWith("QPLC-")))
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
                    List<int> tl = qplcu.ReadDevice(cname, queryList);
                    IODataCollection.UpdateIOQueryList(cname, tl);
                }
                catch(Exception e)
                {
                    string queryStr = string.Join(",", queryList);
                    String err = $"{e.Message} {cname}@{queryStr}";                    
                    IOInterface.updateError = true;
                    RunningTransaction.Remove(cname);
                    if (RunningTransaction.Count == 0)
                    {
                        QPLCThreadAborted = true;
                    }

                    IOInterface.ShowError(err);
                    return;
                }

                if (QPLCThreadAbort)
                {
                    RunningTransaction.Remove(cname);
                    if (RunningTransaction.Count == 0)
                    {
                        QPLCThreadAborted = true;
                    }
                    return;
                }
            }            
        }

        private static void QPLCConnect()
        {
            List<Task> tasks= new List<Task>();
            List<string> IPs=new List<string>();
            var tks=new CancellationTokenSource();
            var tk=tks.Token;
            foreach (ControllerListSource cls in IODataCollection.controllerNameList.Where(c=>c.name.StartsWith("QPLC-")))
            {
                tasks.Add(Task.Run(()=>qplcu.Connect(cls.name, int.Parse(cls.IP)),tk));
                IPs.Add(cls.IP);
            }
            Task.WaitAll(tasks.ToArray(),5000);
            int i=0;
            foreach (Task t in tasks)
            {
                if (!t.IsCompleted)
                {
                    tks.Cancel();
                    throw new Exception($"QPLC连接超时，站号：{IPs.ElementAt(i)}");
                }
                i++;
            }
        }

        //not in use, use acs's as master
        // private static void IODataUpdate()
        // {
        //     while (true)
        //     {
        //         Thread.Sleep(10);
        //         IODataCollection.UpdateDataSetIOFromQueryList();
        //         IODataCollection.RemoveOldFromChangeDict();
        //     }
        // }

        private static void StopThreadQPLCQuery()
        {
            QPLCThreadAbort = true;
            int i = 0;
            while ((!QPLCThreadAborted) && (i < 10)) 
            {
                Thread.Sleep(100);
                i++;
            };
            try
            {
                QPLCQueryThread.Abort();
            }
            catch
            {

            }
            
            qplcu.CloseComm();
            QPLCThreadAbort = false;
            QPLCThreadAborted = false;
        }        

        private static void StopThreadIODataUpdate()
        {
            IODataCollection.initJsonSent = false;            
            IODataCollection.ClearChangeDict();
            IODataUpdateThread.Abort();
        }
    }

    public static partial class QPLCUpdater
    {
        private static Thread QPLCQueryThread =new Thread(()=> { }), IODataUpdateThread=new Thread(()=> { });
        private static bool QPLCThreadAbort = false, QPLCThreadAborted = false;
        private static List<String> RunningTransaction = new List<String>();
        public static QPLCControllerUtilities qplcu = new QPLCControllerUtilities();
    }
}
