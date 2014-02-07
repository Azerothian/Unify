using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unify.Network.Interfaces;
using Unify.Util;

namespace Unify.Network.Http
{
    public class HttpClient : INetworkConnectionModule
    {
			public event GenericVoidDelegate OnConnectedEvent;

			public event GenericVoidDelegate<byte[]> OnDataReceive;

			public event GenericVoidDelegate<int> OnDataSentEvent;

			public event GenericVoidDelegate OnDisconnectingEvent;

			public event GenericVoidDelegate OnDisconnectedEvent;

			RestClient client;
			public void Connect(string ip, int port)
			{

				client = new RestClient(string.Format("http://{0}:{1}/", ip, port));

				var request = new RestRequest("new/", Method.GET);

				var response = client.Execute(request);
				if(response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					if(OnConnectedEvent != null)
					{
						OnConnectedEvent();
					}
				}


				
			}

			public void Send(byte[] data)
			{
				var request = new RestRequest("post/", Method.POST);
				//request.
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
