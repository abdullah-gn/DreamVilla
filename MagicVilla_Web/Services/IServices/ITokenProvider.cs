using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices
{
	public interface ITokenProvider
	{
		TokenDto GetToken();
		void SetToken(TokenDto tokenDto);
		void ClearToken();
	}
}
