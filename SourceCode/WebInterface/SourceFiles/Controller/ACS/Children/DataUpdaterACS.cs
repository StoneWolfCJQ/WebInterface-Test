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

        public static void StartUpdate(out bool result)
        {
            result = true;
            bool result2;
            try
            {
                result2 = ACSConnect();
            }
            catch (Exception e)
            {
                String err = e.Message;
                IOInterface.ShowError(err);
                acsu.CloseComm();
                result = false;
                return;
            }
            if (result2)
            {
                StartThreadACSQuery();                
            }
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
                String queryStr = ACSQueryUtilities.GenACSSimQueryStr(cls.IOList);
                acsu.BeginTransaction(cls.name, queryStr, new AsyncCallback(OnTransaction), acsu);
                RunningTransaction.Add(cls.name);
            }
        }

        private static void OnTransaction(IAsyncResult ar)
        {
            object[] os = (object[])ar.AsyncState;
            String cname = (String)os[0];
            ACSControllerUtilities _acsu = (ACSControllerUtilities)os[1];
            String queryStr = ACSQueryUtilities.GenACSSimQueryStr(IODataCollection.queryList.Find(cls => cls.name == cname).IOList);
            try
            {
                String transStr = _acsu.EndTransaction(ar);
                List<int> tl = ACSQueryUtilities.ConvertACSTransStr(transStr, out bool result);
                if (!result)
                {
                    String err = $"变量名错误或未定义：{cname}@{queryStr}=>{transStr}";                    
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
                String err = $"{e.Message}：{cname}@{queryStr}";
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

        private static bool ACSConnect()
        {
            bool result = false;
            foreach (ControllerListSource cls in IODataCollection.controllerNameList.Where(c=>c.name.StartsWith("ACS-")))
            {
                result = true;
                if ( IODataCollection.GetControllerName(cls.name) == ACSBaseConfig.sim)
                {
                    acsu.SimConnect();
                    continue;
                }
                acsu.TCPConnect(cls.name, cls.IP);
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
        private static List<String> RunningTransaction = new List<String>();
        public static ACSControllerUtilities acsu = new ACSControllerUtilities();
    }
}
