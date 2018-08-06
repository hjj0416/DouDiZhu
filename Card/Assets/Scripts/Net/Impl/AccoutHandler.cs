using Protocol.code;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AccoutHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case AccountCode.LOGIN:
                loginResponse((int)value);
                break;

            case AccountCode.REGITS_SRES:
                registResponse((int)value);
                break;
            default:
                break;
        }
    }

    private PromptMsg promptMsg = new PromptMsg();

    /// <summary>
    /// 登录响应
    /// </summary>
    private void loginResponse(int result)
    {
        switch (result)
        {
            case 0:
                //跳转场景
                LoadSceneMsg msg = new LoadSceneMsg(1,()=>
                {
                    //向服务器获取信息
                    SocketMsg socketMsg = new SocketMsg(OpCode.USER,UserCode.GET_INFO_CREQ,null);
                    Dispatch(AreaCode.NET,0,socketMsg);
                });
                Dispatch(AreaCode.SENCE, SceneEvent.LOAD_SCENE, msg);
                break;
            case -1:
                promptMsg.Change("账号不存在", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            case -2:
                promptMsg.Change("账号在线", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            case -3:
                promptMsg.Change("账号密码不匹配", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            default:
                break;
        }

        //if (result == "登陆成功")
        //{
        //    promptMsg.Change(result.ToString(), Color.green);
        //    Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
        //    //跳转场景
        //    //TODO
        //    return;
        //}

        ////登录错误
        //promptMsg.Change(result.ToString(), Color.red);
        //Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
    }

    /// <summary>
    /// 注册响应
    /// </summary>
    private void registResponse(int result)
    {
        switch (result)
        {
            case 0:
                promptMsg.Change("注册成功", Color.green);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            case -1:
                promptMsg.Change("账号已经存在", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            case -2:
                promptMsg.Change("账号输入不合法", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            case -3:
                promptMsg.Change("密码不合法", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            default:
                break;
        }
        //if (result == "注册成功")
        //{
        //    promptMsg.Change(result.ToString(), Color.green);
        //    Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
        //    //跳转场景
        //    //TODO
        //    return;
        //}

        ////登录错误
        //promptMsg.Change(result.ToString(), Color.red);
        //Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
    }
}
