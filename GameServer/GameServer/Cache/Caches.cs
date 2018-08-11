using GameServer.Cache.Fight;
using GameServer.Cache.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache
{
    public static class Caches
    {
        public static AccountCache Account { get; set; }
        public static UserCache User { get; set; }
        public static MatchCache Match { get; set; }
        public static FightCache Fight { get; set; }

        static Caches()
        {
            Account = new AccountCache();
            User = new UserCache();
            Match = new MatchCache();
            Fight = new FightCache();
        }
    }
}
