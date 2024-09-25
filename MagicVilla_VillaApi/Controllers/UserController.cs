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
        private readonly ILocalUserRepository userrepo;
        private APIResponse _response;

        public UserController(ILocalUserRepository _Userrepo)
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
            if (usre == null || usre.Id == null )
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


    }
}
