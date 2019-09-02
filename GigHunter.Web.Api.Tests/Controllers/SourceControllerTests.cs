using System;
using System.Net;
using FluentAssertions.Json;
using GigHunter.DomainModels.Models;
using GigHunter.TestUtilities.Database;
using GigHunter.TestUtilities.DataGenerators;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace GigHunter.Web.Api.Tests.Controllers
{
	[TestFixture]
	public class SourceControllerTests
	{
		private MongoDatabaseUtilities<Source> _mongoDatabaseUtilities;
		private TestClient _testClient;

		[SetUp]
		public void Setup()
		{
			_mongoDatabaseUtilities = new MongoDatabaseUtilities<Source>("sources");
			_testClient = new TestClient();
		}

		[Test]
		public void Get_ValidMongoId_ReturnsSourceAsJson()
		{
			var source = new Source
			{
				ApiToken = "apiToken",
				BaseUrl = "BaseUrl",
				ClientId = "ClientId",
				ClientSecret = "ClientSecret",
				Name = "Name",
				TokenExpiryDate = new DateTime(2018, 03, 01)
			};
			_mongoDatabaseUtilities.AddItem(source);

			var response = _testClient.Get($"/api/source/{source.Id}");
			var responseBody = response.Content.ReadAsStringAsync().Result;


			JToken actual = JToken.Parse(responseBody);
			JToken expected = JToken.Parse(source.ToJson());

			actual.Should().BeEquivalentTo(expected);
		}

		[Test]
		public void Get_MongoIdNotPresent_Returns404()
		{
			var mongoId = IdGenerator.NewMongoId().ToString();
			var response = _testClient.Get($"/api/source/{mongoId}");

			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Test]
		public void Get_InvalidMongoId_Returns400()
		{
			var invalidId = "123";
			var response = _testClient.Get($"/api/source/{invalidId}");

			Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[TearDown]
		public void ClearSourceCollection()
		{
			_mongoDatabaseUtilities.RemoveCollection();
		}
	}
}
