using AhpilyServer;
using GameServer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol.code;

namespace GameServer
{
    public class NetMsgCenter : IApplication
    {
        IHandler account = new AccountHandler();
        public void OnDisconnect(ClientPeer client)
        {
            account.OnDisconnect(client);
        }

        public void OnRecive(ClientPeer client, SocketMessage msg)
        {
            switch (msg.OpCode)
            {
                case OpCode.ACCOUNT:
                    account.OnRecive(client,msg.SubCode,msg.Value);
                    break;
                default:
                    break;
            }
        }
    }
}
