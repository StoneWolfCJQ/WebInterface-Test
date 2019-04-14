using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WebInterface
{
    public partial class SocketListener
    {
        public SocketListener(IPEndPoint IPAddressProg)
        {
            listenSocket.Bind(IPAddressProg);
        }

        public void StartListen()
        {
            listenSocket.Listen(100);
            listenSocket.BeginAccept(new AsyncCallback(OnAccept), listenSocket);
        }

        public void StopListen()
        {
            listenSocket.Close();
            ClientClose();
        }

        private void ClientClose()
        {
            foreach (SocketClient sc in clientList)
            {
                sc.StopListen();
            }
        }

        ~SocketListener()
        {
            listenSocket.Close();
        }
        private void OnAccept(IAsyncResult ar)
        {
            Socket server = ar.AsyncState as Socket;
            try
            {
                Socket clientSocket = server.EndAccept(ar);
                SocketClient client = new SocketClient(clientSocket);
                server.BeginAccept(new AsyncCallback(OnAccept), server);
                clientList.Add(client);
            }
            catch
            {
                return;
            }
        }
    }

    public partial class SocketListener
    {
        Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<SocketClient> clientList = new List<SocketClient>();
    }


}
