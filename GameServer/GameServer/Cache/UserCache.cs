using GameServer.Model;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using AhpilyServer.Concurrent;

namespace GameServer.Cache
{
    public class UserCache
    {
        //角色id对应的数据模型
        private Dictionary<int, UserModel> idModelDict = new Dictionary<int, UserModel>();

        //帐号id对应的角色id
        private Dictionary<int, int> accIdUIdDict = new Dictionary<int, int>();

        ConcurrentInt id = new ConcurrentInt(-1);

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="client"></param>
        /// <param name="name"></param>
        /// <param name="accountId"></param>
        public void Create(string name,int accountId)
        {
            UserModel model = new UserModel(id.Add_Get(),name,accountId);
            idModelDict.Add(model.Id,model);
            accIdUIdDict.Add(model.AccountModel,model.Id);
        }

        /// <summary>
        /// 判断此账号下是否有角色
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public bool isExist(int accountId)
        {
            return accIdUIdDict.ContainsKey(accountId);
        }

        /// <summary>
        /// 根据账号id获取角色数据模型
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public UserModel GetModelByAccountId(int accountId)
        {
            int userId = accIdUIdDict[accountId];
            UserModel model = idModelDict[userId];
            return model;
        }

        /// <summary>
        /// 根据账号id获取角色id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public int GetId(int accountId)
        {
            return accIdUIdDict[accountId];
        }

        //存储在线玩家  只有在线玩家才有client对象
        private Dictionary<int, ClientPeer> idClientDict = new Dictionary<int, ClientPeer>();
        private Dictionary<ClientPeer, int> clientIdDict = new Dictionary<ClientPeer, int>();

        public bool IsOnline(ClientPeer client)
        {
            return clientIdDict.ContainsKey(client);
        }

        public bool IsOnline(int id)
        {
            return idClientDict.ContainsKey(id);
        }

        /// <summary>
        /// 角色上线
        /// </summary>
        /// <param name="client"></param>
        /// <param name="id"></param>
        public void Online(ClientPeer client,int id)
        {
            idClientDict.Add(id,client);
            clientIdDict.Add(client,id);
        }

        /// <summary>
        /// 角色下线
        /// </summary>
        /// <param name="client"></param>
        public void Offline(ClientPeer client)
        {
            int id = clientIdDict[client];
            clientIdDict.Remove(client);
            idClientDict.Remove(id);
        }

        /// <summary>
        /// 根据连接对象获取角色模型
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public UserModel GetModelByClientPeer(ClientPeer client)
        {
            int id = clientIdDict[client];
            UserModel model = idModelDict[id];
            return model;
        }

        /// <summary>
        /// 根据角色id获取连接对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClientPeer GetClientPeer(int id)
        {
            return idClientDict[id];
        }

        public int GetId(ClientPeer client)
        {
            return clientIdDict[client];
        }
    }
}
