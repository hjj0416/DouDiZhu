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

        public ClientPeer()
        {
            this.ReciveArgs = new SocketAsyncEventArgs();           
            this.ReciveArgs.UserToken = this;
            this.ReciveArgs.SetBuffer(new byte[1024], 0, 1024);
            this.SendArgs = new SocketAsyncEventArgs();
            this.SendArgs.Completed += SendArgs_Completed;
        }

        public delegate void ReceiveCompleted(ClientPeer client,SocketMessage msg);

        /// <summary>
        /// 一个消息解析完成的回调
        /// </summary>
        public ReceiveCompleted reciveCompleted; 

        private List<byte> dataCache = new List<byte>();

        /// <summary>
        /// 接受异步套接字请求
        /// </summary>
        public SocketAsyncEventArgs ReciveArgs { get; set; }

        /// <summary>
        /// 是否正在处理接收的数据
        /// </summary>
        private bool IsReciveProcess = false;
        
        /// <summary>
        /// 自身处理数据包
        /// </summary>
        /// <param name="packet"></param>
        public void StartRecive(byte[] packet)
        {
            dataCache.AddRange(packet);
            if (!IsReciveProcess)
                processRecive();
        }

        private void processRecive()
        {
            IsReciveProcess = true;
            //解析数据包
            byte[] data=EncodeTool.DecodePacket(ref dataCache);

            if(data==null)
            {
                IsReciveProcess = false;
                return;
            }

            SocketMessage msg = EncodeTool.DecodeMsg(data);

            //回调给上层
            reciveCompleted?.Invoke(this, msg);

            processRecive();
        }

        #region 断开连接
        public void Disconnect()
        {
            //清空数据
            dataCache.Clear();
            IsReciveProcess = false;
            sendQueue.Clear();
            isSendProcess = false;

            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            ClientSocket = null;
        }
        #endregion

        #region 发送数据

        private Queue<byte[]> sendQueue = new Queue<byte[]>();

        private bool isSendProcess = false;
        /// <summary>
        /// 发送的异步套接字操作
        /// </summary> 
        private SocketAsyncEventArgs SendArgs;

        /// <summary>
        /// 发送时断开连接的回调
        /// </summary>
        /// <param name="client"></param>
        /// <param name="resaon"></param>
        public delegate void SendDisconnect(ClientPeer client, string resaon);

        public SendDisconnect sendDisconnect;

        public void Send(int opCode,int subCode,object value)
        {
            SocketMessage msg = new SocketMessage(opCode,subCode,value);
            byte[] data = EncodeTool.EncodeMsg(msg);
            byte[] packet = EncodeTool.EncodePacket(data);

            Send(packet);
        }

        public void Send(byte[] packet)
        {
            //存入消息队列
            sendQueue.Enqueue(packet);
            if (!isSendProcess)
                Send();
        }

        /// <summary>
        /// 处理发送的消息
        /// </summary>
        private void Send()
        {
            isSendProcess = true;
            if(sendQueue.Count==0)
            {
                isSendProcess = false;
                return;
            }
            byte[] packet = sendQueue.Dequeue();
            SendArgs.SetBuffer(packet,0,packet.Length);
            bool result=ClientSocket.SendAsync(SendArgs);
            if(result==false)
            {
                processSend();    
            }
        }

        private void SendArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            processSend();
        }

        /// <summary>
        /// 当发送完成时调用
        /// </summary>
        private void processSend()
        {
            if(SendArgs.SocketError!=SocketError.Success)
            {
                sendDisconnect(this,SendArgs.SocketError.ToString());
            }else
            {
                Send();
            }
        }
        #endregion
    }
}
