using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : UIBase {

    private Text txtName;
    private Text txtLv;
    private Slider sldExp;
    private Text txtExp;
    private Text txtBeen;
    private Button BtnSet;
    private Button BtnMatch;

    private SocketMsg socketMsg;

    private void Awake()
    {
        Bind(UIEvent.REFRESH_INFO_PANEL);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.REFRESH_INFO_PANEL:
                UserDto User = message as UserDto;
                RefreshPanel(User.Name,User.Lv,User.Exp,User.Been);
                break;
            default: 
                break;
        }
    }

    // Use this for initialization
    void Start () {

        txtName = transform.Find("txtName").GetComponent<Text>();
        txtLv = transform.Find("txtLv").GetComponent<Text>();
        sldExp = transform.Find("sldExp").GetComponent<Slider>();
        txtExp = transform.Find("txtExp").GetComponent<Text>();
        txtBeen = transform.Find("txtBeen").GetComponent<Text>();
        BtnSet = transform.Find("BtnSet").GetComponent<Button>();
        BtnMatch = transform.Find("BtnMatch").GetComponent<Button>();

        BtnSet.onClick.AddListener(BtnSetClick);
        BtnMatch.onClick.AddListener(BtnMatchClick);

        socketMsg = new SocketMsg();
    }

    public override void OnDestroy()
    {
        BtnSet.onClick.RemoveAllListeners();
        BtnMatch.onClick.RemoveAllListeners();
    }

    void BtnSetClick()
    {
        Dispatch(AreaCode.UI,UIEvent.SETTING_PANEL_ACTIVE,true);
    }
    
    void BtnMatchClick()
    {
        Dispatch(AreaCode.UI,UIEvent.MATCH_PANEL_ACTIVE,true);
        //向服务器发起开始匹配的请求
        socketMsg.Change(OpCode.MATCH, MatchCode.ENTER_CREQ, null);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
	
    /// <summary>
    /// 服务器数据刷新视图
    /// </summary>
    private void RefreshPanel(string name,int lv,int exp,int been)
    {
        txtName.text = name;
        txtLv.text = string.Format("LV.{0}",lv);
        //等级和经验之间的公式exp=lv*100
        txtExp.text = string.Format("{0}/{1}",exp,lv*100);
        sldExp.value = (float)exp / (lv * 100);
        txtBeen.text = string.Format("x{0}",been);
    }
}
