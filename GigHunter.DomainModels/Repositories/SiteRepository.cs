using System.Collections.Generic;
using System.Threading.Tasks;
using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Utilities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GigHunter.DomainModels.Repositories
{
	public class SiteRepository : RepositoryBase, IRepository<Site>
	{
		private readonly IMongoCollection<Site> _siteCollection;

		public SiteRepository()
		{
			_siteCollection = mongoDatabase.GetCollection<Site>("sites");
		}

		public async Task Add(Site site)
		{
			await _siteCollection.InsertOneAsync(site);
		}

		public async Task<List<Site>> GetAll()
		{
			return await _siteCollection.Find(Filter<Site>.Empty).ToListAsync();
		}

		public async Task<List<Site>> GetById(ObjectId id)
		{
			return await _siteCollection.Find(Filter<Site>.Id(id)).ToListAsync();
		}

		public async Task<List<Site>> GetById(string id)
		{
			return await _siteCollection.Find(Filter<Site>.IdAsString(id)).ToListAsync();
		}

		public bool UpdateById(ObjectId id, Site updatedItem)
		{
			var updateDefinition = SiteUpdateDefinition(updatedItem);

			var result = _siteCollection.UpdateOne(Filter<Site>.Id(id), updateDefinition);
			return result.MatchedCount != 0;
		}

		public bool UpdateById(string id, Site updatedItem)
		{
			var updateDefinition = SiteUpdateDefinition(updatedItem);

			var result = _siteCollection.UpdateOne(Filter<Site>.IdAsString(id), updateDefinition);
			return result.MatchedCount != 0;
		}
		
		public bool DeleteById(ObjectId id)
		{
			var result = _siteCollection.DeleteOne(Filter<Site>.Id(id));
			return result.DeletedCount == 1;
		}

		public bool DeleteById(string id)
		{
			var result = _siteCollection.DeleteOne(Filter<Site>.IdAsString(id));
			return result.DeletedCount == 1;
		}

		private static UpdateDefinition<Site> SiteUpdateDefinition(Site updatedItem)
		{
			return Builders<Site>.Update
				.Set("Name", updatedItem.Name)
				.Set("BaseUrl", updatedItem.BaseUrl);
		}

	}
}
