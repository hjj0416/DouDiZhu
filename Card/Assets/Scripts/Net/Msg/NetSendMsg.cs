using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 封装发送网络消息的类
/// </summary>
public class NetSendMsg
{
    public int OpCode;
    public int SubCode;
    public object Value;

    public void Change(int opCode,int subCode,object value)
    {
        this.OpCode = opCode;
        this.SubCode = subCode;
        this.Value = value;
    }
}

