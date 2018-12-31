using NUnit.Framework;
using System;
using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Repositories;
using GigHunter.DomainModel.Tests.Utilities;
using MongoDB.Bson;
using GigHunter.DomainModel.Tests.Assertors;

namespace GigHunter.DomainModel.Tests.Repositories
{
	[TestFixture]
	public class GigRepositoryTests
	{
		private GigRepository _gigRepository;
		private MongoDatabaseUtilities<Gig> _mongoDatabaseUtilities;
		private Gig _testGigOne;
		private Gig _testGigTwo;
		private Gig _testGigThree;

		[SetUp]
		public void Setup()
		{
			_gigRepository = new GigRepository();
			_mongoDatabaseUtilities = new MongoDatabaseUtilities<Gig>("gigs");

			_testGigOne = TestGigOne();
			_testGigTwo = TestGigTwo();
			_testGigThree = TestGigThree();
		}

		[Test]
		public void Add_SingleValidGig_ShouldBeInsertedIntoDatabase()
		{
			// Perform
			_gigRepository.Add(_testGigOne).Wait();
			
			// Verify - retrived via test code, not production code
			var retrivedGig = _mongoDatabaseUtilities.FindRecordById(_testGigOne.Id);

			GigAssertor.New()
				.Expected(_testGigOne)
				.Actual(retrivedGig[0])
				.DoAssert();
		}
		
		[Test]
		public void GetAll_ThreeItemsInCollection_ShouldReturnAllThree()
		{
			_gigRepository.Add(_testGigOne).Wait();
			_gigRepository.Add(_testGigTwo).Wait();
			_gigRepository.Add(_testGigThree).Wait();

			var retrivedGigs = _gigRepository.GetAll().Result;

			Assert.AreEqual(3, retrivedGigs.Count);

			GigAssertor.New()
				.Expected(_testGigOne)
				.Actual(retrivedGigs.Find(g => g.Id == _testGigOne.Id))
				.DoAssert();

			GigAssertor.New()
				.Expected(_testGigTwo)
				.Actual(retrivedGigs.Find(g => g.Id == _testGigTwo.Id))
				.DoAssert();

			GigAssertor.New()
				.Expected(_testGigThree)
				.Actual(retrivedGigs.Find(g => g.Id == _testGigThree.Id))
				.DoAssert();
		}

		[Test]
		public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
		{
			var result = _gigRepository.GetAll().Result;
			CollectionAssert.IsEmpty(result);
		}
		
		[Test]
		public void GetById_ValidObjectId_ShouldReturnSingleGigInList()
		{
			_gigRepository.Add(_testGigOne).Wait();
			// This has the same details but a different Id to the above
			_gigRepository.Add(TestGigOne()).Wait();

			// Perform
			var result = _gigRepository.GetById(_testGigOne.Id).Result;

			// Verify
			Assert.AreEqual(1, result.Count);
			GigAssertor.New()
				.Expected(_testGigOne)
				.Actual(result[0])
				.DoAssert();
		}

		[Test]
		public void GetById_ValidStringId_ShouldReturnSingleGigInList()
		{
			_gigRepository.Add(_testGigOne).Wait();
			// This has the same details but a different Id to the above
			_gigRepository.Add(TestGigOne()).Wait();

			// Perform
			var idAsString = _testGigOne.Id.ToString();
			var result = _gigRepository.GetById(idAsString).Result;

			// Verify
			Assert.AreEqual(1, result.Count);
			GigAssertor.New()
				.Expected(_testGigOne)
				.Actual(result[0])
				.DoAssert();
		}

		[Test]
		public void GetId_InvalidObjectId_ShouldReturnEmptyList()
		{
			_gigRepository.Add(_testGigOne).Wait();

			var id = new ObjectId();
			var result = _gigRepository.GetById(id).Result;

			// Empty List returned
			CollectionAssert.IsEmpty(result);
		}

		[Test]
		public void GetId_InvalidStringId_ShouldReturnEmptyList()
		{
			_gigRepository.Add(_testGigOne).Wait();

			var id = new ObjectId().ToString();
			var result = _gigRepository.GetById(id).Result;

			// Empty List returned
			CollectionAssert.IsEmpty(result);
		}
		
		[Test]
		public void UpdateById_ValidObjectId_ShouldUpdateAndReturnTrue()
		{
			_gigRepository.Add(_testGigOne).Wait();
			_gigRepository.Add(_testGigTwo).Wait();

			// Change values now inserted
			_testGigOne.Artist = "Radiohead";
			_testGigOne.Venue = "Sheffield";
			_testGigOne.Date = new DateTime(2019, 4, 18);
			_testGigOne.TicketUri = "http://stackoverflow.com";

			// Update the record
			var result = _gigRepository.UpdateById(_testGigOne.Id, _testGigOne);

			// Validate
			Assert.IsTrue(result);

			var recordFromDatabase = _mongoDatabaseUtilities.FindRecordById(_testGigOne.Id);
			GigAssertor.New()
				.Expected(_testGigOne)
				.Actual(recordFromDatabase[0])
				.DoAssert();
		}

		[Test]
		public void UpdateById_ValidStringId_ShouldUpdateAndReturnTrue()
		{
			_gigRepository.Add(_testGigOne).Wait();
			_gigRepository.Add(_testGigTwo).Wait();

			// Change values now inserted
			_testGigOne.Artist = "Radiohead";
			_testGigOne.Venue = "Sheffield";
			_testGigOne.Date = new DateTime(2019, 4, 18);
			_testGigOne.TicketUri = "http://stackoverflow.com";

			// Update the record
			var idAsString = _testGigOne.Id.ToString();
			var result = _gigRepository.UpdateById(idAsString, _testGigOne);

			// Validate
			Assert.IsTrue(result);

			var recordFromDatabase = _mongoDatabaseUtilities.FindRecordById(_testGigOne.Id);
			GigAssertor.New()
				.Expected(_testGigOne)
				.Actual(recordFromDatabase[0])
				.DoAssert();
		}

