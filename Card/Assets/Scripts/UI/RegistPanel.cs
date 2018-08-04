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

    private void Awake()
    {
        Bind(UIEvent.REGIST_PANEL_ACTIVE);
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
        btnRegist = transform.Find("BtnRegist").GetComponent<Button>();
        btnClose = transform.Find("BtnClose").GetComponent<Button>();

        inputAcc = transform.Find("InputAcc").GetComponent<InputField>();
        inputPwd = transform.Find("InputPwd").GetComponent<InputField>();
        inputPwd2 = transform.Find("InputPwd2").GetComponent<InputField>();

        btnRegist.onClick.AddListener(RegistClick);
        btnClose.onClick.AddListener(CloseClick);

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
            return;
        if (string.IsNullOrEmpty(inputPwd.text)
            || inputPwd.text.Length < 6
            || inputPwd.text.Length > 16)
            return;
        if (string.IsNullOrEmpty(inputPwd2.text)
            || (inputPwd2.text != inputPwd.text))
            return;
    }

    void CloseClick()
    {
        setPanelActive(false);
    }
	
}
