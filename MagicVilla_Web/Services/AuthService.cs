using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
	public class AuthService : IAuthService
	{

		private readonly IHttpClientFactory httpClient;
		private string url;
		private readonly IBaseService _baseService;
		public AuthService(IHttpClientFactory _HttpClient, IConfiguration Config, IBaseService baseService)
		{
			httpClient = _HttpClient;
			url = Config.GetValue<string>("ServiceUrl:VillaApi");
			_baseService = baseService;
		}

		public async Task<T> LoginAsync<T>(LoginRequestDto obj)
		{
			return await _baseService.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.POST,
				ApiUrl = url + $"/api/{SD.ApiVersion}/UserAuth/login",
				Data = obj

			},withBearer:false);
		}

		public async Task<T> RegisterAsync<T>(RegisterDto obj)
		{
			return await _baseService.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.POST,
				ApiUrl = url + $"/api/{SD.ApiVersion}/UserAuth/register",
				Data = obj

			}, withBearer: false);
		}



	}
}
