using AhpilyServer;
using AhpilyServer.Concurrent;
using GameServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache
{
    /// <summary>
    /// 帐号缓存
    /// </summary>
   public class AccountCache
    {
        //帐号的数据模型
        private Dictionary<string, AccountModel> accModelDict = new Dictionary<string, AccountModel>();

        /// <summary>
        /// 是否存在帐号
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool IsExist(string account)
        {
            return accModelDict.ContainsKey(account);
        }
        /// <summary>
        /// 存储帐号的ID
        /// 数据库用自增处理
        /// </summary>
        private ConcurrentInt id = new ConcurrentInt(-1);

        /// <summary>
        /// 创建账号模型
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public void Create(string account,string password)
        {
            AccountModel model = new AccountModel(id.Add_Get(),account,password);
            accModelDict.Add(model.Account,model);
        }

        /// <summary>
        /// 获取账户对应的数据模型
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public AccountModel GetModel(string account)
        {
            return accModelDict[account];
        }

        /// <summary>
        /// 帐号密码是否匹配
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool IsMatch(string account,string password)
        {
            AccountModel model = accModelDict[account];
            return model.Password == password;
        }

        //帐号对应连接对象
        private Dictionary<string, ClientPeer> accClientDict = new Dictionary<string, ClientPeer>();
        private Dictionary<ClientPeer, string> clientAccDict = new Dictionary<ClientPeer, string>();

        /// <summary>
        /// 帐号是否在线
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool IsOnline(string account)
        {
            return accClientDict.ContainsKey(account);
        }

        /// <summary>
        /// 客户端连接对象是否在线
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool IsOnline(ClientPeer client)
        {
            return clientAccDict.ContainsKey(client);
        }

        /// <summary>
        /// 用户上线
        /// </summary>
        /// <param name="client"></param>
        /// <param name="account"></param>
        public void Online(ClientPeer client,string account)
        {
            accClientDict.Add(account,client);
            clientAccDict.Add(client,account);
        }

        /// <summary>
        /// 用户下线
        /// </summary>
        /// <param name="client"></param>
        public void OffLine(ClientPeer client)
        {
            string account = clientAccDict[client];
            clientAccDict.Remove(client);
            accClientDict.Remove(account);
        }

        /// <summary>
        /// 用户下线
        /// </summary>
        /// <param name="client"></param>
        public void OffLine(string account)
        {
            ClientPeer client = accClientDict[account];
            clientAccDict.Remove(client);
            accClientDict.Remove(account);
        }

        /// <summary>
        /// 获取在线玩家的ID
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetId(ClientPeer client)
        {
            string account = clientAccDict[client];
            AccountModel model = accModelDict[account];
            return model.Id;
        }
    }
}
