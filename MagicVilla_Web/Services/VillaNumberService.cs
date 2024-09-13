using MagicVilla_Web.Models.Dto;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using static MagicVilla_Utility.SD;
using Newtonsoft.Json.Linq;

namespace MagicVilla_Web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
	{
		private readonly IHttpClientFactory httpClient;
		private string url;
        public VillaNumberService(IHttpClientFactory _HttpClient , IConfiguration Config):base(_HttpClient)
        {
			httpClient = _HttpClient; 
			url = Config.GetValue<string>("ServiceUrl:VillaApi");
        }

		public Task<T> CreateAsync<T>(VillaNumberCreateDTO Villadto , string token)
		{
			return  SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.POST,
				ApiUrl = url+ $"/api/{SD.ApiVersion}/VillaNumberApi",
				Data = Villadto,
                token = token

            });
		}

		public Task<T> DeleteAsync<T>(int id, string token)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.DELETE,
				ApiUrl = url + $"/api/{SD.ApiVersion}/VillaNumberApi/" + id,
				Data = null,
                token = token
            });
		}

		public Task<T> GetAllAsync<T>(string token)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.GET,
				ApiUrl = url + $"/api/{SD.ApiVersion}/VillaNumberApi",
                token = token
            });
		}

		public Task<T> GetAsync<T>(int id, string token)
        {
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.GET,
				ApiUrl = url + $"/api/{SD.ApiVersion}/VillaNumberApi/" + id,
				token= token

			});
		}

		public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO Villadto, string token)
		{
			return SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.PUT,
				ApiUrl = url + $"/api/{SD.ApiVersion}/VillaNumberApi/" + Villadto.VillaNo,
				Data = Villadto,
				token = token

			});
		}
	}
}
