using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Messages.NickServ;
using Unify.Network;
using Unify.Server.Interfaces;
using Unify.Util;

namespace Unify.Server.Modules
{
    public class NickServ : IModule
    {

			Dictionary<Guid, string> Usernames = new Dictionary<Guid, string>();

      public event Action<string, NetworkConnection> OnUserLoggedIn;
      public UnifyServer UnifyServer { get; set; }

      public void Start()
      {
        //UnifyServer.Cache.CreateKey("nickserv.users");
      }

      public void Update()
      {

      }


      public void Stop()
      {

      }

      public void OnClientConnected(Guid key, NetworkConnection client)
      {
        client.On<LoginRequest>("nickserv.login", (NetworkConnection _client, LoginRequest message) => {

					bool exists = Usernames.ContainsValue(message.Username);
            if (!exists)
            {
							Usernames.Add(key, message.Username);
              client.Emit("nickserv.confirm",  new LoginResponse() { Successful = true });
              if (OnUserLoggedIn != null)
              {
                OnUserLoggedIn(message.Username, client);
              }
              return;
            
          }
          client.Emit("nickserv.confirm", new LoginResponse() { Successful = false });
        
        });
      }


      public void OnClientDisconnected(Guid key, NetworkConnection client)
      {
				Usernames.Remove(key);
      }

    }
}
