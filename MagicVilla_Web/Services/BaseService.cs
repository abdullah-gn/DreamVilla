using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using static MagicVilla_Utility.SD;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        private readonly ITokenProvider _tokenProvider;
        protected readonly string VillaUrl;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IApiMessageRequestBuilder _apiMessageRequestBuilder;
		public BaseService(IHttpClientFactory ClientFactory, ITokenProvider tokenProvider, IConfiguration configuration, 
			IHttpContextAccessor contextAccessor, IApiMessageRequestBuilder apiMessageRequestBuilder)
		{
			responseModel = new();
			httpClient = ClientFactory;
			_tokenProvider = tokenProvider;
			VillaUrl = configuration.GetValue<string>("ServiceUrl:VillaApi");
			_contextAccessor = contextAccessor;
			_apiMessageRequestBuilder = apiMessageRequestBuilder;
		}
		public async Task<T> SendAsync<T>(ApiRequest ApiRequest, bool withBearer=true)
        {
            try
            {
                HttpClient client = httpClient.CreateClient("MagicVilla");

				var messageFactory = () =>
				{
					return _apiMessageRequestBuilder.Build(ApiRequest);
				};
			

                HttpResponseMessage httpResponseMsg = null;

				APIResponse finalApiResponse = new()
				{
					IsSucceed = false,

				};
                

				httpResponseMsg = await SendWithRefreshTokenAsync(client,messageFactory,withBearer);
                var responseContent = await httpResponseMsg.Content.ReadAsStringAsync();
                try
                 {

					switch (httpResponseMsg.StatusCode)
					{
						case HttpStatusCode.NotFound:
							finalApiResponse.ErrorMessage = new List<string>() { "Not Found" };
							break;
						case HttpStatusCode.Forbidden:
							finalApiResponse.ErrorMessage = new List<string>() { "Access Denied" };
							break;
						case HttpStatusCode.Unauthorized:
							finalApiResponse.ErrorMessage = new List<string>() { "Unauthorized" };
							break;
						case HttpStatusCode.InternalServerError:
							finalApiResponse.ErrorMessage = new List<string>() { "InternalServerError" };
							break;
						default:
							var apiContent = await httpResponseMsg.Content.ReadAsStringAsync();
							finalApiResponse.IsSucceed = true;
							finalApiResponse = JsonConvert.DeserializeObject<APIResponse>(responseContent);
							break;
					}
					
				}
                catch (Exception ex)
                {
					finalApiResponse.ErrorMessage = new List<string>() { "Error : ", ex.Message.ToString() };
				}
				var Apires = JsonConvert.SerializeObject(finalApiResponse);
				var ApiObj = JsonConvert.DeserializeObject<T>(Apires);
				return ApiObj;
			}
			catch (AuthException authEx)
			{
				throw;
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

		private async Task<HttpResponseMessage> SendWithRefreshTokenAsync(HttpClient httpClient,
			Func<HttpRequestMessage> httpReqMessageFactory, bool withBearer = true)
		{
			if (!withBearer)
			{
				return await httpClient.SendAsync(httpReqMessageFactory());
			}
			else
			{
				TokenDto tokenDto = _tokenProvider.GetToken();
				if (tokenDto!=null && string.IsNullOrEmpty(tokenDto.AccessToken))
				{
					httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenDto.AccessToken);

				}
				try
				{
					var response = await httpClient.SendAsync(httpReqMessageFactory());
					if (response.IsSuccessStatusCode)
						return response;
					// If this fails then we can pass refresh token
					if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.Unauthorized)
					{
						//Generate new token from refresh token / sing in with that new access token the retry
						await InvokeRefreshTokenEndPoint(httpClient, tokenDto);
						response = await httpClient.SendAsync(httpReqMessageFactory());
						return response;
					}
					return response;
			
				}
				catch (AuthException authEx)
				{
					throw;
				}
				catch(HttpRequestException httpRequestException)
				{
					if (httpRequestException.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					{
						//refresh token and retry the request
						await InvokeRefreshTokenEndPoint(httpClient, tokenDto);
						return await httpClient.SendAsync(httpReqMessageFactory());
					}
					throw;
				}
			}

			
		}

		private async Task InvokeRefreshTokenEndPoint(HttpClient httpClient, TokenDto tokenDto)
		{
			HttpRequestMessage message = new();
			message.Headers.Add("Accept", "application/json");
			message.RequestUri = new Uri($"{VillaUrl}/api/{SD.ApiVersion}/UserAuth/refresh");
			message.Method = HttpMethod.Post;
			message.Content = new StringContent(JsonConvert.SerializeObject(tokenDto), Encoding.UTF8, "application/json");
			var response = await httpClient.SendAsync(message);
			var content = await response.Content.ReadAsStringAsync();
			var apiResponse = JsonConvert.DeserializeObject<APIResponse>(content);

			if (apiResponse?.IsSucceed != true)
			{

				await _contextAccessor.HttpContext.SignOutAsync();
				_tokenProvider.ClearToken();
				throw new AuthException();
			}
			else
			{
				var tokenDataString = JsonConvert.SerializeObject(apiResponse.Result);
				var respTokenDto = JsonConvert.DeserializeObject<TokenDto>(tokenDataString);

				if (respTokenDto != null && string.IsNullOrEmpty(tokenDataString))
				{
					//New method to sing in the user with the new accesstoken that we received.
					await SignInWithNewTokens(respTokenDto);
					httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", respTokenDto.AccessToken);
				
				}
			}
			
		}


		private async Task SignInWithNewTokens(TokenDto tokenDto)
		{
			var Handler = new JwtSecurityTokenHandler();
			var jwtToken = Handler.ReadJwtToken(tokenDto.AccessToken);



			var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
			identity.AddClaim(new Claim(ClaimTypes.Name, jwtToken.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
			identity.AddClaim(new Claim(ClaimTypes.Role, jwtToken.Claims.FirstOrDefault(u => u.Type == "role").Value));

			var priciple = new ClaimsPrincipal(identity);

			await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, priciple);
			_tokenProvider.SetToken(tokenDto);
		}



    }
}
