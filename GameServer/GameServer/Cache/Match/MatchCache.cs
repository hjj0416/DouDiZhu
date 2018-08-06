using AhpilyServer.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache.Match
{
   public  class MatchCache
    {
        /// <summary>
        /// 正在等待的用户id 和 房间id
        /// </summary>
        private Dictionary<int, int> uidRoomIdDict = new Dictionary<int, int>();

        /// <summary>
        /// 正在等待的房间id 和 房间数据模型
        /// </summary>
        private Dictionary<int, MatchRoom> idModelDict = new Dictionary<int, MatchRoom>();

        /// <summary>
        /// 房间对象
        /// </summary>
        Queue<MatchRoom> roomQueue = new Queue<MatchRoom>();

        /// <summary>
        /// 代表房间的id
        /// </summary>
        private ConcurrentInt id = new ConcurrentInt(-1);

        /// <summary>
        /// 进入匹配队列
        /// </summary>
        /// <returns></returns>
        public MatchRoom Enter(int userId)
        {
            foreach(MatchRoom mr in idModelDict.Values)
            {
                if (mr.IsFull())
                    continue;
                mr.Enter(mr.Id);
                uidRoomIdDict.Add(userId,mr.Id);
                return mr;
            }
            //自己开房间
            MatchRoom room = null;
            if (roomQueue.Count > 0)
                room = roomQueue.Dequeue();
            else
                room = new MatchRoom(id.Add_Get());

            room.Enter(userId);
            idModelDict.Add(room.Id,room);
            uidRoomIdDict.Add(userId,room.Id);
            return room;
        }

        /// <summary>
        /// 离开匹配房间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public MatchRoom Leave(int userId)
        {
            int roomId = uidRoomIdDict[userId];
            MatchRoom room = idModelDict[roomId];
            room.Leave(userId);
            uidRoomIdDict.Remove(userId);
            if(room.IsEmpty())
            {
                //空房间放入重用队列
                idModelDict.Remove(roomId);
                roomQueue.Enqueue(room);
            }
            return room;
        }

        /// <summary>
        /// 判断用户是否正在匹配
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsMatching(int userId)
        {
            return uidRoomIdDict.ContainsKey(userId);
        }

        /// <summary>
        /// 获取玩家所在的等待房间
        /// </summary>
        /// <param name="userId"></param>
        public MatchRoom GetRoom(int userId)
        {
            int roomId = uidRoomIdDict[userId];
            MatchRoom room = idModelDict[roomId];
            return room;
        }

        /// <summary>
        /// 摧毁房间
        /// </summary>
        /// <param name="room"></param>
        public void Destroy(MatchRoom room)
        {
            idModelDict.Remove(room.Id);
            foreach(var userId in room.UIdList)
            {
                uidRoomIdDict.Remove(userId);
            }
            room.UIdList.Clear();
            room.ReadyUIdList.Clear();
            roomQueue.Enqueue(room);
        }
    }
}
