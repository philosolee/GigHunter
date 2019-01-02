using System;
using System.Collections.Generic;
using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Repositories;

namespace GigHunter.Service.Core
{
	public class SiteCrawler
	{
		private List<Source> _sitesToCheck;
		private List<Artist> _artistsToCheck;

		private readonly SourceRepository _siteRepository;
		private readonly ArtistRepository _artistRepository;

		public SiteCrawler(SourceRepository siteRepository, ArtistRepository artistRepository)
		{
			_siteRepository = siteRepository;
			_artistRepository = artistRepository;
			GetSitesToCheck();
			GetArtistsToCheck();
		}

		public void CrawlSites()
		{
			foreach (var site in _sitesToCheck)
			{
				
			}
		}

		private void GetSitesToCheck()
		{
			throw new NotImplementedException();
		}

		private void GetArtistsToCheck()
		{
			throw new NotImplementedException();
		}
	}
}
