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
                client.Send(message as SocketMessage);
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        Connected(); 
    }

    private ClientPeer client = new ClientPeer("127.0.0.1", 6666);

    public void Connected()
    {
        client.Connect();
    }

    private void Update()
    {
        if (client == null)
            return;
        while(client.socketMsgList.Count>0)
        {
            SocketMessage msg = client.socketMsgList.Dequeue();
            //TODO 操作这个msg
        }
    }
}

