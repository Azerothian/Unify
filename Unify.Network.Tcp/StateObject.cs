using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Unify.Network.Tcp
{
	public class StateObject
	{
		//			public MemoryManager Memory;
		public StateObject(int bufferSize)
		{
			Buffer = new byte[bufferSize];
			//Memory = new MemoryManager();

		}
		public Socket ServerSocket;
		public byte[] Buffer;
	}
}
