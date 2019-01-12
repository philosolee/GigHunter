using System;
using System.Collections.Generic;
using System.Net.Http;
using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Repositories;
using GigHunter.Service.Core.ApiClients;
using GigHunter.Service.Core.ApiClients.Ents24.Responses;
using GigHunter.Service.Core.Wrappers;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;

namespace GigHunter.Service.Core.Tests.ApiClients
{
	[TestFixture]
	public class Ents24ClientTests
	{
		private Mock<SourceRepository> _mockSourceRepository;
		private Mock<IRestClientWrapper> _mockRestClient;
		private List<Source> _testSource = new List<Source>();

		[SetUp]
		public void Setup()
		{
			_mockSourceRepository = new Mock<SourceRepository>();
			_mockRestClient = new Mock<IRestClientWrapper>();
		}

		[Test]
		public void Initialise_ValidCredenitalsNoApiToken_ShouldGetApiToken()
		{
			// Setup
			var source = new Source
			{
				Name = "Ents24",
				BaseUrl = "https://api.ents24.com",
				ClientId = "9cf2fed5cfbf039aecd0f50b83218ab59fefc68d",
				ClientSecret = "599c661ed7d3c99cd7d2d98d0a23361d68227720"
			};

			var mockResponse = ValidRestResponse();
			_mockRestClient.Setup(c => c.Execute<AuthorisationResponse>(It.IsAny<RestRequest>())).Returns(mockResponse.Object);
			_mockRestClient.Setup(c => c.AddDefaultHeader("Authorization", It.IsAny<string>()));

			// Perform
			var ents24Client = new Ents24Client(source, _mockSourceRepository.Object, _mockRestClient.Object);

			// Verify			
			Assert.NotNull(ents24Client);
			_mockRestClient.VerifySet(c => c.BaseUrl = It.Is<Uri>(a => a == new Uri(source.BaseUrl)));
			_mockRestClient.Verify(c => c.AddDefaultHeader("Authorization", It.IsAny<string>()), Times.Once);

			// mockResponse.VerifySet(r => r.Headers = It.Is<List<Parameter>>(a => a.Contains(new Parameter("Authorization", It.IsAny<string>(), ParameterType.HttpHeader))));

			_mockSourceRepository.Verify(r => r.UpdateById(It.IsAny<ObjectId> (), source), Times.Once);
			Assert.AreEqual(DateTime.Now.AddDays(60).ToShortDateString(), source.TokenExpiryDate.ToShortDateString());
		}

		[Test]
		public void Initialise_ValidCredenitalsCurrentApiToken_ShouldSetClientAuthenticator()
		{
			// Setup
			var source = new Source
			{
				Name = "Ents24",
				BaseUrl = "https://api.ents24.com",
				ClientId = "9cf2fed5cfbf039aecd0f50b83218ab59fefc68d",
				ClientSecret = "599c661ed7d3c99cd7d2d98d0a23361d68227720",
				ApiToken = "CurrentToken",
				TokenExpiryDate = DateTime.Now.AddDays(30)
			};

			// Perform
			var ents24Client = new Ents24Client(source, _mockSourceRepository.Object, _mockRestClient.Object);

			// Verify			
			Assert.NotNull(ents24Client);
			_mockRestClient.VerifySet(c => c.BaseUrl = It.Is<Uri>(a => a == new Uri(source.BaseUrl)));
			_mockSourceRepository.Verify(r => r.UpdateById(It.IsAny<ObjectId>(), source), Times.Never);
		}

		[Test]
		public void Initialise_ValidCredenitalsExpiredApiToken_ShouldSetClientAuthenticator()
		{
			// Setup
			var source = new Source
			{
				Name = "Ents24",
				BaseUrl = "https://api.ents24.com",
				ClientId = "9cf2fed5cfbf039aecd0f50b83218ab59fefc68d",
				ClientSecret = "599c661ed7d3c99cd7d2d98d0a23361d68227720",
				ApiToken = "CurrentToken",
				TokenExpiryDate = DateTime.Now.AddDays(-1)
			};

			var mockResponse = ValidRestResponse();
			_mockRestClient.Setup(c => c.Execute<AuthorisationResponse>(It.IsAny<RestRequest>())).Returns(mockResponse.Object);

			// Perform
			var ents24Client = new Ents24Client(source, _mockSourceRepository.Object, _mockRestClient.Object);

			// Verify			
			Assert.NotNull(ents24Client);
			_mockRestClient.VerifySet(c => c.BaseUrl = It.Is<Uri>(a => a == new Uri(source.BaseUrl)));
			_mockSourceRepository.Verify(r => r.UpdateById(It.IsAny<ObjectId>(), source), Times.Once);
			Assert.AreEqual(DateTime.Now.AddDays(60).ToShortDateString(), source.TokenExpiryDate.ToShortDateString());
		}

