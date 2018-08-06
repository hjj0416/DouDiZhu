using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : UIBase {

    private Button BtnClose;
    private Toggle TogAudio;
    private Slider SldVolume;
    private Button BtnQuit;

    private void Awake()
    {
        Bind(UIEvent.SETTING_PANEL_ACTIVE);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.SETTING_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Start () {

        BtnClose = transform.Find("BtnClose").GetComponent<Button>();
        TogAudio = transform.Find("TogAudio").GetComponent<Toggle>();
        SldVolume = transform.Find("SldVolume").GetComponent<Slider>();
        BtnQuit = transform.Find("BtnQuit").GetComponent<Button>();

        BtnClose.onClick.AddListener(BtnCloseClick);
        BtnQuit.onClick.AddListener(BtnQuitClick);
        TogAudio.onValueChanged.AddListener(TogAudio_onValueChanged);
        SldVolume.onValueChanged.AddListener(SldVolume_onValueChanged);

        setPanelActive(false);
    }

    public override void OnDestroy()
    {
        BtnClose.onClick.RemoveAllListeners();
        BtnQuit.onClick.RemoveAllListeners();
        TogAudio.onValueChanged.RemoveAllListeners();
        SldVolume.onValueChanged.RemoveAllListeners();
    }

    /// <summary>
    /// 关闭面板
    /// </summary>
    void BtnCloseClick()
    {
        setPanelActive(false);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    void BtnQuitClick()
    {
        Application.Quit();
    }

    /// <summary>
    /// 音乐开关
    /// </summary>
    /// <param name="result"></param>
    void TogAudio_onValueChanged(bool value)
    {
        //操作声音
        //TODO
    }

    /// <summary>
    /// 滑动条滑动时
    /// </summary>
    /// <param name="value"></param>
    void SldVolume_onValueChanged(float value)
    {

    }
}
