using System.Collections.Generic;
using System.Threading.Tasks;

namespace GigHunter.DomainModels.Repositories
{
	public interface IRepository<T>
	{
		Task Add(T item);

		Task<List<T>> GetById(string id);

		Task<List<T>> GetAll();

		bool UpdateById(string id, T updatedItem);

		long DeleteById(string id);
	}
}
