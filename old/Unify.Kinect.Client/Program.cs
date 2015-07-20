using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Client;
using Unify.Network;
using Unify.Network.Serialiser;
using Unify.Network.Tcp;
using Unify.Util;

namespace Unify.Kinect.Client
{
  public class Program
  {
    static NetworkConnection _networkClient;
    public static void Main(params string[] args)
    {
      Util.Log.OnLog += Log_OnLog;

      Console.WriteLine("REady when you are!");
      Console.ReadLine();

      _networkClient = new NetworkConnection();
      _networkClient.SetSerialiser<BinarySerialiser>();
      _networkClient.AddNetworkClient<TcpClient>();

      var skeletonModule = new SkeletonModule();


      UnifyClient client = new UnifyClient(_networkClient, skeletonModule);

      _networkClient.OnConnected += (NetworkConnection connection) =>
      {
        Log.Info("Connected to the Server.");
        //nickServ.Login(username);

      };

      _networkClient.Connect("127.0.0.1", 8855);
      Console.ReadLine();

      _networkClient.Disconnect();


    }
    static void Log_OnLog(Util.Log.LogType type, string message, params object[] objects)
    {
      Console.WriteLine("[" + type.ToString() + "] " + String.Format(message, objects));
    }
  }
}
