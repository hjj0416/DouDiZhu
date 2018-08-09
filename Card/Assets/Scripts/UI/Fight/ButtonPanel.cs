using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPanel : UIBase {

    private void Awake()
    {
        Bind(UIEvent.CHANGE_MUTIPLE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.CHANGE_MUTIPLE:
                changeMutiple((int)message);
                break;
            default:
                break;
        }
    }

    private Text txtBeen;
    private Text txtMutiple;
    private Button btnChat;
    private Button[] btns;
    private Image imgChoose;
    private SocketMsg socketMsg;

    private void Start()
    {
        txtBeen = transform.Find("txtBeen").GetComponent<Text>();
        txtMutiple = transform.Find("txtMutiple").GetComponent<Text>();
        btnChat = transform.Find("btnChat").GetComponent<Button>();
        btns = new Button[7];
        imgChoose = transform.Find("imgChoose").GetComponent<Image>();
        for(int i=0;i<7;i++)
        {
            btns[i] = imgChoose.transform.GetChild(i).GetComponent<Button>();
        }
        btns[0].onClick.AddListener(ChatClick);
        btns[1].onClick.AddListener(ChatClick1);
        btns[2].onClick.AddListener(ChatClick2);
        btns[3].onClick.AddListener(ChatClick3);
        btns[4].onClick.AddListener(ChatClick4);
        btns[5].onClick.AddListener(ChatClick5);
        btns[6].onClick.AddListener(ChatClick6);

        btnChat.onClick.AddListener(setChooseActive);

        imgChoose.gameObject.SetActive(false);
        refreshPanel(Models.GameModel.UserDto.Been);

        socketMsg = new SocketMsg();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        btnChat.onClick.RemoveListener(setChooseActive);
        btns[0].onClick.RemoveListener(ChatClick);
        btns[1].onClick.RemoveListener(ChatClick1);
        btns[2].onClick.RemoveListener(ChatClick2);
        btns[3].onClick.RemoveListener(ChatClick3);
        btns[4].onClick.RemoveListener(ChatClick4);
        btns[5].onClick.RemoveListener(ChatClick5);
        btns[6].onClick.RemoveListener(ChatClick6);
    }

    /// <summary>
    /// 刷新自身面板的信息
    /// </summary>
    public void refreshPanel(int beenCount)
    {
        this.txtBeen.text =" × " +beenCount;
    }

    /// <summary>
    /// 改变倍数
    /// </summary>
    /// <param name="muti"></param>
    private void changeMutiple(int mutiple)
    {
        txtMutiple.text = "倍数 × " + mutiple;
    }

    /// <summary>
    /// 设置选择对话面板显示
    /// </summary>
    private void setChooseActive()
    {
        bool active = imgChoose.gameObject.activeInHierarchy;
        imgChoose.gameObject.SetActive(!active);
    }

    /// <summary>
    /// 点击某一句聊天内容的时候调用
    /// </summary>
    private void ChatClick()
    {
        socketMsg.Change(OpCode.CHAT,ChatCode.CREQ,1);
        Dispatch(AreaCode.NET,0,socketMsg);
    }
    private void ChatClick1()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 2);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    private void ChatClick2()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 3);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    private void ChatClick3()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 4);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    private void ChatClick4()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 5);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    private void ChatClick5()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 6);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    private void ChatClick6()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 7);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
}

