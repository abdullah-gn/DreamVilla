using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
	public class AuthService :  BaseService , IAuthService
	{

		private readonly IHttpClientFactory httpClient;
		private string url;
		public AuthService(IHttpClientFactory _HttpClient, IConfiguration Config) : base(_HttpClient)
		{
			httpClient = _HttpClient;
			url = Config.GetValue<string>("ServiceUrl:VillaApi");
		}

		public Task<T> LoginAsync<T>(LoginRequestDto obj)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.POST,
				ApiUrl = url + $"/api/{SD.ApiVersion}/UserAuth/login",
				Data = obj

			});
		}

		public Task<T> RegisterAsync<T>(RegisterDto obj)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.POST,
				ApiUrl = url + $"/api/{SD.ApiVersion}/UserAuth/register",
				Data = obj

			});
		}



	}
}
