using DreamVilla_VillaApi.Models;
using DreamVilla_VillaApi.Models.Dto;

namespace DreamVilla_VillaApi.Repository.IRepository
{
	public interface IUserRepository
	{
		bool IsUniqueUser(string username);
		Task<AppUserDto> Register(RegisterDto registeruserDto);
		Task<TokenDto> Login(LoginRequestDto loginuserDto);
		Task<TokenDto> RefreshAccessToken(TokenDto tokenDto);
		Task RevokeRefreshToken(TokenDto tokenDto);
	}
}