		[Test]
		public void UpdateById_InvalidObjectId_ShouldReturnFalseAndMakeNoChanges()
		{
			// Setup
			_gigRepository.Add(_testGigOne).Wait();

			var newDetails = new Gig
			{
				Artist = "Radiohead",
				Venue = "Sheffield",
				Date = new DateTime(2019, 4, 18),
				TicketUri = "http://stackoverflow.com"
			};

			// Perform
			var invalidId = new ObjectId();
			var result = _gigRepository.UpdateById(invalidId, newDetails);

			// Verify
			Assert.IsFalse(result);

			var recordFromDatabase = _mongoDatabaseUtilities.FindRecordById(_testGigOne.Id);
			GigAssertor.New()
				.Expected(_testGigOne)
				.Actual(recordFromDatabase[0])
				.DoAssert();
		}

		[Test]
		public void UpdateById_InvalidStringId_ShouldReturnFalseAndMakeNoChanges()
		{
			// Setup
			_gigRepository.Add(_testGigOne).Wait();

			var newDetails = new Gig
			{
				Artist = "Radiohead",
				Venue = "Sheffield",
				Date = new DateTime(2019, 4, 18),
				TicketUri = "http://stackoverflow.com"
			};

			// Perform
			var invalidIdAsSting = new ObjectId().ToString();
			var result = _gigRepository.UpdateById(invalidIdAsSting, newDetails);

			// Verify
			Assert.IsFalse(result);

			var recordFromDatabase = _mongoDatabaseUtilities.FindRecordById(_testGigOne.Id);
			GigAssertor.New()
				.Expected(_testGigOne)
				.Actual(recordFromDatabase[0])
				.DoAssert();
		}

		[Test]
		public void DeleteById_ValidObjectId_ShouldBeDeletedAndReturnTrue()
		{
			_gigRepository.Add(_testGigOne).Wait();
			_gigRepository.Add(_testGigTwo).Wait();

			var countBefore = _mongoDatabaseUtilities.CountRecordsInCollection();

			// Perform
			var numberDeleted = _gigRepository.DeleteById(_testGigOne.Id);

			// Verify
			Assert.AreEqual(1, numberDeleted);
			var countAfter = _mongoDatabaseUtilities.CountRecordsInCollection();
			Assert.AreEqual(countBefore - 1, countAfter);

			var findResult = _mongoDatabaseUtilities.FindRecordById(_testGigOne.Id);
			CollectionAssert.IsEmpty(findResult);
		}

		[Test]
		public void DeleteById_ValidStringId_ShouldBeDeletedAndReturnTrue()
		{
			_gigRepository.Add(_testGigOne).Wait();
			_gigRepository.Add(_testGigTwo).Wait();

			var countBefore = _mongoDatabaseUtilities.CountRecordsInCollection();

			// Perform
			var idAsString = _testGigOne.Id.ToString();
			var numberDeleted = _gigRepository.DeleteById(idAsString);

			// Verify
			Assert.AreEqual(1, numberDeleted);
			var countAfter = _mongoDatabaseUtilities.CountRecordsInCollection();
			Assert.AreEqual(countBefore - 1, countAfter);

			var findResult = _mongoDatabaseUtilities.FindRecordById(_testGigOne.Id);
			CollectionAssert.IsEmpty(findResult);
		}

		[Test]
		public void DeleteById_InvalidObjectId_ShouldReturnFalse()
		{
			// Setup - Gig added to ensure no others are removed.
			_gigRepository.Add(_testGigOne).Wait();
			var countBefore = _mongoDatabaseUtilities.CountRecordsInCollection();

			// Perform
			var invalidId = new ObjectId();
			var numberDeleted = _gigRepository.DeleteById(invalidId);

			// Verify
			Assert.AreEqual(0, numberDeleted);
			var countAfter = _mongoDatabaseUtilities.CountRecordsInCollection();
			Assert.AreEqual(countBefore, countAfter);
		}

		[Test]
		public void DeleteById_InvalidStringId_ShouldReturnFalse()
		{
			// Setup - Gig added to ensure no others are removed.
			_gigRepository.Add(_testGigOne).Wait();
			var countBefore = _mongoDatabaseUtilities.CountRecordsInCollection();

			// Perform
			var invalidIdAsString = new ObjectId().ToString();
			var numberDeleted = _gigRepository.DeleteById(invalidIdAsString);

			// Verify
			Assert.AreEqual(0, numberDeleted);
			var countAfter = _mongoDatabaseUtilities.CountRecordsInCollection();
			Assert.AreEqual(countBefore, countAfter);
		}
		
		[TearDown]
		public void DropCollection()
		{
			_mongoDatabaseUtilities.RemoveCollection();
		}

		#region TestData
		private static Gig TestGigOne()
		{
			return new Gig
			{
				Artist = "Tool",
				Date = new DateTime(2018, 05, 15),
				Venue = "Amsterdam",
				TicketUri = "http://google.com"
			};
		}

		private static Gig TestGigTwo()
		{
			return new Gig
			{
				Artist = "The Mars Volta",
				Date = new DateTime(2019, 12, 31),
				Venue = "Machester",
				TicketUri = "http://bbc.co.uk"
			};
		}

		private static Gig TestGigThree()
		{
			return new Gig
			{
				Artist = "ISIS",
				Date = new DateTime(2019, 07, 01),
				Venue = "Leeds",
				TicketUri = "http://reddit.com"
			};
		}
		#endregion
	}
}
