using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigHunter.Service.Core.ApiClients;
using GigHunter.Service.Core.ApiClients.Ents24;
using GigHunter.Service.Core.Wrappers;
using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Repositories;
using RestSharp;

namespace GigHunter.Service.Core
{
	public class SiteCrawler
	{
		private List<Source> _sitesToCheck;
		private List<Artist> _artistsToCheck;

		private readonly IRepository<Source> _sourceRepository;
		private readonly IRepository<Artist> _artistRepository;
		private readonly IRepository<Gig> _gigRepository;

		public SiteCrawler(IRepository<Artist> artistRepository, IRepository<Source> sourceRepository, IRepository<Gig> gigRepository)
		{
			_sourceRepository = sourceRepository;
			_artistRepository = artistRepository;
			_gigRepository = gigRepository;
			GetSitesToCheck();
			GetArtistsToCheck();
		}

		public void CrawlSites()
		{
			foreach (var site in _sitesToCheck)
			{
				var apiClient = GetApiClientFor(site);
				var gigsFromSource = CheckAllArtists(apiClient);
				UpdateDatabaseWithGigs(gigsFromSource);
			}
		}

		private List<Gig> CheckAllArtists(IApiClient apiClient)
		{
			var gigs = new List<Gig>();
			foreach (var artist in _artistsToCheck)
			{
				gigs.AddRange(apiClient.GigsForArtist(artist.Name));
			}
			return gigs;
		}

		private Ents24Client GetApiClientFor(Source site)
		{
			var restClient = new RestClientWrapper(new RestClient());
			return new Ents24Client(site, restClient);
		}

		private void GetSitesToCheck()
		{
			_sitesToCheck = _sourceRepository.GetAll();
		}

		private void GetArtistsToCheck()
		{
			_artistsToCheck = _artistRepository.GetAll();
		}

		private void UpdateDatabaseWithGigs(List<Gig> gigsFromSource)
		{
			var updateTask = new Task(() =>
			{
				foreach (var gig in gigsFromSource)
				{
					var currentGigsForArtist = _gigRepository.GetByName(gig.Artist);

					if (currentGigsForArtist.Contains(gig))
					{
						currentGigsForArtist.First(g => g == gig).TicketUrls.AddRange(gig.TicketUrls);
					}
					else
					{
						_gigRepository.Add(gig);
					}
				}
			});
			updateTask.Start();
		}

	}
}
