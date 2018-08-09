using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistPanel : UIBase {

    private Button btnRegist;
    private Button btnClose;
    private InputField inputAcc;
    private InputField inputPwd;
    private InputField inputPwd2;

    private PromptMsg promptMsg;
    private SocketMsg socketMsg;
    private void Awake()
    {
        
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.REGIST_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Start () {
        Bind(UIEvent.REGIST_PANEL_ACTIVE);
        btnRegist = transform.Find("BtnRegist").GetComponent<Button>();
        btnClose = transform.Find("BtnClose").GetComponent<Button>();

        inputAcc = transform.Find("InputAcc").GetComponent<InputField>();
        inputPwd = transform.Find("InputPwd").GetComponent<InputField>();
        inputPwd2 = transform.Find("InputPwd2").GetComponent<InputField>();

        btnRegist.onClick.AddListener(RegistClick);
        btnClose.onClick.AddListener(CloseClick);

        promptMsg = new PromptMsg();
        socketMsg = new SocketMsg();
        setPanelActive(false);
    }

    public override void OnDestroy()
    {
        btnRegist.onClick.RemoveAllListeners();
        btnClose.onClick.RemoveAllListeners();
    }

    void RegistClick()
    {
        if (string.IsNullOrEmpty(inputAcc.text))
        {
            promptMsg.Change("帐号不能为空！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            Debug.Log("帐号不能为空！");
            return;
        }
        if (string.IsNullOrEmpty(inputPwd.text)|| string.IsNullOrEmpty(inputPwd2.text))
        {
            promptMsg.Change("密码不能为空", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            Debug.Log("密码不能为空");
            return;
        }
        if(inputPwd.text.Length < 4|| inputPwd.text.Length > 16)
        {
            promptMsg.Change("密码位数在4-16位之间", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            Debug.Log("密码位数在4-16位之间");
            return;
        }
        if ((inputPwd2.text != inputPwd.text))
        {
            promptMsg.Change("两次输入的密码不一致！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            Debug.Log("两次输入的密码不一致！");
            return;
        }

        AccountDto dto = new AccountDto(inputAcc.text, inputPwd.text);
        socketMsg.Change(OpCode.ACCOUNT, AccountCode.REGIST_CREQ, dto);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
     
    void CloseClick()
    {
        setPanelActive(false);
    }
	
}
