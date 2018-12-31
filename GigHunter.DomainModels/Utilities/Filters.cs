using System;
using GigHunter.DomainModels.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GigHunter.DomainModels.Utilities
{
	internal class Filter<T>
	{
		internal static FilterDefinition<T> Empty => Builders<T>.Filter.Empty;

		internal static FilterDefinition<T> IdAsString(string id)
		{
			return Builders<T>.Filter.Eq("Id", IdAsObjectId(id));
		}

		internal static FilterDefinition<T> Id(ObjectId id)
		{
			return Builders<T>.Filter.Eq("Id", id);
		}

		internal static FilterDefinition<T> Name(string name)
		{
			return Builders<T>.Filter.Eq("Name", name);
		}

		private static ObjectId IdAsObjectId(string id)
		{
			return new ObjectId(id);
		}
	}
}
