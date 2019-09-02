using System;
using GigHunter.DomainModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GigHunter.Web.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			var settings = Settings.GetInstance();
			settings.MongoServer = Configuration.GetConnectionString("MongoServer");
			settings.MongoDatabase = Configuration.GetConnectionString("MongoDatabase");

			Console.WriteLine(settings.MongoDatabase);
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
