using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices
{
	public interface IVillaService
	{
		Task<T> GetAsync<T>(int id);
		Task<T> GetAllAsync<T>();
	
		Task<T> CreateAsync<T>(VillaCreateDTO Villadto);
		Task<T> UpdateAsync<T>(VillaUpdateDTO Villadto);
		Task<T> DeleteAsync<T>(int id);

	}
}
