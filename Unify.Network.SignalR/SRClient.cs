using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unify.Network.Interfaces;

namespace Unify.Network.SignalR
{
  public class SRClient : INetworkConnectionModule
  {

    HubConnection _hubConnection;
    IHubProxy _hubProxy;
    public bool IsDisconnected
    {
      get
      {
        return _hubConnection.State == ConnectionState.Disconnected;
      }
    }

    public string Key
    {
      get
      {
        return _hubConnection.ConnectionId;
      }
    }

    public event Action OnConnectedEvent;
    public event Action<byte[]> OnDataReceive;
    public event Action<int> OnDataSentEvent;
    public event Action OnDisconnectedEvent;
    public event Action OnDisconnectingEvent;

    public async void Connect(Uri uri)
    {
      _hubConnection = new HubConnection(uri.ToString());
      _hubConnection.Closed += _hubConnection_Closed;
      _hubProxy = _hubConnection.CreateHubProxy("UnifyHub");
      _hubProxy.On<byte[]>("OnReceive", OnDataReceive);
      await _hubConnection.Start();
      if(OnConnectedEvent != null)
      {
        OnConnectedEvent();
      }
    }

    private void _hubConnection_Closed()
    {
      if(OnDisconnectedEvent != null)
      {
        OnDisconnectedEvent();
      }
    }

    public void Disconnect()
    {
      if (OnDisconnectingEvent != null)
      {
        OnDisconnectingEvent();
      }
      _hubConnection.Stop();

      _hubConnection.Dispose();
    }

    public async void Send(byte[] data)
    {
      await _hubProxy.Invoke<byte[]>("OnReceive", data);
      if (OnDataSentEvent != null)
      {
        OnDataSentEvent(data.Length);
      }
    }
  }
}
