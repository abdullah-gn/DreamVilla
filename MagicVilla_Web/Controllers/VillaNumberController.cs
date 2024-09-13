using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.ViewModels;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
	{
		private readonly IMapper mapper;
		private readonly IVillaNumberService VillaNumberService;
		private readonly IVillaService VillaService;

		public VillaNumberController(IMapper _Mapper , IVillaNumberService _VillaNumberService , IVillaService _VillaService)
        {
			VillaNumberService = _VillaNumberService;
			VillaService = _VillaService;
			mapper = _Mapper;

		}
		public async Task<IActionResult> VillaNumberIndex()
		{
			List<VillaNumberDTO> VillasNumber = new();

			var Apiresponse = await VillaNumberService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

			if (Apiresponse != null && Apiresponse.IsSucceed)
			{
				VillasNumber = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString((Apiresponse.Result)));
			}
			return View(VillasNumber);
		}


		public async Task<IActionResult> CreateVillaNumber() {

			VillaNumberCreateVM CreateVM = new ();
			var Apiresponse = await VillaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));


			if (Apiresponse != null && Apiresponse.IsSucceed)
			{
				CreateVM.VillasMenuList = JsonConvert.DeserializeObject<List<VillaDTO>>
					(Convert.ToString(Apiresponse.Result)).Select(i => new SelectListItem
					{
						Text = i.Name,
						Value = i.Id.ToString()
					});
					
					return View(CreateVM);
			}
			return View();
		
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM VillaNumDto)
		{
			if(ModelState.IsValid && VillaNumDto.VillaNumber.VillaId !=0)
			{
				var Apiresponse = await VillaNumberService.CreateAsync<APIResponse>(VillaNumDto.VillaNumber, HttpContext.Session.GetString(SD.SessionToken));

				if (Apiresponse != null && Apiresponse.IsSucceed)
				{
					return RedirectToAction(nameof(VillaNumberIndex));
				}
				else
				{
					if (Apiresponse.ErrorMessage.Count > 0)
					{
						ModelState.AddModelError("ErrorMessage", Apiresponse.ErrorMessage.FirstOrDefault());
					}
				}

			}
			
			var Apiresp = await VillaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));


			if (Apiresp != null && Apiresp.IsSucceed)
			{
				VillaNumDto.VillasMenuList = JsonConvert.DeserializeObject<List<VillaDTO>>
					(Convert.ToString(Apiresp.Result)).Select(i => new SelectListItem
					{
						Text = i.Name,
						Value = i.Id.ToString()
					});

				return View(VillaNumDto);
			}

			return View(VillaNumDto);

		}


		public async Task<IActionResult> UpdateVillaNumber(int id)
		{
			VillaNumberUpdateVM UpdateeVM = new();

		
			var Apiresponse = await VillaNumberService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));

			if (Apiresponse != null && Apiresponse.IsSucceed)
			{

				VillaNumberDTO Villa = JsonConvert.DeserializeObject<VillaNumberDTO>(Apiresponse.Result.ToString());

				UpdateeVM.VillaNumber = mapper.Map<VillaNumberUpdateDTO>(Villa);
			}

			Apiresponse = await VillaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

			if (Apiresponse != null && Apiresponse.IsSucceed)
			{

				UpdateeVM.VillasMenuList = JsonConvert.DeserializeObject<List<VillaDTO>>(
				Convert.ToString((Apiresponse.Result))).Select(i => new SelectListItem
				{
					Value = i.Id.ToString(),
					Text = i.Name,
					Selected = true,

				});
				return View(UpdateeVM);
			}
			
			return NotFound();
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM VillaNumDto)
		{
			if (ModelState.IsValid)
			{
				var Apiresponse = await VillaNumberService.UpdateAsync<APIResponse>(VillaNumDto.VillaNumber, HttpContext.Session.GetString(SD.SessionToken));

				if (Apiresponse != null && Apiresponse.IsSucceed)
				{
					return RedirectToAction(nameof(VillaNumberIndex));
				}
				else
				{
					if (Apiresponse.ErrorMessage.Count > 0)
					{
						ModelState.AddModelError("ErrorMessage", Apiresponse.ErrorMessage.FirstOrDefault());
					}

				}

			}

			var Apiresp = await VillaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

			if (Apiresp != null && Apiresp.IsSucceed)
			{

				VillaNumDto.VillasMenuList = JsonConvert.DeserializeObject<List<VillaDTO>>(
				Convert.ToString((Apiresp.Result))).Select(i => new SelectListItem
				{
					Value = i.Id.ToString(),
					Text = i.Name,
					Selected = true,

				});
				
			}

			return View(VillaNumDto);

		}

		public async Task<IActionResult> DeleteVillaNumber(int id)
		{

			var Apiresponse = await VillaNumberService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));

			if (Apiresponse != null && Apiresponse.IsSucceed)
			{
				VillaNumberDTO Villa = JsonConvert.DeserializeObject<VillaNumberDTO>(Apiresponse.Result.ToString());
				return View(Villa);
			}

			return NotFound();

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteVillaNumber(VillaNumberDTO Villaa)
		{

			var Apiresponse = await VillaNumberService.DeleteAsync<APIResponse>(Villaa.VillaNo, HttpContext.Session.GetString(SD.SessionToken));

			if (Apiresponse != null && Apiresponse.IsSucceed)
			{
				return RedirectToAction(nameof(VillaNumberIndex));
			}


			return View(Villaa);
		}




	}
}
