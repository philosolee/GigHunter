using MongoDB.Bson;

namespace GigHunter.DomainModels.Repositories
{
	public interface IEntity
	{
		string Id { get; set; }
	}
}
