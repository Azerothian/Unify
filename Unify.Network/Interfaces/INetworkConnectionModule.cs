using System;

namespace Unify.Network.Interfaces
{
	public interface INetworkConnectionModule
	{
		event Action OnConnectedEvent;
		event Action<byte[]> OnDataReceive;
		event Action<int> OnDataSentEvent;
		event Action OnDisconnectingEvent;
		event Action OnDisconnectedEvent;

    string Key { get; }

		void Connect(Uri uri);
		void Send(byte[] data);
		void Disconnect();

		bool IsDisconnected { get; }

	}
}
