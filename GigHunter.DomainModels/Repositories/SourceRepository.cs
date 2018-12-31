using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Utilities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GigHunter.DomainModels.Repositories
{
	public class SourceRepository : RepositoryBase, IRepository<Source>
	{
		private readonly IMongoCollection<Source> _sourceCollection;

		public SourceRepository()
		{
			_sourceCollection = mongoDatabase.GetCollection<Source>("sources");
		}

		public async Task Add(Source site)
		{
			await _sourceCollection.InsertOneAsync(site);
		}

		public async Task<List<Source>> GetAll()
		{
			return await _sourceCollection.Find(Filter<Source>.Empty).ToListAsync();
		}

		public async Task<List<Source>> GetById(ObjectId id)
		{
			return await _sourceCollection.Find(Filter<Source>.Id(id)).ToListAsync();
		}

		public async Task<List<Source>> GetById(string id)
		{
			return await _sourceCollection.Find(Filter<Source>.IdAsString(id)).ToListAsync();
		}

		public async Task<List<Source>> GetByName(string sourceName)
		{
			return await _sourceCollection.Find(Filter<Source>.Name(sourceName)).ToListAsync();
		}

		public bool UpdateById(ObjectId id, Source updatedItem)
		{
			var updateDefinition = SiteUpdateDefinition(updatedItem);

			var result = _sourceCollection.UpdateOne(Filter<Source>.Id(id), updateDefinition);
			return result.MatchedCount != 0;
		}

		public bool UpdateById(string id, Source updatedItem)
		{
			var updateDefinition = SiteUpdateDefinition(updatedItem);

			var result = _sourceCollection.UpdateOne(Filter<Source>.IdAsString(id), updateDefinition);
			return result.MatchedCount != 0;
		}
		
		public bool DeleteById(ObjectId id)
		{
			var result = _sourceCollection.DeleteOne(Filter<Source>.Id(id));
			return result.DeletedCount == 1;
		}

		public bool DeleteById(string id)
		{
			var result = _sourceCollection.DeleteOne(Filter<Source>.IdAsString(id));
			return result.DeletedCount == 1;
		}

		private static UpdateDefinition<Source> SiteUpdateDefinition(Source updatedItem)
		{
			return Builders<Source>.Update
				.Set("Name", updatedItem.Name)
				.Set("BaseUrl", updatedItem.BaseUrl)
				.Set("ApiToken", updatedItem.ApiToken)
				.Set("TokenExpiryDate", updatedItem.TokenExpiryDate);
		}

	}
}
