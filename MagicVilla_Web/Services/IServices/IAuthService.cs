using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices
{
	public interface IAuthService
	{
		Task<T> LoginAsync<T>(LoginRequestDto ObjToCreate);
		Task<T> RegisterAsync<T>(RegisterDto ObjToCreate);
		Task<T> LogoutAsync<T>(TokenDto obj);

	}
}
