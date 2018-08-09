using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePanel : UIBase {

    private InputField InputName;
    private Button BtnCreate;

    private PromptMsg promptMsg;
    private SocketMsg socketMsg;

    private void Awake()
    {
        Bind(UIEvent.CREATE_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.CREATE_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }
    // Use this for initialization
    void Start () {

        InputName = transform.Find("InputName").GetComponent<InputField>();
        BtnCreate = transform.Find("BtnCreate").GetComponent<Button>();

        BtnCreate.onClick.AddListener(CreateClick);

        promptMsg = new PromptMsg();
        socketMsg = new SocketMsg();

        setPanelActive(false);
	}

    public override void OnDestroy()
    {
        BtnCreate.onClick.RemoveAllListeners();
    }

    void CreateClick()
    {
        if(string.IsNullOrEmpty(InputName.text))
        {
            //非法输入
            promptMsg.Change("请正确输入您的名称",Color.red);
            Dispatch(AreaCode.UI,UIEvent.PROMPT_MSG,promptMsg);
        }
        //向服务器发送创建请求
        socketMsg.Change(OpCode.USER, UserCode.CREATE_CREQ, InputName.text);
        Dispatch(AreaCode.NET,0,socketMsg);
    }
}
