using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Network.Interfaces;
using Unify.Util;

namespace Unify.Network.lidgrenUdp
{
	public class UdpClient : INetworkConnectionModule
	{
		public event GenericVoidDelegate OnConnectedEvent;

		public event GenericVoidDelegate<byte[]> OnDataReceive;

		public event GenericVoidDelegate<int> OnDataSentEvent;

		public event GenericVoidDelegate OnDisconnectingEvent;

		public event GenericVoidDelegate OnDisconnectedEvent;

		public void Connect(string ip, int port)
		{
			throw new NotImplementedException();
		}

		public void Send(byte[] data)
		{
			throw new NotImplementedException();
		}

		public void Disconnect()
		{
			throw new NotImplementedException();
		}


		public bool IsDisconnected
		{
			get { throw new NotImplementedException(); }
		}


    public string Key
    {
      get { throw new NotImplementedException(); }
    }
  }
}
