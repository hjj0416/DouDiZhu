using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using Protocol.code;
using Protocol.Dto;

namespace GameServer.Logic
{
    public class AccountHandler : IHandler
    {
        public void OnDisconnect(ClientPeer client)
        {
            throw new NotImplementedException();
        }

        public void OnRecive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case AccountCode.REGIST_CREQ:
                    {
                        AccountDto dto = value as AccountDto;
                        Console.WriteLine(dto.Account);
                        Console.WriteLine(dto.Password);
                    }
                    break;
                case AccountCode.LOGIN:
                    {
                        AccountDto dto = value as AccountDto;
                        Console.WriteLine(dto.Account);
                        Console.WriteLine(dto.Password);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
