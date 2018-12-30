using MongoDB.Driver;
using GigHunter.DomainModels;
using MongoDB.Bson;
using System.Collections.Generic;

namespace GigHunter.DomainModel.Tests.Utilities
{
	internal class MongoDatabaseUtilities
	{
		private static string _connectionString => RepositoryBase.ConnectionString;
		private static string _databaseName => RepositoryBase.DatabaseName;
		public IMongoDatabase mongoDatabase;

		private MongoDatabaseUtilities()
		{
			var client = new MongoClient(_connectionString);
			mongoDatabase = client.GetDatabase(_databaseName);
		}

		public static MongoDatabaseUtilities New()
		{
			return new MongoDatabaseUtilities();
		}

		public List<T> FindRecordById<T>(ObjectId id, string CollectionName)
		{
			var filter = Builders<T>.Filter.Eq("Id", id);
			var collection = mongoDatabase.GetCollection<T>(CollectionName);
			return collection.Find(filter).ToListAsync().Result;
		}

		public long CountRecordsInCollection<T>(string collectionName)
		{
			var collection = mongoDatabase.GetCollection<T>(collectionName);

			var filter = Builders<T>.Filter.Empty;
			return collection.CountDocuments(filter);				
		}

		public void RemoveCollection<T>(string CollectionName)
		{
			mongoDatabase.DropCollection(CollectionName); 
		}
	}
}
