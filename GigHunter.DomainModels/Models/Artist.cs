using System;
using MongoDB.Bson.Serialization.Attributes;

namespace GigHunter.DomainModels.Models
{
	public class Artist : EntityBase
	{
		public string Name { get; set; }

		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime LastSearchedForDate { get; set; }
	}
}
