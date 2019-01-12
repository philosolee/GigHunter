using GigHunter.TestUtilities;
using NUnit.Framework;

namespace GigHunter.DomainModels.Tests
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
