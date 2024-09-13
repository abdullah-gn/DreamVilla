using DreamVilla_VillaApi.Models;
using System.Linq.Expressions;

namespace DreamVilla_VillaApi.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? IncludeProperties = null,
			int PageSize=0,int PageNumber=1);
		Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool isTracked = true, string? IncludeProperties = null);

		Task AddAsync(T VillaEntity);
		Task RemoveAsync(T VillaEntity);
		Task SaveAsync();
	}
}
