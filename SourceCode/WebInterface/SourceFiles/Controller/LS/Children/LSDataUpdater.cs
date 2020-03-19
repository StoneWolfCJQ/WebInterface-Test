using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using WebInterface.QueryUtilities;

namespace WebInterface
{
    public static partial class LSUpdater 
    {    
        public static bool CreateQuery()
        {
            IODataCollection.GenerateQueryList("LS");
            if ((IODataCollection.queryList.FindAll(c => c.name.StartsWith("LS-")).Count == 0) && (IODataCollection.controllerNameList.ToList().FindAll(c => c.name.StartsWith("LS-")).Count() != 0))
            {
                return false;
            }
            return true;
        }

        public static void StartUpdate(out bool result)
        {
            result = true;
            bool result2;
            try
            {
                result2 = LSConnect();
            }
            catch (Exception e)
            {
                String err = e.Message;
                IOInterface.ShowError(err);
                LSu.CloseComm();
                result = false;
                return;
            }

            if (result2)
            {
                StartThreadLSQuery();
            }
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

        private static void StatThreadIODataUpdate()
        {
            IODataUpdateThread = new Thread(IODataUpdate);
            IODataUpdateThread.IsBackground = true;
            IODataUpdateThread.Start();
        }

        private static void LSQuery()
        {
            foreach (ControllerListSource cls in IODataCollection.queryList.Where(c => c.name.StartsWith("LS-")))
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
                    String err = $"{e.Message} {cname}@{queryStr}";                    
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

        private static bool LSConnect()
        {
            bool result = true;
            int i = LTDMC.dmc_board_init();
            if (i <= 0 || i > 8)
            {
                result=false;
            }

            return result;
        }

        private static void IODataUpdate()
        {
            while (true)
            {
                Thread.Sleep(10);
                IODataCollection.UpdateDataSetIOFromQueryList();
                IODataCollection.RemoveOldFromChangeDict();
            }
        }

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
        private static List<String> RunningTransaction = new List<String>();
        public static LSControllerUtilities LSu = new LSControllerUtilities();
    }
}
