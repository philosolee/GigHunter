using System;
using GigHunter.TestUtilities.Database;
using GigHunter.TestUtilities.Assertors;
using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Repositories;
using MongoDB.Bson;
using NUnit.Framework;

namespace GigHunter.DomainModel.Tests.Repositories
{
	[TestFixture]
	public class SourceRepositoryTest
	{
		private SourceRepository _siteRepository;
		private MongoDatabaseUtilities<Source> _mongoDatabaseUtilities;
		private Source _testSiteOne;
		private Source _testSiteTwo;
		private Source _testSiteThree;

		[SetUp]
		public void ResetTestSources()
		{
			_siteRepository = new SourceRepository();
			_mongoDatabaseUtilities = new MongoDatabaseUtilities<Source>("sources");

			_testSiteOne = TestSiteOne();
			_testSiteTwo = TestSiteTwo();
			_testSiteThree = TestSiteThree();
		}

		[Test]
		public void Add_SingleValidSource_ShouldBeInserted()
		{
			_siteRepository.Add(_testSiteOne).Wait();

			var retrievedSource = _mongoDatabaseUtilities.FindRecordById(_testSiteOne.Id);

			SiteAssertor.New()
				.Expected(_testSiteOne)
				.Actual(retrievedSource[0])
				.DoAssert();
		}

		[Test]
		public void GetAll_ThreeItemsInCollection_ShouldReturnAllThree()
		{
			_siteRepository.Add(_testSiteOne).Wait();
			_siteRepository.Add(_testSiteTwo).Wait();
			_siteRepository.Add(_testSiteThree).Wait();

			var resultFromDatabase = _siteRepository.GetAll().Result;

			Assert.AreEqual(3, resultFromDatabase.Count);

			SiteAssertor.New()
				.Expected(_testSiteOne)
				.Actual(resultFromDatabase.Find(a => a.Id == _testSiteOne.Id))
				.DoAssert();

			SiteAssertor.New()
				.Expected(_testSiteTwo)
				.Actual(resultFromDatabase.Find(a => a.Id == _testSiteTwo.Id))
				.DoAssert();

			SiteAssertor.New()
				.Expected(_testSiteThree)
				.Actual(resultFromDatabase.Find(a => a.Id == _testSiteThree.Id))
				.DoAssert();
		}

		[Test]
		public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
		{
			var resultFromDatabase = _siteRepository.GetAll().Result;
			CollectionAssert.IsEmpty(resultFromDatabase);
		}

		[Test]
		public void GetbyId_ValidObjectId_ShouldReturnSingleSource()
		{
			_siteRepository.Add(_testSiteOne).Wait();
			// This has same details, but different Id to the above
			_siteRepository.Add(TestSiteOne()).Wait();

			var result = _siteRepository.GetById(_testSiteOne.Id).Result;

			Assert.AreEqual(1, result.Count);
			SiteAssertor.New()
				.Expected(_testSiteOne)
				.Actual(result[0])
				.DoAssert();
		}

		[Test]
		public void GetbyId_ValidStringId_ShouldReturnSingleSource()
		{
			_siteRepository.Add(_testSiteOne).Wait();
			// This has same details, but different Id to the above
			_siteRepository.Add(TestSiteOne()).Wait();

			var idAsString = _testSiteOne.Id.ToString();
			var result = _siteRepository.GetById(idAsString).Result;

			Assert.AreEqual(1, result.Count);
			SiteAssertor.New()
				.Expected(_testSiteOne)
				.Actual(result[0])
				.DoAssert();
		}

		[Test]
		public void GetById_InvalidObjectId_ShouldReturnEmptyList()
		{
			_siteRepository.Add(_testSiteOne).Wait();

			var idToLookFor = new ObjectId();
			var result = _siteRepository.GetById(idToLookFor).Result;

			Assert.AreEqual(0, result.Count);
		}

		[Test]
		public void GetById_InvalidStringId_ShouldReturnEmptyList()
		{
			_siteRepository.Add(_testSiteOne).Wait();

			var idAsString = new ObjectId().ToString();
			var result = _siteRepository.GetById(idAsString).Result;

			Assert.AreEqual(0, result.Count);
		}

