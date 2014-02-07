using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Util;


namespace Unify.Network.Interfaces
{
	public interface INetworkServerModule
	{
		event GenericVoidDelegate<INetworkConnectionModule> OnClientConnected;
		void StartListening(int port);

		void Disconnect();
	}
}
