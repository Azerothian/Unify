using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Messages.Chat;
using Unify.Network;

namespace Unify.Server.Modules.Chat
{
  public class Channel
  {
    //TODO: Server side validation on the user name being sent for the messages.

    public List<ChatUser> Users = new List<ChatUser>();
    public UnifyServer UnifyServer { get; set; }
    public string Name;
    
    public Channel(string name, UnifyServer server)
    {
      Name = name;
      UnifyServer = server;
    }
    public void AddUser(string username, NetworkConnection connection)
    {
      lock (Users)
      {
				//TODO : Add notification for other users that a user has joined the channel.
        var newUser = new ChatUser(username, connection, this);
        Users.Add(newUser);
        newUser.OnDisconnected += newUser_OnDisconnected;
        connection.Emit<ChannelResponse>("chat.channel", new ChannelResponse() { IsSuccessful = true, Channel = Name });
      }
    }

    private void newUser_OnDisconnected(ChatUser user)
    {
      lock (Users)
      {
				//TODO : Add notification for other users that a user has left the channel.
        Users.Remove(user);
      }
    }
    public void SendUserList(NetworkConnection connection, ChannelRequest request)
    {
      if (request.ChannelRequestType == ChannelRequestType.List && request.Channel == Name)
      {
        var list = new ChannelUserListResponse();
        list.RoomName = Name;
        list.Users = (from v in Users select v.Username).ToArray();
        connection.Emit<ChannelUserListResponse>("chat.channel.userlist", list);
      }
    }

    public void SendMessage(ChatMessage request)
    {
      lock (Users)
      {
        foreach (var user in Users)
        {
          user.NetworkConnection.Emit<ChatMessage>("chat.message", request);
        }
      }
    }
  }
}
