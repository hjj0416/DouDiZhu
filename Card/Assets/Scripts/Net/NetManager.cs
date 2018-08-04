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
    }

    private void Start()
    {
        Connected("127.0.0.1",6666);
    }

    private ClientPeer client;

    public void Connected(string ip,int port)
    {
        client = new ClientPeer(ip,port);
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

