﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ACS.SPiiPlusNET;
using System.Runtime.Remoting.Messaging;

namespace WebInterface
{
    public partial class ACSControllerUtilities
    {
        public ACSControllerUtilities()
        {
            aList = new Dictionary<string, Api>();
        }

        public ACSControllerUtilities(ACSControllerUtilities _acsu)
        {
            aList = new Dictionary<string, Api>(_acsu.aList);
        }

        ~ACSControllerUtilities()
        {
            aList = null;
        }

        public void CloseComm()
        {
            foreach (Api a in aList.Values)
            {
                a.CloseComm();
            }
            aList.Clear();
        }

        public string Transaction(string cname, string queryStr)
        {
            return aList[cname].Transaction(queryStr);
        }

        public void SimConnect()
        {
            simApi = new Api();
            simApi.OpenCommSimulator();
            aList.Add(IODataCollection.MergeTypeNameAndControllerName("ACS",ACSBaseConfig.sim), simApi);
        }

        public void TCPConnect(string cname, string IP)
        {
            Api tapi = new Api();
            tapi.OpenCommEthernetTCP(IP, 701);
            aList.Add(cname, tapi);
        }

        public void BeginTransaction(string cname, string queryStr, AsyncCallback ac, ACSControllerUtilities _acsu)
        {
            ACSControllerUtilities acsu = new ACSControllerUtilities(_acsu);
            AsyncTransactionDel tdel = new AsyncTransactionDel(Transaction);
            IAsyncResult result = tdel.BeginInvoke(cname, queryStr, new AsyncCallback(ac), new object[] { cname, acsu});
        }

        public string EndTransaction(IAsyncResult ar)
        {
            AsyncResult ra = (AsyncResult)ar;
            AsyncTransactionDel tdel = (AsyncTransactionDel)ra.AsyncDelegate;
            string s= tdel.EndInvoke(ar);
            return s;
        }
    }

    public partial class ACSControllerUtilities
    {
        public Dictionary<string, Api> aList;
        public Api simApi;
        private bool stopUpdate = false;
        private delegate string AsyncTransactionDel(string cname, string queryStr);
    }
}
