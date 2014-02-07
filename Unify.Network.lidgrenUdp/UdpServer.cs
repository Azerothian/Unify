using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Unify.Network.Interfaces;

namespace Unify.Network.lidgrenUdp
{
	public class UdpServer : INetworkServerModule

    {
			public event Util.GenericVoidDelegate<INetworkConnectionModule> OnClientConnected;
			NetServer _server;
			public void StartListening(int port)
			{
				
				NetPeerConfiguration config = new NetPeerConfiguration("UnifyUdp");
				config.Port = port;
				_server = new NetServer(config);
				//_server.

			}
			public void Disconnect() { }
		}
}
