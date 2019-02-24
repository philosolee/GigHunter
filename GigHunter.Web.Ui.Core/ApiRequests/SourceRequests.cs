using System;
using System.Collections.Generic;
using System.Text;
using GigHunter.DomainModels.Models;
using RestSharp;
using RestSharp.Serialization.Json;

namespace GigHunter.Web.Ui.Core.ApiRequests
{
	public class SourceRequests
	{
		private RestClient _client;

		public SourceRequests()
		{
			_client = new RestClient("https://localhost:44311/api/");
		}

		public List<Source> GetSources()
		{
			var request = new RestRequest("source", Method.GET);
			var response = _client.Execute<List<Source>>(request);

			if (response.IsSuccessful)
				return response.Data;

			throw response.ErrorException;
		}

		public Source AddSource(Source source)
		{
			var request = new RestRequest("source", Method.POST);

			var serialiser = new JsonSerializer();
			request.AddJsonBody(serialiser.Serialize(source));

			var response = _client.Execute<Source>(request);

			if (response.IsSuccessful)
				return response.Data;

			throw response.ErrorException;
		}
	}
}
