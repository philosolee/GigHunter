using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using GigHunter.DomainModels;

namespace GigHunter.TestUtilities.Database
{
	public class MongoDatabaseUtilities<T>
	{
		public IMongoDatabase mongoDatabase;

		private readonly string _collectionName;

		public MongoDatabaseUtilities(string collectionName)
		{
			var client = new MongoClient(Settings.GetInstance().MongoServer);
			mongoDatabase = client.GetDatabase(Settings.GetInstance().MongoDatabase);
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
