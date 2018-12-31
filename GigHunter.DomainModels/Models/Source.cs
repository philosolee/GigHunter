using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GigHunter.DomainModels.Models
{
	public class Source : IModel
	{
		[BsonId]
		public ObjectId Id { get; set; }

		public string Name { get; set; }

		public string BaseUrl { get; set; }

		public string ClientId { get; set; }

		public string ClientSecret { get; set; }

		public string ApiToken { get; set; }

		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime TokenExpiryDate { get; set; } 
	}
}
