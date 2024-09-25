using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices
{
	public interface IVillaNumberService
	{
		Task<T> GetAsync<T>(int id);
		Task<T> GetAllAsync<T>();
	
		Task<T> CreateAsync<T>(VillaNumberCreateDTO Villadto);
		Task<T> UpdateAsync<T>(VillaNumberUpdateDTO Villadto);
		Task<T> DeleteAsync<T>(int id);

	}
}
