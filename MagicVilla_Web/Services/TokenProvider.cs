using MagicVilla_Utility;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        void ITokenProvider.ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(SD.AccessToken);
            _contextAccessor.HttpContext?.Response.Cookies.Delete(SD.RefreshToken);
		}

        public TokenDto GetToken()
        {
            try
            {
                bool hasAccessToken = _contextAccessor.HttpContext.Request.Cookies.TryGetValue(SD.AccessToken, out string accessToken);
                bool hasRefreshToken = _contextAccessor.HttpContext.Request.Cookies.TryGetValue(SD.RefreshToken, out string refreshToken);
				if (hasAccessToken)
                {
                    return new TokenDto
                  {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                  };
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        void ITokenProvider.SetToken(TokenDto tokenDto)
        {
            var cookiesOptions = new CookieOptions { Expires = DateTime.UtcNow.AddDays(30) };
            _contextAccessor.HttpContext?.Response.Cookies.Append(SD.AccessToken, tokenDto.AccessToken, cookiesOptions);
            _contextAccessor.HttpContext?.Response.Cookies.Append(SD.RefreshToken, tokenDto.RefreshToken, cookiesOptions);
		}
    }
}
