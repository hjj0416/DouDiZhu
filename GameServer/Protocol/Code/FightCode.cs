using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
   public class FightCode
    {
        public const int GRAB_LANDLORD_CERQ = 0;//抢地主
        public const int GRAB_LANDLORD_BRQ = 1;//广播抢地主的结果
        public const int TURN_GRAB_BRO = 2;//服务器广播转换抢地主的结果

        public const int DEAL_CREQ = 3;//客户端发起出牌的请求
        public const int DEAL_SERS = 4;//服务器给客户端出牌的响应
        public const int DEAL_BRO = 5;//服务器广播出牌的结果

        public const int PASS_CREQ = 6;//客户端不出牌的请求
        public const int PASS_SRES = 7;//服务器发给客户端不出的响应

        public const int TURN_DEAL_BRO = 8;//服务器广播转换出牌的结果

        public const int LEAVE_BRO = 9;//玩家离开的广播

        public const int OVER_BRO = 10;//游戏结束的广播

        public const int GET_CARD_SRES = 11;//服务器给客户端卡牌的响应
    }
}
