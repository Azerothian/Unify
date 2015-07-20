using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Unify.Network.Interfaces;
using Unify.Util;

namespace Unify.Network.Tcp
{
	public class TcpServer : INetworkServerModule
	{

		public event Action<INetworkConnectionModule> OnClientConnected;
		public event Action<INetworkConnectionModule> OnClientDisconnected;

		private Dictionary<string, TcpClient> _clients = new Dictionary<string, TcpClient>();

		private Socket server;

		public void StartListening(Uri uri)
		{
			IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port);

			server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			server.NoDelay = true;
			server.Bind(ipep);
			server.Listen(10);
			server.BeginAccept(AcceptConnection, server);
		}
		private void AcceptConnection(IAsyncResult iar)
		{
			try
			{
				Socket oldserver = (Socket)iar.AsyncState;
				Socket client = oldserver.EndAccept(iar);
				client.NoDelay = true;

				var stream = new TcpClient(client);
				_clients.Add(String.Format("{0}", client.RemoteEndPoint), stream);
				var key = client.RemoteEndPoint.ToString();
				if (OnClientConnected != null)
				{
					OnClientConnected(_clients[key]);
				}
				_clients[key].OnDisconnectedEvent += () =>
					{
						if(OnClientDisconnected != null)
						{
							OnClientDisconnected(_clients[key]);
						}
						_clients.Remove(key);
					};

			}
			catch (Exception ex)
			{
				Log.Critical(ex.Message);
			}
			finally
			{
				try
				{
					server.BeginAccept(AcceptConnection, server);
				}
				catch (ObjectDisposedException)
				{
					//is shutting down!!
				}
			}
		}

		public void Disconnect()
		{

			foreach (var v in _clients.Keys.ToArray())
			{
				_clients[v].Disconnect();
			}

			if (server.Connected)
			{
				server.Shutdown(SocketShutdown.Both);
			}
			server.Close();
		}



	}
}
