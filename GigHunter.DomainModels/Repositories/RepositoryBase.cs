using System.Collections.Generic;
using System.Threading.Tasks;
using GigHunter.DomainModels.Repositories;
using GigHunter.DomainModels.Utilities;
using MongoDB.Bson;
using MongoDB.Driver;


namespace GigHunter.DomainModels
{
	public abstract class RepositoryBase<T> : IRepository<T> where T : EntityBase
	{
		public static string ConnectionString => Properties.Settings.Default.ConnectionString;
		public static string DatabaseName => Properties.Settings.Default.Database;
		public readonly IMongoCollection<T> MongoCollection;
		private IMongoDatabase mongoDatabase;

		public RepositoryBase(string collectionName)
		{
			var client = new MongoClient(ConnectionString);
			mongoDatabase = client.GetDatabase(DatabaseName);
			MongoCollection = mongoDatabase.GetCollection<T>(collectionName);
		}

		public async Task Add(T Entity)
		{
			await MongoCollection.InsertOneAsync(Entity);
		}

		public async Task<List<T>> GetAll()
		{
			return await MongoCollection.Find(Filter<T>.Empty).ToListAsync();
		}

		public async Task<List<T>> GetById(ObjectId id)
		{
			return await MongoCollection.Find(Filter<T>.Id(id)).ToListAsync();
		}
		
		public bool UpdateById(ObjectId id, T updatedEntity)
		{
			var updateDefinition = EntityUpdateDefinition(updatedEntity);
			var result = MongoCollection.UpdateOne(Filter<T>.Id(id), updateDefinition);
			return result.ModifiedCount != 0;
		}

		public abstract UpdateDefinition<T> EntityUpdateDefinition(T updatedEntity);

		public bool DeleteById(ObjectId id)
		{
			var result = MongoCollection.DeleteOne(Filter<T>.Id(id));
			return result.DeletedCount == 1;
		}
	}
}
