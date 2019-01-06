using GigHunter.DomainModels.Repositories;
using GigHunter.Service.Core;

namespace GigHunter.Service.Host
{
	class Program
	{
		static void Main(string[] args)
		{
			var siteCrawler = new SiteCrawler(new ArtistRepository(), new SourceRepository(), new GigRepository());
			siteCrawler.CrawlSites();
		}
	}
}
