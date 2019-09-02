using GigHunter.DomainModels.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GigHunter.DomainModels.Models
{
	public class EntityBase : IEntity
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
	}

}