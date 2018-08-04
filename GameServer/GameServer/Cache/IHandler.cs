using AhpilyServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache
{
    public interface IHandler
    {
        void OnRecive(ClientPeer client, int subCode,object value);

        void OnDisconnect(ClientPeer client);
    }
}
