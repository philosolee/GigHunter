using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using GigHunter.DomainModels;
using Microsoft.Extensions.Configuration;

namespace GigHunter.TestUtilities
{
	public class TestConfiguration
	{
		public static void SetDatabaseFromConfig()
		{
			var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

			Settings.GetInstance().MongoServer= config.GetConnectionString("MongoServer");
			Settings.GetInstance().MongoDatabase = config.GetConnectionString("MongoDatabase");
		}
	}
}
