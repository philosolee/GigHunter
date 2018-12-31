using System.Collections.Generic;
using System.Threading.Tasks;
using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Utilities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GigHunter.DomainModels.Repositories
{
	public class ArtistRepository : RepositoryBase, IRepository<Artist>
	{
		private readonly IMongoCollection<Artist> _artistCollection;

		public ArtistRepository()
		{
			_artistCollection = mongoDatabase.GetCollection<Artist>("artists");
		}
		
		public async Task Add(Artist artist)
		{
			await _artistCollection.InsertOneAsync(artist);
		}

		public async Task<List<Artist>> GetAll()
		{
			var result = await _artistCollection.Find(Filter<Artist>.Empty).ToListAsync();
			return result;
		}

		public async Task<List<Artist>> GetById(ObjectId id)
		{
			var result = await _artistCollection.Find(Filter<Artist>.Id(id)).ToListAsync();
			return result;
		}

		public async Task<List<Artist>> GetById(string id)
		{
			var result = await _artistCollection.Find(Filter<Artist>.IdAsString(id)).ToListAsync();
			return result;
		}

		public bool UpdateById(ObjectId id, Artist updatedArtist)
		{
			var update = ArtistUpdateDefinition(updatedArtist);

			var result = _artistCollection.UpdateOne(Filter<Artist>.Id(id), update);

			return result.ModifiedCount != 0;
		}

		public bool UpdateById(string id, Artist updatedArtist)
		{
			var updateDefinition = ArtistUpdateDefinition(updatedArtist);

			var result = _artistCollection.UpdateOne(Filter<Artist>.IdAsString(id), updateDefinition);

			return result.ModifiedCount != 0;
		}

		public bool DeleteById(ObjectId id)
		{
			var result = _artistCollection.DeleteOne(Filter<Artist>.Id(id));
			return result.DeletedCount == 1;
		}

		public bool DeleteById(string id)
		{
			var result = _artistCollection.DeleteOne(Filter<Artist>.IdAsString(id));
			return result.DeletedCount == 1;
		}

		private static UpdateDefinition<Artist> ArtistUpdateDefinition(Artist updatedArtist)
		{
			return Builders<Artist>.Update
				.Set("Name", updatedArtist.Name)
				.Set("LastSearchedForDate", updatedArtist.LastSearchedForDate);
		}
	}
}
