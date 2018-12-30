using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GigHunter.DomainModels.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GigHunter.DomainModels.Repositories
{
	public class GigRepository : RepositoryBase
	{
		private IMongoCollection<Gig> _gigCollection;

		private GigRepository()
		{
			_gigCollection = mongoDatabase.GetCollection<Gig>("gigs");
		}

		public static GigRepository New()
		{
			return new GigRepository();
		}

		public async Task InsertGig(Gig gig)
		{
			await _gigCollection.InsertOneAsync(gig);
		}

		public async Task<List<Gig>> GetGigById(string id)
		{
			var result = await _gigCollection.Find(FilterForId(id)).ToListAsync();
			return result;
		}	

		public async Task<List<Gig>> GetAllGigs()
		{
			var filter = Builders<Gig>.Filter.Empty;
			var result = await _gigCollection.Find(filter).ToListAsync();
			return result;
		}

		public async Task<bool> UpdateGigById(string id, Gig newGigDetails)
		{
			var update = Builders<Gig>.Update
				.Set("Artist", newGigDetails.Artist)
				.Set("Venue", newGigDetails.Venue)
				.Set("Date", newGigDetails.Date)
				.Set("TicketUri", newGigDetails.TicketUri);
			
			var result = _gigCollection.UpdateOne(FilterForId(id), update);

			return result.ModifiedCount != 0;
		}

		public long DeleteGigById(string id)
		{
			var result = _gigCollection.DeleteOne(FilterForId(id));
			return result.DeletedCount;
		}

		private static FilterDefinition<Gig> FilterForId(string id)
		{
			return Builders<Gig>.Filter.Eq("Id", IdAsObjectId(id));
		}

		private static ObjectId IdAsObjectId(string id)
		{
			return new ObjectId(id);
		}


	}
}
