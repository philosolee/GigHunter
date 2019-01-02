using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Repositories;
using NUnit.Framework;

namespace GigHunter.TestUtilities.Assertors
{
	public class ArtistAssertor : IModelAssertor
	{
		private Artist _actual;
		private Artist _expected;

		public static IModelAssertor New()
		{
			return new ArtistAssertor();
		}

		public IModelAssertor Actual(IEntity actual)
		{
			_actual = (Artist)actual;
			return this;
		}

		public IModelAssertor Expected(IEntity expected)
		{
			_expected = (Artist)expected;
			return this;
		}

		public void DoAssert()
		{
			Assert.AreEqual(_expected.Id, _actual.Id);
			Assert.AreEqual(_expected.Name, _actual.Name);
			Assert.AreEqual(_expected.LastSearchedForDate, _actual.LastSearchedForDate);
		}

	}
}
