using System;
using System.Collections.Generic;
using GigHunter.TestUtilities.Assertors;
using GigHunter.TestUtilities.Database;
using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Repositories;
using MongoDB.Bson;
using NUnit.Framework;

namespace GigHunter.DomainModels.Tests.Repositories
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
			_gigRepository.Add(_testGigOne);
			
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
			_gigRepository.Add(_testGigOne);
			_gigRepository.Add(_testGigTwo);
			_gigRepository.Add(_testGigThree);

			var retrivedGigs = _gigRepository.GetAll();

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
			var result = _gigRepository.GetAll();
			CollectionAssert.IsEmpty(result);
		}
		
		[Test]
		public void GetById_ValidObjectId_ShouldReturnSingleGigInList()
		{
			_gigRepository.Add(_testGigOne);
			// This has the same details but a different Id to the above
			_gigRepository.Add(TestGigOne());

			// Perform
			var result = _gigRepository.GetById(_testGigOne.Id);

			// Verify
			GigAssertor.New()
				.Expected(_testGigOne)
				.Actual(result)
				.DoAssert();
		}

		[Test]
		public void GetId_InvalidObjectId_ShouldReturnEmptyList()
		{
			_gigRepository.Add(_testGigOne);

			var id = new ObjectId();
			var result = _gigRepository.GetById(id);

			// Empty List returned
			Assert.IsNull(result);
		}

		[Test]
		public void UpdateById_ValidObjectId_ShouldUpdateAndReturnTrue()
		{
			_gigRepository.Add(_testGigOne);
			_gigRepository.Add(_testGigTwo);

			// Change values now inserted
			_testGigOne.Artist = "Radiohead";
			_testGigOne.Venue = "Sheffield";
			_testGigOne.Date = new DateTime(2019, 4, 18);
			_testGigOne.TicketUrls.Add("http://stackoverflow.com");

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
		public void UpdateById_InvalidObjectId_ShouldReturnFalseAndMakeNoChanges()
		{
			// Setup
			_gigRepository.Add(_testGigOne);

			var newDetails = new Gig
			{
				Artist = "Radiohead",
				Venue = "Sheffield",
				Date = new DateTime(2019, 4, 18),
				TicketUrls = new List<string> { "http://stackoverflow.com" }
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
		public void DeleteById_ValidObjectId_ShouldBeDeletedAndReturnTrue()
		{
			_gigRepository.Add(_testGigOne);
			_gigRepository.Add(_testGigTwo);

			var countBefore = _mongoDatabaseUtilities.CountRecordsInCollection();

			// Perform
			var result = _gigRepository.DeleteById(_testGigOne.Id);

			// Verify
			Assert.IsTrue(result);
			var countAfter = _mongoDatabaseUtilities.CountRecordsInCollection();
			Assert.AreEqual(countBefore - 1, countAfter);

			var findResult = _mongoDatabaseUtilities.FindRecordById(_testGigOne.Id);
			CollectionAssert.IsEmpty(findResult);
		}

		[Test]
		public void DeleteById_InvalidObjectId_ShouldReturnFalse()
		{
			// Setup - Gig added to ensure no others are removed.
			_gigRepository.Add(_testGigOne);
			var countBefore = _mongoDatabaseUtilities.CountRecordsInCollection();

			// Perform
			var invalidId = new ObjectId();
			var result = _gigRepository.DeleteById(invalidId);

			// Verify
			Assert.IsFalse(result);
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
				TicketUrls = new List<string> { "http://google.com" }
			};
		}

		private static Gig TestGigTwo()
		{
			return new Gig
			{
				Artist = "The Mars Volta",
				Date = new DateTime(2019, 12, 31),
				Venue = "Machester",
				TicketUrls = new List<string> { "http://bbc.co.uk" }
			};
		}

		private static Gig TestGigThree()
		{
			return new Gig
			{
				Artist = "ISIS",
				Date = new DateTime(2019, 07, 01),
				Venue = "Leeds",
				TicketUrls = new List<string> { "http://reddit.com", "http://google.com" }
			};
		}
		#endregion
	}
}
