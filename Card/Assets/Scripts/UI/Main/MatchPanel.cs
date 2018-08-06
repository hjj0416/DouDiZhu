using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchPanel : UIBase
{

    private void Awake()
    {
        Bind(UIEvent.MATCH_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.MATCH_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    private Image imgBg;
    private Text txtDes;
    private Button btnCancel;
    private Button btnEnter;

    // Use this for initialization
    void Start()
    {


        imgBg = transform.Find("ImgBg").GetComponent<Image>();
        txtDes = transform.Find("txtDes").GetComponent<Text>();
        btnCancel = transform.Find("btnCancel").GetComponent<Button>();
        btnEnter = transform.Find("btnEnter").GetComponent<Button>();

        btnCancel.onClick.AddListener(cancelClick);

        setPanelActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (txtDes.gameObject.activeInHierarchy == false)
            return;

        timer += Time.deltaTime;
        if (timer >= intervalTime)
        {
            doAnimation();
            timer = 0f;
        }
    }

    public override void OnDestroy()
    {
        btnCancel.onClick.RemoveListener(cancelClick);
    }

    private void matchClick()
    {
        //向服务器发起开始匹配的请求
        //TODO

        setObjectsActive(true);
    }

    private void cancelClick()
    {
        //向服务器发起离开匹配的请求
        //TODO

        setPanelActive(false);
    }

    /// <summary>
    /// 控制点击匹配按钮之后的显示的物体
    /// </summary>
    private void setObjectsActive(bool active)
    {
        imgBg.gameObject.SetActive(active);
        txtDes.gameObject.SetActive(active);
        btnCancel.gameObject.SetActive(active);
    }

    private string defaultText = "正在寻找房间";
    //点的数量
    private int dotCount = 0;
    //动画的间隔时间
    private float intervalTime = 1f;
    //计时器
    private float timer = 0f;

    /// <summary>
    /// 做动画
    /// </summary>
    private void doAnimation()
    {
        txtDes.text = defaultText;

        //点增加
        dotCount++;
        if (dotCount > 5)
            dotCount = 1;

        for (int i = 0; i < dotCount; i++)
        {
            txtDes.text += ".";
        }
    }

}
