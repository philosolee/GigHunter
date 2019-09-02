using MongoDB.Bson;

namespace GigHunter.TestUtilities.DataGenerators
{
	public class IdGenerator
	{
		public static ObjectId NewMongoId()
		{
			return new ObjectId();
		}
	}
}
