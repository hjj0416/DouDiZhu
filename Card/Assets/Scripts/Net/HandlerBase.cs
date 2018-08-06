using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 客户端处理基类
/// </summary>
public abstract class HandlerBase
{
    public abstract void OnReceive(int subCode,object value);

    protected void Dispatch(int areaCode,int eventCode,object message)
    {
        MsgCenter.Instance.Dispatch(areaCode,eventCode,message); 
    }
}

