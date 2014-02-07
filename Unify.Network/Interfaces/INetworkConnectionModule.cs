using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Util;


namespace Unify.Network.Interfaces
{
	public interface INetworkConnectionModule
	{
		event GenericVoidDelegate OnConnectedEvent;
		event GenericVoidDelegate<byte[]> OnDataReceive;
		event GenericVoidDelegate<int> OnDataSentEvent;
		event GenericVoidDelegate OnDisconnectingEvent;
		event GenericVoidDelegate OnDisconnectedEvent;

    string Key { get; }

		void Connect(string ip, int port);
		void Send(byte[] data);
		void Disconnect();

		bool IsDisconnected { get; }

	}
}
