using MagicVilla_Web.Models.Dto;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using static MagicVilla_Utility.SD;

namespace MagicVilla_Web.Services
{
    public class VillaService : IVillaService
	{
		private readonly IHttpClientFactory httpClient;
		private string url;
		private readonly IBaseService _baseService;
		public VillaService(IHttpClientFactory _HttpClient, IConfiguration Config, IBaseService baseService)
		{
			httpClient = _HttpClient;
			url = Config.GetValue<string>("ServiceUrl:VillaApi");
			_baseService = baseService;
		}

		public async Task<T> CreateAsync<T>(VillaCreateDTO Villadto)
		{
			return  await _baseService.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.POST,
				ApiUrl = url+ $"/api/{SD.ApiVersion}/VillaApi",
				Data = Villadto,
				ContentType = SD.ContentType.MultipartFormData
            });
		}

		public async Task<T> DeleteAsync<T>(int id)
		{
			return await  _baseService.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.DELETE,
				ApiUrl = url + $"/api/{SD.ApiVersion}/VillaApi/" + id,
				Data = null
            });
		}

		public async Task<T> GetAllAsync<T>()
		{
			return await _baseService.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.GET,
				ApiUrl = url + $"/api/{SD.ApiVersion}/VillaApi"
            });
		}

		public async Task<T> GetAsync<T>(int id)
		{
			return await _baseService.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.GET,
				ApiUrl = url + $"/api/{SD.ApiVersion}/VillaApi/" + id
			});
		}

		public async Task<T> UpdateAsync<T>(VillaUpdateDTO Villadto)
		{
			return await _baseService.SendAsync<T>(new ApiRequest()
			{
				ApiType = SD.ApiType.PUT,
				ApiUrl = url + $"/api/{SD.ApiVersion}/VillaApi/" + Villadto.Id,
				Data = Villadto,
                ContentType = SD.ContentType.MultipartFormData
			});
		}
	}
}
