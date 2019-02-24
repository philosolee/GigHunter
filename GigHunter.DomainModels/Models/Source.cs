using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace GigHunter.DomainModels.Models
{
	public class Source : EntityBase
	{
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "baseUrl")]
		public string BaseUrl { get; set; }

		[JsonProperty(PropertyName = "clientId")]
		public string ClientId { get; set; }

		[JsonProperty(PropertyName = "clientSecret")]
		public string ClientSecret { get; set; }

		[JsonProperty(PropertyName = "apiToken")]
		public string ApiToken { get; set; }

		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		[JsonProperty(PropertyName = "tokenExpiryDate")]
		public DateTime TokenExpiryDate { get; set; } 
	}
}