		[Test]
		public void GetByName_ValidName_ShouldReturnSingleSource()
		{
			_siteRepository.Add(_testSiteOne).Wait();
			// This has same details, but different Id to the above
			_siteRepository.Add(_testSiteTwo).Wait();

			var result = _siteRepository.GetByName(_testSiteOne.Name).Result;

			Assert.AreEqual(1, result.Count);
			SiteAssertor.New()
				.Expected(_testSiteOne)
				.Actual(result[0])
				.DoAssert();
		}


		[Test]
		public void GetByName_ValidNameMultiplePresent_ShouldReturnMultipleSources()
		{
			_siteRepository.Add(_testSiteOne).Wait();
			// This has same details, but different Id to the above
			_siteRepository.Add(TestSiteOne()).Wait();

			var result = _siteRepository.GetByName(_testSiteOne.Name).Result;

			Assert.AreEqual(2, result.Count);
			
			foreach(var source in result)
			{
				Assert.AreEqual(_testSiteOne.Name, source.Name);
			}

			Assert.AreNotEqual(result[0].Id, result[1].Id);
		}


		[Test]
		public void GetByName_InvalidName_ShouldReturnSingleSource()
		{
			_siteRepository.Add(_testSiteOne).Wait();
			// This has same details, but different Id to the above
			_siteRepository.Add(TestSiteOne()).Wait();

			var result = _siteRepository.GetByName("invalidName").Result;

			Assert.AreEqual(0, result.Count);
		}

		[Test]
		public void UpdateById_ValidObjectId_ShouldUpdateAndReturnTrue()
		{
			_siteRepository.Add(_testSiteOne).Wait();
			_siteRepository.Add(_testSiteTwo).Wait();

			_testSiteOne.Name = "Altered Name One";
			_testSiteOne.BaseUrl = "https://www.changedurlone.com";

			var result = _siteRepository.UpdateById(_testSiteOne.Id, _testSiteOne);
			Assert.IsTrue(result);

			var recordFromDatabase = _mongoDatabaseUtilities.FindRecordById(_testSiteOne.Id);
			SiteAssertor.New()
				.Expected(_testSiteOne)
				.Actual(recordFromDatabase[0])
				.DoAssert();
		}

		[Test]
		public void UpdateById_ValidStringId_ShouldUpdateAndReturnTrue()
		{
			// Setup
			_siteRepository.Add(_testSiteOne).Wait();
			_siteRepository.Add(_testSiteTwo).Wait();

			_testSiteOne.Name = "Altered Name One";
			_testSiteOne.BaseUrl = "https://www.changedurlone.com";

			// Perform
			var idAsString = _testSiteOne.Id.ToString();
			var result = _siteRepository.UpdateById(idAsString, _testSiteOne);

			// Verify
			Assert.AreEqual(true, result);

			var recordFromDatabase = _mongoDatabaseUtilities.FindRecordById(_testSiteOne.Id);
			SiteAssertor.New()
				.Expected(_testSiteOne)
				.Actual(recordFromDatabase[0])
				.DoAssert();
		}

		[Test]
		public void UpdateById_InvalidObjectId_ShouldReturnFalseAndNotUpdate()
		{
			// Setup
			_siteRepository.Add(_testSiteOne).Wait();

			var updatedDetails = new Source
			{
				Name = "Altered Name One",
				BaseUrl = "https://www.changedurlone.com"
			};

			// Perform
			var invalidObjectId = new ObjectId();
			var result = _siteRepository.UpdateById(invalidObjectId, _testSiteOne);

			// Verify
			Assert.IsFalse(result);

			var recordFromDatabase = _mongoDatabaseUtilities.FindRecordById(_testSiteOne.Id);
			SiteAssertor.New()
				.Expected(_testSiteOne)
				.Actual(recordFromDatabase[0])
				.DoAssert();
		}

