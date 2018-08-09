using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using GameServer.Cache;
using Protocol.Code;
using Protocol.Dto;

namespace GameServer.Logic
{
    public class AccountHandler : IHandler
    {
        AccountCache accountCache = Caches.Account;

        public void OnDisconnect(ClientPeer client)
        {
           if(accountCache.IsOnline(client))
                accountCache.OffLine(client);
        }

        public void OnRecive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case AccountCode.REGIST_CREQ:
                    {
                        AccountDto dto = value as AccountDto;
                        Console.Write(string.Format("用户注册请求：帐号：{0}，密码：{1}   ",dto.Account,dto.Password));
                        Regist(client,dto.Account,dto.Password);
                    }
                    break;
                case AccountCode.LOGIN:
                    {
                        AccountDto dto = value as AccountDto;
                        Console.Write(string.Format("用户登录请求：帐号：{0}，密码：{1}   ", dto.Account, dto.Password));
                        Login(client, dto.Account, dto.Password);
                    }
                    break;
                default:
                    break;
            }
        }

        private void Regist(ClientPeer client,string account,string password)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (accountCache.IsExist(account))
                {
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, -1);
                    Console.WriteLine(string.Format("错误：帐号已经存在"));
                    return;
                }

                if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
                {
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, -2);
                    Console.WriteLine(string.Format("错误：账号不合法"));
                    return;
                }

                if (string.IsNullOrEmpty(password) || password.Length < 4 || password.Length > 16)
                {
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, -3);
                    Console.WriteLine(string.Format("错误：密码不合法"));
                    return;
                }

                accountCache.Create(account, password);
                client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, 0);
                Console.WriteLine(string.Format("注册成功"));
            });
            
        }

        private void Login(ClientPeer client, string account, string password)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (!accountCache.IsExist(account))
                {
                    //帐号不存在
                    client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, -1);
                    Console.WriteLine(string.Format("错误：帐号不存在"));
                    return;
                }

                if (!accountCache.IsMatch(account, password))
                {
                    //帐号密码不匹配
                    client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, -2);
                    Console.WriteLine(string.Format("错误：帐号密码不匹配"));
                    return;
                }

                if (accountCache.IsOnline(account))
                {
                    //帐号在线
                    client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, -3);
                    Console.WriteLine(string.Format("错误：帐号在线"));
                    return;
                }

                //登陆成功
                accountCache.Online(client, account);
                client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, 0);
                Console.WriteLine(string.Format("登陆成功"));
            });          
        }
    }
}
