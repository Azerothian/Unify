using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Network.Interfaces;
using Unify.Util;


namespace Unify.Network
{
	public class NetworkServer
	{
		public ISerialiser Serialiser;
		public List<INetworkServerModule> NetworkServers {get;set;}

		public event GenericVoidDelegate<NetworkConnection> OnClientConnected;
		public event GenericVoidDelegate<NetworkConnection> OnClientDisconnected;
		//public event GenericVoidDelegate<UnifyClient, object> OnClientMessage;
		protected List<INetworkServerModule> Servers = new List<INetworkServerModule>();
		protected List<NetworkConnection> Clients = new List<NetworkConnection>();
		public NetworkServer()
		{
			//Serialiser = new TSerialiser();
		}

		public void StartServer<TNetworkServer>(int port) where TNetworkServer : INetworkServerModule, new()
		{
			var newServer = new TNetworkServer();
			newServer.StartListening(port);
			newServer.OnClientConnected += newServer_OnClientConnected;
			Servers.Add(newServer);

		}
		public void SetSerialiser<TSerialiser>() where TSerialiser : ISerialiser, new()
		{
			Serialiser = new TSerialiser();

		}


		void newServer_OnClientConnected(INetworkConnectionModule obj1)
		{
			var newClient = new NetworkConnection(obj1);
			newClient.SetSerialiser(Serialiser);
			//newClient.OnMessagedRecieved += (UnifyClient cli, object message) =>
			//{
			//	if (OnClientMessage != null)
			//	{
			//		OnClientMessage(cli, message);
			//	}
			//};
			newClient.OnDisconnected += (NetworkConnection cli) =>
			{
				if (OnClientDisconnected != null)
				{
					OnClientDisconnected(newClient);
				}
				Clients.Remove(newClient);
			};
			if (OnClientConnected != null)
			{
				OnClientConnected(newClient);
			}
			Clients.Add(newClient);
		}


		//public void SetSerialiser<TSerialiser>() where TSerialiser : ISerialiser, new()
		//{
		//	//Serialiser = new TSerialiser();
		//}

		public void Disconnect()
		{
			foreach (var s in Servers)
			{
				s.Disconnect();

			}
		}

  }
}
