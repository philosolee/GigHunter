using GigHunter.TestUtilities;
using NUnit.Framework;

namespace GigHunter.Service.Core.Tests
{
	[SetUpFixture]
	public class Hooks
	{
		[OneTimeSetUp]
		public void ConfigureDatabase()
		{
			TestConfiguration.SetDatabaseFromConfig();
		}
	}
}
