using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GigHunter.Web.Api.Tests
{
	public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
	{
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			var f = builder.ConfigureAppConfiguration(c => c.AddJsonFile("appsetting.json"));
		}
	}
}
