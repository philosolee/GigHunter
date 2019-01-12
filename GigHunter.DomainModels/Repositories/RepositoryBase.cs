using System.Collections.Generic;
using System.Linq;
using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Utilities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GigHunter.DomainModels.Repositories
{
	public abstract class RepositoryBase<T> : IRepository<T> where T : EntityBase
	{
		public readonly IMongoCollection<T> MongoCollection;

		protected RepositoryBase(string collectionName)
		{
			var client = new MongoClient(Settings.GetInstance().MongoServer);
			var mongoDatabase = client.GetDatabase(Settings.GetInstance().MongoDatabase);
			MongoCollection = mongoDatabase.GetCollection<T>(collectionName);
		}

		public virtual void Add(T entity)
		{
			MongoCollection.InsertOneAsync(entity).Wait();
		}

		public virtual List<T> GetAll()
		{
			return MongoCollection.Find(Filter<T>.Empty).ToListAsync().Result;
		}

		public virtual T GetById(ObjectId id)
		{
			var result = MongoCollection.Find(Filter<T>.Id(id)).ToListAsync().Result;

			return result.FirstOrDefault(x => x.Id == id);

		}

		public virtual bool Exists(ObjectId id)
		{
			return MongoCollection.CountDocuments(Filter<T>.Id(id)) != 0;
		}

		public virtual List<T> GetByName(string sourceName)
		{
			return MongoCollection.Find(Filter<T>.Name(sourceName)).ToListAsync().Result;
		}

		public virtual bool UpdateById(ObjectId id, T updatedEntity)
		{
			var updateDefinition = EntityUpdateDefinition(updatedEntity);
			var result = MongoCollection.UpdateOne(Filter<T>.Id(id), updateDefinition);
			return result.ModifiedCount != 0;
		}

		public abstract UpdateDefinition<T> EntityUpdateDefinition(T updatedEntity);

		public virtual bool DeleteById(ObjectId id)
		{
			var result = MongoCollection.DeleteOne(Filter<T>.Id(id));
			return result.DeletedCount == 1;
		}
	}
}
