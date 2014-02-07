using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Unify.Network.Interfaces;
using Unify.Util;
using Unify.Extensions;

namespace Unify.Network.Http
{
	public class HttpServerClient : INetworkConnectionModule
	{
		public event GenericVoidDelegate OnConnectedEvent;

		public event GenericVoidDelegate<byte[]> OnDataReceive;

		public event GenericVoidDelegate<int> OnDataSentEvent;

		public event GenericVoidDelegate OnDisconnectingEvent;

		public event GenericVoidDelegate OnDisconnectedEvent;


		private MemoryManager _sendBuffer = new MemoryManager();

		public void Connect(string ip, int port)
		{
			throw new NotImplementedException();
		}

		public void Send(byte[] data)
		{
			lock (_sendBuffer)
			{
				_sendBuffer.Write(data);
			}
		}

		public void SendClientWaitingMessages(HttpListener listener, HttpListenerRequest request, HttpListenerResponse response)
		{
			int sent = 0;
			response.KeepAlive = false;
			System.IO.Stream output = response.OutputStream;
			lock (_sendBuffer)
			{
				if (_sendBuffer.Length > 0)
				{
					response.StatusCode = 200;
					var buffer = _sendBuffer.GetBuffer();
					sent = (int)_sendBuffer.Length;
					response.ContentLength64 = _sendBuffer.Length;
					output.Write(buffer, 0, (int)_sendBuffer.Length);
				}
				else
				{
					response.StatusCode = 204;
				}
			}
			// You must close the output stream.
			output.Close();
			if(sent > 0 && OnDataSentEvent != null)
			{
				OnDataSentEvent(sent);

			}
		}

		public void GetClientMessages(HttpListener listener, HttpListenerRequest request, HttpListenerResponse response)
		{
			response.StatusCode = 200;
			if (OnDataReceive != null)
			{
				var data = request.InputStream.ReadToEnd();
				
				OnDataReceive(data);
			}
			response.OutputStream.Close();
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
