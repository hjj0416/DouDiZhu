using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    //匹配的操作码
    public class MatchCode
    {
        //进入匹配队列
        public const int ENTER_CREQ = 0;
        public const int ENTER_SRES = 1;
        public const int ENTER_BRO = 2;

        //离开匹配队列
        public const int LEAVE_CREQ = 3;
        public const int LEAVE_BRO = 4;

        //准备
        public const int READY_CREQ = 5;
        public const int READY_BRO = 6;

        //开始游戏
        public const int START_BRO = 7;
    }
}
