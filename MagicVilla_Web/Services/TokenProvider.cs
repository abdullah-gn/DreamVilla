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
        }

        public TokenDto GetToken()
        {
            try
            {
                bool hasAccessToken = _contextAccessor.HttpContext.Request.Cookies.TryGetValue(SD.AccessToken, out string accessToken);
                if (hasAccessToken)
                {
                    return new TokenDto { AccessToken = accessToken };
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
            var cookiesOptions = new CookieOptions { Expires = DateTime.UtcNow.AddDays(15) };
            _contextAccessor.HttpContext?.Response.Cookies.Append(SD.AccessToken, tokenDto.AccessToken, cookiesOptions);
        }
    }
}
