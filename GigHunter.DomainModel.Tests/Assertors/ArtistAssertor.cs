using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GigHunter.DomainModels.Models;
using NUnit.Framework;

namespace GigHunter.DomainModel.Tests.Assertors
{
	public class ArtistAssertor : IModelAssertor
	{
		private Artist _actual;
		private Artist _expected;

		public static IModelAssertor New()
		{
			return new ArtistAssertor();
		}

		public IModelAssertor Actual(IModel actual)
		{
			_actual = (Artist)actual;
			return this;
		}

		public IModelAssertor Expected(IModel expected)
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
