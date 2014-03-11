using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Cache.Internal;
using Unify.Entities;
using Unify.Network;
using Unify.Network.Serialiser;
using Unify.Network.Tcp;
using Unify.Server.Modules;
using Unify.Server.Modules.Entities;
using Unify.Util;

namespace Unify.Server.ConApp
{
	class Program
	{
		static void Log_OnLog(Log.LogType type, string message, params object[] objects)
		{
			System.Console.WriteLine("[" + type.ToString() + "] " + message, objects);
		}
		static void Main(string[] args)
		{

			Log.OnLog += Log_OnLog;
			NetworkServer _netServer = new NetworkServer();
			_netServer.SetSerialiser<BinarySerialiser>();

			UnifyServer _server = new UnifyServer(_netServer);

			_server.AddModule<NickServ>();
			_server.AddModule<ChatModule>();
			var entityModule = _server.AddModule<EntityModule>();
			entityModule.EntityProcessor = new EntityProcessor();


			//_server.SetCache<InternalCache>();

			_server.Start();
			_netServer.StartServer<TcpServer>(6321);
			Console.WriteLine("Press the enter key to quit..");
			Console.ReadLine();

			_netServer.Disconnect();

		}

		static void nickServ_OnUserLoggedIn(string username, NetworkConnection client)
		{


		}
	}
}
