using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存储游戏数据的存储类
/// </summary>
public class GameModel  {

    public UserDto UserDto { get; set; }//登录用户的数据

    public MatchRoomDto MatchRoomDto { get; set; }//匹配房间的数据

    public UserDto GetUserDto(int userId)
    {
        return MatchRoomDto.UIdUserDict[userId];
    }

    public int GetRightUserId()
    {
        return MatchRoomDto.RightId;
    }
}
