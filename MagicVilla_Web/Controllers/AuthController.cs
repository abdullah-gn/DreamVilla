﻿using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MagicVilla_Web.Controllers
{
	public class AuthController : Controller
	{
		private readonly IAuthService authService;

		public AuthController(IAuthService AuthService)
        {
			authService = AuthService;
		}

		[HttpGet]
		public IActionResult Login()
		{
			LoginRequestDto Loginmodel = new();
			return View(Loginmodel);

		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginRequestDto Loginmodel)
		{
			if(Loginmodel != null)
			{

				APIResponse Resp = await authService.LoginAsync<APIResponse>(Loginmodel);
				if(Resp != null && Resp.IsSucceed)
				{
                    LoginResponseDto model = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(Resp.Result));

                    var Handler = new JwtSecurityTokenHandler();
					var jwtToken = Handler.ReadJwtToken(model.Token);



					var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
					identity.AddClaim(new Claim(ClaimTypes.Name,jwtToken.Claims.FirstOrDefault(u=>u.Type == "unique_name").Value));
					identity.AddClaim(new Claim(ClaimTypes.Role, jwtToken.Claims.FirstOrDefault(u => u.Type == "role").Value));

					var priciple = new ClaimsPrincipal(identity);

					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,priciple);

                    HttpContext.Session.SetString(SD.SessionToken, model.Token);
					return RedirectToAction("Index","Home");
				}
				else
				{
					ModelState.AddModelError("Custom", Resp.ErrorMessage.FirstOrDefault());
					return View(Loginmodel);
				}

			}

			return View(Loginmodel);

		}



		[HttpGet]
		public IActionResult Register()
		{
			RegisterDto Registermodel = new();
			var Roles = new List<SelectListItem>()
			{
				new SelectListItem{Text = SD.Admin  , Value = SD.Admin},
				new SelectListItem{Text = SD.Customer , Value = SD.Customer},
			};
			ViewBag.RolesList = Roles;
			return View(Registermodel);

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterDto Registermodel)
		{
			if (string.IsNullOrEmpty(Registermodel.Role))
			{
				Registermodel.Role = SD.Customer;
			}


		APIResponse Result =  await authService.RegisterAsync<APIResponse>(Registermodel);
			if(Result != null && Result.IsSucceed)
			{
				return RedirectToAction("Login");

			}
			var Roles = new List<SelectListItem>()
			{
				new SelectListItem{Text = SD.Admin  , Value = SD.Admin},
				new SelectListItem{Text = SD.Customer , Value = SD.Customer},
			};
			ViewBag.RolesList = Roles;
			return View(Registermodel);
		}



		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			HttpContext.Session.SetString(SD.SessionToken, "");
			return RedirectToAction("Login");
		}

		public IActionResult AccessDenied()
		{
			return View();
		}

	}
}
