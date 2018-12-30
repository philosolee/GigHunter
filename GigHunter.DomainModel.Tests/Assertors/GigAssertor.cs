using GigHunter.DomainModels.Models;
using NUnit.Framework;
using System;

namespace GigHunter.DomainModel.Tests.Assertors
{
	public class GigAssertor
	{
		private Gig _expected;
		private Gig _actual;

		public static GigAssertor New()
		{
			return new GigAssertor();
		}

		public GigAssertor Expected(Gig expected)
		{
			_expected = expected;
			return this;
		}

		public GigAssertor Actual(Gig actual)
		{
			_actual = actual;
			return this;
		}

		public void DoAssert()
		{
			Assert.AreEqual(_expected.Id, _actual.Id);
			Assert.AreEqual(_expected.Artist, _actual.Artist);
			Assert.AreEqual(_expected.Venue, _actual.Venue);
			Assert.AreEqual(_expected.Date, _actual.Date);
			Assert.AreEqual(_expected.TicketUri, _actual.TicketUri);
		}
	}
}
