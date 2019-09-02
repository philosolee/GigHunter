using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using GigHunter.DomainModels.Models;
using RestSharp;
using RestSharp.Serialization.Json;

namespace GigHunter.Web.Ui.Core.ApiRequests
{
	public class SourceRequests
	{
		private RestClient _client;

		public SourceRequests(string uri)
		{
			_client = new RestClient(uri);
		}

		public List<Source> GetSources()
		{
			var request = new RestRequest("source", Method.GET, DataFormat.Json);
			var response = _client.Execute<List<Source>>(request);

			if (!response.IsSuccessful)
				throw new HttpRequestException("Unable to retrieve data", response?.ErrorException);

			return response.Data;
		}

		public Source AddSource(Source source)
		{
			var request = new RestRequest("source", Method.POST);

			var serialiser = new JsonSerializer();

			var bodyParameter = new Parameter("application/json", serialiser.Serialize(source), ParameterType.RequestBody);
			request.AddParameter(bodyParameter);

			var response = _client.Execute<Source>(request);

			if (response.IsSuccessful)
				return response.Data;

			throw response.ErrorException;
		}
	}
}
