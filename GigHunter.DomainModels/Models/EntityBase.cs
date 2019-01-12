using GigHunter.DomainModels.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GigHunter.DomainModels.Models
{
	public class EntityBase : IEntity
	{
		[BsonId]
		public ObjectId Id { get; set; }
	}

}