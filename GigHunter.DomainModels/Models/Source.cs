using System;
using MongoDB.Bson.Serialization.Attributes;

namespace GigHunter.DomainModels.Models
{
	public class Source : EntityBase
	{
		public string Name { get; set; }

		public string BaseUrl { get; set; }

		public string ClientId { get; set; }

		public string ClientSecret { get; set; }

		public string ApiToken { get; set; }

		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime TokenExpiryDate { get; set; }

		public string ToJson()
		{
			return $"{{\"id\":\"{Id}\", " +
			       $"\"name\":\"{Name}\"," +
			       $"\"baseUrl\":\"{BaseUrl}\", " +
			       $"\"clientId\":\"{ClientId}\", " +
			       $"\"clientSecret\": \"{ClientSecret}\", " +
			       $"\"apiToken\": \"{ApiToken}\", " +
			       $"\"tokenExpiryDate\": \"{TokenExpiryDate.ToString("s")}\"}}";
		}
	}
}
