using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Network;

namespace Unify.Server.Interfaces
{
	public interface IModule
	{
    UnifyServer UnifyServer { get; set; }
    void OnClientConnected(Guid key, NetworkConnection client);
    void OnClientDisconnected(Guid key, NetworkConnection client);
		void Start();
		void Stop();
	}
}
