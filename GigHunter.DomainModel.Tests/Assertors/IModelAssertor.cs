using GigHunter.DomainModels.Models;

namespace GigHunter.DomainModel.Tests.Assertors
{
	public interface IModelAssertor
	{
		IModelAssertor Expected(IModel expected);
		IModelAssertor Actual(IModel actual);
		void DoAssert();
	}
}
