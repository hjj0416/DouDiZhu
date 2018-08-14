using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Protocol.Dto;
using Protocol.Dto.Fight;

public class StatePanel : UIBase
{
    protected virtual void Awake()
    {
        Bind(UIEvent.PLAYER_HIDE_STATE);
        Bind(UIEvent.PLAYER_READY);
        Bind(UIEvent.PLAYER_LEAVE);
        Bind(UIEvent.PLAYER_ENTER);
        Bind(UIEvent.PLAYER_CHAT);
        Bind(UIEvent.PLAY_CHANGE_IDENTITY);
        Bind(UIEvent.SHOW_TIMER_PANEL);
        Bind(UIEvent.HIDE_TIMER_PANEL);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.PLAYER_HIDE_STATE:
                {
                    txtReady.gameObject.SetActive(false);
                }
                break;
            case UIEvent.PLAYER_READY:
                {
                    if (userDto == null)
                        break;
                    int userId = (int)message;
                    //如果是自身角色 就显示
                    if (userDto.Id == userId)
                        ReadyState();
                    break;
                }
            case UIEvent.PLAYER_LEAVE:
                {
                    if (userDto == null)
                        break;
                    int userId = (int)message;
                    if (userDto.Id == userId)
                        setPanelActive(false);
                    break;
                }
            case UIEvent.PLAYER_ENTER:
                {
                    if (userDto == null)
                        break;
                    int userId = (int)message;
                    if (userDto.Id == userId)
                    {
                        setPanelActive(true);
                        SetName(userDto.Name);
                        //Debug.Log("statePanelEnter");
                    }
                    break;
                }
            case UIEvent.PLAYER_CHAT:
                {
                    if (userDto == null)
                        break;
                    ChatMsg msg = message as ChatMsg;
                    if (userDto.Id == msg.UserId)
                        showChat(msg.Text);
                    break;
                }
            case UIEvent.PLAY_CHANGE_IDENTITY:
                {
                    if (userDto == null)
                        break;
                    int userId = (int)message;
                    if (userDto.Id == userId)
                        setIdentity(1);
                    break;
                }
            case UIEvent.SHOW_TIMER_PANEL:
                {
                    if (userDto == null)
                        break;
                    TimeDto timeDto = message as TimeDto;
                    if (userDto.Id == timeDto.Id)
                    {
                        ShowTimer(timeDto.time);
                    }
                    break;
                }
            case UIEvent.HIDE_TIMER_PANEL:
                {
                    if (userDto == null)
                        break;
                    int userId = (int)message;
                    if (userDto.Id == userId)
                        setTimerActive(false);
                        break;
               }
            default:
                break;
        }
    }

    /// <summary>
    /// 角色的数据
    /// </summary>
    protected UserDto userDto;

    protected Image imgIdentity;
    protected Text txtReady;
    protected Image imgChat;
    protected Text txtChat;
    protected Text txtTime;
    protected Text txtName;

    protected virtual void Start()
    {
        imgIdentity = transform.Find("imgIdentity").GetComponent<Image>();
        txtReady = transform.Find("txtReady").GetComponent<Text>();
        imgChat = transform.Find("imgChat").GetComponent<Image>();
        txtChat = imgChat.transform.Find("Text").GetComponent<Text>();
        txtTime = transform.Find("txtTime").GetComponent<Text>();
        txtName = transform.Find("txtName").GetComponent<Text>();


        //默认状态
        txtReady.gameObject.SetActive(false);
        imgChat.gameObject.SetActive(false);
        txtTime.gameObject.SetActive(false);
    }

    protected virtual void ReadyState()
    {
        txtReady.gameObject.SetActive(true);
    }

    protected virtual void SetName(string name)
    {
        txtName.text = name;
    }

    /// <summary>
    /// 设置身份
    ///     0 就是农民 1 就是地主
    /// </summary>
    protected void setIdentity(int identity)
    {
        //string identityStr = identity == 0 ? "Farmer" : "Landlord";
        if (identity == 0)
        {
            imgIdentity.sprite = Resources.Load<Sprite>("Identity/Farmer");
        }
        else if (identity == 1)
        {
            imgIdentity.sprite = Resources.Load<Sprite>("Identity/Landlord");
        }
    }

    /// <summary>
    /// 聊天界面显示时间
    /// </summary>
    protected int ChatShowTime = 3;
    /// <summary>
    /// 计时器
    /// </summary>
    protected float timer1 = 0f;

    /// <summary>
    /// 是否显示聊天
    /// </summary>
    private bool isChatShow = false;

    /// <summary>
    /// 是否显示计时器
    /// </summary>
    protected bool isTimerShow = false;
    /// <summary>
    /// 计时器显示的时间
    /// </summary>
    protected float TimerShowTime;
    /// <summary>
    /// 保存上次更新界面时的时间
    /// </summary>
    private int txtTimer = -1;

    protected virtual void Update()
    {
        if (isChatShow == true)
        {
            timer1 += Time.deltaTime;
            if (timer1 >= ChatShowTime)
            {
                setChatActive(false);
                timer1 = 0f;
                isChatShow = false;
            }
        }
        if (isTimerShow)
        {
            TimerShowTime = TimerShowTime - Time.deltaTime;
            if (TimerShowTime < 0)
            {
               // setTimerActive(false);
                isTimerShow = false;
            }
            UpdateTime(TimerShowTime);
        }
    }

    protected void setChatActive(bool active)
    {
        imgChat.gameObject.SetActive(active);
    }

    protected void setTimerActive(bool active)
    {
        txtTime.gameObject.SetActive(active);
    }

    /// <summary>
    /// 每秒刷新显示界面计时器
    /// </summary>
    /// <param name="time"></param>
    private void UpdateTime(float time)
    {
        if (time < 0)       
            return;

        int tTime = (int)time;
        if(tTime!=txtTimer)
        {
            txtTime.text = tTime.ToString();
            txtTimer = tTime; ;
        }
    }

    /// <summary>
    /// 外界调用的  显示聊天
    /// </summary>
    /// <param name="text">聊天的文字</param>
    protected void showChat(string text)
    {
        //设置文字
        txtChat.text = text;
        //显示动画
        setChatActive(true);
        isChatShow = true;
    }

    /// <summary>
    /// 外界调用的 显示计时器
    /// </summary>
    /// <param name="time"></param>
    protected void ShowTimer(long time)
    {
        //转换为秒
        TimerShowTime = time/10000000.0f;
        txtTimer = -1;
        setTimerActive(true);
        isTimerShow = true;
    }

}
