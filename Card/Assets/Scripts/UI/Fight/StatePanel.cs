using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Protocol.Dto;

public class StatePanel : UIBase
{
    protected virtual void Awake()
    {
        Bind(UIEvent.PLAYER_HIDE_STATE);
        Bind(UIEvent.PLAYER_READY);
        Bind(UIEvent.PLAYER_LEAVE);
        Bind(UIEvent.PLAYER_ENTER);
        Bind(UIEvent.PLAYER_CHAT);
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
                        setPanelActive(true);
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

    protected virtual void Start()
    {
        imgIdentity = transform.Find("imgIdentity").GetComponent<Image>();
        txtReady = transform.Find("txtReady").GetComponent<Text>();
        imgChat = transform.Find("imgChat").GetComponent<Image>();
        txtChat = imgChat.transform.Find("Text").GetComponent<Text>();

        //默认状态
        txtReady.gameObject.SetActive(false);
        imgChat.gameObject.SetActive(false);
    }

    protected virtual void ReadyState()
    {
        txtReady.gameObject.SetActive(true);
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
    /// 显示时间
    /// </summary>
    protected int showTime = 3;
    /// <summary>
    /// 计时器
    /// </summary>
    protected float timer = 0f;
    /// <summary>
    /// 是否显示
    /// </summary>
    private bool isShow = false;

    protected virtual void Update()
    {
        if (isShow == true)
        {
            timer += Time.deltaTime;
            if (timer >= showTime)
            {
                setChatActive(false);
                timer = 0f;
                isShow = false;
            }
        }
    }

    protected void setChatActive(bool active)
    {
        imgChat.gameObject.SetActive(active);
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
        isShow = true;
    }

}
