using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using GameServer.Cache;
using GameServer.Model;
using Protocol.Code;
using Protocol.Dto;

namespace GameServer.Logic
{
    public class UserHandler : IHandler
    {
        private UserCache userCache = Caches.User;
        private AccountCache accountCache = Caches.Account;

        public void OnDisconnect(ClientPeer client)
        {
            if (userCache.IsOnline(client)) 
                userCache.Offline(client);
        }

        public void OnRecive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case UserCode.CREATE_CREQ:
                    Create(client,value.ToString());
                    break;
                case UserCode.GET_INFO_CREQ:
                    GetInfo(client);
                    break;
                case UserCode.ONLINE_CREQ:
                    Online(client);
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="client"></param>
        /// <param name="name"></param>
        private void Create(ClientPeer client, string name)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (!accountCache.IsOnline(client))
                {
                    client.Send(OpCode.USER, UserCode.CREATE_SRES, -1);//非法登录
                    Console.WriteLine("创建角色---非法登录");
                    return;
                }
                int accountId = accountCache.GetId(client);
                if (userCache.isExist(accountId))
                {
                    client.Send(OpCode.USER, UserCode.CREATE_SRES, -2);//重复创建
                    Console.WriteLine("创建角色---重复创建");
                    return;
                }
                userCache.Create(name, accountId);
                client.Send(OpCode.USER, UserCode.CREATE_SRES, -0);//创建成功
                Console.WriteLine("创建角色---创建成功");
            });

        }


        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="client"></param>
        private void GetInfo(ClientPeer client)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (!accountCache.IsOnline(client))
                {
                    //client.Send(OpCode.USER, UserCode.GET_INFO_SRES, null);//非法登录
                    Console.WriteLine("获取角色信息---非法登录");
                    return;
                }
                int accountId = accountCache.GetId(client);
                if ((userCache.isExist(accountId)) == false)
                {
                    client.Send(OpCode.USER, UserCode.GET_INFO_SRES, null);//没有角色无法获取信息
                    Console.WriteLine("获取角色信息---没有角色无法获取信息");
                    return;
                }
                //上线角色
                Online(client);
                UserModel model = userCache.GetModelByAccountId(accountId);
                UserDto dto = new UserDto(model.Id,model.Name,model.Been,model.WinCount,model.LoseCount,model.RunCount,model.Lv,model.Exp);
                client.Send(OpCode.USER, UserCode.GET_INFO_SRES, dto);
            });
        }

        /// <summary>
        /// 上线
        /// </summary>
        /// <param name="client"></param>
        private void Online(ClientPeer client)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (!accountCache.IsOnline(client))
                {
                    client.Send(OpCode.USER, UserCode.GET_INFO_SRES, -1);//非法登录
                    Console.WriteLine("上线---非法登录");
                    return;
                }
                int accountId = accountCache.GetId(client);
                if (userCache.isExist(accountId) == false)
                {
                    client.Send(OpCode.USER, UserCode.ONLINE_SRES, -2);//没有角色
                    Console.WriteLine("上线---没有角色");
                    return;
                }
                int userId = userCache.GetId(accountId);
                userCache.Online(client, userId);
                client.Send(OpCode.USER, UserCode.ONLINE_SRES, 0);//上线成功
                Console.WriteLine(string.Format("上线---上线成功，当前在线玩家：{0}人",accountCache.GetOnlineNum()));
            });

        }
    }
}
