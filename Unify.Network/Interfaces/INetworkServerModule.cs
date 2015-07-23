using System;
namespace Unify.Network.Interfaces
{
	public interface INetworkServerModule
	{
		event Action<INetworkConnectionModule> OnClientConnected;
		void StartListening(Uri uri);

		void Disconnect();
	}
}
