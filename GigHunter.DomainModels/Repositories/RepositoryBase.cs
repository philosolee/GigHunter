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

		public virtual T GetById(string id)
		{
			var result = MongoCollection.Find(Filter<T>.Id(new ObjectId(id))).ToListAsync().Result;

			return result.FirstOrDefault(x => x.Id == id);

		}

		public virtual bool Exists(string id)
		{
			return MongoCollection.CountDocuments(Filter<T>.Id(new ObjectId(id))) != 0;
		}

		public virtual List<T> GetByName(string sourceName)
		{
			return MongoCollection.Find(Filter<T>.Name(sourceName)).ToListAsync().Result;
		}

		public virtual bool UpdateById(string id, T updatedEntity)
		{
			var updateDefinition = EntityUpdateDefinition(updatedEntity);
			var result = MongoCollection.UpdateOne(Filter<T>.Id(new ObjectId(id)), updateDefinition);
			return result.ModifiedCount != 0;
		}

		public abstract UpdateDefinition<T> EntityUpdateDefinition(T updatedEntity);

		public virtual bool DeleteById(string id)
		{
			var result = MongoCollection.DeleteOne(Filter<T>.Id(new ObjectId(id)));
			return result.DeletedCount == 1;
		}
	}
}
