using Protocol.Code;
using Protocol.Constant;
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
                Debug.Log("GET_CARD_SRES");
                getCards(value as List<CardDto>);
                break;
            case FightCode.TURN_GRAB_BRO:
                Debug.Log("TURN_GRAB_BRO");
                turnGrabBro((int)value);
                break;
            case FightCode.GRAB_LANDLORD_BRQ:
                Debug.Log("GRAB_LANDLORD_BRQ");
                grabLandlordBro(value as GrabDto);
                break;
            case FightCode.TURN_DEAL_BRO:
                Debug.Log("TURN_DEAL_BRO");
                turnDealBro((int)value);
                break;
            case FightCode.DEAL_BRO:
                Debug.Log("DEAL_BRO");
                dealBro(value as DealDto);
                break;
            case FightCode.DEAL_SERS:
                Debug.Log("DEAL_SERS");
                dealResponse((int)value);
                break;
            case FightCode.OVER_BRO:
                Debug.Log("OVER_BRO");
                overBro(value as OverDto);
                break;
            case FightCode.PASS_SRES:
                Debug.Log("PASS_SRES");
                nDealResponse((int)value);
                break;
            case FightCode.LEAVE_BRO:
                Debug.Log("LEAVE_BRO");
                leaveBro((int)value);
                break;
            default:
                break;
        }
    }

    PromptMsg promptMsg = new PromptMsg();

    /// <summary>
    /// 离开的广播处理
    /// </summary>
    /// <param name="leaveUserId"></param>
    private void leaveBro(int leaveUserId)
    {
        //发消息 隐藏玩家的状态面板所有游戏物体
        Dispatch(AreaCode.UI, UIEvent.PLAYER_LEAVE, leaveUserId);

    }

    /// <summary>
    /// 结束广播
    /// </summary>
    /// <param name="overDto"></param>
    private void overBro(OverDto overDto)
    {
        if(overDto.WinUIdList.Contains(Models.GameModel.Id))
        {
            Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_EFFECT_AUDIO, "Fight/MusicEx_win");
        }
        else
        {
            Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_EFFECT_AUDIO, "Fight/MusicEx_Lose");
        }

        Dispatch(AreaCode.UI,UIEvent.SHOW_OVER_PANEL,overDto);
    }

    /// <summary>
    /// 服务器发回不出的响应
    /// </summary>
    /// <param name="result"></param>
    private void nDealResponse(int result)
    {
        //玩家可以不出牌，隐藏按钮
        if (result == 0)
        {
            Dispatch(AreaCode.UI,UIEvent.SHOW_DEAL_BUTTON,false);
            //播放音效
            Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_EFFECT_AUDIO, "Fight/Woman_buyao1");
        }
        else if(result==-1)//玩家是最大出牌者，不可以不出牌
        {
            //提示必须出牌
            Dispatch(AreaCode.UI, UIEvent.SHOW_DEAL_BUTTON, true);
            promptMsg.Change("你的出牌回合必须出牌", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
        }
    }

    /// <summary>
    /// 出牌响应
    /// </summary>
    /// <param name="result"></param>
    private void dealResponse(int result)
    {
        if (result==-1)
        {
            //玩家出的牌管不上上一个玩家出的牌
            promptMsg.Change("玩家出的牌管不上上一个玩家出的牌",Color.red);
            Dispatch(AreaCode.UI,UIEvent.PROMPT_MSG,promptMsg);
            //重新显示出牌按钮
            Dispatch(AreaCode.UI,UIEvent.SHOW_DEAL_BUTTON,true);
        }else if(result==0)
        {
            //隐藏出牌按钮
            Dispatch(AreaCode.UI, UIEvent.SHOW_DEAL_BUTTON, false);
        }
    }

    /// <summary>
    /// 同步出牌
    /// </summary>
    /// <param name="dto"></param>
    private void dealBro(DealDto dto)
    {
        //移除出完的手牌
        int userId = dto.UserId;
        int eventCode = -1;
        if (dto.UserId == Models.GameModel.MatchRoomDto.LeftId)
        {
            eventCode = CharacterEvent.REMOVE_LEFT_CARD;
        }
        else if (dto.UserId == Models.GameModel.MatchRoomDto.RightId)
        {
            eventCode = CharacterEvent.REMOVE_RIGHT_CARD;
        }
        else if (dto.UserId == Models.GameModel.UserDto.Id)
        {
            eventCode = CharacterEvent.REMOVE_MY_CARD;
        }
        Dispatch(AreaCode.CHARACTER, eventCode, dto.RemainCardList);
        //显示到桌面上
        Dispatch(AreaCode.CHARACTER,CharacterEvent.UPDATE_SHOW_DESK,dto.selectCardList);
        //播放出牌音效
        playDealAudio(dto.Type,dto.Weight);
    }

    /// <summary>
    /// 播放出牌音效
    /// </summary>
    private void playDealAudio(int cardType,int weight)
    {
        string audioName = "Fight/";

        switch (cardType)
        {
            case CardType.SINGLE:
                audioName += "Woman_" + weight;
                break;
            case CardType.DOUBLE:
                audioName += "Woman_dui" + weight/2;
                break;
            case CardType.STRAIGHT:
                audioName += "Woman_shunzi";
                break;
            case CardType.DOUBLE_STRAIGHT:
                audioName += "Woman_liandui";
                break;
            case CardType.TRIPLE_STRAIGHT:
                audioName += "Woman_feiji";
                break;
            case CardType.THREE:
                audioName += "Woman_tuple"+weight/3;
                break;
            case CardType.THREE_ONE:
                audioName += "Woman_sandaiyi";
                break;
            case CardType.THREE_TWO:
                audioName += "Woman_sandaiyidui";
                break;
            case CardType.BOOM:
                audioName += "Woman_zhadan";
                break;
            case CardType.JOKER_BOOM:
                audioName += "Woman_wangzha";
                break;
            default:
                break;
        }

        Dispatch(AreaCode.AUDIO,AudioEvent.PLAY_EFFECT_AUDIO,audioName);
    }

    /// <summary>
    /// 转换出牌
    /// </summary>
    /// <param name="userId"></param>
    private void turnDealBro(int userId)
    {
        if(Models.GameModel.Id==userId)
        {
            Dispatch(AreaCode.UI,UIEvent.SHOW_DEAL_BUTTON,true);
        }
    }

    /// <summary>
    /// 抢地主成功的处理
    /// </summary>
    /// <param name="dto"></param>
    private void grabLandlordBro(GrabDto dto)
    {
        //更改UI的身份显示
        Dispatch(AreaCode.UI,UIEvent.PLAY_CHANGE_IDENTITY,dto.userId);
        //播放抢地主声音
        Dispatch(AreaCode.AUDIO,AudioEvent.PLAY_EFFECT_AUDIO,"Fight/Woman_Order");
        //显示三张牌
        Dispatch(AreaCode.UI,UIEvent.SET_TABLE_CARD,dto.TableCardList);
        //给对应的地主玩家 添加手牌显示
        int eventCode = -1;
        if(dto.userId== Models.GameModel.MatchRoomDto.LeftId)
        {
            eventCode = CharacterEvent.ADD_LEFT_CARD;
        }
        else if(dto.userId == Models.GameModel.MatchRoomDto.RightId)
        {
            eventCode = CharacterEvent.ADD_RIGHT_CARD;
        }
        else if (dto.userId == Models.GameModel.UserDto.Id)
        {
            eventCode = CharacterEvent.ADD_MY_CARD;
        }
        Dispatch(AreaCode.CHARACTER,eventCode,dto);
    }

    /// <summary>
    /// 是不是第一个玩家抢地主
    /// </summary>
    private bool isFirst = true;

    /// <summary>
    /// 转换抢地主
    /// </summary>
    /// <param name="userId"></param>
    private void turnGrabBro(int userId)
    {
        if(isFirst)
        {
            isFirst = false;
        }
        else
        {
            //播放声音
            Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_EFFECT_AUDIO, "Fight/Woman_NoOrder");
        }
        //如果是自身 显示
        if(userId==Models.GameModel.UserDto.Id)
        {
            Dispatch(AreaCode.UI,UIEvent.SHOW_GRAB_BUTTON,true);
        }
    }

    private void getCards(List<CardDto> cardList)
    {
        //给自己玩家创建牌的对象
        Dispatch(AreaCode.CHARACTER,CharacterEvent.INIT_MY_CARD,cardList);
        Dispatch(AreaCode.CHARACTER, CharacterEvent.INIT_RIGHT_CARD, null);
        Dispatch(AreaCode.CHARACTER, CharacterEvent.INIT_LEFT_CARD, null);

        //设置倍数为1
        Dispatch(AreaCode.UI,UIEvent.CHANGE_MUTIPLE,1);
    }
}
