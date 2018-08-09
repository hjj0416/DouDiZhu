using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    public class AccountCode
    {
        //注册操作码
        public const int REGIST_CREQ = 0;//client request 帐号密码
        public const int REGIST_SRES = 1;//sever response

        //登录操作码
        public const int LOGIN = 2;//帐号密码
    }
}
