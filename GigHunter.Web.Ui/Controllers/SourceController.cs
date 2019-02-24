using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigHunter.DomainModels.Models;
using GigHunter.Web.Ui.Core.ApiRequests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
			return View();
		}
	}
}