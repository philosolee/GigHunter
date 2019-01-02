using GigHunter.DomainModels.Repositories;

namespace GigHunter.TestUtilities.Assertors
{
	public interface IModelAssertor
	{
		IModelAssertor Expected(IEntity expected);
		IModelAssertor Actual(IEntity actual);
		void DoAssert();
	}
}
