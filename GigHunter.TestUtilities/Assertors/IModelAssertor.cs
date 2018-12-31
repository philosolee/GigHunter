using GigHunter.DomainModels.Models;

namespace GigHunter.TestUtilities.Assertors
{
	public interface IModelAssertor
	{
		IModelAssertor Expected(IModel expected);
		IModelAssertor Actual(IModel actual);
		void DoAssert();
	}
}
