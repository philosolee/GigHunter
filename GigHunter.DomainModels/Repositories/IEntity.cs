using MongoDB.Bson;

namespace GigHunter.DomainModels.Repositories
{
	public interface IEntity
	{
		ObjectId Id { get; set; }
	}
}
