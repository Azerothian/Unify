using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Unify.Network.Tcp;
using Unify.Serialiser;
using Unify.Serialiser.Json;

using Unify.Network;
using Unify.Network.Serialiser;
using Unify.Util;

namespace Unify.ConsoleV2Test
{
	class Program
	{
		public static Random Rand = new Random();

		

		static void Log_OnLog(Log.LogType type, string message, params object[] objects)
		{
			System.Console.WriteLine("[" + type.ToString() + "] " + message, objects);
		}
		static void Main(string[] args)
		{
			List<ClientTester> clients = new List<ClientTester>();
			Log.OnLog += Log_OnLog;

			ServerTester stest = new ServerTester();
			Log.Info("1");
			Thread.Sleep(1000);
			Log.Info("12");
			for (var i = 0; i < 100; i++)
			{
				ClientTester ctest = new ClientTester(6112);
				ClientTester ctest2 = new ClientTester(6113);
				clients.Add(ctest);
				clients.Add(ctest2);
			}

			Log.Info("Fin?");
			System.Console.ReadLine();
			foreach(var c in clients)
			{
				c.Disconnect();

			}
			stest.Disconnect();
		}

	}

	public class ClientTester
	{
		NetworkClient _client;
		public ClientTester(int port)
		{
			_client = new NetworkClient();
			_client.AddNetworkClient<TcpClient>();
			_client.SetSerialiser<BinarySerialiser>();

			_client.On("response", (NetworkClient sender, DateTime message) =>
			{

				var result = (DateTime.Now - message).TotalMilliseconds;
				//Thread.Sleep(10);
				Log.Info("Time: {0}",result);
				Thread.Sleep(Program.Rand.Next(1000));
				_client.Emit("response", sender, DateTime.Now);
				
			});
			_client.On("connected", (NetworkClient sender, DateTime message) =>
			{
				_client.Emit("response", sender, DateTime.Now);
			});
			_client.Connect("127.0.0.1", port);
			
		}


		internal void Disconnect()
		{
			_client.Disconnect();
		}
	}

	public class ServerTester
	{
		NetworkServer _server = new NetworkServer();
		public ServerTester()
		{
			_server.SetSerialiser<BinarySerialiser>();
			_server.OnClientConnected += (NetworkClient client) =>
				{
					//Log.Info("Client Connected");
					client.On("response", (NetworkClient sender, DateTime message) =>
				{
					//Log.Info("[Server] Response");
					client.Emit("response", sender, message);
				});
				};
			_server.StartServer<TcpServer>(6112);
			_server.StartServer<TcpServer>(6113);

		}
		internal void Disconnect()
		{
			_server.Disconnect();
		}

	}

	
}
