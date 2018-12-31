using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GigHunter.DomainModels.Models;
using NUnit.Framework;


namespace GigHunter.TestUtilities.Assertors
{
	public class SiteAssertor : IModelAssertor
	{
		private Site _actual;
		private Site _expected;

		public static IModelAssertor New()
		{
			return new SiteAssertor();
		}

		public IModelAssertor Actual(IModel actual)
		{
			_actual = (Site)actual;
			return this;
		}

		public IModelAssertor Expected(IModel expected)
		{
			_expected = (Site)expected;
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
