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
		[Test]
		public void InsertGig_SingleValidGig_ShouldBeInsertedIntoDatabase()
		{
			// Setup	
			var gig = TestGigOne();

			// Perform
			GigRepository.New()
				.InsertGig(gig)
				.Wait();
			
			// Verify - retrived via test code, not production code
			var retrivedGig = MongoDatabaseUtilities.New().FindRecordById<Gig>(gig.Id, "gigs");

			GigAssertor.New()
				.Expected(gig)
				.Actual(retrivedGig[0])
				.DoAssert();
		}
		
		[Test]
		public void GetGigById_ExistsInDatabase_ShouldReturnSingleGigInList()
		{
			// Setup
			var gig = TestGigOne();

			GigRepository.New()
				.InsertGig(gig)
				.Wait();

			// Perform - Retrieved by production code
			var idAsString = gig.Id.ToString();
			var retrivedGig = GigRepository.New().GetGigById(idAsString).Result;

			// Verify
			GigAssertor.New()
				.Expected(gig)
				.Actual(retrivedGig[0])
				.DoAssert();
		}	

		[Test]
		public void GetGigById_DoesntExistInDatabase_ShouldReturnEmptyList()
		{
			var id = new ObjectId().ToString();
			var retrivedGig = GigRepository.New().GetGigById(id).Result;

			// Empty List returned
			Assert.AreEqual(0, retrivedGig.Count);
		}

		[Test]
		public void GetAllGigs_ThreeItemsInCollection_ShouldReturnAllThree()
		{
			var gig1 = TestGigOne();
			var gig2 = TestGigTwo();
			var gig3 = TestGigThree();

			GigRepository.New().InsertGig(gig1).Wait();
			GigRepository.New().InsertGig(gig2).Wait();
			GigRepository.New().InsertGig(gig3).Wait();


			var retrivedGigs = GigRepository.New().GetAllGigs().Result;

			Assert.AreEqual(3, retrivedGigs.Count);

			GigAssertor.New()
				.Expected(gig1)
				.Actual(retrivedGigs.Find(g => g.Id == gig1.Id))
				.DoAssert();

			GigAssertor.New()
				.Expected(gig2)
				.Actual(retrivedGigs.Find(g => g.Id == gig2.Id))
				.DoAssert();

			GigAssertor.New()
				.Expected(gig3)
				.Actual(retrivedGigs.Find(g => g.Id == gig3.Id))
				.DoAssert();
		}

		[Test]
		public void UpdateGigById_ValidId_ShouldUpdateAndReturnTrue()
		{
			var gig = TestGigOne();
			GigRepository.New().InsertGig(gig).Wait();

			// Change values now inserted
			gig.Artist = "Radiohead";
			gig.Venue = "Sheffield";
			gig.Date = new DateTime(2019, 4, 18);
			gig.TicketUri = "http://stackoverflow.com";

			// Update the record
			var result = GigRepository.New().UpdateGigById(gig.Id.ToString(), gig).Result;

			// Validate
			Assert.AreEqual(true, result);

			var databaseRecord = MongoDatabaseUtilities.New().FindRecordById<Gig>(gig.Id, "gigs");
			GigAssertor.New()
				.Expected(gig)
				.Actual(databaseRecord[0])
				.DoAssert();
		}

		[Test]
		public void UpdateGigById_InvalidId_ShouldReturnFalse()
		{
			var id = new ObjectId().ToString();
			var result = GigRepository.New().UpdateGigById(id, TestGigThree()).Result;

			Assert.AreEqual(false, result);
		}

		[Test]
		public void DeleteGigById_ValidId_ShouldBeDeletedAndReturnTrue()
		{
			var gig1 = TestGigOne();
			var gig2 = TestGigTwo();

			GigRepository.New().InsertGig(gig1).Wait();
			GigRepository.New().InsertGig(gig2).Wait();

			var countBefore = MongoDatabaseUtilities.New().CountRecordsInCollection<Gig>("gigs");

			// Perform
			var deleteResult = GigRepository.New().DeleteGigById(gig1.Id.ToString());

			// Verify
			Assert.AreEqual(1, deleteResult);
			var countAfter = MongoDatabaseUtilities.New().CountRecordsInCollection<Gig>("gigs");
			Assert.AreEqual(countBefore - 1, countAfter);

			var findResult = MongoDatabaseUtilities.New().FindRecordById<Gig>(gig1.Id, "gigs");
			Assert.AreEqual(0, findResult.Count);
		}

		[Test]
		public void DeleteGigById_InvalidId_ShouldReturnFalse()
		{
			// Setup - Gig added to ensure no others are removed.
			GigRepository.New().InsertGig(TestGigOne()).Wait();
			var countBefore = MongoDatabaseUtilities.New().CountRecordsInCollection<Gig>("gigs");

			// Perform
			var invalidId = new ObjectId().ToString();
			var deleteResult = GigRepository.New().DeleteGigById(invalidId);

			// Verify
			Assert.AreEqual(0, deleteResult);
			var countAfter = MongoDatabaseUtilities.New().CountRecordsInCollection<Gig>("gigs");
			Assert.AreEqual(countBefore, countAfter);
		}


		[TearDown]
		public void DropCollection()
		{
			MongoDatabaseUtilities.New().RemoveCollection<Gig>("gigs");
		}

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


	}
}
