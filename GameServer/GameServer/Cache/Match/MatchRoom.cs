using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache.Match
{
    public class MatchRoom
    {
        //唯一标识
        public int Id { get; private set; }

        //在房间内的用户id
        public List<int> UIdList { get; private set; }

        //已经准备的玩家id列表
        public List<int> ReadyUIdList { get; private set; }

        public MatchRoom(int id)
        {
            this.Id = id;
            this.UIdList = new List<int>();
            this.ReadyUIdList = new List<int>();
        }

        /// <summary>
        /// 房间是否满了
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            return UIdList.Count == 3;
        }

        /// <summary>
        /// 房间是否空了
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return UIdList.Count == 0;
        }

        /// <summary>
        /// 是否所有人都准备了
        /// </summary>
        /// <returns></returns>
        public bool IsAllReady()
        {
            return ReadyUIdList.Count == 3;
        }

        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="userId"></param>
        public void Enter(int userId)
        {
            UIdList.Add(userId);
        }

        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="userId"></param>
        public void Leave(int userId)
        {
            UIdList.Remove(userId);
        }

        /// <summary>
        /// 准备
        /// </summary>
        /// <param name="userId"></param>
        public void Ready(int userId)
        {
            ReadyUIdList.Add(userId);
        }
    }
}
