using GigHunter.DomainModels.Models;
using MongoDB.Driver;

namespace GigHunter.DomainModels.Repositories
{
	public class ArtistRepository : RepositoryBase<Artist>, IRepository<Artist>
	{
		public ArtistRepository() : base("artists")
		{
		}
		
		public override UpdateDefinition<Artist> EntityUpdateDefinition(Artist updatedArtist)
		{
			return Builders<Artist>.Update
				.Set("Name", updatedArtist.Name)
				.Set("LastSearchedForDate", updatedArtist.LastSearchedForDate);
		}
	}
}
