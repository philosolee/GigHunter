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

		T GetById(string id);

		List<T> GetByName(string name);

		bool Exists(string id);

		bool UpdateById(string id, T updatedItem);

		bool DeleteById(string id);

		UpdateDefinition<T> EntityUpdateDefinition(T updatedEntity);
	}
}
