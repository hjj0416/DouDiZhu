using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class ClientPeer {

    private Socket socket;

    private string ip;
    private int port;

    public ClientPeer(string ip,int port)
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            this.ip = ip;
            this.port = port;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            throw;
        }
    }

    public void Connect()
    {
        try
        {
            socket.Connect(ip, port);
            Debug.Log("连接服务器成功！");
            StartRecive();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    #region 接收数据
    //接收数据缓冲区
    private byte[] reciveBuffer = new byte[1024];

    private List<byte> dataCache = new List<byte>();

    private bool isProcessRecive = false;

    public Queue<SocketMessage> socketMsgList = new Queue<SocketMessage>();

    //开始异步接收数据
    private void StartRecive()
    {
        if(socket==null&&socket.Connected)
        {
            Debug.Log("没有连接，无法发送数据");
            return;
        }

        socket.BeginReceive(reciveBuffer,0,1024,SocketFlags.None,recevideCallBack,socket);
    }

    /// <summary>
    /// 收到消息的回调
    /// </summary>
    /// <param name="ar"></param>
    private void recevideCallBack(IAsyncResult ar)
    {
        try
        {
            int length = socket.EndReceive(ar);
            byte[] tmpByteArray = new byte[length];
            Buffer.BlockCopy(reciveBuffer,0,tmpByteArray,0,length);
            //处理收到的数据
            dataCache.AddRange(tmpByteArray);
            if (isProcessRecive == false)
                processRecive();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            throw;
        }
    }

    private void processRecive()
    {
        isProcessRecive = true;
        byte[] data=EncodeTool.DecodePacket(ref dataCache);

        if(dataCache==null)
        {
            isProcessRecive = false;
            return;
        }

        SocketMessage msg = EncodeTool.DecodeMsg(data);
        socketMsgList.Enqueue(msg);

        //尾递归
        processRecive();
    }

    #endregion

    #region 发送数据
    public void Send(int opCode,int subCode,object value)
    {
        SocketMessage msg = new SocketMessage(opCode,subCode,value);
        Send(msg);
        
    }
    public void Send(SocketMessage msg)
    {
        byte[] data = EncodeTool.EncodeMsg(msg);
        byte[] packet = EncodeTool.EncodePacket(data);
        try
        {
            socket.Send(packet);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    #endregion
}
