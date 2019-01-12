using MongoDB.Bson;

namespace GigHunter.DomainModels.Utilities
{
	public class Converters
	{
		public static ObjectId ToObjectId(string id)
		{
			return new ObjectId(id);
		}
	}
}
