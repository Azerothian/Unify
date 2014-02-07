using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Unify.Network.Http
{
	public class AdvancedWebClient : WebClient
	{
		private WebRequest _request = null;
		public CookieContainer CookieContainer { get; set; }
		public AdvancedWebClient()
			: this(new CookieContainer())
		{
		}

		public AdvancedWebClient(CookieContainer cookies)
		{
			this.CookieContainer = cookies;
		}

		protected override WebRequest GetWebRequest(Uri address)
		{
			WebRequest _request = base.GetWebRequest(address);
			if (_request is HttpWebRequest)
			{
				(_request as HttpWebRequest).CookieContainer = this.CookieContainer;
			}
			HttpWebRequest httpRequest = (HttpWebRequest)_request;
			httpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			return httpRequest;
		}

		protected override WebResponse GetWebResponse(WebRequest request)
		{
			WebResponse response = base.GetWebResponse(request);
			String setCookieHeader = response.Headers[HttpResponseHeader.SetCookie];

			if (setCookieHeader != null)
			{
				//do something if needed to parse out the cookie.
				if (setCookieHeader != null)
				{
					Cookie cookie = new Cookie(); //create cookie
					this.CookieContainer.Add(cookie);
				}
			}
			return response;
		}
		public HttpStatusCode StatusCode()
		{
			HttpStatusCode result;

			if (this._request == null)
			{
				throw (new InvalidOperationException("Unable to retrieve the status code, maybe you haven't made a request yet."));
			}

			HttpWebResponse response = base.GetWebResponse(this._request)
																 as HttpWebResponse;

			if (response != null)
			{
				result = response.StatusCode;
			}
			else
			{
				throw (new InvalidOperationException("Unable to retrieve the status code, maybe you haven't made a request yet."));
			}

			return result;
		}
	}
}
