using System;
using GigHunter.DomainModel.Tests.Assertors;
using GigHunter.DomainModel.Tests.Utilities;
using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Repositories;
using MongoDB.Bson;
using NUnit.Framework;

namespace GigHunter.DomainModel.Tests.Repositories
{
	[TestFixture]
	public class ArtistRepositoryTest
	{
		private ArtistRepository _artistRepository;
		private MongoDatabaseUtilities<Artist> _mongoDatabaseUtilities;
		private Artist _testArtistOne;
		private Artist _testArtistTwo;
		private Artist _testArtistThree;

		[SetUp]
		public void ResetTestArtists()
		{
			_artistRepository = new ArtistRepository();
			_mongoDatabaseUtilities = new MongoDatabaseUtilities<Artist>("artists");

			_testArtistOne = TestArtistOne();
			_testArtistTwo = TestArtistTwo();
			_testArtistThree = TestArtistThree();
		}

		[Test]
		public void Add_SingleValidArtist_ShouldBeInserted()
		{
			_artistRepository.Add(_testArtistOne).Wait();

			var retrievedArtist = _mongoDatabaseUtilities.FindRecordById(_testArtistOne.Id);

			ArtistAssertor.New()
				.Expected(_testArtistOne)
				.Actual(retrievedArtist[0])
				.DoAssert();
		}

		[Test]
		public void GetAll_ThreeItemsInCollection_ShouldReturnAllThree()
		{
			_artistRepository.Add(_testArtistOne).Wait();
			_artistRepository.Add(_testArtistTwo).Wait();
			_artistRepository.Add(_testArtistThree).Wait();

			var resultFromDatabase = _artistRepository.GetAll().Result;

			Assert.AreEqual(3, resultFromDatabase.Count);

			ArtistAssertor.New()
				.Expected(_testArtistOne)
				.Actual(resultFromDatabase.Find(a => a.Id == _testArtistOne.Id))
				.DoAssert();

			ArtistAssertor.New()
				.Expected(_testArtistTwo)
				.Actual(resultFromDatabase.Find(a => a.Id == _testArtistTwo.Id))
				.DoAssert();

			ArtistAssertor.New()
				.Expected(_testArtistThree)
				.Actual(resultFromDatabase.Find(a => a.Id == _testArtistThree.Id))
				.DoAssert();
		}

		[Test]
		public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
		{
			var resultFromDatabase = _artistRepository.GetAll().Result;
			CollectionAssert.IsEmpty(resultFromDatabase);
		}

		[Test]
		public void GetbyId_ValidObjectId_ShouldReturnSingleArtist()
		{
			_artistRepository.Add(_testArtistOne).Wait();
			// This has same details, but different Id to the above
			_artistRepository.Add(TestArtistOne()).Wait();

			var result = _artistRepository.GetById(_testArtistOne.Id).Result;

			Assert.AreEqual(1, result.Count);
			ArtistAssertor.New()
				.Expected(_testArtistOne)
				.Actual(result[0])
				.DoAssert();
		}

		[Test]
		public void GetbyId_ValidStringId_ShouldReturnSingleArtist()
		{
			_artistRepository.Add(_testArtistOne).Wait();
			// This has same details, but different Id to the above
			_artistRepository.Add(TestArtistOne()).Wait();

			var idAsString = _testArtistOne.Id.ToString();
			var result = _artistRepository.GetById(idAsString).Result;

			Assert.AreEqual(1, result.Count);
			ArtistAssertor.New()
				.Expected(_testArtistOne)
				.Actual(result[0])
				.DoAssert();
		}

		[Test]
		public void GetById_InvalidObjectId_ShouldReturnEmptyList()
		{
			_artistRepository.Add(_testArtistOne).Wait();

			var idToLookFor = new ObjectId();
			var result = _artistRepository.GetById(idToLookFor).Result;

			Assert.AreEqual(0, result.Count);
		}

		[Test]
		public void GetById_InvalidStringId_ShouldReturnEmptyList()
		{
			_artistRepository.Add(_testArtistOne).Wait();

			var idAsString = new ObjectId().ToString();
			var result = _artistRepository.GetById(idAsString).Result;

			Assert.AreEqual(0, result.Count);
		}

		[Test]
		public void UpdateById_ValidObjectId_ShouldUpdateAndReturnTrue()
		{
			_artistRepository.Add(_testArtistOne).Wait();
			_artistRepository.Add(_testArtistTwo).Wait();

			_testArtistOne.Name = "Ringo Star";
			_testArtistOne.LastSearchedForDate = new DateTime(2019, 08, 10);

			var result = _artistRepository.UpdateById(_testArtistOne.Id, _testArtistOne);
			Assert.IsTrue(result);

			var recordFromDatabase = _mongoDatabaseUtilities.FindRecordById(_testArtistOne.Id);
			ArtistAssertor.New()
				.Expected(_testArtistOne)
				.Actual(recordFromDatabase[0])
				.DoAssert();
		}

		[Test]
		public void UpdateById_ValidStringId_ShouldUpdateAndReturnTrue()
		{
			// Setup
			_artistRepository.Add(_testArtistOne).Wait();
			_artistRepository.Add(_testArtistTwo).Wait();

			_testArtistOne.Name = "Ringo Star";
			_testArtistOne.LastSearchedForDate = new DateTime(2019, 08, 10);

			// Perform
			var idAsString = _testArtistOne.Id.ToString();
			var result = _artistRepository.UpdateById(idAsString, _testArtistOne);

			// Verify
			Assert.AreEqual(true, result);

			var recordFromDatabase = _mongoDatabaseUtilities.FindRecordById(_testArtistOne.Id);
			ArtistAssertor.New()
				.Expected(_testArtistOne)
				.Actual(recordFromDatabase[0])
				.DoAssert();
		}

