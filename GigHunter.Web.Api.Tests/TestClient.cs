using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using GigHunter.TestUtilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace GigHunter.Web.Api.Tests
{
	public class TestClient
	{
		private readonly WebApplicationFactory<Startup> _factory = new WebApplicationFactory<Startup>();

		public TestClient()
		{
		}

		public HttpResponseMessage Get(string url)
		{;
			using (var client = GetClient())
			{
				return client.GetAsync(url).Result;
			}
		}
		
		private HttpClient GetClient()
		{
			var appSettings = Path.GetFullPath("appsettings.json");
			return _factory.WithWebHostBuilder(b => b.ConfigureAppConfiguration(c => c.AddJsonFile(appSettings)))
				.CreateClient();
		}
	}
}
