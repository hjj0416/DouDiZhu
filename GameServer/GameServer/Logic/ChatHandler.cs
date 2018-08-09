using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using GameServer.Cache;
using GameServer.Cache.Match;
using Protocol.Code;
using Protocol.Constant;
using Protocol.Dto;

namespace GameServer.Logic
{
    public class ChatHandler : IHandler
    {
        private UserCache userCache = Caches.User;
        private MatchCache matchCache = Caches.Match;

        public void OnDisconnect(ClientPeer client)
        {
            
        }

        public void OnRecive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case ChatCode.CREQ:
                    chatRequest(client,(int)value);
                    break;
                default:
                    break;
            }
        }

        private void chatRequest(ClientPeer client,int chatType)
        {
            if (userCache.IsOnline(client) == false)
                return;
            int userId = userCache.GetId(client);
            ChatDto chatDto = new ChatDto(userId,chatType);
            if(matchCache.IsMatching(userId))
            {
                MatchRoom mRoom = matchCache.GetRoom(userId);
                mRoom.Brocast(OpCode.CHAT,ChatCode.SERS, chatDto);
                Console.WriteLine("快捷喊话："+chatDto);
            }
            else if(false)
            {
                //检测战斗房间
            }
        }
    }
}
