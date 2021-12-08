using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using WebInterface.QueryUtilities;

namespace WebInterface
{
    public static partial class ACSUpdater 
    {    
        public static bool CreateQuery()
        {
            IODataCollection.GenerateQueryList("ACS");
            if ((IODataCollection.queryList.FindAll(c => c.name.StartsWith("ACS-")).Count() == 0) && (IODataCollection.controllerNameList.ToList().FindAll(c=>c.name.StartsWith("ACS-")).Count()!=0))
            {
                return false;
            }
            return true;
        }

        public static void Connect(CancellationToken ct)
        {
            try
            {
                ACSConnect(ct);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void StartUpdate()
        {
            StartThreadACSQuery();
            StatThreadIODataUpdate();
        }

        public static void StopUpdate()
        {
            StopThreadACSQuery();
            StopThreadIODataUpdate();
        }

        public static void ForceStopUpdate()
        {
            if (ACSQueryThread.IsAlive)
            {
                ACSQueryThread.Abort();
            }
           
            acsu.CloseComm();
            if (IODataUpdateThread.IsAlive)
            {
                IODataUpdateThread.Abort();
            }
            
        }

        private static void StartThreadACSQuery()
        {
            ACSQueryThread = new Thread(ACSQuery);
            ACSQueryThread.IsBackground = true;
            ACSQueryThread.Start();
        }

        private static void StatThreadIODataUpdate()
        {
            IODataUpdateThread = new Thread(IODataUpdate);
            IODataUpdateThread.IsBackground = true;
            IODataUpdateThread.Start();
        }

        private static void ACSQuery()
        {
            foreach (ControllerListSource cls in IODataCollection.queryList.Where(c => c.name.StartsWith("ACS-")))
            {
                string queryStr = ACSQueryUtilities.GenACSSimQueryStr(cls.IOList);
                acsu.BeginTransaction(cls.name, queryStr, new AsyncCallback(OnTransaction), acsu);
                RunningTransaction.Add(cls.name);
            }
        }

        private static void OnTransaction(IAsyncResult ar)
        {
            object[] os = (object[])ar.AsyncState;
            string cname = (string)os[0];
            ACSControllerUtilities _acsu = (ACSControllerUtilities)os[1];
            string queryStr = ACSQueryUtilities.GenACSSimQueryStr(IODataCollection.queryList.Find(cls => cls.name == cname).IOList);
            try
            {
                string transStr = _acsu.EndTransaction(ar);
                List<int> tl = ACSQueryUtilities.ConvertACSTransStr(transStr, out bool result);
                if (!result)
                {
                    string err = $"变量名错误或未定义：{cname}@{queryStr}=>{transStr}";                    
                    IOInterface.updateError = true;
                    RunningTransaction.Remove(cname);
                    IOInterface.ShowError(err);
                    return;
                }
                IODataCollection.UpdateIOQueryList(cname, tl);
                if (!ACSThreadAbort)
                {
                    Thread.Sleep(50);
                    _acsu.BeginTransaction(cname, queryStr, new AsyncCallback(OnTransaction), _acsu);
                }
                else
                {
                    RunningTransaction.Remove(cname);
                    if (RunningTransaction.Count == 0)
                    {
                        ACSThreadAborted = true;
                    }
                }
            }
            catch (Exception e)
            {
                string err = $"{e.Message}：{cname}@{queryStr}";
                IOInterface.updateError = true;
                RunningTransaction.Remove(cname);
                if (RunningTransaction.Count == 0)
                {
                    ACSThreadAborted = true;
                }

                IOInterface.ShowError(err);
            }
            GC.Collect();
            
        }

        private static void ACSConnect(CancellationToken ct)
        {
            foreach (ControllerListSource cls in IODataCollection.controllerNameList.Where(c=>c.name.StartsWith("ACS-")))
            {
                ACSConnectTask(cls.name, cls.IP, ct);
                if (ct.IsCancellationRequested)
                {
                    return;
                }
            }
        }

        static void ACSConnectTask(string cname, string IP, CancellationToken ct)
        {
            bool exception = false;
            string eMessage = "";
            int i = 0;
            ManualResetEvent mre = new ManualResetEvent(false);
            Thread t = new Thread(() => {
                try
                {
                    if (IODataCollection.GetControllerName(cname) == ACSBaseConfig.sim)
                    {
                        acsu.SimConnect();
                    }
                    else
                    {
                        acsu.TCPConnect(cname, IP);
                    }
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
                        throw new Exception($"ACS连接超时，IP：{IP}");
                    }
                    return;
                }
            }
            if (exception)
            {
                throw new Exception($"ACS连接错误，IP：{IP}，代码：{eMessage}");
            }

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

        private static void StopThreadACSQuery()
        {
            ACSThreadAbort = true;
            int i = 0;
            while ((!ACSThreadAborted) && (i < 10)) 
            {
                Thread.Sleep(100);
                i++;
            };
            try
            {
                ACSQueryThread.Abort();
            }
            catch
            {

            }
            
            acsu.CloseComm();
            ACSThreadAbort = false;
            ACSThreadAborted = false;
        }        

        private static void StopThreadIODataUpdate()
        {
            IODataCollection.initJsonSent = false;            
            IODataCollection.ClearChangeDict();
            IODataUpdateThread.Abort();
        }
    }

    public static partial class ACSUpdater
    {
        private static Thread ACSQueryThread=new Thread(()=> { }), IODataUpdateThread=new Thread(()=> { });
        private static bool ACSThreadAbort = false, ACSThreadAborted = false;
        private static List<string> RunningTransaction = new List<string>();
        public static ACSControllerUtilities acsu = new ACSControllerUtilities();
    }
}
