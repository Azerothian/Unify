using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unify.Network.Interfaces;

namespace Unify.Network.SignalR
{
  public class SRServer : INetworkServerModule
  {
    Dictionary<string, UnifySRConnection> _connections { get; set; }
    static SRServer _server;
    public static SRServer Context
    {
      get
      {
        return _server;
      }
    }
    public SRServer()
    {
      _connections = new Dictionary<string, UnifySRConnection>();
      _server = this;
    }

    public event Action<INetworkConnectionModule> OnClientConnected;
    IDisposable _webapp;
    public void Disconnect()
    {
      _webapp.Dispose();
    }

    public void StartListening(Uri uri)
    {
      _webapp = WebApp.Start<Startup>(uri.ToString().Replace("0.0.0.0","*"));
    }

    public void OnSRClientConnected(string connectionId)
    {
      if (!_connections.ContainsKey(connectionId))
      {
        _connections.Add(connectionId, new UnifySRConnection(connectionId));
      }
      if(OnClientConnected != null)
      {
        OnClientConnected(_connections[connectionId]);
      }
    }
    public void OnSRClientDisconnected(string connectionId)
    {
      if (_connections.ContainsKey(connectionId))
      {
        _connections[connectionId].OnDisconnect();
      }
      _connections.Remove(connectionId);
    }

    internal void OnSRDataReceived(string connectionId, byte[] data)
    {
      if (_connections.ContainsKey(connectionId))
      {
        _connections[connectionId].OnDataReceived(data);
      }
    }
  }

  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      app.UseCors(CorsOptions.AllowAll);
      app.MapSignalR();
    }
  }

  public class UnifySRConnection : INetworkConnectionModule
  {
    bool _connected = true;
    string _connectionId { get; set; }
    IHubContext _hubContext;

    public bool IsDisconnected
    {
      get
      {
        return !_connected;
      }
    }

    public string Key
    {
      get
      {
        return _connectionId;
      }
    }

    public event Action OnConnectedEvent;
    public event Action<byte[]> OnDataReceive;
    public event Action<int> OnDataSentEvent;
    public event Action OnDisconnectedEvent;
    public event Action OnDisconnectingEvent;

    public UnifySRConnection(string connectionId)
    {
      _connectionId = connectionId;
      _hubContext = GlobalHost.ConnectionManager.GetHubContext<UnifyHub>();
    }

    public void OnDisconnect()
    {
      _connected = false;
      if (OnDisconnectedEvent != null)
      {
        OnDisconnectedEvent();
      }
    }

    public void Connect(Uri uri)
    {
      throw new NotImplementedException();
    }

    public void Disconnect()
    {
      throw new NotImplementedException();
    }

    public void OnDataReceived(byte[] data)
    {
      if (OnDataReceive != null)
      {
        OnDataReceive(data);
      }
    }

    public async void Send(byte[] data)
    {
      await _hubContext.Clients.Client(_connectionId).OnReceive(data);
      if (OnDataSentEvent != null)
      {
        OnDataSentEvent(data.Length);
      }
    }
  }

  public class UnifyHub : Hub
  {

    public override Task OnConnected()
    {
      SRServer.Context.OnSRClientConnected(Context.ConnectionId);
      return base.OnConnected();
    }
    public override Task OnDisconnected(bool stopCalled)
    {
      SRServer.Context.OnSRClientDisconnected(Context.ConnectionId);
      return base.OnDisconnected(stopCalled);
    }
    public override Task OnReconnected()
    {
      SRServer.Context.OnSRClientConnected(Context.ConnectionId);
      return base.OnReconnected();
    }

    public void OnReceive(byte[] data)
    {
      SRServer.Context.OnSRDataReceived(Context.ConnectionId, data);
    }
  }
}
