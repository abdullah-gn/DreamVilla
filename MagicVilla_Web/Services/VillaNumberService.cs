using MagicVilla_Web.Models.Dto;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using static MagicVilla_Utility.SD;
using Newtonsoft.Json.Linq;

namespace MagicVilla_Web.Services
{
    public class VillaNumberService : IVillaNumberService
	{
		private readonly IHttpClientFactory httpClient;
		private string url;
		private readonly IBaseService _baseService;
		public VillaNumberService(IHttpClientFactory _HttpClient, IConfiguration Config, IBaseService baseService)
		{
			httpClient = _HttpClient;
			url = Config.GetValue<string>("ServiceUrl:VillaApi");
			_baseService = baseService;
		}

		public async Task<T> CreateAsync<T>(VillaNumberCreateDTO Villadto)
		{
			return await _baseService.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.POST,
				ApiUrl = url+ $"/api/{SD.ApiVersion}/VillaNumberApi",
				Data = Villadto
            });
		}

		public async Task<T> DeleteAsync<T>(int id)
		{
			return await _baseService.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.DELETE,
				ApiUrl = url + $"/api/{SD.ApiVersion}/VillaNumberApi/" + id,
				Data = null
            });
		}

		public Task<T> GetAllAsync<T>()
		{
			return _baseService.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.GET,
				ApiUrl = url + $"/api/{SD.ApiVersion}/VillaNumberApi"          
            });
		}

		public async Task<T> GetAsync<T>(int id)
        {
			return await _baseService.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.GET,
				ApiUrl = url + $"/api/{SD.ApiVersion}/VillaNumberApi/" + id
			});
		}

		public async Task<T> UpdateAsync<T>(VillaNumberUpdateDTO Villadto)
		{
			return await _baseService.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.PUT,
				ApiUrl = url + $"/api/{SD.ApiVersion}/VillaNumberApi/" + Villadto.VillaNo,
				Data = Villadto
			});
		}
	}
}
