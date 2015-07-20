using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Client.Interfaces;
using Unify.Messages.NickServ;
using Unify.Network;
using Unify.Util;

namespace Unify.Client.Modules
{
  public class NicknameModule : IModule
  {
    public string Username { get; set; }
    public event Action<LoginResponse> OnLoginResponse;

    public void Login(string username)
    {
      Username = username;
      UnifyClient.Connection.Emit("nickserv.login", new LoginRequest() { Username = username });
    }

    public void Initialise()
    {
      UnifyClient.Connection.On<LoginResponse>("nickserv.confirm", (NetworkConnection cli, LoginResponse response) =>
         {
           if (OnLoginResponse != null)
           {
             OnLoginResponse(response);
           }
         });
    }


    public void OnConnected()
    {

    }

    public void OnDisconnected()
    {

    }
    public UnifyClient UnifyClient { get; set; }

  }
}
