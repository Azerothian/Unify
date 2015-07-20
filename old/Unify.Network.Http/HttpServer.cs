using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Network.Interfaces;
using Unify.Network.Tcp;
using Unify.Util;
using Unify.Extensions;
using System.Net;

namespace Unify.Network.Http
{
	public class HttpServer : INetworkServerModule
	{

		private Dictionary<Guid, HttpServerClient> Clients = new Dictionary<Guid, HttpServerClient>();

		HttpListener _listener;
		public int SessionTimeout = 600;
		public event Action<INetworkConnectionModule> OnClientConnected;

		public void StartListening(int port)
		{
			_listener = new HttpListener();
			_listener.Prefixes.Add(string.Format("http://*:{0}/new/", port));
			_listener.Prefixes.Add(string.Format("http://*:{0}/get/", port));
			_listener.Prefixes.Add(string.Format("http://*:{0}/post/", port));
			_listener.Prefixes.Add(string.Format("http://*:{0}/close/", port));
			_listener.Start();
			_listener.BeginGetContext(ListenerCallback, _listener);

		}

		void ListenerCallback(IAsyncResult result)
		{
			if (_listener.IsListening)
			{
				HttpListener listener = (HttpListener)result.AsyncState;
				// Call EndGetContext to complete the asynchronous operation.
				HttpListenerContext context = listener.EndGetContext(result);
				HttpListenerRequest request = context.Request;
				HttpListenerResponse response = context.Response;

				switch (request.Url.PathAndQuery.Replace("/",""))
				{
					case "new":
						HttpServerClient _client = new HttpServerClient();
						Guid identifier = Guid.NewGuid();

						var cookie = new Cookie("session", identifier.ToString());
						cookie.Path = "/";
						cookie.Expires = DateTime.Now.AddMinutes(30);
						response.SetCookie(cookie);
						Clients.Add(identifier, _client);
						response.StatusCode = 200;
						response.OutputStream.Close();

						if(OnClientConnected != null)
						{
							OnClientConnected(_client);

						}
						break;
					case "get":
						var getClient = GetClientByRequest(request);
						if (getClient != null)
						{
							getClient.GetClientMessages(listener, request, response);
						}
						else
						{
							response.StatusCode = 404;
							response.OutputStream.Close();
						}
						break;
					case "post":
						var postClient = GetClientByRequest(request);
						if (postClient != null)
						{
							postClient.SendClientWaitingMessages(listener, request, response);
						}
						else
						{
							response.StatusCode = 404;
							response.OutputStream.Close();
						}
						break;

				}

				_listener.BeginGetContext(ListenerCallback, _listener);
			}
		}
		public HttpServerClient GetClientByRequest(HttpListenerRequest request)
		{
			if (request.Cookies["session"] != null)
			{
				Guid guid = new Guid(request.Cookies["session"].Value);
				if (Clients.ContainsKey(guid))
				{
					return Clients[guid];
				}
			}
			return null;
		}



		public void Disconnect()
		{
			_listener.Stop();
		}



	}
}
