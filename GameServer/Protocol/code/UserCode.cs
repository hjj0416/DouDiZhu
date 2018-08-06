using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.code
{
    /// <summary>
    /// 有关用户的操作码
    /// </summary>
    public class UserCode
    {
        public const int CREATE_SRES = 0;
        public const int CREATE_CERQ = 1;
        public const int GET_INFO_CREQ = 2;
        public const int GET_INFO_SRES = 3;
        public const int ONLINE_CERQ = 4;
        public const int ONLINE_SRES = 5;
    }
}
