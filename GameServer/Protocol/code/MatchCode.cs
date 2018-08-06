using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.code
{
    //匹配的操作码
    public class MatchCode
    {
        //进入匹配队列
        public const int ENTER_CREQ = 0;
        public const int ENTER_SRES = 1;

        //离开匹配队列
        public const int LEVEL_CREQ = 2;
        public const int LEVEL_BRO = 3;

        //准备
        public const int READY_CREQ = 4;
        public const int READY_BRO = 5;

        //开始游戏
        public const int START_BRO = 6;
    }
}
