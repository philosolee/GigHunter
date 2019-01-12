using GigHunter.DomainModels.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace GigHunter.DomainModels
{
	public class EntityBase : IEntity
	{
		[BsonId]
		public ObjectId Id { get; set; }
	}

}