		[Test]
		public void UpdateById_InvalidStringId_ShouldReturnFalseAndNotUpdate()
		{
			// Setup
			_siteRepository.Add(_testSiteOne).Wait();

			var updatedDetails = new Source
			{
				Name = "Altered Name One",
				BaseUrl = "https://www.changedurlone.com"
			};

			// Perform
			var invalidIdAsString = new ObjectId().ToString();
			var result = _siteRepository.UpdateById(invalidIdAsString, updatedDetails);

			// Verify
			Assert.IsFalse(result);

			var recordFromDatabase = _mongoDatabaseUtilities.FindRecordById(_testSiteOne.Id);
			SiteAssertor.New()
				.Expected(_testSiteOne)
				.Actual(recordFromDatabase[0])
				.DoAssert();
		}

		[Test]
		public void DeleteById_ValidObjectId_ShouldBeDeletedAndReturnNumberOfRecordsDeleted()
		{
			_siteRepository.Add(_testSiteOne).Wait();
			_siteRepository.Add(_testSiteTwo).Wait();

			var countBefore = _mongoDatabaseUtilities.CountRecordsInCollection();

			// Perform
			var result = _siteRepository.DeleteById(_testSiteOne.Id);

			// Verify
			Assert.IsTrue(result);

			var countAfter = _mongoDatabaseUtilities.CountRecordsInCollection();
			Assert.AreEqual(countBefore - 1, countAfter);

			var findResult = _mongoDatabaseUtilities.FindRecordById(_testSiteOne.Id);
			CollectionAssert.IsEmpty(findResult);
		}

		[Test]
		public void DeleteById_ValidStringId_ShouldBeDeletedAndReturnNumberOfRecordsDeleted()
		{
			_siteRepository.Add(_testSiteOne).Wait();
			_siteRepository.Add(_testSiteTwo).Wait();

			var countBefore = _mongoDatabaseUtilities.CountRecordsInCollection();

			// Perform
			var idAsString = _testSiteOne.Id.ToString();
			var result = _siteRepository.DeleteById(idAsString);

			// Verify
			Assert.IsTrue(result);

			var countAfter = _mongoDatabaseUtilities.CountRecordsInCollection();
			Assert.AreEqual(countBefore - 1, countAfter);

			var findResult = _mongoDatabaseUtilities.FindRecordById(_testSiteOne.Id);
			CollectionAssert.IsEmpty(findResult);
		}

		[Test]
		public void DeleteById_InvalidObjectId_ShouldReturnZeroAndNotDeleteAnything()
		{
			_siteRepository.Add(_testSiteOne).Wait();
			_siteRepository.Add(_testSiteTwo).Wait();

			var countBefore = _mongoDatabaseUtilities.CountRecordsInCollection();

			// Perform
			var invalidId = new ObjectId();
			var result = _siteRepository.DeleteById(invalidId);

			// Verify
			Assert.IsFalse(result);

			var countAfter = _mongoDatabaseUtilities.CountRecordsInCollection();
			Assert.AreEqual(countBefore, countAfter);
		}

		[Test]
		public void DeleteById_InvalidStringId_ShouldReturnZeroAndNotDeleteAnything()
		{
			_siteRepository.Add(_testSiteOne).Wait();
			_siteRepository.Add(_testSiteTwo).Wait();

			var countBefore = _mongoDatabaseUtilities.CountRecordsInCollection();

			// Perform
			var idAsString = new ObjectId().ToString();
			var result = _siteRepository.DeleteById(idAsString);

			// Verify
			Assert.IsFalse(result);

			var countAfter = _mongoDatabaseUtilities.CountRecordsInCollection();
			Assert.AreEqual(countBefore, countAfter);
		}

		[TearDown]
		public void DropSourceCollection()
		{
			_mongoDatabaseUtilities.RemoveCollection();
		}

		#region Test Data
		private static Source TestSiteOne()
		{
			return new Source
			{
				Name = "SeeTickets",
				BaseUrl = "https://www.seetickets.com"
			};
		}

		private static Source TestSiteTwo()
		{
			return new Source
			{
				Name = "TicketMaster",
				BaseUrl = "https://www.ticketmaster.com"
			};
		}

		private static Source TestSiteThree()
		{
			return new Source
			{
				Name = "Ents24",
				BaseUrl = "https://www.ents24.com"
			};
		}
		#endregion
	}
}
