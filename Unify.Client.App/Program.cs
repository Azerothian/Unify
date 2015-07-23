using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unify.Client.Modules;
using Unify.Client.Modules.Chat;
using Unify.Client.Modules.Entities;
using Unify.Messages.NickServ;
using Unify.Network;
using Unify.Network.Tcp;
using Unify.Network.WebSockets;
using Unify.Serialiser.JsonNet;
using Unify.Util;

namespace Unify.Client.App
{
  class Program
  {
    static void Log_OnLog(Log.LogType type, string message, params object[] objects)
    {
      System.Console.WriteLine("[" + type.ToString() + "] " + message, objects);
    }
    static void Main(string[] args)
    {

      Thread.Sleep(1000);
      Log.OnLog += Log_OnLog;
      Console.WriteLine("Unify.Client.App 0.0101a 03022014");
      Console.Write("Username:");
      var username = Console.ReadLine();
      Log.Info("Connecting to the Server...");
      NetworkConnection networkClient = new NetworkConnection();
      networkClient.SetSerialiser<JsonNetSerialiser>();
      Console.WriteLine("1. TCP");
      Console.WriteLine("2. WS");
      var uri = "";
      switch(Console.ReadKey().Key)
      {
        case ConsoleKey.D1:
          networkClient.AddNetworkClient<TcpClient>();
          uri = "tcp://127.0.0.1:6322/";
          break;
        case ConsoleKey.D2:
          networkClient.AddNetworkClient<WSClient>();
          uri = "ws://127.0.0.1:6321/";
          break;
      }

      //

      var nickServ = new NicknameModule();
      var chatModule = new ChatModule();
			var entityModule = new EntityModule();
			entityModule.OnEntityCreate += entityModule_OnEntityCreate;
			entityModule.OnEntityDelete += entityModule_OnEntityDelete;
			entityModule.OnEntityUpdate += entityModule_OnEntityUpdate;
      UnifyClient client = new UnifyClient(networkClient, nickServ, chatModule, entityModule);

      networkClient.OnConnected += (NetworkConnection connection) =>
      {
        Log.Info("Connected to the Server.");
        nickServ.Login(username);
        
      };


      chatModule.OnJoinChannel += (string room, NetworkConnection connection) =>
      {
        Log.Info("You have joined channel: {0}", room);
      };
      chatModule.OnChatMessage += (Messages.Chat.ChatMessage message, NetworkConnection connection) =>
      {
        Log.Info("[" + message.Target + "]"+message.From +">" + message.Message);
      };
      chatModule.OnChannelUserList += (string room, IEnumerable<string> users, NetworkConnection connection) =>
      {
        Log.Info("Channel List Received: {0}", users.Count());
      };

      nickServ.OnLoginResponse += (LoginResponse response) =>
      {
        chatModule.Username = nickServ.Username;
      };

      networkClient.Connect(new Uri(uri));
      
      do
      {
        Console.Write(">");
        var output = Console.ReadLine();
        if (output == "/exit")
          break;
        chatModule.SendMessage("Global", output);
      } while (true);
			networkClient.Disconnect();
    }

		static void entityModule_OnEntityUpdate(Messages.Entities.EntityUpdate obj1)
		{
			Log.Info("[Entity] Update Entity.. IsCurrentUser {0}", obj1.IsUser);
		}

		static void entityModule_OnEntityDelete(Messages.Entities.EntityDelete obj1)
		{
			Log.Info("[Entity] Delete Entity..");
		}

		static void entityModule_OnEntityCreate(Messages.Entities.EntityCreate obj1)
		{
			Log.Info("[Entity] Create Entity..");
		}



  }
}
