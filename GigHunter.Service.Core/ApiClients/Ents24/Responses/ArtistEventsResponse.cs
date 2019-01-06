using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigHunter.Service.Core.ApiClients.Ents24.Responses
{
	public class ArtistEventResponse
	{
		public string Id { get; set; }
		public string Headline { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string StartTimeString { get; set; }
		public VenueResponse Venue { get; set; }
		public string WebLink { get; set; }
	}
}
