using GigHunter.Web.Ui.Core.ApiRequests;
using Microsoft.AspNetCore.Mvc;
using GigHunter.DomainModels.Models;

namespace GigHunter.Web.Ui.Controllers
{
	public class SourceController : Controller
	{
		public IActionResult Source()
		{
			var sourceRequest = new SourceRequests();
			ViewBag.Sources = sourceRequest.GetSources();
			return View();
		}

		[HttpPost]
		public IActionResult Source(Source source)
		{
			var sourceRequest = new SourceRequests();
			sourceRequest.AddSource(source);
			ViewBag.Sources = sourceRequest.GetSources();
			return View();
		}
	}
}