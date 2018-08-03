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
    /// 服务器端
    /// </summary>
    public class ServerPeer
    {
        /// <summary>
        /// 服务器端的socket对象
        /// </summary>
        private Socket serverSocket;

        /// <summary>
        /// 限制客户端连接数量的信号量
        /// </summary>
        private Semaphore acceptSemaphore;

        /// <summary>
        /// 客户端对象的连接池
        /// </summary>
        private ClientPeerPool clientPeerPool;

        /// <summary>
        /// 设置应用层
        /// </summary>
        private IApplication application;

        public void SetApplication(IApplication app)
        {
            this.application = app;
        }

        /// <summary>
        /// 用来开启服务器
        /// </summary>
        /// <param name="port">端口号</param>
        ///<param name="maxCount">最大连接数量</param>
        public void Start(int port, int maxCount)
        {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                acceptSemaphore = new Semaphore(maxCount, maxCount);

                //直接new出最大数量的连接对象
                clientPeerPool = new ClientPeerPool(maxCount);
                ClientPeer tmpClientPeer = null;
                for (int i = 0; i < maxCount; i++)
                {
                    tmpClientPeer = new ClientPeer();
                    tmpClientPeer.ReciveArgs = new SocketAsyncEventArgs();
                    tmpClientPeer.ReciveArgs.Completed += recive_Completed;
                    tmpClientPeer.ReciveArgs.UserToken = tmpClientPeer;
                    tmpClientPeer.reciveCompleted += reciveCompleted;
                    tmpClientPeer.sendDisconnect = Disconnect;
                    clientPeerPool.Enqueue(tmpClientPeer);
                }

                serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                serverSocket.Listen(10);

                Console.WriteLine("服务器启动...");

                startAccept(null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #region 接收客户端的连接

        /// <summary>
        /// 开始等待客户端的连接
        /// </summary>
        private void startAccept(SocketAsyncEventArgs e)
        {
            if (e == null)
            {
                e = new SocketAsyncEventArgs();
                e.Completed += accept_Completed;
            }

            bool result = serverSocket.AcceptAsync(e);
            //返回值判断异步事件是否执行完毕 如果返回了true 代表正在执行 执行完毕后会触发
            //                                 如果返回false 代表已经执行完成 直接处理
            if (result == false)
            {
                processAccept(e);
            }
        }

        /// <summary>
        /// 接受连接请求异步事件完成时候触发
        /// </summary>
        private void accept_Completed(object sender, SocketAsyncEventArgs e)
        {
            processAccept(e);
        }

        /// <summary>
        /// 处理连接请求
        /// </summary>
        private void processAccept(SocketAsyncEventArgs e)
        {
            //限制线程的访问
            acceptSemaphore.WaitOne();

            //得到客户端的对象 
            ClientPeer client = clientPeerPool.Dequeue();
            client.ClientSocket=e.AcceptSocket;
            //再进行保存处理

            e.AcceptSocket = null;
            startAccept(e);
        }

        #endregion

        /// <summary>
        /// 开始接收数据
        /// </summary>
        /// <param name="client"></param>
        #region 接收数据
        private void startRecive(ClientPeer client)
        {
            try
            {
               bool result=client.ClientSocket.ReceiveAsync(client.ReciveArgs);
                if(result==false)
                {
                    processRecive(client.ReciveArgs);
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 处理接受的请求
        /// </summary>
        private void processRecive(SocketAsyncEventArgs e)
        {
            ClientPeer client = e.UserToken as ClientPeer;
            //判断网络消息是否接收成功&&有值
            if (client.ReciveArgs.SocketError == SocketError.Success && client.ReciveArgs.BytesTransferred > 0)
            {
                byte[] packet = new byte[client.ReciveArgs.BytesTransferred];
                Buffer.BlockCopy(client.ReciveArgs.Buffer,0, packet, 0,client.ReciveArgs.BytesTransferred);
                //让客户端自身处理数据包
                client.StartRecive(packet);
                //尾递归
                startRecive(client);
            }
            else if (client.ReciveArgs.BytesTransferred == 0)
            {
                if(client.ReciveArgs.SocketError==SocketError.Success)
                {
                    //客户端主动断开连接
                    Disconnect(client, "客户端主动断开连接");
                }
                else
                {
                    //网络异常被动断开连接
                    Disconnect(client, client.ReciveArgs.SocketError.ToString());
                }
            }
        }

        /// <summary>
        /// 当接受完成时触发的事件
        /// </summary>
        /// <param name="e"></param>
        private void recive_Completed(object sender, SocketAsyncEventArgs e)
        {
            processRecive(e);
        }

        /// <summary>
        /// 一条数据解析完成的处理
        /// </summary>
        /// <param name="clientPeer"></param>
        /// <param name="value"></param>
        private void reciveCompleted(ClientPeer clientPeer,SocketMessage msg)
        {
            application.OnRecive(clientPeer,msg);

        }
        #endregion

        #region 断开连接
        public void Disconnect(ClientPeer client,string reason)
        {
            try
            {
                if (client == null)
                    throw new Exception("当前指定的客户端连接为空，无法断开连接！");
                application.OnDisconnect(client);

                //通知应用层
                client.Disconnect();
                clientPeerPool.Enqueue(client);
                acceptSemaphore.Release();
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region 发送数据

        #endregion

    }
}
