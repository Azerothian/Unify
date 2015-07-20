using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Network.Interfaces;
using WebSocketSharp;

namespace Unify.Network.WebSockets
{
  public class WSClient : INetworkConnectionModule
  {
    WebSocket _socket;

    public event Action OnConnectedEvent;
    public event Action<byte[]> OnDataReceive;
    public event Action<int> OnDataSentEvent;
    public event Action OnDisconnectingEvent;
    public event Action OnDisconnectedEvent;


    public string Key
    {
      get { return _socket.Url.ToString(); }
    }

    public void Connect(Uri uri)
    {
      _socket = new WebSocket(uri.ToString());
      _socket.OnOpen += _socket_OnOpen;
      _socket.OnMessage += _socket_OnMessage;
      _socket.OnClose += _socket_OnClose;
      _socket.Connect();
    }

    void _socket_OnClose(object sender, CloseEventArgs e)
    {
      if (OnDisconnectedEvent != null)
      {
        OnDisconnectedEvent();
      }
    }

    void _socket_OnMessage(object sender, MessageEventArgs e)
    {
      if (OnDataReceive != null)
      {
        OnDataReceive(e.RawData);
      }
    }

    void _socket_OnOpen(object sender, EventArgs e)
    {
      if (OnConnectedEvent != null)
      {
        OnConnectedEvent();
      }
    }

    public void Send(byte[] data)
    {
      _socket.SendAsync(data, (bool sent) =>
      {
        if (OnDataSentEvent != null)
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
      _socket.Close();
    }

    public bool IsDisconnected
    {
      get {
        return !_socket.IsAlive;
      }
    }
  }
}
