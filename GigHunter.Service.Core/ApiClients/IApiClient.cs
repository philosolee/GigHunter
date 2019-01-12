using System.Collections.Generic;
using GigHunter.DomainModels.Models;

namespace GigHunter.Service.Core.ApiClients
{
	public interface IApiClient
	{
		void Authenticate();

		List<Gig> GigsForArtist(string artistName);
	}
}