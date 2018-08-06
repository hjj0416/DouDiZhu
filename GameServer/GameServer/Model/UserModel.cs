using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    public class UserModel
    {
        public int Id;//id
        public string Name;//角色名
        public int Been;//欢乐豆

        public int WinCount;//胜场
        public int LoseCount;//负场
        public int RunCount;//逃跑场

        public int Lv;//等级
        public int Exp;//经验

        public int AccountModel;//外键：与帐号关联

        public UserModel(int id,string name,int accountId)
        {
            this.Id = id;
            this.Name = name;
            this.Been = 10000;
            this.WinCount = 0;
            this.LoseCount = 0;
            this.RunCount = 0;
            this.Lv = 1;
            this.Exp = 0;
            this.AccountModel = accountId;
        }
    }
}
