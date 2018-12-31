using System;
using System.Net.Http;
using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Repositories;
using GigHunter.Service.Core.ApiClients;
using GigHunter.Service.Core.Execeptions;
using GigHunter.TestUtilities.Database;
using NUnit.Framework;

namespace GigHunter.Service.Core.Tests.ApiClients
{
	[TestFixture]
	public class Ents24ClientTests
	{
		private SourceRepository _sourceRepository;

		[SetUp]
		public void Setup()
		{
			_sourceRepository = new SourceRepository();
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

			// Perform
			_sourceRepository.Add(source).Wait();
			var ents24Client = new Ents24Client(_sourceRepository);

			// Verify			
			var ents24Source = _sourceRepository.GetAll().Result;

			Assert.NotNull(ents24Source[0].ApiToken);
			Assert.NotNull(ents24Source[0].TokenExpiryDate);
			Assert.AreEqual(1, DateTime.Compare(ents24Source[0].TokenExpiryDate, DateTime.Now));
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
			_sourceRepository.Add(source).Wait();

			// Verify
			Assert.Throws(typeof(HttpRequestException), () => new Ents24Client(_sourceRepository));			
		}

		[Test]
		public void Initialise_NoRecordInDatabase_ShouldThrowException()
		{
			Assert.Throws(typeof(ItemNotFoundException), () => new Ents24Client(_sourceRepository));
		}

		[TearDown]
		public void Teardown()
		{
			var databaseUtilities = new MongoDatabaseUtilities<Source>("sources");
			databaseUtilities.RemoveCollection();
		}

	}
}
