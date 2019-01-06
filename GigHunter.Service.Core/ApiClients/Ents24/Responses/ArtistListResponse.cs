using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigHunter.Service.Core.ApiClients.Ents24.Responses
{
	public class ArtistListResponse
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsDeceased { get; set; }
		public int UpcomingEvents { get; set; }
		public List<string> Genre { get; set; }
		public string WebLink { get; set; }
		public int FansOnEnts24 { get; set; }
		public DateTime LastUpdate { get; set; }
		public DateTime Created { get; set; }
	}
}
