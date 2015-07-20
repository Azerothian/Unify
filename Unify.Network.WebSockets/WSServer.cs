using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Network.Interfaces;
using WebSocketSharp.Server;

namespace Unify.Network.WebSockets
{
  public class WSServer : INetworkServerModule
  {
    static WSServer _wsserver;
    public static WSServer Context
    {
      get
      {
        return _wsserver;
      }
    }
    public WSServer()
    {
      _wsserver = this;
    }

    public event Action<INetworkConnectionModule> OnClientConnected;
    WebSocketServer _server;
    public void StartListening(Uri uri)
    {
      _server = new WebSocketServer(uri.ToString());
      _server.AddWebSocketService<WSServerService>("/");
      _server.Start();
    }

    public void Disconnect()
    {
      _server.Stop();
    }
    public void ClientConnected(WSServerService wss) {
      OnClientConnected(wss);
    }
  }
  public class WSServerService : WebSocketBehavior, INetworkConnectionModule
  {

    public event Action OnConnectedEvent;

    public event Action<byte[]> OnDataReceive;

    public event Action<int> OnDataSentEvent;

    public event Action OnDisconnectingEvent;

    public event Action OnDisconnectedEvent;


    public string userEndPoint { get; set; }
    public string Key
    {
      get { return userEndPoint; }
    }

    public void Connect(Uri uri)
    {
      throw new NotImplementedException();
    }

    public new void Send(byte[] data)
    {
      SendAsync(data, (bool complete) =>
      {
        if(OnDataSentEvent != null)
        {
          OnDataSentEvent(data.Length);
        }
      });
    }

    public void Disconnect()
    {
      if(OnDisconnectingEvent != null)
      {
        OnDisconnectingEvent();
      }

      this.Context.WebSocket.Close();
      
    }

    public bool IsDisconnected
    {
      get { return !this.Context.WebSocket.IsAlive; }
    }
    protected override void OnOpen()
    {
      base.OnOpen();
      userEndPoint = this.Context.UserEndPoint.ToString(); 
      WSServer.Context.ClientConnected(this);
      if (OnConnectedEvent != null)
      {
        OnConnectedEvent();
      }
    }
    protected override void OnClose(WebSocketSharp.CloseEventArgs e)
    {
      base.OnClose(e);
      if (OnDisconnectedEvent != null)
      {
        OnDisconnectedEvent();
      }
    }
    protected override void OnError(WebSocketSharp.ErrorEventArgs e)
    {
      base.OnError(e);
      
    }
    protected override void OnMessage(WebSocketSharp.MessageEventArgs e)
    {
      base.OnMessage(e);
      if (OnDataReceive != null)
      {
        OnDataReceive(e.RawData);
      }
    }
  }
}
