using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unify.Network;
using Unify.Network.Serialiser;
using Unify.Network.Tcp;

namespace Unify.Kinect.Server
{
    public class Program
    {

      static NetworkServer _networkServer;
      static Unify.Server.UnifyServer _unityServer;
      public static void Main(params string[] args)
      {
        Util.Log.OnLog += Log_OnLog;


        
        _networkServer = new Network.NetworkServer();
        _networkServer.SetSerialiser<BinarySerialiser>();

        _unityServer = new Unify.Server.UnifyServer(_networkServer);
        _unityServer.AddModule<SkeletonHostModule>();
        _networkServer.StartServer<TcpServer>(8855);
        _unityServer.Start();


        Console.ReadLine();
        _unityServer.Stop();


      }



      static void Log_OnLog(Util.Log.LogType type, string message, params object[] objects)
      {
        Console.WriteLine("[" + type.ToString() + "] " + String.Format(message, objects));
      }
    }
}
