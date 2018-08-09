using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case UserCode.CREATE_SRES:
                CreateResponse((int)value);
                break;
            case UserCode.GET_INFO_SRES:
                GetInfoResponse(value as UserDto);
                break;
            case UserCode.ONLINE_SRES:
                OnlineResponse((int)value);
                break;
            default:
                break;
        }
    }
    private SocketMsg socketMsg = new SocketMsg(); 

    /// <summary>
    /// 获取信息的回应
    /// </summary>
    /// <param name="userDto"></param>
    private void GetInfoResponse(UserDto user)
    {
        if(user==null)
        {
            //没有角色创建面板
            Dispatch(AreaCode.UI,UIEvent.CREATE_PANEL_ACTIVE,true);
            Debug.Log("没有角色创建面板");
        }
        else
        {
            Debug.Log("有角色");
            Dispatch(AreaCode.UI, UIEvent.CREATE_PANEL_ACTIVE, false);
            //保存服务器发来的数据
            Models.GameModel.UserDto = user;

            //更新本地界面显示
            Dispatch(AreaCode.UI,UIEvent.REFRESH_INFO_PANEL,user);
        }
    }

    /// <summary>
    /// 上线的响应
    /// </summary>
    /// <param name="result"></param>
    private void OnlineResponse(int result)
    {
        if(result==0)
        {
            //上线成功
            Debug.Log("上线成功");
        }
        else if(result==-1)
        {
            Debug.Log("非法登录");
        }
        else if(result==-2)
        {
            //没有角色不能创建
            Debug.Log("没有角色");
        }
    }

    /// <summary>
    /// 创建角色的响应
    /// </summary>
    /// <param name="result"></param>
    private void CreateResponse(int result)
    {
        if(result==-1)
        {

        }
        else if(result==-2)
        {

        }
        else if(result==0)
        {
            //创建成功
            Dispatch(AreaCode.UI, UIEvent.CREATE_PANEL_ACTIVE, false);
            socketMsg.Change(OpCode.USER,UserCode.GET_INFO_CREQ,null);
            Dispatch(AreaCode.NET,0,socketMsg);
        }
    }
}
