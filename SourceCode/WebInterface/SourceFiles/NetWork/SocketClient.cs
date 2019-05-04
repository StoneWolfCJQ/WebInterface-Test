using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using WebInterface.QueryUtilities;

namespace WebInterface
{
    public class StateObject
    {
        public Socket workSocket = null;
        public const int BUFFER_SIZE = 1024*640;
        public byte[] buffer = new byte[BUFFER_SIZE];
        public StringBuilder sb = new StringBuilder();
    }

    partial class SocketClient
    {
        public SocketClient(Socket _clientSocket)
        {
            clientSocket = _clientSocket;
            BeginCommunicate();
        }

        public void StopListen()
        {
            try
            {
                clientSocket.Disconnect(false);
                clientSocket.Close();
            }
            catch
            {

            }
        }

        public void BeginCommunicate()
        {
            StateObject so = new StateObject();
            so.workSocket = clientSocket;
            clientSocket.BeginReceive(so.buffer, 0, StateObject.BUFFER_SIZE, 0, new AsyncCallback(ReceiveCallBack), so);
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            StateObject so = (StateObject)ar.AsyncState;
            Socket s = so.workSocket;

            try
            {
                Int32 read = s.EndReceive(ar);

                if (read > 0)
                {
                    String str = Encoding.UTF8.GetString(so.buffer, 0, read);
                    Console.WriteLine(str);  //将请求显示到界面
                    Resolve(str, clientSocket);  //解析、路由、处理
                    if (s.Connected)
                        s.BeginReceive(so.buffer, 0, StateObject.BUFFER_SIZE, 0, new AsyncCallback(ReceiveCallBack), so);
                    else
                        s.Close();
                }
            }
            catch (Exception e)
            {
                string ss = e.Message;
                s.Close();
            }
        }

        /// <summary>
        /// 按照HTTP协议格式 解析浏览器发送的请求字符串
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private void Resolve(string request, Socket response)
        {
            //浏览器发送的请求字符串request格式类似这样：
            //GET /index.html HTTP/1.1
            //Host: 127.0.0.1:8081
            //Connection: keep-alive
            //Cache-Control: max-age=0
            //If-Modified-Since: Mon, 26 Apr 2010 13:22:17 GMT
            //
            //id=123&pass=123       （post方式提交的表单数据，get方式提交数据直接在url中）

            string[] strs = request.Split(new string[] { "\r\n" }, StringSplitOptions.None);  //以“换行”作为切分标志
            if (strs.Length > 0)  //解析出请求路径、post传递的参数(get方式传递参数直接从url中解析)
            {
                string[] items = strs[0].Split(' ');  //items[1]表示请求url中的路径部分（不含主机部分）
                Dictionary<string, string> param = new Dictionary<string, string>();// 解析头部其他部分，比如Cookie, Last Modified等
                foreach (String sl in strs)
                {
                    if (sl.Contains(':'))
                    {
                        String key = sl.Substring(0, sl.IndexOf(':'));
                        String value = sl.Substring(sl.IndexOf(':') + 1);
                        param.Add(key, value);
                    }
                }
                
                Route(items[1], param, response);  //路由处理
            }
        }

        /// <summary>
        /// 按照请求路径（不包括主机部分）  进行路由处理
        /// </summary>
        /// <param name="path"></param>
        /// <param name="param"></param>
        /// <param name="response"></param>
        void Route(string path, Dictionary<string, string> param, Socket response)
        {
            String firstDir = path.Split('/')[1];
            if (path.EndsWith("index.html") || path.EndsWith("/"))  //请求首页
            {
                HTTPRequestHandler.HomePage(response);
            }
            else if ((firstDir != "init") && (!firstDir.StartsWith("query")))
            {
                String requestFileType = firstDir;
                HTTPRequestHandler.SendHeadFile(response, requestFileType, path);
            }
            else if (firstDir == "init")
            {
                HTTPRequestHandler.SendInitJson(response);
            }
            else if (firstDir.StartsWith("query"))
            {
                String[] queryStr = firstDir.Split('?');
                String queryContent = queryStr[1];
                String lastModifiedSinceutcStr;
                if (param.ContainsKey("If-Modified-Since"))
                {
                    lastModifiedSinceutcStr = param["If-Modified-Since"].TrimStart(' ');
                }
                else
                {
                    lastModifiedSinceutcStr = DateTime.MinValue.ToString("O");
                }
                DateTime lastModifiedSinceutc = Utilities.ConvertRoundTripTimeDateTimeUTC(lastModifiedSinceutcStr);
                HTTPRequestHandler.AJAXAction(response, queryContent, lastModifiedSinceutc);
            }
        }
    }

    partial class SocketClient
    {
        Socket clientSocket;
    }
}
