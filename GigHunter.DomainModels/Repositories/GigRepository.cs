using GigHunter.DomainModels.Models;
using MongoDB.Driver;

namespace GigHunter.DomainModels.Repositories
{
	public class GigRepository : RepositoryBase<Gig>, IRepository<Gig>
	{
		public GigRepository() : base("gigs")
		{			
		}

		public override UpdateDefinition<Gig> EntityUpdateDefinition(Gig updatedEntity)
		{
			return Builders<Gig>.Update
			.Set("Artist", updatedEntity.Artist)
			.Set("Venue", updatedEntity.Venue)
			.Set("Date", updatedEntity.Date)
			.Set("TicketUrls", updatedEntity.TicketUrls);
		}
	}
}
