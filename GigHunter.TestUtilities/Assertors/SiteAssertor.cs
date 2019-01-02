using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Repositories;
using NUnit.Framework;


namespace GigHunter.TestUtilities.Assertors
{
	public class SiteAssertor : IModelAssertor
	{
		private Source _actual;
		private Source _expected;

		public static IModelAssertor New()
		{
			return new SiteAssertor();
		}

		public IModelAssertor Actual(IEntity actual)
		{
			_actual = (Source)actual;
			return this;
		}

		public IModelAssertor Expected(IEntity expected)
		{
			_expected = (Source)expected;
			return this;
		}

		public void DoAssert()
		{
			Assert.AreEqual(_expected.Id, _actual.Id);
			Assert.AreEqual(_expected.Name, _actual.Name);
			Assert.AreEqual(_expected.BaseUrl, _actual.BaseUrl);
		}
	}
}
