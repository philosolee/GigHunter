using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Repositories;
using GigHunter.Service.Core.ApiClients.Ents24.Responses;
using GigHunter.Service.Core.Wrappers;
using RestSharp;

namespace GigHunter.Service.Core.ApiClients
{
	public class Ents24Client : IApiClient
	{
		private readonly IRestClientWrapper _client;
		private Source _source;
		private readonly SourceRepository _sourceRepository;

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public Ents24Client(Source source, IRestClientWrapper client) : this(source, new SourceRepository(), client)
		{
		}

		public Ents24Client(Source source, SourceRepository sourceRepository, IRestClientWrapper client)
		{
			_source = source;
			_sourceRepository = sourceRepository;

			ValidateSource(); 

			_client = client;
			_client.BaseUrl = new Uri(_source.BaseUrl);
			Authenticate();
		}

		public void Authenticate()
		{
			if (_source.ApiToken == null || DateTime.Compare(_source.TokenExpiryDate, DateTime.Now) <= 0)
			{
				var authorisationResponse = AuthenicateWithApi();
				UpdateDatabase(authorisationResponse);
			}
			_client.AddDefaultHeader("Authorization", _source.ApiToken);
		}

		private void ValidateSource()
		{
			if (_source == null) throw new NullReferenceException();
		}

		private void UpdateDatabase(AuthorisationResponse authorisationResponse)
		{			
			_source.ApiToken = authorisationResponse.AccessToken;
			_source.TokenExpiryDate = DateTimeOffset.FromUnixTimeSeconds(authorisationResponse.Expires).DateTime.ToLocalTime();
			_sourceRepository.UpdateById(_source.Id, _source);
		}
	
		public List<Gig> GigsForArtist(string artistName)
		{
			var artistDetails = GetArtistId(artistName);

			if (artistDetails.UpcomingEvents == 0) return new List<Gig>();

			var artistGigs = GetGigsByArtistId(artistDetails.Id);

			return artistGigs.ConvertAll(g => new Gig() {
				Artist = artistName,
				Date = g.StartDate,
				Venue = g.Venue.Name,
				TicketUrls = new List<string> { g.WebLink }
			});
		}

		private AuthorisationResponse AuthenicateWithApi()
		{
			var authParameters = new List<Parameter>
			{
				new Parameter("content-type", "application/x-www-form-urlencoded", ParameterType.HttpHeader),
				new Parameter("client_id", _source.ClientId, ParameterType.GetOrPost),
				new Parameter("client_secret", _source.ClientSecret, ParameterType.GetOrPost),
			};

			var request = CreateRequest("auth/token", Method.POST, authParameters, addAuthorizationHeader: false);

			var response = _client.Execute<AuthorisationResponse>(request);

			if (!response.IsSuccessful) throw new HttpRequestException($"Authentication with Ents24 failed with code: {response.StatusCode} - {response.StatusDescription}");

			return response.Data;
		}

		private ArtistListResponse GetArtistId(string artistName)
		{
			var parameter = new Parameter("name", artistName, ParameterType.QueryString);
			var request = CreateRequest("artist/list", Method.GET, new List<Parameter> { parameter });

			var response = _client.Execute<List<ArtistListResponse>>(request);

			return response.Data.First(a => a.Name.ToLower() == artistName.ToLower());				
		}

		private List<ArtistEventResponse> GetGigsByArtistId(string artistId)
		{
			var parameter = new Parameter("id", artistId, ParameterType.QueryString);
			var request = CreateRequest("artist/events", Method.GET, new List<Parameter> { parameter });

			var response = _client.Execute<List<ArtistEventResponse>>(request);

			return response.Data;
		}

		private IRestRequest CreateRequest(string url, Method method, List<Parameter> parameters, bool addAuthorizationHeader=true)
		{
			var request = new RestRequest(url, method);
			if (addAuthorizationHeader)	parameters.Add(new Parameter("Authorization", _source.ApiToken, ParameterType.HttpHeader));
			request.Parameters.AddRange(parameters);
			return request;
		}
	}
}
