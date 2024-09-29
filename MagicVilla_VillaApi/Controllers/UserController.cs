using DreamVilla_VillaApi.Models;
using DreamVilla_VillaApi.Models.Dto;
using DreamVilla_VillaApi.Repository;
using DreamVilla_VillaApi.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace DreamVilla_VillaApi.Controllers
{

    [ApiController]
    [Route("api/v{Version:apiVersion}/UserAuth")]
    [ApiVersionNeutral]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userrepo;
        private APIResponse _response;

        public UserController(IUserRepository _Userrepo)
        {
            userrepo = _Userrepo;
            _response = new();

        }

        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto Loginmodel)
        {
            var loginResponse = await userrepo.Login(Loginmodel);

            if (loginResponse == null || string.IsNullOrEmpty(loginResponse.AccessToken))
            {
                _response.IsSucceed = false;
                _response.ErrorMessage.Add("Invalid User Name or Password");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            _response.IsSucceed = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = loginResponse;
            return Ok(_response);
        }



        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto Registermodel)
        {
            bool IsUnique = userrepo.IsUniqueUser(Registermodel.UserName);
            if (!IsUnique)
            {
                _response.IsSucceed = false;
                _response.ErrorMessage.Add("UserName is Already Exist");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var usre = await userrepo.Register(Registermodel);
            if (usre == null || usre.Id == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSucceed = false;
                _response.ErrorMessage.Add("Error while Registering");
                //// _response.ErrorMessage.Add(usre.Errors);

            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = usre;
            return Ok(_response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> GetNewTokenFromRefreshToken([FromBody] TokenDto tokenDto)
        {
            if (ModelState.IsValid)
            {
				var tokenDtoResponse = await userrepo.RefreshAccessToken(tokenDto);

				if (tokenDtoResponse == null || string.IsNullOrEmpty(tokenDtoResponse.AccessToken))
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.IsSucceed = false;
					_response.ErrorMessage.Add("Invalid Token");
					return BadRequest(_response);
				}
				_response.StatusCode = HttpStatusCode.OK;
				_response.IsSucceed = true;
				_response.Result = tokenDtoResponse;
				return Ok(_response);

			}
            else
			{
				_response.IsSucceed = false;
				_response.ErrorMessage.Add("Invalid input");
				return BadRequest(_response);

			}  
        }

		[HttpPost("revoke")]
		public async Task<IActionResult> RevokeRefreshToken([FromBody] TokenDto tokenDto)
		{
			
			if (ModelState.IsValid)
			{
				await userrepo.RevokeRefreshToken(tokenDto);
				_response.StatusCode = HttpStatusCode.OK;
				_response.IsSucceed = true;
				return Ok(_response);
			}
			_response.IsSucceed = false;
			_response.ErrorMessage.Add("Invalid input");
			return BadRequest(_response);
		}
	}
}
