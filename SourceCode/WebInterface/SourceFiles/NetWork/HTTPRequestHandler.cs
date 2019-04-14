using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json.Linq;
using WebInterface.QueryUtilities;

namespace WebInterface
{
    static partial class HTTPRequestHandler
    {
        public static void HomePage(Socket response)
        {
            string statusline = "HTTP/1.1 200 OK\r\n";   //状态行
            byte[] statusline_to_bytes = Encoding.UTF8.GetBytes(statusline);            
            byte[] content_to_bytes = File.ReadAllBytes("WebInterfaceTest.html"); //Encoding.UTF8.GetBytes(content);

            string header = string.Format("Content-Type:text/html;charset=UTF-8\r\nContent-Length:{0}\r\n", content_to_bytes.Length);
            String modified = string.Format("Last-Modified:{0}\r\n", Utilities.ConvertDateTimeUTCNowToHTTPGMT());
            byte[] header_to_bytes = Encoding.UTF8.GetBytes(header);  //应答头
            byte[] modifiedToBytes = Encoding.UTF8.GetBytes(modified);

            response.Send(statusline_to_bytes);  //发送状态行
            response.Send(modifiedToBytes);
            response.Send(header_to_bytes);  //发送应答头
            response.Send(new byte[] { (byte)'\r', (byte)'\n' });  //发送空行
            response.Send(content_to_bytes);  //发送正文（html）
            response.Close();
        }

        public static void SendHeadFile(Socket response, String requestFileType, String requestFileName)
        {
            string statusline = "HTTP/1.1 200 OK\r\n";   //状态行
            byte[] statusline_to_bytes = Encoding.UTF8.GetBytes(statusline);

            byte[] content_to_bytes = File.ReadAllBytes(Directory.GetCurrentDirectory()+requestFileName.Replace('/','\\'));
            String contentTypeStr = "";
            switch (requestFileType)
            {
                case "css":contentTypeStr = "Content-Type:text/css;";
                    break;
                case "js":contentTypeStr = "Content-Type:text/javascript;";
                    break;
                case "img":contentTypeStr = "Content-Type:img/jpg;";
                    break;
                default:break;
            }

            string header = string.Format(contentTypeStr+ "charset=UTF-8\r\nContent-Length:{0}\r\n", content_to_bytes.Length);
            String modified = string.Format("Last-Modified:{0}\r\n", Utilities.ConvertDateTimeUTCNowToHTTPGMT());
            byte[] header_to_bytes = Encoding.UTF8.GetBytes(header);  //应答头
            byte[] modifiedToBytes = Encoding.UTF8.GetBytes(modified);

            response.Send(statusline_to_bytes);  //发送状态行
            response.Send(modifiedToBytes);
            response.Send(header_to_bytes);  //发送应答头
            response.Send(new byte[] { (byte)'\r', (byte)'\n' });  //发送空行
            response.Send(content_to_bytes);  //发送正文（html）
            response.Close();
        }

        public static void SendInitJson(Socket response)
        {
            string statusline = "HTTP/1.1 200 OK\r\n";   //状态行
            byte[] statusline_to_bytes = Encoding.UTF8.GetBytes(statusline);
            //byte[] content_to_bytes = File.ReadAllBytes(@"F:\Files\Temp\html test\德龙\json\WebInterfaceSimTest.json"); //Encoding.UTF8.GetBytes(content);          
            
            byte[] content_to_bytes = Encoding.UTF8.GetBytes(JSONHandler.SendInitialJSONFromDataCollection(out DateTime lastModifiedTimeutc));
            string header = string.Format("Content-Type:application/json;charset=UTF-8\r\nContent-Length:{0}\r\n", content_to_bytes.Length);
            String modified = string.Format("Last-Modified:{0}\r\n", Utilities.ConvertDateTimeUTCToHTTPGMT(lastModifiedTimeutc));
            byte[] header_to_bytes = Encoding.UTF8.GetBytes(header);  //应答头
            byte[] modifiedToBytes = Encoding.UTF8.GetBytes(modified);

            response.Send(statusline_to_bytes);  //发送状态行
            response.Send(modifiedToBytes);
            response.Send(header_to_bytes);  //发送应答头
            response.Send(new byte[] { (byte)'\r', (byte)'\n' });  //发送空行
            response.Send(content_to_bytes);  //发送正文（html）
            response.Close();

        }

        public static void AJAXAction(Socket response, String queryContent, DateTime lastModifiedSinceutc)
        {
            if (queryContent != "")
            {
                String[] sourceStr = queryContent.Split('&');
                String cName = sourceStr[0].Split('=')[1];
                String IOName = sourceStr[1].Split('=')[0];
                String aAction = sourceStr[1].Split('=')[1];

                switch (aAction)
                {
                    case "ON":
                    case "OFF":
                        IO.ChangeOnOFF(cName, IOName, aAction);
                        break;
                    default:
                        IO.ChangeCheckStatus(cName, IOName, aAction);
                        break;
                }
            }
            
            string statusline = "HTTP/1.1 200 OK\r\n";   //状态行
            byte[] statusline_to_bytes = Encoding.UTF8.GetBytes(statusline);
            byte[] content_to_bytes = Encoding.UTF8.GetBytes(JSONHandler.SendChangeJSONFromDataCollection(lastModifiedSinceutc, out DateTime lastModifiedTimeutc));
            string header = string.Format("Content-Type:application/json;charset=UTF-8\r\nContent-Length:{0}\r\nConnection: keep-alive\r\n", content_to_bytes.Length);
            String modified = string.Format("Last-Modified:{0}\r\n", Utilities.ConvertDateTimeUTCToHTTPGMT(lastModifiedTimeutc));
            byte[] header_to_bytes = Encoding.UTF8.GetBytes(header);  //应答头
            byte[] modifiedToBytes= Encoding.UTF8.GetBytes(modified);

            response.Send(statusline_to_bytes);  //发送状态行
            response.Send(modifiedToBytes);
            response.Send(header_to_bytes);  //发送应答头
            response.Send(new byte[] { (byte)'\r', (byte)'\n' });  //发送空行
            response.Send(content_to_bytes);  //发送正文（html）

        }
    }

    static partial class HTTPRequestHandler
    {
        public static String[] action = { };
        public static IOInterface IO;
    }
}
