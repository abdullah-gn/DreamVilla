using DreamVilla_VillaApi.Models;
using System.Linq.Expressions;

namespace DreamVilla_VillaApi.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
		Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool isTracked = true);

		Task AddAsync(T VillaEntity);
		Task RemoveAsync(T VillaEntity);
		Task SaveAsync();
	}
}
