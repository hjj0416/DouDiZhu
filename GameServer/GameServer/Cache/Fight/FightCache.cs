using AhpilyServer;
using AhpilyServer.Concurrent;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache.Fight
{

    /// <summary>
    /// 战斗的缓存层
    /// </summary>
   public class FightCache
    {
        /// <summary>
        /// 用户id 对应的房间id
        /// </summary>
        private Dictionary<int, int> uidRoomIDict = new Dictionary<int, int>();

        /// <summary>
        /// 房间id对应的房间模型对象
        /// </summary>
        private Dictionary<int, FightRoom> idRoomDict = new Dictionary<int, FightRoom>();

        /// <summary>
        /// 重用房间队列
        /// </summary>
        private Queue<FightRoom> roomQueue = new Queue<FightRoom>();

        /// <summary>
        /// 房间的id
        /// </summary>
        private ConcurrentInt id = new ConcurrentInt(-1);

        public UserCache userCache = Caches.User;

        /// <summary>
        /// 创建战斗房间
        /// </summary>
        /// <param name="uidList"></param>
        /// <returns></returns>
        public FightRoom Create(List<int> uidList)
        {
            FightRoom room = null;
            //先检测是否有可以重用的房间
            if (roomQueue.Count > 0)
            {
                room = roomQueue.Dequeue();
                room.Init(uidList);
            }

            else
                room = new FightRoom(id.Add_Get(),uidList);

            //绑定映射关系
            foreach (int uid in uidList)
            {
                uidRoomIDict.Add(uid,room.Id);
                ClientPeer client = userCache.GetClientPeer(uid);
                room.StartFight(uid,client);
            }
            idRoomDict.Add(room.Id,room);
            return room;
        }

        /// <summary>
        /// 获取房间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FightRoom GetRoom(int id)
        {
            if(idRoomDict.ContainsKey(id)==false)
            {
                throw new Exception("不存在这个房间");
            }
            return idRoomDict[id];
        }

        public bool IsFighting(int userId)
        {
            return uidRoomIDict.ContainsKey(userId);
        }

        /// <summary>
        /// 根据用户id获取所在房间
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public FightRoom GetRoomByUId(int uid)
        {
            if(uidRoomIDict.ContainsKey(uid)==false)
            {
                throw new Exception("当前用户不在这个房间");
            }
            int roomId = uidRoomIDict[uid];
            FightRoom room = GetRoom(roomId);
            return room;
        }

        /// <summary>
        /// 摧毁房间
        /// </summary>
        /// <param name="room"></param>
        public void Destroy(FightRoom room)
        {
            //移除映射关系
            idRoomDict.Remove(room.Id);
            foreach (PlayerDto player in room.PlayerList)
            {
                uidRoomIDict.Remove(player.UserId);
            }
            //初始化房间的数据
            room.PlayerList.Clear();
            room.LeaveUIdList.Clear();
            room.TableCardList.Clear();
            room.libraryModel.Init();
            room.Multiple = 1;
            room.roundModel.Init();
            //添加到重用队列
            roomQueue.Enqueue(room);
        }
    }
}
