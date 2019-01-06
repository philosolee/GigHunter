using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GigHunter.Service.Core.Wrappers
{
	public class RestClientWrapper : RestClient, IRestClientWrapper
	{
		private IRestClient _client;

		public RestClientWrapper(IRestClient client)
		{
			_client = client;
		}

		public void AddDefaultHeader(string headerName, string value)
		{
			_client.AddDefaultHeader(headerName, value);
		}
	}
}
