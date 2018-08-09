using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightStatePanel : StatePanel {

    protected override void Awake()
    {
        base.Awake();
        Bind(UIEvent.SET_RIGHT_PLAYER_DATA);
    }

    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode,message);
        switch (eventCode)
        {
            case UIEvent.SET_RIGHT_PLAYER_DATA:
                this.userDto = message as UserDto;
                break;
            default:
                break;
        }
    }

    protected override void Start()
    {
        base.Start();
        if (Models.GameModel.MatchRoomDto.RightId != -1)
        {
            this.userDto = Models.GameModel.MatchRoomDto.UIdUserDict[Models.GameModel.MatchRoomDto.RightId];
        }
        else
        {
            setPanelActive(false);
        }
    }
}
