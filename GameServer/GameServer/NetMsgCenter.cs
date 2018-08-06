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
        IHandler user = new UserHandler();

        public void OnDisconnect(ClientPeer client)
        {
            user.OnDisconnect(client);
            account.OnDisconnect(client);
        }

        public void OnRecive(ClientPeer client, SocketMessage msg)
        {
            switch (msg.OpCode)
            {
                case OpCode.ACCOUNT:
                    account.OnRecive(client,msg.SubCode,msg.Value);
                    break;
                case OpCode.USER:
                    user.OnRecive(client,msg.SubCode,msg.Value);
                    break;
                default:
                    break;
            }
        }
    }
}
