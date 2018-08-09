using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using Protocol.Code;
using GameServer.Cache.Match;
using GameServer.Cache;
using Protocol.Dto;
using GameServer.Model;

namespace GameServer.Logic
{
    public delegate void StartFight(List<int> uidList);

    public class MatchHandler : IHandler
    {
       public StartFight startFight;

        private MatchCache matchCache = Caches.Match;
        private UserCache userCache = Caches.User;

        public void OnDisconnect(ClientPeer client)
        {
            if (!userCache.IsOnline(client))
            {
                return;
            }
            int userId = userCache.GetId(client);
            if (matchCache.IsMatching(userId))
            {
                leave(client);
            }
        }

        public void OnRecive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case MatchCode.ENTER_CREQ:
                    enter(client);
                    break;
                case MatchCode.LEAVE_CREQ:
                    leave(client);
                    break;
                case MatchCode.READY_CREQ:
                    ready(client);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 进入
        /// </summary>
        /// <param name="client"></param>
        private void enter(ClientPeer client)//, int id)
        {
            SingleExecute.Instance.Execute(
                delegate ()
                {
                    if (!userCache.IsOnline(client))
                        return;
                    int userId = userCache.GetId(client);

                    //if (userId != id)
                    //    return;

                    //如果用户已经在匹配房间等待了 那就无视
                    if (matchCache.IsMatching(userId))
                    {
                        client.Send(OpCode.MATCH, MatchCode.ENTER_SRES, -1);//重复加入
                        return;
                    }
                    //正常进入
                    MatchRoom room = matchCache.Enter(userId, client);
                    //广播给房间内除了当前客户端的其他用户，有新玩家计入了  参数：新进入的玩家的用户id
                    #region 构造一个UserDto  Dto就是针对UI定义的 UI需要什么我们就加什么字段
                    UserModel model = userCache.GetModelById(userId);
                    UserDto userDto = new UserDto(model.Id, model.Name, model.Been, model.WinCount, model.LoseCount, model.RunCount, model.Lv, model.Exp);
                    #endregion
                    room.Brocast(OpCode.MATCH, MatchCode.ENTER_BRO, userDto, client);
                    //返回给当前客户端 给他房间的数据模型
                    MatchRoomDto dto = makeRoomDto(room);
                    client.Send(OpCode.MATCH, MatchCode.ENTER_SRES, dto);

                    Console.WriteLine("有玩家进入匹配房间");
                }
                );
        }


        /// <summary>
        /// 离开
        /// </summary>
        /// <param name="client"></param>
        private void leave(ClientPeer client)
        {
            SingleExecute.Instance.Execute(
                delegate ()
                {
                    if (!userCache.IsOnline(client))
                        return;
                    int userId = userCache.GetId(client);
                    //用户没有匹配 不能退出 非法操作
                    if (matchCache.IsMatching(userId) == false)
                    {
                        Console.WriteLine("用户没有匹配 不能退出 非法操作");
                        return;
                    }
                    //正常离开
                    MatchRoom room = matchCache.Leave(userId);
                    //广播给房间内所有人 有人离开了 参数：离开的用户id
                    room.Brocast(OpCode.MATCH, MatchCode.LEAVE_BRO, userId);

                    Console.WriteLine("有玩家离开匹配房间");
                });
        }


        /// <summary>
        /// 准备
        /// </summary>
        /// <param name="client"></param>
        private void ready(ClientPeer client)
        {
            SingleExecute.Instance.Execute(
                () =>
                {
                    if (userCache.IsOnline(client) == false)
                        return;
                    int userId = userCache.GetId(client);
                    if (matchCache.IsMatching(userId) == false)
                        return;
                    //一定要注意安全的验证
                    MatchRoom room = matchCache.GetRoom(userId);
                    room.Ready(userId);
                    room.Brocast(OpCode.MATCH, MatchCode.READY_BRO, userId);

                    //检测：是否所有玩家都准备了
                    if (room.IsAllReady())
                    {
                        //开始战斗 
                        startFight(room.GetUIdList);
                        //通知房间内的玩家  要进行战斗了 给客户端群发消息
                        room.Brocast(OpCode.MATCH, MatchCode.START_BRO, null);
                        //销毁房间
                        matchCache.Destroy(room);
                    }
                }
                );
        }



        private MatchRoomDto makeRoomDto(MatchRoom room)
        {
            MatchRoomDto dto = new MatchRoomDto();
            //给 UIdClientDict 赋值
            foreach (var uid in room.UIdClientDict.Keys)
            {
                UserModel model = userCache.GetModelById(uid);
                UserDto userDto = new UserDto(model.Id, model.Name, model.Been, model.WinCount, model.LoseCount, model.RunCount, model.Lv, model.Exp);
                dto.UIdUserDict.Add(uid, userDto);
                //fix bug
                dto.UIdList.Add(uid);
            }
            dto.ReadyUIdList = room.ReadyUIdList;
            return dto;
        }


    }
}
