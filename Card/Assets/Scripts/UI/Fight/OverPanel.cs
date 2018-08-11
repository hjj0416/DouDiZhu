using Protocol.Code;
using Protocol.Constant;
using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏结束面板
/// </summary>
public class OverPanel : UIBase {

    private void Awake()
    {
        Bind(UIEvent.SHOW_OVER_PANEL);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.SHOW_OVER_PANEL:
                RefreshShow(message as OverDto);
                break;
            default:
                break;
        }
    }
    private Text txtWinIdentity;
    private Text txtWinBeen;
    private Button btnBack;

	// Use this for initialization
	void Start () {
        txtWinIdentity =transform.Find("txtWinIdentity").GetComponent<Text>();
        txtWinBeen = transform.Find("txtWinBeen").GetComponent<Text>();
        btnBack = transform.Find("btnBack").GetComponent<Button>();
        btnBack.onClick.AddListener(backClick);

        setPanelActive(false);
    }
	
    /// <summary>
    /// 刷新显示
    /// </summary>
	private void RefreshShow(OverDto dto)
    {
        setPanelActive(true);
        //显示谁胜利
        txtWinIdentity.text = Identity.GetString(dto.WinIdentity);
        //判断自己是否胜利
        if(dto.WinUIdList.Contains(Models.GameModel.Id))
        {
            txtWinIdentity.text += "胜利";
            txtWinBeen.text = "欢乐豆：+";
        }else
        {
            txtWinIdentity.text += "失败";
            txtWinBeen.text = "欢乐豆：-";
        }
        //显示豆子数量
        txtWinBeen.text += dto.BeenCount;
    }

    /// <summary>
    /// 返回点击事件
    /// </summary>
    private void backClick()
    {
        LoadSceneMsg msg = new LoadSceneMsg(1, () =>
        {
            //向服务器获取信息
            SocketMsg socketMsg = new SocketMsg(OpCode.USER, UserCode.GET_INFO_CREQ, null);
            Dispatch(AreaCode.NET, 0, socketMsg);
        });
        Dispatch(AreaCode.SENCE, SceneEvent.LOAD_SCENE, msg);
    }
}
