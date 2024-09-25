using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using static MagicVilla_Utility.SD;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        private readonly ITokenProvider _tokenProvider;
		public BaseService(IHttpClientFactory ClientFactory, ITokenProvider tokenProvider)
		{
			responseModel = new();
			httpClient = ClientFactory;
			_tokenProvider = tokenProvider;
		}
		public async Task<T> SendAsync<T>(ApiRequest ApiRequest, bool withBearer)
        {
            try
            {
                HttpClient client = httpClient.CreateClient("MagicVilla");
                HttpRequestMessage Msg = new HttpRequestMessage();
                if (ApiRequest.ContentType == ContentType.MultipartFormData)
                {
                    Msg.Headers.Add("Accept", "*/*");
                }
                else
                {
                    Msg.Headers.Add("Accept", "application/json");
                }
                if (withBearer && _tokenProvider.GetToken() != null)
                {
                    var token = _tokenProvider.GetToken();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                }

                if (ApiRequest.ContentType == ContentType.MultipartFormData)
                {
                    var content = new MultipartFormDataContent();

                    foreach (var prop in ApiRequest.Data.GetType().GetProperties())
                    {
                        var value = prop.GetValue(ApiRequest.Data);
                        if (value is FormFile)
                        {
                            var file = (FormFile)value;
                            if (file!=null)
                            {
                                content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                            }
                        }
                        else
                        {
                            content.Add(new StringContent(value == null ? "" : value.ToString()), prop.Name);
                        }
                    }
                    Msg.Content = content;
                }
                else
                {
                    if (ApiRequest.Data != null)
                    {

                        Msg.Content = new StringContent(JsonConvert.SerializeObject(ApiRequest.Data),
                            Encoding.UTF8, "application/json");
                    }

                }
                    Msg.RequestUri = new Uri(ApiRequest.ApiUrl);

                //in case of Data != null >>>> POST > PUT 

         
                switch (ApiRequest.ApiType)
                {
                    case SD.ApiType.GET:
                        Msg.Method = HttpMethod.Get;
                        break;
                    case SD.ApiType.POST:
                        Msg.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        Msg.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        Msg.Method = HttpMethod.Delete;
                        break;
                }

                HttpResponseMessage ResponseMsg = null;

                if (!string.IsNullOrEmpty(ApiRequest.token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",ApiRequest.token);
                }

                ResponseMsg = await client.SendAsync(Msg);
                var responseContent = await ResponseMsg.Content.ReadAsStringAsync();
                try
                 {
					APIResponse APIResp = JsonConvert.DeserializeObject<APIResponse>(responseContent);
					if (APIResp != null && 
                      (ResponseMsg.StatusCode == System.Net.HttpStatusCode.NotFound || ResponseMsg.StatusCode == System.Net.HttpStatusCode.BadRequest))
					{
						APIResp.StatusCode = System.Net.HttpStatusCode.NotFound;
						APIResp.IsSucceed = false;
						var Apires = JsonConvert.SerializeObject(APIResp);
						var ApiObj = JsonConvert.DeserializeObject<T>(Apires);
						return ApiObj;
					}

				}
                catch (Exception ex)
                {
					var APIRespException = JsonConvert.DeserializeObject<T>(responseContent);
                    return APIRespException;

				}

				var APIRes = JsonConvert.DeserializeObject<T>(responseContent);
                return APIRes;
			}

            catch (Exception ex)
            {
                var errorDTO = new APIResponse
                {
                    ErrorMessage = new List<string> { ex.Message },
                    IsSucceed = false
                };

                var ApiErrorJson = JsonConvert.SerializeObject(errorDTO);
                var APIResp = JsonConvert.DeserializeObject<T>(ApiErrorJson);
                return APIResp;


            }
        }
    }
}
