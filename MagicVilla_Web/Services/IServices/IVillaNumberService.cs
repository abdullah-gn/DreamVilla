using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices
{
	public interface IVillaNumberService
	{
		Task<T> GetAsync<T>(int id, string token);
		Task<T> GetAllAsync<T>(string token);
	
		Task<T> CreateAsync<T>(VillaNumberCreateDTO Villadto, string token);
		Task<T> UpdateAsync<T>(VillaNumberUpdateDTO Villadto, string token);
		Task<T> DeleteAsync<T>(int id, string token);

	}
}
