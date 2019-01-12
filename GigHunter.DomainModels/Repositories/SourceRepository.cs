using GigHunter.DomainModels.Models;
using MongoDB.Driver;

namespace GigHunter.DomainModels.Repositories
{
	public class SourceRepository : RepositoryBase<Source>
	{
		public SourceRepository() : base("sources")
		{
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
