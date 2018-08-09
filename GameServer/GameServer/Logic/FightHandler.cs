using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using GameServer.Cache;
using GameServer.Cache.Fight;
using GameServer.Model;
using Protocol.Code;
using Protocol.Dto.Fight;

namespace GameServer.Logic
{
    public class FightHandler : IHandler
    {
        public FightCache fightCache = Caches.fight;
        public UserCache userCache = Caches.User;

        public void OnDisconnect(ClientPeer client)
        {
            leave(client);
        }

        public void OnRecive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case FightCode.GRAB_LANDLORD_CERQ:
                    bool result = (bool)value;
                    grabLandlord(client,result);
                    break;
                case FightCode.DEAL_CREQ:
                    deal(client,value as DealDto);
                    break;
                case FightCode.PASS_CREQ:
                    pass(client);
                    break;
                default:
                    break;
            }
        }

        private void leave(ClientPeer client)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (!userCache.IsOnline(client))
                    return;
                int userId = userCache.GetId(client);
                FightRoom fightRoom = fightCache.GetRoomByUId(userId);

                //中途退出
                fightRoom.LeaceUIdList.Add(userId);
                Brocast(fightRoom,OpCode.FIGHT,FightCode.LEAVE_BRO,userId);

                if(fightRoom.LeaceUIdList.Count==3)
                {
                    //给逃跑玩家添加逃跑场次
                    for (int i = 0; i < fightRoom.LeaceUIdList.Count; i++)
                    {
                        UserModel um = userCache.GetModelById(fightRoom.LeaceUIdList[i]);
                        um.RunCount++;
                        um.Been -= fightRoom.Multiple*1000 * 3;
                        um.Exp += 0;
                        userCache.Update(um);
                    }
                    //销毁缓存层的房间数据
                    fightCache.Destroy(fightRoom);
                }
            });
        }

        /// <summary>
        /// 出牌的处理
        /// </summary>
        private void deal(ClientPeer client,DealDto dto)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (!userCache.IsOnline(client))
                    return;
                int userId = userCache.GetId(client);
                FightRoom fightRoom = fightCache.GetRoomByUId(userId);

                //玩家如果掉线退出
                if(fightRoom.LeaceUIdList.Contains(userId))
                {
                    //直接跳过
                    //TODO
                }
                bool canDel= fightRoom.DeadCard(dto.Type, dto.Weight, dto.Length, dto.UserId, dto.selectCardList);
                if(canDel==false)
                {
                    //玩家出的牌管不上
                    client.Send(OpCode.FIGHT,FightCode.DEAL_SERS,-1);
                    return;
                }
                else
                {
                    //给自身客户端发送出牌成功的消息
                    client.Send(OpCode.FIGHT,FightCode.DEAL_SERS,0);
                    //广播给所有客户端
                    Brocast(fightRoom,OpCode.FIGHT,FightCode.DEAL_BRO,dto);
                    //检查一下剩余手牌数
                    List<CardDto> remainCard= fightRoom.GetPlayerModel(userId).CardList;
                    if(remainCard.Count==0)
                    {
                        //游戏结束
                        gameOver(userId,fightRoom);
                    }else
                    {
                        //转换出牌
                        Turn(fightRoom);
                    }
                }
            });
        }

        /// <summary>
        /// 不出
        /// </summary>
        /// <param name="client"></param>
        void pass(ClientPeer client)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (!userCache.IsOnline(client))
                    return;
                int userId = userCache.GetId(client);
                FightRoom fightRoom = fightCache.GetRoomByUId(userId);

                //分两种情况
                //当前玩家是最大者必须出牌
                if(fightRoom.roundModel.BiggestUId==userId)
                {
                    client.Send(OpCode.FIGHT,FightCode.PASS_SRES,-1);
                    return;
                }
                //可以不出
                client.Send(OpCode.FIGHT,FightCode.PASS_SRES,0);
                Turn(fightRoom);
            });
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="room"></param>
        private void gameOver(int userId,FightRoom room)
        {
            int winIdentity = room.GetPlayerIdeentity(userId);
            int winBeen = room.Multiple * 1000;
            List<int> winUIds = room.GetSameIdentityUIds(winIdentity);

            //给胜利的玩家添加胜场
            for (int i = 0; i < winUIds.Count; i++)
            {
                UserModel um = userCache.GetModelById(winUIds[i]);
                um.WinCount++;
                um.Been = winBeen;
                um.Exp += 100;
                userCache.Update(um);
            }
            //给失败的玩家添加负场
            List<int> loseUIds = room.GetDifferentIdentityUIds(winIdentity);
            for (int i = 0; i < loseUIds.Count; i++)
            {
                UserModel um = userCache.GetModelById(loseUIds[i]);
                um.LoseCount++;
                um.Been -= winBeen;
                um.Exp += 10;
                userCache.Update(um);
            }
            //给逃跑玩家添加逃跑场次
            for (int i = 0; i < room.LeaceUIdList.Count; i++)
            {
                UserModel um = userCache.GetModelById(room.LeaceUIdList[i]);
                um.RunCount++;
                um.Been -= winBeen * 3;
                um.Exp += 0;
                userCache.Update(um);
            }
            //给客户端发消息 谁赢了以及赢的豆子，身份？
            OverDto dto = new OverDto();
            dto.WinIdentity = winIdentity;
            dto.WinUIdList = winUIds;
            dto.BeenCount = winBeen;
            Brocast(room,OpCode.FIGHT,FightCode.OVER_BRO,dto);

            //在缓存层销毁房间数据
            fightCache.Destroy(room);
        }

        /// <summary>
        /// 转换出牌
        /// </summary>
        /// <param name="room"></param>
        private void Turn(FightRoom room)
        {
            int nextUId = room.Turn();
            //如果下一个玩家掉线
            if(room.IsOffline(nextUId))
            {
                //下一个也掉线了 递归
                Turn(room);
                //TODO 
                //掉线人物AI
            }
            else
            {
                //玩家不掉线就给他发消息让他出牌
                ClientPeer nextClient = userCache.GetClientPeer(nextUId);
                nextClient.Send(OpCode.FIGHT,FightCode.TURN_DEAL_BRO,nextUId);
            }
        }

        /// <summary>
        /// 抢地主
        /// </summary>
        /// <param name="client"></param>
        /// <param name="result"></param>
        private void grabLandlord(ClientPeer client,bool result)
        {
            SingleExecute.Instance.Execute(()=>
            {
                if (!userCache.IsOnline(client))
                    return;
                int userId = userCache.GetId(client);
                FightRoom fightRoom = fightCache.GetRoomByUId(userId); 

                if(result==true)
                {
                    //抢
                    fightRoom.SetLandlord(userId);
                    //给每个客户端发送消息谁当了地主
                    GrabDto dto = new GrabDto(userId, fightRoom.TableCardList);
                    Brocast(fightRoom,OpCode.FIGHT,FightCode.GRAB_LANDLORD_BRQ,dto);
                }
                else
                {
                    //不抢
                    int nextUId = fightRoom.GetNextUId(userId);
                    Brocast(fightRoom,OpCode.FIGHT,FightCode.TURN_DEAL_BRO,nextUId);
                }
            });
        }

        /// <summary>
        /// 开始战斗
        /// </summary>
        public void startFight(List<int> uidList)
        {
            SingleExecute.Instance.Execute(()=>
            {
                //创建战斗房间
                FightRoom room = fightCache.Create(uidList);
                room.InitPlayerCards();
                room.Sort();
                //发送给每个客户端 他自身有什么牌
                foreach (int uid in uidList)
                {
                    ClientPeer client = userCache.GetClientPeer(uid);
                    List<CardDto> cardList = room.getUserCards(uid);
                    client.Send(OpCode.FIGHT,FightCode.GET_CARD_SRES, cardList);
                }

                //开始抢地主
                int firstUserId = room.GetFirstUId();
                Brocast(room, OpCode.FIGHT, FightCode.GRAB_LANDLORD_BRQ, firstUserId, null);
            });
        }

        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="subCode"></param>
        /// <param name="value"></param>
        /// <param name="clientPeer"></param>
        public void Brocast(FightRoom room,int opCode,int subCode,object value,ClientPeer exClient= null)
        {
            SocketMessage msg = new SocketMessage(opCode, subCode, value);
            byte[] data = EncodeTool.EncodeMsg(msg);
            byte[] packet = EncodeTool.EncodePacket(data);

            foreach (var player in room.PlayerList)
            {
                ClientPeer client = userCache.GetClientPeer(player.UserId);
                if (client == exClient)
                    continue;
                client.Send(packet);
            }
        }
    }
}