		[Test]
		public void UpdateById_InvalidObjectId_ShouldReturnFalseAndNotUpdate()
		{
			// Setup
			_artistRepository.Add(_testArtistOne).Wait();

			var updatedDetails = new Artist
			{
				Name = "Ringo Star",
				LastSearchedForDate = new DateTime(2019, 08, 10)
			};

			// Perform
			var invalidObjectId = new ObjectId();
			var result = _artistRepository.UpdateById(invalidObjectId, _testArtistOne);

			// Verify
			Assert.IsFalse(result);

			var recordFromDatabase = _mongoDatabaseUtilities.FindRecordById(_testArtistOne.Id);
			ArtistAssertor.New()
				.Expected(_testArtistOne)
				.Actual(recordFromDatabase[0])
				.DoAssert();
		}

		[Test]
		public void UpdateById_InvalidStringId_ShouldReturnFalseAndNotUpdate()
		{
			// Setup
			_artistRepository.Add(_testArtistOne).Wait();

			var updatedDetails = new Artist
			{
				Name = "Ringo Star",
				LastSearchedForDate = new DateTime(2019, 08, 10)
			};

			// Perform
			var invalidIdAsString = new ObjectId().ToString();
			var result = _artistRepository.UpdateById(invalidIdAsString, updatedDetails);

			// Verify
			Assert.IsFalse(result);

			var recordFromDatabase = _mongoDatabaseUtilities.FindRecordById(_testArtistOne.Id);
			ArtistAssertor.New()
				.Expected(_testArtistOne)
				.Actual(recordFromDatabase[0])
				.DoAssert();
		}

		[Test]
		public void DeleteById_ValidObjectId_ShouldBeDeletedAndReturnNumberOfRecordsDeleted()
		{
			_artistRepository.Add(_testArtistOne).Wait();
			_artistRepository.Add(_testArtistTwo).Wait();

			var countBefore = _mongoDatabaseUtilities.CountRecordsInCollection();

			// Perform
			var numberDeleted = _artistRepository.DeleteById(_testArtistOne.Id);

			// Verify
			Assert.AreEqual(1, numberDeleted);

			var countAfter = _mongoDatabaseUtilities.CountRecordsInCollection();
			Assert.AreEqual(countBefore - 1, countAfter);

			var findResult = _mongoDatabaseUtilities.FindRecordById(_testArtistOne.Id);
			CollectionAssert.IsEmpty(findResult);
		}

		[Test]
		public void DeleteById_ValidStringId_ShouldBeDeletedAndReturnNumberOfRecordsDeleted()
		{
			_artistRepository.Add(_testArtistOne).Wait();
			_artistRepository.Add(_testArtistTwo).Wait();

			var countBefore = _mongoDatabaseUtilities.CountRecordsInCollection();

			// Perform
			var idAsString = _testArtistOne.Id.ToString();
			var numberDeleted = _artistRepository.DeleteById(idAsString);

			// Verify
			Assert.AreEqual(1, numberDeleted);

			var countAfter = _mongoDatabaseUtilities.CountRecordsInCollection();
			Assert.AreEqual(countBefore - 1, countAfter);

			var findResult = _mongoDatabaseUtilities.FindRecordById(_testArtistOne.Id);
			CollectionAssert.IsEmpty(findResult);
		}

		[Test]
		public void DeleteById_InvalidObjectId_ShouldReturnZeroAndNotDeleteAnything()
		{
			_artistRepository.Add(_testArtistOne).Wait();
			_artistRepository.Add(_testArtistTwo).Wait();

			var countBefore = _mongoDatabaseUtilities.CountRecordsInCollection();

			// Perform
			var invalidId = new ObjectId();
			var numberDeleted = _artistRepository.DeleteById(invalidId);

			// Verify
			Assert.AreEqual(0, numberDeleted);

			var countAfter = _mongoDatabaseUtilities.CountRecordsInCollection();
			Assert.AreEqual(countBefore, countAfter);
		}

		[Test]
		public void DeleteById_InvalidStringId_ShouldReturnZeroAndNotDeleteAnything()
		{
			_artistRepository.Add(_testArtistOne).Wait();
			_artistRepository.Add(_testArtistTwo).Wait();

			var countBefore = _mongoDatabaseUtilities.CountRecordsInCollection();

			// Perform
			var idAsString = new ObjectId().ToString();
			var numberDeleted = _artistRepository.DeleteById(idAsString);

			// Verify
			Assert.AreEqual(0, numberDeleted);

			var countAfter = _mongoDatabaseUtilities.CountRecordsInCollection();
			Assert.AreEqual(countBefore, countAfter);
		}

		[TearDown]
		public void DropArtistCollection()
		{
			_mongoDatabaseUtilities.RemoveCollection();
		}

		#region Test Data
		private static Artist TestArtistOne()
		{
			return new Artist
			{
				Name = "John Lennon",
				LastSearchedForDate = new DateTime(2018, 12, 31)
			};
		}

		private static Artist TestArtistTwo()
		{
			return new Artist
			{
				Name = "Paul Macartney",
				LastSearchedForDate = new DateTime(2019, 05, 15)
			};
		}

		private static Artist TestArtistThree()
		{
			return new Artist
			{
				Name = "John Lennon",
				LastSearchedForDate = new DateTime(2018, 12, 31)
			};
		}
		#endregion
	}
}
