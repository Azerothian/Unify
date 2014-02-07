using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Network;

namespace Unify.Client.Interfaces
{
  public interface IModule
  {
    UnifyClient UnifyClient { get; set; }
    void OnConnected();
    void OnDisconnected();
    void Initialise();
  }
}
