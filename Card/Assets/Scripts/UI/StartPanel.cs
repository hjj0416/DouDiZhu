using Protocol.Dto;
using Protocol.code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : UIBase {

    private Button btnLogin;
    private Button btnClose;
    private InputField inputAccount;
    private InputField inputPsaaword;

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
            return;
        if (string.IsNullOrEmpty(inputPsaaword.text)
            || inputPsaaword.text.Length < 6
            || inputPsaaword.text.Length > 16)
            return;

        AccountDto dto = new AccountDto(inputAccount.text, inputPsaaword.text);
        SocketMessage socketMsg = new SocketMessage(OpCode.ACCOUNT, AccountCode.LOGIN, dto);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    void CloseClick()
    {
        setPanelActive(false);
    }
	
}
