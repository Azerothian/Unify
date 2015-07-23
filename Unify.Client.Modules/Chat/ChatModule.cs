using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Client.Interfaces;
using Unify.Messages.Chat;
using Unify.Network;
using Unify.Util;

namespace Unify.Client.Modules.Chat
{
  public class ChatModule : IModule
  {
    public string Username { get; set; }
    public event Action<ChatMessage, NetworkConnection> OnChatMessage;
    public event Action<string, NetworkConnection> OnJoinChannel;
    public event Action<string, IEnumerable<string>, NetworkConnection> OnChannelUserList;

    public void OnConnected()
    {

    }

    public void OnDisconnected()
    {

    }

    public void Initialise()
    {
      UnifyClient.Connection.On<ChannelResponse>("chat.channel",
        (NetworkConnection connection, ChannelResponse response) =>
        {
          if (response.IsSuccessful && OnJoinChannel != null)
          {
            OnJoinChannel(response.Channel, connection);
          }
        });
      UnifyClient.Connection.On<ChannelUserListResponse>("chat.channel.userlist",
        (NetworkConnection connection, ChannelUserListResponse response) =>
        {
          if (OnChannelUserList != null)
          {
            OnChannelUserList(response.RoomName, response.Users, connection);
          }
        });
      UnifyClient.Connection.On<ChatMessage>("chat.message",
        (NetworkConnection connection, ChatMessage response) =>
          {
            if (OnChatMessage != null)
            {
              OnChatMessage(response, connection);
            }
          });
    }
    public void SendMessage(string channel, string message, params string[] formatObjs)
    {
      UnifyClient.Connection.Emit<ChatMessage>(
        "chat.message",
        new ChatMessage()
        {
          From = Username,
          Target = channel,
          ChatEventType = ChatEventType.Message,
          Message = string.Format(message, formatObjs)
        });
    }
    public UnifyClient UnifyClient { get; set; }
  }
}
