using Protocol.code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NetManager : ManagerBase
{
    public static NetManager Instance = null;

    private void Awake()
    {
        Instance = this;
        Add(0, this);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case 0:
                client.Send(message as SocketMsg);
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        Connected(); 
    }

    //private ClientPeer client = new ClientPeer("115.159.4.33", 6666);
    private ClientPeer client = new ClientPeer("127.0.0.1", 6666);

    public void Connected()
    {
        client.Connect();
    }

    private void Update()
    {
        if (client == null)
            return;
        while(client.SocketMsgQueue.Count>0)
        {
            SocketMsg msg = client.SocketMsgQueue.Dequeue();
            ReciveSocketMsg(msg);
        }
    }

    #region 处理接收到的服务器发来的消息

    HandlerBase accountHandler = new AccoutHandler();
    HandlerBase userHandler = new UserHandler();

    private void ReciveSocketMsg(SocketMsg msg)
    {
        switch(msg.OpCode)
        {
            case OpCode.ACCOUNT:
                accountHandler.OnReceive(msg.SubCode,msg.Value);
                break;
            case OpCode.USER:
                userHandler.OnReceive(msg.SubCode,msg.Value);
                break;
            default:
                break;
        }
    }

    #endregion
}

