using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    /// <summary>
    /// 房间数据对应的传输模型
    /// </summary>
    [Serializable]
    public class MatchRoomDto
    {
        /// <summary>
        /// 用户id对应的用户数据的传输模型
        /// </summary>
        public Dictionary<int, UserDto> UIdUserDict;

        /// <summary>
        /// 准备的玩家id列表
        /// </summary>
        public List<int> ReadyUIdList;

        /// <summary>
        /// 存储玩家进入的顺序 
        /// </summary>
        public List<int> UIdList;

        public MatchRoomDto()
        {
            this.UIdUserDict = new Dictionary<int, UserDto>();
            this.ReadyUIdList = new List<int>();
            //fix bug
            this.UIdList = new List<int>();
        }

        public void Add(UserDto newUser)
        {
            this.UIdUserDict.Add(newUser.Id, newUser);
            this.UIdList.Add(newUser.Id);
        }

        public void Leave(int userId)
        {
            this.UIdUserDict.Remove(userId);
            this.UIdList.Remove(userId);
        }

        public void Ready(int userId)
        {
            this.ReadyUIdList.Add(userId);
        }

        public int LeftId;//左边玩家的id
        public int RightId;//代表右边玩家的id

        /// <summary>
        /// 重置位置
        ///  在每次玩家进入或者离开房间的时候 都需要调整一下位置 
        /// </summary>
        public void ResetPosition(int myUserId)
        {
            LeftId = -1;
            RightId = -1;

            // 1
            if (UIdList.Count == 1)
            {

            }
            // 2
            else if (UIdList.Count == 2)
            {
                // x a
                if (UIdList[0] == myUserId)
                {
                    RightId = UIdList[1];
                }
                // a x
                if (UIdList[1] == myUserId)
                {
                    LeftId = UIdList[0];
                }
            }
            // 3.
            else if (UIdList.Count == 3)
            {
                // x a b
                if (UIdList[0] == myUserId)
                {
                    LeftId = UIdList[2];
                    RightId = UIdList[1];
                }
                // b x a
                if (UIdList[1] == myUserId)
                {
                    LeftId = UIdList[0];
                    RightId = UIdList[2];
                }
                // a b x
                if (UIdList[2] == myUserId)
                {
                    LeftId = UIdList[1];
                    RightId = UIdList[0];
                }
            }
        }

    }
}
