using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GigHunter.DomainModels.Repositories
{
	public interface IRepository<T> where T : EntityBase
	{
		Task Add(T item);

		Task<List<T>> GetAll();

		Task<List<T>> GetById(ObjectId id);

		bool UpdateById(ObjectId id, T updatedItem);

		bool DeleteById(ObjectId id);

		UpdateDefinition<T> EntityUpdateDefinition(T updatedEntity);
	}
}
