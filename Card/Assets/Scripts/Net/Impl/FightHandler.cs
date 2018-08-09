using Protocol.Code;
using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case FightCode.GET_CARD_SRES:
                getCards(value as List<CardDto>);
                break;
            default:
                break;
        }
    }

    private void getCards(List<CardDto> cardList)
    {
        //给自己玩家创建牌的对象
        //TODO

        //设置倍数为1
        Dispatch(AreaCode.UI,UIEvent.CHANGE_MUTIPLE,1);
    }
}
