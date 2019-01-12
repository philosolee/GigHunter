using System.Collections.Generic;
using GigHunter.DomainModels.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GigHunter.DomainModels.Repositories
{
	public interface IRepository<T> where T : EntityBase
	{
		void Add(T entity);

		List<T> GetAll();

		T GetById(ObjectId id);

		List<T> GetByName(string name);

		bool Exists(ObjectId id);

		bool UpdateById(ObjectId id, T updatedItem);

		bool DeleteById(ObjectId id);

		UpdateDefinition<T> EntityUpdateDefinition(T updatedEntity);
	}
}
