using DreamVilla_VillaApi.Models;
using DreamVilla_VillaApi.Models.Dto;

namespace DreamVilla_VillaApi.Repository.IRepository
{
	public interface ILocalUserRepository
	{
		bool IsUniqueUser(string username);

		Task<AppUserDto> Register(RegisterDto registeruserDto);

		Task<TokenDto> Login(LoginRequestDto loginuserDto);

	}
}
