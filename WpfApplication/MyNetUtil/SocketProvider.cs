using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MyNetUtil {
    public class SocketProvider {
        private Socket serverSocketListener;  //Socket监听处理请求
        private Thread listenThread;    //监听socket 线程
        Dictionary<string, Socket> dict = new Dictionary<string, Socket>();//存放套接字
        Dictionary<string, Thread> dictThread = new Dictionary<string, Thread>();//存放线程

        public void start(int port) {
            //本机预使用的IP和端口
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            serverSocketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocketListener.Bind(ipep);
            serverSocketListener.Listen(10);
            listenThread = new Thread(listenConnecting);
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void listenConnecting() {
            while (true) {
                Socket client = serverSocketListener.Accept();
                var sss = client.RemoteEndPoint.ToString().Split(':');
                dict.Add(client.RemoteEndPoint.ToString(), client);
                Thread receivingThread = new Thread(reciveMsg);
                dictThread.Add(client.RemoteEndPoint.ToString(), receivingThread);
                receivingThread.IsBackground = true;
                receivingThread.Start(client);
            }
        }

        private void reciveMsg(object obj) {
            Socket client = (Socket)obj;
            while (true) {
                byte[] buffer = new byte[1024];
                int length = -1;
                try {
                    length = client.Receive(buffer);
                    if (length > 0) {
                        //处理业务
                        string msg = Encoding.ASCII.GetString(buffer, 0, length);
                        byte[] content = Encoding.ASCII.GetBytes("服务器返回：" + msg);
                        client.Send(content, SocketFlags.None);
                    } else {
                        dict.Remove(client.RemoteEndPoint.ToString());
                        dictThread.Remove(client.RemoteEndPoint.ToString());
                        break;
                    }
                } catch {
                    dict.Remove(client.RemoteEndPoint.ToString());
                    dictThread.Remove(client.RemoteEndPoint.ToString());
                    break;
                } 
            }
        }

    }
}
