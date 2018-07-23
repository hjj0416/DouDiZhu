using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AhpilyServer
{
    /// <summary>
    /// 封装的客户端的连接对象
    /// </summary>
    public class ClientPeer
    {
        public Socket ClientSocket { get; set; }

        private List<byte> dataCache = new List<byte>();

        public SocketAsyncEventArgs ReciveArgs;
    }
}
