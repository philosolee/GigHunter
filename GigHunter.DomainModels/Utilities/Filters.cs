using MongoDB.Bson;
using MongoDB.Driver;

namespace GigHunter.DomainModels.Utilities
{
	internal class Filter<T>
	{
		public static FilterDefinition<T> Empty => Builders<T>.Filter.Empty;

		public static FilterDefinition<T> IdAsString(string id)
		{
			return Builders<T>.Filter.Eq("Id", IdAsObjectId(id));
		}

		public static FilterDefinition<T> Id(ObjectId id)
		{
			return Builders<T>.Filter.Eq("Id", id);
		}

		private static ObjectId IdAsObjectId(string id)
		{
			return new ObjectId(id);
		}
	}
}