		[Test]
		public void Initialise_CurrentApiKeyDontPassInSourceRepository_ShouldSetClientAuthenticator()
		{
			// Setup
			var source = new Source
			{
				Name = "Ents24",
				BaseUrl = "https://api.ents24.com",
				ClientId = "9cf2fed5cfbf039aecd0f50b83218ab59fefc68d",
				ClientSecret = "599c661ed7d3c99cd7d2d98d0a23361d68227720",
				ApiToken = "CurrentToken",
				TokenExpiryDate = DateTime.Now.AddDays(30)
			};

			// Perform
			var ents24Client = new Ents24Client(source, _mockRestClient.Object);

			// Verify			
			Assert.NotNull(ents24Client);
			_mockRestClient.VerifySet(c => c.BaseUrl = It.Is<Uri>(a => a == new Uri(source.BaseUrl)));
			_mockRestClient.Verify(c => c.Execute<AuthorisationResponse>(It.IsAny<RestRequest>()), Times.Never);
		}

		[Test]
		public void Initialise_ValidCredenitalsCurrentExpiryDateNoToken_ShouldSetClientAuthenticator()
		{
			// Setup
			var source = new Source
			{
				Name = "Ents24",
				BaseUrl = "https://api.ents24.com",
				ClientId = "9cf2fed5cfbf039aecd0f50b83218ab59fefc68d",
				ClientSecret = "599c661ed7d3c99cd7d2d98d0a23361d68227720",
				TokenExpiryDate = DateTime.Now.AddDays(30)
			};

			var mockResponse = ValidRestResponse();
			_mockRestClient.Setup(c => c.Execute<AuthorisationResponse>(It.IsAny<RestRequest>())).Returns(mockResponse.Object);

			// Perform
			var ents24Client = new Ents24Client(source, _mockSourceRepository.Object, _mockRestClient.Object);

			// Verify			
			Assert.NotNull(ents24Client);
			_mockRestClient.VerifySet(c => c.BaseUrl = It.Is<Uri>(a => a == new Uri(source.BaseUrl)));
			_mockSourceRepository.Verify(r => r.UpdateById(It.IsAny<ObjectId>(), source), Times.Once);
			Assert.AreEqual(DateTime.Now.AddDays(60).ToShortDateString(), source.TokenExpiryDate.ToShortDateString());
		}

		[Test]
		public void Initialise_InvalidCredenitals_ShouldThrowException()
		{
			// Setup
			var source = new Source
			{
				Name = "Ents24",
				BaseUrl = "https://api.ents24.com",
				ClientId = "invalidId",
				ClientSecret = "invalidSecret"
			};

			var mockResponse = InvalidRestResponse();
			_mockRestClient.Setup(c => c.Execute<AuthorisationResponse>(It.IsAny<RestRequest>())).Returns(mockResponse.Object);

			// Verify
			Assert.Throws(typeof(HttpRequestException), () => new Ents24Client(source, _mockSourceRepository.Object, _mockRestClient.Object));
		}

		[Test]
		public void Initialise_NullSource_ShouldThrowException()
		{
			Source source = null;
			Assert.Throws(typeof(NullReferenceException), () => new Ents24Client(source, _mockSourceRepository.Object, _mockRestClient.Object));
		}

		[Test]
		public void GigsForArtist_ArtistWithGigs_ShouldReturnListOfGigs()
		{
			var source = new Source
			{
				Name = "Ents24",
				BaseUrl = "https://api.ents24.com",
				ClientId = "9cf2fed5cfbf039aecd0f50b83218ab59fefc68d",
				ClientSecret = "599c661ed7d3c99cd7d2d98d0a23361d68227720"
			};

			var ents24Client = new Ents24Client(source, _mockSourceRepository.Object, new RestClientWrapper(new RestClient()));

			var gigs = ents24Client.GigsForArtist("tool");

			Assert.NotNull(gigs);
			Assert.AreEqual(3, gigs.Count);
		}
		
		[TearDown]
		public void Teardown()
		{
		}


		private static Mock<IRestResponse<AuthorisationResponse>> ValidRestResponse()
		{
			var res = new Mock<IRestResponse<AuthorisationResponse>>();
			res.Setup(r => r.IsSuccessful).Returns(true);
			res.Setup(r => r.Data).Returns(ValidAuthorisationRepsonse);
			return res;
		}

		private static Mock<IRestResponse<AuthorisationResponse>> InvalidRestResponse()
		{
			var res = new Mock<IRestResponse<AuthorisationResponse>>();
			res.Setup(r => r.IsSuccessful).Returns(false);
			return res;
		}

		private static AuthorisationResponse ValidAuthorisationRepsonse()
		{
			var unixTimeIn60Days = new DateTimeOffset(DateTime.Now.AddDays(60)).ToUnixTimeSeconds();
			return new AuthorisationResponse
			{
				AccessToken = "token-allowing-access",
				TokenType = "bearer",
				Bearer = "bearer",
				Expires = unixTimeIn60Days,
				ExpiresIn = 5184000
			};
		}
	}
}
