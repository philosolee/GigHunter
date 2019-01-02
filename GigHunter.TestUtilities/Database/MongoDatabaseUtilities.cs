using MongoDB.Driver;
using GigHunter.DomainModels;
using MongoDB.Bson;
using System.Collections.Generic;

namespace GigHunter.TestUtilities.Database
{
	public class MongoDatabaseUtilities<T>
	{
		public IMongoDatabase mongoDatabase;

		private static string _connectionString => Properties.Settings.Default.ConnectionString;
		private static string _databaseName => Properties.Settings.Default.Database;
		private readonly string _collectionName;

		public MongoDatabaseUtilities(string collectionName)
		{
			var client = new MongoClient(_connectionString);
			mongoDatabase = client.GetDatabase(_databaseName);
			_collectionName = collectionName;
		}

		public List<T> FindRecordById(ObjectId id)
		{
			var filter = Builders<T>.Filter.Eq("Id", id);
			var collection = mongoDatabase.GetCollection<T>(_collectionName);
			return collection.Find(filter).ToListAsync().Result;
		}

		public long CountRecordsInCollection()
		{
			var collection = mongoDatabase.GetCollection<T>(_collectionName);

			var filter = Builders<T>.Filter.Empty;
			return collection.CountDocuments(filter);				
		}

		public void RemoveCollection()
		{
			mongoDatabase.DropCollection(_collectionName); 
		}
	}
}
