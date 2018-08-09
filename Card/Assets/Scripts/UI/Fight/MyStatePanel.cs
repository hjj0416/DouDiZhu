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
            UIEvent.SET_MY_PLAER_DATA,
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
            case UIEvent.SET_MY_PLAER_DATA:
                {
                    this.userDto = message as UserDto;
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

    private void DealClick()
    {

    }

    private void NDealClick()
    {

    }

    private void GrabOnclick(bool result)
    {
        if(result==true)
        {
            //抢地主
        }else
        {
            //不抢
        }
    }

    void ReadyClick()
    {
        //向服务器发送准备
        socketMsg.Change(OpCode.MATCH,MatchCode.READY_CREQ,null);
        Dispatch(AreaCode.NET,0,socketMsg);
    }
}
