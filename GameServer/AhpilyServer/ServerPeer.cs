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

            //限制线程的访问
            acceptSemaphore.WaitOne();

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
            //得到客户端的对象 
            //Socket clientSocket = e.AcceptSocket;
            ClientPeer client = clientPeerPool.Dequeue();
            client.ClientSocket=e.AcceptSocket;
            //再进行保存处理
            //TODO 一直接受客户端发来的数据

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
                client.ClientSocket.ReceiveAsync(client.ReciveArgs);
            }catch(Exception e)
            {
                throw;
            }
        }
        #endregion

        #region 断开连接

        #endregion

        #region 发送数据

        #endregion

    }
}
