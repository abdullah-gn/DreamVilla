using AutoMapper;
using DreamVilla_Web;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
	public class VillaController : Controller
	{
		private readonly IMapper Mapper;
		private readonly IVillaService VillaService;

		public VillaController(IMapper _Mapper, IVillaService VillaService)
		{
			this.Mapper = _Mapper;
			this.VillaService = VillaService;
		}

		public async Task<IActionResult> VillaIndex()
		{
			List<VillaDTO> Villas = new();

			var Apiresponse = await VillaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

			if (Apiresponse != null && Apiresponse.IsSucceed)
			{
				Villas = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString((Apiresponse.Result)));
			}

			return View(Villas);
		}

        [Authorize(Roles = "Admin")]

        public IActionResult CreateVilla()
		{

			return View();

		}



        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateVilla(VillaCreateDTO VillaData)
		{
			if (ModelState.IsValid)
			{
				var Apiresponse = await VillaService.CreateAsync<APIResponse>(VillaData, HttpContext.Session.GetString(SD.SessionToken));

				if (Apiresponse != null && Apiresponse.IsSucceed)
				{
					TempData["Success"] = "Villa created Successfully";
					return RedirectToAction(nameof(VillaIndex));
				}
			}
			TempData["Error"] = "Error Occured";
			return View(VillaData);

		}

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateVilla(int id)

		{
			if (id != null)
			{
				var Apiresponse = await VillaService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));

				if (Apiresponse != null && Apiresponse.IsSucceed)
				{
					VillaDTO Villa = JsonConvert.DeserializeObject<VillaDTO>(Apiresponse.Result.ToString());
					return View(Mapper.Map<VillaUpdateDTO>(Villa));
				}
			}
			return NotFound();
		}

        [Authorize(Roles = "Admin")]

        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateVilla(VillaUpdateDTO Villa)
		{
			if (ModelState.IsValid)
			{
				var Apiresponse = await VillaService.UpdateAsync<APIResponse>(Villa, HttpContext.Session.GetString(SD.SessionToken));

				if (Apiresponse != null && Apiresponse.IsSucceed)
				{
					TempData["Success"] = "Villa Updated Successfully";
					return RedirectToAction(nameof(VillaIndex));
				}

			}
			TempData["Error"] = "Error Occured";
			return View(Villa);
		}

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteVilla(int id)
		{
			
				var Apiresponse = await VillaService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));

				if (Apiresponse != null && Apiresponse.IsSucceed)
				{
					VillaDTO Villa = JsonConvert.DeserializeObject<VillaDTO>(Apiresponse.Result.ToString());
					return View(Villa);
				}
			
			return NotFound();

		}

        [Authorize(Roles = "Admin")]
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteVilla(VillaDTO Villa)
		{
			
				var Apiresponse = await VillaService.DeleteAsync<APIResponse>(Villa.Id, HttpContext.Session.GetString(SD.SessionToken));

				if (Apiresponse != null && Apiresponse.IsSucceed)
				{
				TempData["Success"] = "Villa Deleted Successfully";
				return RedirectToAction(nameof(VillaIndex));
				}

			TempData["Error"] = "Error Occured";
			return View(Villa);
		}
	}
}
