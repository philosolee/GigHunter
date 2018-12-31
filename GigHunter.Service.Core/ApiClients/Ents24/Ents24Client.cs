using System;
using System.Linq;
using System.Net.Http;
using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Repositories;
using GigHunter.Service.Core.ApiClients.Ents24;
using GigHunter.Service.Core.Execeptions;
using RestSharp;
using RestSharp.Authenticators;

namespace GigHunter.Service.Core.ApiClients
{
	public class Ents24Client
	{
		private const string ApiBaseUrl = "https://api.ents24.com";
		private const string SourceName = "Ents24";
		private const string ClientId = "9cf2fed5cfbf039aecd0f50b83218ab59fefc68d";
		private const string ClientSecret = "599c661ed7d3c99cd7d2d98d0a23361d68227720";

		private readonly IRestClient client;
		private Source _source;
		private SourceRepository _sourceRepository;

		public Ents24Client(SourceRepository sourceRepository)
		{
			_sourceRepository = sourceRepository;
			_source = sourceRepository.GetByName(SourceName).Result.FirstOrDefault();

			ValidateSource(); 

			client = new RestClient { BaseUrl = new Uri(_source.BaseUrl) };
			Authenicate();
		}

		private void ValidateSource()
		{
			if (_source == null) throw new ItemNotFoundException("Unable to find the details for 'Ents24' in the database");
		}

		public void Authenicate()
		{
			if (_source.ApiToken == null || DateTime.Compare(_source.TokenExpiryDate, DateTime.Now) <= 0)
			{
				var authorisationResponse = AuthenicateWithApi();
				UpdateDatabase(authorisationResponse);
			}
			client.Authenticator = new JwtAuthenticator(_source.ApiToken);
		}

		private void UpdateDatabase(AuthorisationResponse authorisationResponse)
		{
			
			_source.ApiToken = authorisationResponse.AccessToken;
			_source.TokenExpiryDate = DateTimeOffset.FromUnixTimeSeconds(authorisationResponse.Expires).DateTime.ToLocalTime();
			_sourceRepository.UpdateById(_source.Id, _source);
		}

		private AuthorisationResponse AuthenicateWithApi()
		{
			var request = new RestRequest("auth/token", Method.POST);
			request.AddHeader("content-type", "application/x-www-form-urlencoded");
			request.AddParameter("client_id", _source.ClientId);
			request.AddParameter("client_secret", _source.ClientSecret);

			var response = client.Execute<AuthorisationResponse>(request);

			if (!response.IsSuccessful) throw new HttpRequestException($"Authentication with Ents24 failed with code: {response.StatusCode} - {response.StatusDescription}");

			return response.Data;
		}
	}
}
