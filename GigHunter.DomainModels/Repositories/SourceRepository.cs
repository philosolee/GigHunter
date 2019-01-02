using System.Collections.Generic;
using System.Threading.Tasks;
using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Utilities;
using MongoDB.Driver;

namespace GigHunter.DomainModels.Repositories
{
	public class SourceRepository : RepositoryBase<Source>, IRepository<Source>
	{
		public SourceRepository() : base("sources")
		{
			//_sourceCollection = mongoDatabase.GetCollection<Source>("sources");
		}

		public async Task<List<Source>> GetByName(string sourceName)
		{
			return await MongoCollection.Find(Filter<Source>.Name(sourceName)).ToListAsync();
		}

		public override UpdateDefinition<Source> EntityUpdateDefinition(Source updatedItem)
		{
			return Builders<Source>.Update
				.Set("Name", updatedItem.Name)
				.Set("BaseUrl", updatedItem.BaseUrl)
				.Set("ApiToken", updatedItem.ApiToken)
				.Set("TokenExpiryDate", updatedItem.TokenExpiryDate);
		}
	}
}
