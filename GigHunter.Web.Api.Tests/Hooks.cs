using System;
using System.Collections.Generic;
using System.Text;
using GigHunter.TestUtilities;
using GigHunter.TestUtilities.Database;
using NUnit.Framework;

namespace GigHunter.Web.Api.Tests
{
	[SetUpFixture]
	public class Hooks
	{
		[OneTimeSetUp]
		public void ConfigureDatabase()
		{
			TestConfiguration.SetDatabaseFromConfig();
		}

		[OneTimeTearDown]
		public static void DisposeTestClient()
		{
		}
	}
}
