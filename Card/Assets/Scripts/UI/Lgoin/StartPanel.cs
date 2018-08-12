using Protocol.Dto;
using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : UIBase {

    private Button btnLogin;
    private Button btnClose;
    private InputField inputAccount;
    private InputField inputPsaaword;

    private PromptMsg promptMsg;
    private SocketMsg socketMsg;
    private void Awake()
    {
        Bind(UIEvent.START_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.START_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Start () {
        btnLogin = transform.Find("BtnLogin").GetComponent<Button>();
        btnClose = transform.Find("BtnClose").GetComponent<Button>();

        inputAccount = transform.Find("InputAcc").GetComponent<InputField>();
        inputPsaaword = transform.Find("InputPwd").GetComponent<InputField>();

        btnLogin.onClick.AddListener(LoginClick);
        btnClose.onClick.AddListener(CloseClick);

        promptMsg = new PromptMsg();
        socketMsg = new SocketMsg();
        setPanelActive(false);
	}

    public override void OnDestroy()
    {
        btnLogin.onClick.RemoveAllListeners();
        btnClose.onClick.RemoveAllListeners();
    }

    void LoginClick()
    {
        if (string.IsNullOrEmpty(inputAccount.text))
        {
            promptMsg.Change("用户名不能为空",Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            Debug.Log("帐号不能为空！");
            return;
        }
        if (string.IsNullOrEmpty(inputPsaaword.text))
        {
            promptMsg.Change("密码不能为空", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            Debug.Log("密码不能为空");
            return;
        }
        if(inputPsaaword.text.Length<4||inputPsaaword.text.Length>16)
        {
            promptMsg.Change("密码长度应该在4-16位之间", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            Debug.Log("密码长度应该在4-16位之间");
            return;
        }

        AccountDto dto = new AccountDto(inputAccount.text, inputPsaaword.text);
        socketMsg.Change(OpCode.ACCOUNT, AccountCode.LOGIN, dto);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    void CloseClick()
    {
        setPanelActive(false);
    }
	
}
