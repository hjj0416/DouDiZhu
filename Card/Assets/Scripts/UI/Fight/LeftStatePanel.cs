using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftStatePanel : StatePanel
{

    protected override void Awake()
    {
        base.Awake();
        Bind(UIEvent.SET_LEFT_PLAYER_DATA);
    }

    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode,message);
        switch (eventCode)
        {
            case UIEvent.SET_LEFT_PLAYER_DATA:
                this.userDto = message as UserDto;
                break;
            
            default:
                break;
        }
    }

    protected override void Start()
    {
        base.Start();
        if (Models.GameModel.MatchRoomDto.LeftId!=-1)
        {
            this.userDto = Models.GameModel.MatchRoomDto.UIdUserDict[Models.GameModel.MatchRoomDto.LeftId];
        }
        else
        {
            setPanelActive(false);

        }
    }
}
