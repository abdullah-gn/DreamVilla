using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MagicVilla_Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly IMapper Mapper;
		private readonly IVillaService VillaService;

		public HomeController(IMapper _Mapper, IVillaService VillaService)
		{
			this.Mapper = _Mapper;
			this.VillaService = VillaService;
		}
		public async Task<IActionResult> Index()
		{
			List<VillaDTO> Villas = new();

			var Apiresponse = await VillaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

			if (Apiresponse != null && Apiresponse.IsSucceed)
			{
				Villas = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString((Apiresponse.Result)));
			}
			return View(Villas);
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}