using GigHunter.DomainModels.Models;
using NUnit.Framework;

namespace GigHunter.TestUtilities.Assertors
{
	public class GigAssertor : IModelAssertor
	{
		private Gig _expected;
		private Gig _actual;

		public static IModelAssertor New()
		{
			return new GigAssertor();
		}

		public IModelAssertor Expected(IModel expected)
		{
			_expected = (Gig)expected;
			return this;
		}

		public IModelAssertor Actual(IModel actual)
		{
			_actual = (Gig)actual;
			return this;
		}

		public void DoAssert()
		{
			Assert.AreEqual(_expected.Id, _actual.Id);
			Assert.AreEqual(_expected.Artist, _actual.Artist);
			Assert.AreEqual(_expected.Venue, _actual.Venue);
			Assert.AreEqual(_expected.Date, _actual.Date);
			CollectionAssert.AreEquivalent(_expected.TicketUrls, _actual.TicketUrls);
		}

	}
}
