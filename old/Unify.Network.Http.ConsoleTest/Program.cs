using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Util;

namespace Unify.Network.Http.ConsoleTest
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

			HttpServer server = new HttpServer();
			server.StartListening(6112);
			Console.WriteLine("Start..");
			Console.ReadLine();
		}
	}
}
