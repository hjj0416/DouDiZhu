using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
   public class AccountModel
    {
        public int Id;
        public string Account;
        public string Password;

        public AccountModel(int id,string account,string password)
        {
            this.Id = id;
            this.Account = account;
            this.Password = password;
        }
    }
}
