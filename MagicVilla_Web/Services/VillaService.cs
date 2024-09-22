using MagicVilla_Web.Models.Dto;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using static MagicVilla_Utility.SD;

namespace MagicVilla_Web.Services
{
    public class VillaService : BaseService, IVillaService
	{
		private readonly IHttpClientFactory httpClient;
		private string url;
        public VillaService(IHttpClientFactory _HttpClient , IConfiguration Config):base(_HttpClient)
        {
			httpClient = _HttpClient;
			url = Config.GetValue<string>("ServiceUrl:VillaApi");
        }

		public Task<T> CreateAsync<T>(VillaCreateDTO Villadto, string token)
		{
			return  SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.POST,
				ApiUrl = url+ $"/api/{SD.ApiVersion}/VillaApi",
				Data = Villadto,
                token = token,
				ContentType = SD.ContentType.MultipartFormData
            });
		}

		public Task<T> DeleteAsync<T>(int id, string token)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.DELETE,
				ApiUrl = url + $"/api/{SD.ApiVersion}/VillaApi/" + id,
				Data = null,
                token = token,
            });
		}

		public Task<T> GetAllAsync<T>(string token)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.GET,
				ApiUrl = url + $"/api/{SD.ApiVersion}/VillaApi",
                token = token
            });
		}

		public Task<T> GetAsync<T>(int id, string token)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.GET,
				ApiUrl = url + $"/api/{SD.ApiVersion}/VillaApi/" + id,
				token = token
			});
		}

		public Task<T> UpdateAsync<T>(VillaUpdateDTO Villadto, string token)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.PUT,
				ApiUrl = url + $"/api/{SD.ApiVersion}/VillaApi/" + Villadto.Id,
				Data = Villadto,
				token = token,
                ContentType = SD.ContentType.MultipartFormData
			});
		}
	}
}
