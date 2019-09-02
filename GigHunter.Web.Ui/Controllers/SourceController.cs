using GigHunter.Web.Ui.Core.ApiRequests;
using Microsoft.AspNetCore.Mvc;
using GigHunter.DomainModels.Models;
using Microsoft.Extensions.Configuration;

namespace GigHunter.Web.Ui.Controllers
{
	public class SourceController : Controller
	{
		private readonly string _apiUri;

		public SourceController(IConfiguration configuration)
		{
			_apiUri = configuration.GetSection("Uris").GetSection("GigHunterApi").Value;
		}

		public IActionResult Source()
		{
			var sourceRequest = new SourceRequests(_apiUri);
			ViewBag.Sources = sourceRequest.GetSources();
			return View();
		}

		[HttpPost]
		public IActionResult Source(Source source)
		{
			var sourceRequest = new SourceRequests(_apiUri);
			sourceRequest.AddSource(source);
			ViewBag.Sources = sourceRequest.GetSources();
			return View();
		}
	}
}