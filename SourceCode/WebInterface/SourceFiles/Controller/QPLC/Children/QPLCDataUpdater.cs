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

        public static void StartUpdate(out bool result)
        {
            result = true;
            bool result2;
            try
            {
                result2 = QPLCConnect();
            }
            catch (Exception e)
            {
                String err = e.Message;
                IOInterface.ShowError(err);
                qplcu.CloseComm();
                result = false;
                return;
            }

            if (result2)
            {
                StartThreadQPLCQuery();
            }
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

        private static void StatThreadIODataUpdate()
        {
            IODataUpdateThread = new Thread(IODataUpdate);
            IODataUpdateThread.IsBackground = true;
            IODataUpdateThread.Start();
        }

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

        private static bool QPLCConnect()
        {
            bool result = false;
            foreach (ControllerListSource cls in IODataCollection.controllerNameList.Where(c=>c.name.StartsWith("QPLC-")))
            {
                qplcu.Connect(cls.name, int.Parse(cls.IP));
                result = true;
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
