using DreamVilla_VillaApi.Models;
using System.Linq.Expressions;

namespace DreamVilla_VillaApi.Repository.IRepository
{
	public interface IVillaRepository:IRepository<Villa>
	{
		Task <Villa> updateAsync(Villa VillaEntity);

	}
}
