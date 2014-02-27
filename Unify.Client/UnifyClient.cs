using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Client.Interfaces;
using Unify.Network;
using Unify.Network.Serialiser;
using Unify.Network.Tcp;
using Unify.Util;

namespace Unify.Client
{
  public class UnifyClient
  {
    public NetworkConnection Connection;
    public List<IModule> Modules = new List<IModule>();
    public UnifyClient(NetworkConnection client, params IModule[] modules)
    {
      Connection = client;
      if (modules != null)
      {
        Modules.AddRange(modules);
      }
      foreach (var m in Modules)
      {
        m.UnifyClient = this;
				Connection.OnConnected += (NetworkConnection connection) => { m.OnConnected(); };
				Connection.OnDisconnected += (NetworkConnection connection) => { m.OnDisconnected(); };
        m.Initialise();
      }
      //Connection.OnConnected += _client_OnConnected;
      client.On<string>("console", (NetworkConnection cli, string message) =>
      {
        Log.Info("[Server] {0}", message);
      });
    }

  


		//void _client_OnConnected(NetworkConnection client)
		//{
		//	//Log.Info("Client has connected");
		//}

  }
}
