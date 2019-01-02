using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GigHunter.DomainModels.Models
{
	public class Gig : EntityBase
	{
		public string Artist { get; set; }

		[BsonDateTimeOptions(DateOnly = true, Kind = DateTimeKind.Local)]
		public DateTime Date { get; set; }

		public string Venue { get; set; }

		public List<string> TicketUrls { get; set; } = new List<string>();
	}
}
