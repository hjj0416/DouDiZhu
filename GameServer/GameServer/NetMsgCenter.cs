using AhpilyServer;
using GameServer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol.Code;

namespace GameServer
{
    public class NetMsgCenter : IApplication
    {
        IHandler account = new AccountHandler();
        IHandler user = new UserHandler();
        MatchHandler match = new MatchHandler();
        IHandler chat = new ChatHandler();
        FightHandler fight = new FightHandler();

        public NetMsgCenter()
        {
            match.startFight += fight.startFight;
        }

        public void OnDisconnect(ClientPeer client)
        {
            fight.OnDisconnect(client);
            chat.OnDisconnect(client);
            match.OnDisconnect(client);
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
                case OpCode.MATCH:
                    match.OnRecive(client,msg.SubCode,msg.Value);
                    break;
                case OpCode.CHAT:
                    chat.OnRecive(client,msg.SubCode,msg.Value);
                    break;
                case OpCode.FIGHT:
                    chat.OnRecive(client,msg.SubCode,msg.Value);
                    break;
                default:
                    break;
            }
        }
    }
}
