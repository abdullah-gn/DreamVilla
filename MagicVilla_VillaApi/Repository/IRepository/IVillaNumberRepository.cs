using DreamVilla_VillaApi.Models;

namespace DreamVilla_VillaApi.Repository.IRepository
{
	public interface IVillaNumberRepository : IRepository<VillaNumber>
	{
		Task<VillaNumber> updateAsync(VillaNumber VillaNumberEntity);
	}
}
