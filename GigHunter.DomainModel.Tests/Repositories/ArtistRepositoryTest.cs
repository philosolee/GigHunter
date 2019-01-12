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
			_artistRepository.Add(_testArtistOne);

			var retrievedArtist = _mongoDatabaseUtilities.FindRecordById(_testArtistOne.Id);

			ArtistAssertor.New()
				.Expected(_testArtistOne)
				.Actual(retrievedArtist[0])
				.DoAssert();
		}

		[Test]
		public void GetAll_ThreeItemsInCollection_ShouldReturnAllThree()
		{
			_artistRepository.Add(_testArtistOne);
			_artistRepository.Add(_testArtistTwo);
			_artistRepository.Add(_testArtistThree);

			var resultFromDatabase = _artistRepository.GetAll();

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
			var resultFromDatabase = _artistRepository.GetAll();
			CollectionAssert.IsEmpty(resultFromDatabase);
		}

		[Test]
		public void GetbyId_ValidObjectId_ShouldReturnSingleArtist()
		{
			_artistRepository.Add(_testArtistOne);
			// This has same details, but different Id to the above
			_artistRepository.Add(TestArtistOne());

			var result = _artistRepository.GetById(_testArtistOne.Id);

			ArtistAssertor.New()
				.Expected(_testArtistOne)
				.Actual(result)
				.DoAssert();
		}

		[Test]
		public void GetById_InvalidObjectId_ShouldReturnEmptyList()
		{
			_artistRepository.Add(_testArtistOne);

			var idToLookFor = new ObjectId();
			var result = _artistRepository.GetById(idToLookFor);

			Assert.IsNull(result);
		}

		[Test]
		public void UpdateById_ValidObjectId_ShouldUpdateAndReturnTrue()
		{
			_artistRepository.Add(_testArtistOne);
			_artistRepository.Add(_testArtistTwo);

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
		public void UpdateById_InvalidObjectId_ShouldReturnFalseAndNotUpdate()
		{
			// Setup
			_artistRepository.Add(_testArtistOne);

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
		public void DeleteById_ValidObjectId_ShouldBeDeletedAndReturnTrue()
		{
			_artistRepository.Add(_testArtistOne);
			_artistRepository.Add(_testArtistTwo);

			var countBefore = _mongoDatabaseUtilities.CountRecordsInCollection();

			// Perform
			var result = _artistRepository.DeleteById(_testArtistOne.Id);

			// Verify
			Assert.IsTrue(result);

			var countAfter = _mongoDatabaseUtilities.CountRecordsInCollection();
			Assert.AreEqual(countBefore - 1, countAfter);

			var findResult = _mongoDatabaseUtilities.FindRecordById(_testArtistOne.Id);
			CollectionAssert.IsEmpty(findResult);
		}

		[Test]
		public void DeleteById_InvalidObjectId_ShouldNotDeleteAnythingAndReturnFalse()
		{
			_artistRepository.Add(_testArtistOne);
			_artistRepository.Add(_testArtistTwo);

			var countBefore = _mongoDatabaseUtilities.CountRecordsInCollection();

			// Perform
			var invalidId = new ObjectId();
			var result = _artistRepository.DeleteById(invalidId);

			// Verify
			Assert.IsFalse(result);

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
