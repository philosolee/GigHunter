using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace GigHunter.DomainModels.Repositories
{
	public interface IRepository<T>
	{
		Task Add(T item);

		Task<List<T>> GetAll();

		Task<List<T>> GetById(ObjectId id);

		Task<List<T>> GetById(string id);

		bool UpdateById(ObjectId id, T updatedItem);

		bool UpdateById(string id, T updatedItem);

		bool DeleteById(ObjectId id);

		bool DeleteById(string id);
	}
}
