using Protocol.Code;
using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerCtrl : CharacterBase
{

    private void Awake()
    {
        Bind(CharacterEvent.INIT_MY_CARD,
            CharacterEvent.ADD_MY_CARD,
            CharacterEvent.DEAL_CARD,
            CharacterEvent.REMOVE_MY_CARD);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharacterEvent.INIT_MY_CARD:
                StartCoroutine(initCardList(message as List<CardDto>));
                break;
            case CharacterEvent.ADD_MY_CARD:
                addTableCard(message as GrabDto);
                break;
            case CharacterEvent.DEAL_CARD:
                dealSelectCard();
                break;
            case CharacterEvent.REMOVE_MY_CARD:
                removeCard(message as List<CardDto>);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 自身的卡牌列表
    /// </summary>
    private List<CardCtrl> cardCtrlList;

    /// <summary>
    /// 卡牌的父物体
    /// </summary>
    private Transform cardParent;

    private PromptMsg promptMsg;
    private SocketMsg socketMsg;

    // Use this for initialization
    void Start()
    {
        cardParent = transform.Find("CardPoint");
        cardCtrlList = new List<CardCtrl>();

        promptMsg = new PromptMsg();
        socketMsg = new SocketMsg();
    }

    /// <summary>
    /// 出牌
    /// </summary>
    private void dealSelectCard()
    {
        List<CardDto> selectCardList = getSelectCard();
        DealDto dto = new DealDto(selectCardList,Models.GameModel.Id);
        //如果出牌不合法
        if(dto.IsRegular==false)
        {
            promptMsg.Change("请选择合理的手牌！",Color.red);
            Dispatch(AreaCode.UI,UIEvent.PROMPT_MSG,promptMsg);
            return;
        }
        else
        {
            //可以出牌
            socketMsg.Change(OpCode.FIGHT,FightCode.DEAL_CREQ,dto);
            Dispatch(AreaCode.NET,0,socketMsg);
        }
    }

    /// <summary>
    /// 移除卡牌的游戏物体
    /// </summary>
    private void removeCard(List<CardDto> remainCardList)
    {
        int index = 0;
        foreach (var cc in cardCtrlList)
        {
            if (remainCardList.Count == 0)
                break;
            else
            {
                cc.gameObject.SetActive(true);
                cc.Init(remainCardList[index],index,true);
                index++;
                if(index==remainCardList.Count)
                {
                    break;
                }
            }
        }
        //把index之后的牌隐藏掉
        for (int i = index; i < cardCtrlList.Count; i++)
        {
            cardCtrlList[i].selected = false;
            Destroy(cardCtrlList[i].gameObject);
        }
    }

    /// <summary>
    /// 获取选中的牌
    /// </summary>
    private List<CardDto> getSelectCard()
    {
        List<CardDto> selectCardList = new List<CardDto>();
        foreach (var cardCtrl in cardCtrlList)
        {
            if (cardCtrl.selected == true)
            {
                selectCardList.Add(cardCtrl.cardDto);
            }
        }
        return selectCardList;
    }

    /// <summary>
    /// 添加底牌
    /// </summary>
    /// <param name="cardList"></param>
	private void addTableCard(GrabDto dto)
    {
        List<CardDto> tableCards = dto.TableCardList;
        List<CardDto> playerCards = dto.PlayerCardList;

        //复用之前创建的卡牌
        int index = 0;
        foreach (var cardCtrl in cardCtrlList)
        {
            cardCtrl.gameObject.SetActive(true);
            cardCtrl.Init(playerCards[index], index, true);
            index++;
        }
        //在创建新的三张牌
        GameObject cardPrefab = Resources.Load<GameObject>("Card/MyCard");
        for (int i = index; i < playerCards.Count; i++)
        {
            createGo(playerCards[i], i, cardPrefab);
        }
    }

    /// <summary>
    /// 初始化显示卡牌
    /// </summary>
	private IEnumerator initCardList(List<CardDto> cardList)
    {
        GameObject cardPrefab = Resources.Load<GameObject>("Card/MyCard");
        for (int i = 0; i < cardList.Count; i++)
        {
            createGo(cardList[i], i, cardPrefab);
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// 创建卡牌游戏物体
    /// </summary>
    /// <param name="card"></param>
    /// <param name="index"></param>
    private void createGo(CardDto card, int index, GameObject cardPrefab)
    {
        GameObject cardGo = Instantiate(cardPrefab, cardParent) as GameObject;
        cardGo.name = card.Name;
        cardGo.transform.localPosition = new Vector2((0.2f * index), 0);
        CardCtrl cardCtrl = cardGo.GetComponent<CardCtrl>();
        cardCtrl.Init(card, index, true);

        this.cardCtrlList.Add(cardCtrl);
    }
}
