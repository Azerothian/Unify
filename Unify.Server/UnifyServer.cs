using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Interfaces;
using Unify.Network;
using Unify.Network.Interfaces;
using Unify.Server.Interfaces;
using Unify.Util;

namespace Unify.Server
{
    public class UnifyServer
    {
			private Dictionary<string, string> UserKeys = new Dictionary<string, string>();
      public event GenericVoidDelegate<Guid, NetworkConnection> OnClientConnected;
      public event GenericVoidDelegate<Guid, NetworkConnection> OnClientDisconnected;

      public NetworkServer NetworkServer { get; set; }
      //public ICache Cache { get; set; }

      public UnifyServer(NetworkServer server)
      {
        NetworkServer = server;
        NetworkServer.OnClientConnected += NetworkServer_OnClientConnected;
        NetworkServer.OnClientDisconnected += NetworkServer_OnClientDisconnected;
      }

      void NetworkServer_OnClientDisconnected(NetworkConnection client)
      {

				UserKeys.Remove(client.Key);

        //Cache.RemoveByKey("system","user."+client.Key);
      }

      void NetworkServer_OnClientConnected(NetworkConnection client)
      {
        var guid = Guid.NewGuid();

				UserKeys.Add(client.Key, guid.ToString());

        //Cache.Set("system", "user." + client.Key, guid.ToString());
        if (OnClientConnected != null)
        {
          OnClientConnected(guid, client);
        }
      }
      public List<IModule> Modules = new List<IModule>();

      public TModule AddModule<TModule>() where TModule : IModule, new()
      {
        var newModule = new TModule();
        newModule.UnifyServer = this;
        OnClientConnected += newModule.OnClientConnected;
        OnClientDisconnected += newModule.OnClientDisconnected;
        Modules.Add(newModule);
        return newModule;
      }

      public TModule GetModule<TModule>() where TModule : IModule
      {
        foreach (var m in Modules)
        {
          if (m is TModule)
          {
            return (TModule)m;
          }
        }
        return default(TModule);
      }
      

			//public void SetCache<TCache>() where TCache : ICache, new()
			//{
			//	Cache = new TCache();
			//}
      public void Start()
      {
        foreach (var m in Modules)
        {
					Log.Info("[UnifyServer] [Start] " + m.GetType().ToString());
          m.Start();
        }
      }
			
    }
}
