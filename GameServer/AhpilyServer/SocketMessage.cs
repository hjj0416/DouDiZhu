using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhpilyServer
{
    //网络消息
    public class SocketMessage
    {
        //操作码
        public int OpCode { get; set; }
        //子操作
        public int SubCode { get; set; }
        //参数
        public object Value { get; set; }

        public SocketMessage()
        {

        }

        public SocketMessage(int opCode,int subCode,object value)
        {
            this.OpCode = opCode;
            this.SubCode = subCode;
            this.Value = value;
        }
    }
}
