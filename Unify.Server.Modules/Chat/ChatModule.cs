using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Messages.Chat;
using Unify.Network;
using Unify.Server.Interfaces;
using Unify.Server.Modules.Chat;

namespace Unify.Server.Modules
{
  public class ChatModule : IModule
  {
    public List<Channel> Channels = new List<Channel>();
    public UnifyServer UnifyServer { get; set; }

    private NickServ NickServ
    {
      get
      {
        return UnifyServer.GetModule<NickServ>();
      }
    }

    public void OnClientConnected(Guid key, NetworkConnection client)
    {

    }

    public void OnClientDisconnected(Guid key, NetworkConnection client)
    {

    }

    public void Update()
    {
    }

    public void Start()
    {
      Channels.Add(new Channel("Global", UnifyServer));
      //UnifyServer.Cache.CreateKey("chat.channels");
      NickServ.OnUserLoggedIn += NickServ_OnUserLoggedIn;
    }

    void NickServ_OnUserLoggedIn(string username, NetworkConnection connection)
    {
      connection.Emit("console", string.Format("Welcome {0}", username));
      connection.On<ChannelRequest>("chat.channel.userlist", (NetworkConnection conn, ChannelRequest request) =>
      {

        var channel = GetChannel(request.Channel);
        if (channel != null)
        {
          channel.SendUserList(connection, request);
        }
      });
      connection.On<ChatMessage>("chat.message", (NetworkConnection conn, ChatMessage request) =>
      {
        switch (request.ChatEventType)
        {
          case ChatEventType.Message:
            var channel = GetChannel(request.Target);
            if (channel != null)
            {
              channel.SendMessage(request);
            }
            break;
        }

      });
      JoinChannel("Global", username, connection);
    }

    private void JoinChannel(string roomName, string username, NetworkConnection connection)
    {

      var channel = GetChannel(roomName);
      if (channel != null)
      {
        channel.AddUser(username, connection);
      }

    }
    public Channel GetChannel(string name)
    {
      foreach (var c in Channels)
      {
        if (c.Name == name)
          return c;
      }
      return null;
    }

    public void Stop()
    {

    }
  }
}
