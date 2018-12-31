using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GigHunter.DomainModels.Models
{
	public class Site : IModel
	{
		[BsonId]
		public ObjectId Id { get; set; }

		public string Name { get; set; }

		public string BaseUrl { get; set; }
	}
}
