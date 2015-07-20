using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Network;
using Unify.Util;

namespace Unify.Server.Modules.Chat
{
  public class ChatUser
  {
    public event Action<ChatUser> OnDisconnected;
    public string Username { get; set; }
    public NetworkConnection NetworkConnection { get; set; }
    public Channel Channel { get; set; }
    public ChatUser(string username, NetworkConnection connection, Channel channel)
    {
      Username = username;
      NetworkConnection = connection;
      Channel = channel;
      connection.OnDisconnected += connection_OnDisconnected;
    }

    private void connection_OnDisconnected(Network.NetworkConnection connection)
    {
      if (OnDisconnected != null)
      {
        OnDisconnected(this);
      }
    }
  }
}
