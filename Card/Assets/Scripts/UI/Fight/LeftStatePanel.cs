using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        MatchRoomDto room = Models.GameModel.MatchRoomDto;
        int leftId = room.LeftId;
        if (leftId != -1)
        {
            this.userDto = room.UIdUserDict[leftId];
            if(room.ReadyUIdList.Contains(leftId))
            {
                ReadyState();
                Debug.Log("LeftPanelStart");
                SetName(userDto.Name);
            }
        }
        else
        {
            setPanelActive(false);
        }
    }
}
