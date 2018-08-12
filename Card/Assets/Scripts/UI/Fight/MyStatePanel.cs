using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyStatePanel : StatePanel
{

    protected override void Awake()
    {
        base.Awake();
        Bind(UIEvent.SHOW_GRAB_BUTTON,
            UIEvent.SHOW_DEAL_BUTTON,
            UIEvent.PLAYER_HIDE_READY_BUTTON);
    }
    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            case UIEvent.SHOW_GRAB_BUTTON:
                {
                    bool atcive = (bool)message;
                    btnGrab.gameObject.SetActive(atcive);
                    btnNGrab.gameObject.SetActive(atcive);
                    break;
                }
            case UIEvent.SHOW_DEAL_BUTTON:
                {
                    bool atcive = (bool)message;
                    btnDeal.gameObject.SetActive(atcive);
                    btnNDeal.gameObject.SetActive(atcive);
                    break;
                }
            case UIEvent.PLAYER_HIDE_READY_BUTTON:
                {
                    btnReady.gameObject.SetActive(false);
                    break;
                }
            default:
                break;
        }
    }

    private Button btnDeal;
    private Button btnNDeal;
    private Button btnGrab;
    private Button btnNGrab;
    private Button btnReady;

    private SocketMsg socketMsg;

    protected override void Start()
    {
        base.Start();

        btnDeal = transform.Find("btnDeal").GetComponent<Button>();
        btnNDeal = transform.Find("btnNDeal").GetComponent<Button>();
        btnGrab = transform.Find("btnGrab").GetComponent<Button>();
        btnNGrab = transform.Find("btnNGrab").GetComponent<Button>();
        btnReady = transform.Find("btnReady").GetComponent<Button>();

        btnDeal.onClick.AddListener(DealClick);
        btnNDeal.onClick.AddListener(NDealClick);

        btnGrab.onClick.AddListener(
            () =>
            {
                GrabOnclick(true);
            });

        btnNGrab.onClick.AddListener(
            () =>
            {
                GrabOnclick(false);
            });
        btnReady.onClick.AddListener(ReadyClick);

        btnGrab.gameObject.SetActive(false);
        btnNGrab.gameObject.SetActive(false);
        btnDeal.gameObject.SetActive(false);
        btnNDeal.gameObject.SetActive(false);

        socketMsg = new SocketMsg();

        //给自己绑定数据
        UserDto myUserDto = Models.GameModel.MatchRoomDto.UIdUserDict[Models.GameModel.UserDto.Id];
        this.userDto = myUserDto;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        btnDeal.onClick.RemoveAllListeners();
        btnNDeal.onClick.RemoveAllListeners();
        btnGrab.onClick.RemoveAllListeners();
        btnNGrab.onClick.RemoveAllListeners();
        btnReady.onClick.RemoveAllListeners();
    }

    protected override void ReadyState()
    {
        base.ReadyState();
        btnReady.gameObject.SetActive(false);
    }

    /// <summary>
    /// 出牌
    /// </summary>
    private void DealClick()
    {
        //通知角色模块出牌出牌
        Dispatch(AreaCode.CHARACTER,CharacterEvent.DEAL_CARD,null);

        //btnDeal.gameObject.SetActive(false);
        //btnNDeal.gameObject.SetActive(false);
    }

    /// <summary>
    /// 不出
    /// </summary>
    private void NDealClick()
    {
        //发送不出牌
        socketMsg.Change(OpCode.FIGHT,FightCode.PASS_CREQ,null);
        Dispatch(AreaCode.NET,0, socketMsg);

        //btnDeal.gameObject.SetActive(false);
        //btnNDeal.gameObject.SetActive(false);
    }

    /// <summary>
    /// 抢or不抢地主
    /// </summary>
    /// <param name="result"></param>
    private void GrabOnclick(bool result)
    {
        //抢地主or不抢
        socketMsg.Change(OpCode.FIGHT, FightCode.GRAB_LANDLORD_CERQ, result);
        Dispatch(AreaCode.NET, 0, socketMsg);
        //点击之后隐藏按钮
        btnGrab.gameObject.SetActive(false);
        btnNGrab.gameObject.SetActive(false);
    }

    void ReadyClick()
    {
        //向服务器发送准备
        socketMsg.Change(OpCode.MATCH,MatchCode.READY_CREQ,null);
        Dispatch(AreaCode.NET,0,socketMsg);
    }
}
