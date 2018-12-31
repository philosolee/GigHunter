using System.Collections.Generic;
using System.Threading.Tasks;
using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Utilities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GigHunter.DomainModels.Repositories
{
	public class GigRepository : RepositoryBase, IRepository<Gig>
	{
		private readonly IMongoCollection<Gig> _gigCollection;

		public GigRepository()
		{
			_gigCollection = mongoDatabase.GetCollection<Gig>("gigs");
		}

		public async Task Add(Gig gig)
		{
			await _gigCollection.InsertOneAsync(gig);
		}

		public async Task<List<Gig>> GetAll()
		{
			var result = await _gigCollection.Find(Filter<Gig>.Empty).ToListAsync();
			return result;
		}

		public async Task<List<Gig>> GetById(ObjectId id)
		{
			var result = await _gigCollection.Find(Filter<Gig>.Id(id)).ToListAsync();
			return result;
		}

		public async Task<List<Gig>> GetById(string id)
		{
			var result = await _gigCollection.Find(Filter<Gig>.IdAsString(id)).ToListAsync();
			return result;
		}

		public bool UpdateById(ObjectId id, Gig updatedGig)
		{
			var update = GigUpdateDefinition(updatedGig);

			var result = _gigCollection.UpdateOne(Filter<Gig>.Id(id), update);

			return result.ModifiedCount != 0;
		}

		public bool UpdateById(string id, Gig updatedGig)
		{
			var update = GigUpdateDefinition(updatedGig);

			var result = _gigCollection.UpdateOne(Filter<Gig>.IdAsString(id), update);

			return result.ModifiedCount != 0;
		}

		public bool DeleteById(ObjectId id)
		{
			var result = _gigCollection.DeleteOne(Filter<Gig>.Id(id));
			return result.DeletedCount == 1;
		}

		public bool DeleteById(string id)
		{
			var result = _gigCollection.DeleteOne(Filter<Gig>.IdAsString(id));
			return result.DeletedCount == 1;
		}

		private static UpdateDefinition<Gig> GigUpdateDefinition(Gig updatedGig)
		{
			return Builders<Gig>.Update
				.Set("Artist", updatedGig.Artist)
				.Set("Venue", updatedGig.Venue)
				.Set("Date", updatedGig.Date)
				.Set("TicketUrls", updatedGig.TicketUrls);
		}
	}
}
