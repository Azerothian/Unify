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
		public event Action OnConnectedEvent;

		public event Action<byte[]> OnDataReceive;

		public event Action<int> OnDataSentEvent;

		public event Action OnDisconnectingEvent;

		public event Action OnDisconnectedEvent;

		public void Connect(Uri uri)
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
