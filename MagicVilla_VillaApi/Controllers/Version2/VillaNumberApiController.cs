using AutoMapper;
using DreamVilla_VillaApi.Data;
using DreamVilla_VillaApi.Models;
//using DreamVilla_VillaApi.Logging;
using DreamVilla_VillaApi.Models.Dto;
using DreamVilla_VillaApi.Repository;
using DreamVilla_VillaApi.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

//using DreamVilla_VillaApi.Models;

namespace DreamVilla_VillaApi.Controllers.Version2
{

    [ApiController]
    //[Route("api/[Controller]")]
    [Route("api/v{Version:apiVersion}/VillaNumberApi")]
    [ApiVersion("2.0")]
    public class VillaNumberApiController : ControllerBase
    {

        private readonly IVillaNumberRepository VillaNumberRepo;
        private readonly IVillaRepository VillaRepo;
        private readonly IMapper _mapper;
        protected APIResponse _Response;

        public VillaNumberApiController(IVillaNumberRepository db, IMapper mapp, IVillaRepository villaRepo)
        {
            VillaNumberRepo = db;
            _mapper = mapp;
            _Response = new();
            VillaRepo = villaRepo;
        }




        [HttpGet]
        //// [MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ResponseCache(Duration = 30)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                List<VillaNumber> VillaNumbers = await VillaNumberRepo.GetAllAsync(IncludeProperties: "Villa");
                if (VillaNumbers == null)

                {
                    _Response.IsSucceed = false;
                    _Response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_Response);
                }

                _Response.StatusCode = HttpStatusCode.OK;
                _Response.Result = _mapper.Map<List<VillaNumberDTO>>(VillaNumbers);
                return Ok(_Response);
            }

            catch (Exception ex)
            {
                _Response.IsSucceed = false;
                _Response.ErrorMessage = new List<string> { ex.Message };
            }
            return _Response;


        }

        [HttpGet("{VillaNo:int}", Name = "GetVillaNum")]
        //[ProducesResponseType(200,Type = typeof(VillaDTO))]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseCache(Duration = 30)]

        public async Task<ActionResult<APIResponse>> GetVilla(int VillaNo)
        {
            try
            {
                var VillaNumber = await VillaNumberRepo.GetAsync(u => u.VillaNo == VillaNo, IncludeProperties: "Villa");
                if (VillaNumber == null)
                {
                    _Response.IsSucceed = false;
                    _Response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_Response);
                }
                else
                {
                    _Response.Result = _mapper.Map<VillaNumberDTO>(VillaNumber);
                    _Response.StatusCode = HttpStatusCode.OK;
                    return Ok(_Response);

                }
            }

            catch (Exception ex)
            {
                _Response.IsSucceed = false;
                _Response.ErrorMessage = new List<string> { ex.Message };
            }
            return _Response;


        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaNumberCreateDTO CreateVillaNumberDTO)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    _Response.IsSucceed = false;
                    _Response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_Response);
                }

                if (CreateVillaNumberDTO == null)
                {
                    _Response.IsSucceed = false;
                    _Response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(ModelState);

                }

                if (await VillaRepo.GetAsync(u => u.Id == CreateVillaNumberDTO.VillaId) == null)
                {
                    _Response.StatusCode = HttpStatusCode.BadRequest;
                    _Response.IsSucceed = false;
                    ModelState.AddModelError("ErrorMessage", $"Villa with this ID {CreateVillaNumberDTO.VillaNo} is Not exist");
                    return BadRequest(ModelState);

                }


                if (await VillaNumberRepo.GetAsync(n => n.VillaNo == CreateVillaNumberDTO.VillaNo) != null)
                {
                    _Response.StatusCode = HttpStatusCode.BadRequest;
                    _Response.IsSucceed = false;
                    ModelState.AddModelError("ErrorMessage", $"Villa with this Number {CreateVillaNumberDTO.VillaNo} already Exist");

                    return BadRequest(ModelState);
                }

                VillaNumber VillaNumb = _mapper.Map<VillaNumber>(CreateVillaNumberDTO);

                await VillaNumberRepo.AddAsync(VillaNumb);

                _Response.Result = VillaNumb;
                _Response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVillaNum", new { VillaNumb.VillaNo }, _Response);
            }

            catch (Exception ex)
            {
                _Response.IsSucceed = false;
                _Response.ErrorMessage = new List<string> { ex.Message };
            }
            return _Response;
        }



        [HttpDelete("{VillaNo:int}", Name = "DeleteVillaNum")]
        [Authorize(Roles = "Admin")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<APIResponse>> DeleteVilla(int VillaNo)
        {

            try
            {


                VillaNumber VillaNumb = await VillaNumberRepo.GetAsync(n => n.VillaNo == VillaNo);

                if (VillaNumb != null)
                {
                    await VillaNumberRepo.RemoveAsync(VillaNumb);
                    _Response.Result = _mapper.Map<VillaNumberDTO>(VillaNumb);
                    _Response.StatusCode = HttpStatusCode.OK;
                    return Ok(_Response);

                }
                _Response.IsSucceed = false;
                _Response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_Response);
            }

            catch (Exception ex)
            {
                _Response.IsSucceed = false;
                _Response.ErrorMessage = new List<string> { ex.Message };
            }
            return _Response;
        }


        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{VillaNo:int}", Name = "UpdateVillaNum")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<APIResponse>> putVilla(int VillaNo, [FromBody] VillaNumberUpdateDTO UpdateVillaNoDto)
        {
            try
            {
                if (VillaNo == null)
                {
                    _Response.IsSucceed = false;
                    _Response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_Response);
                }
                var villa = await VillaNumberRepo.GetAsync(n => n.VillaNo == VillaNo, isTracked: false);


                if (villa == null)
                {
                    _Response.IsSucceed = false;
                    _Response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_Response);
                }

                if (await VillaRepo.GetAsync(u => u.Id == UpdateVillaNoDto.VillaId) == null)
                {
                    ModelState.AddModelError("ErrorMessage", $"Villa with this Number{UpdateVillaNoDto.VillaId} does not Exist");
                    return BadRequest(ModelState);
                }
                VillaNumber VillaNumberModel = _mapper.Map<VillaNumber>(UpdateVillaNoDto);

                await VillaNumberRepo.updateAsync(VillaNumberModel);
                _Response.Result = VillaNumberModel;
                _Response.StatusCode = HttpStatusCode.OK;
                return Ok(_Response);
            }

            catch (Exception ex)
            {
                _Response.IsSucceed = false;
                _Response.ErrorMessage = new List<string> { ex.Message };
            }
            return _Response;

        }


        [HttpPatch("VillaNo:int", Name = "UpdatePartialVillaNum")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<ActionResult<APIResponse>> patchVilla(int VillaNo, JsonPatchDocument<VillaNumberUpdateDTO> Dtopatch)
        {
            try
            {

                var villaNum = await VillaNumberRepo.GetAsync(u => u.VillaNo == VillaNo, false);
                // we are receiving Dto JasonPatch but we need to fill the dto with the data from villa coming from database

                VillaNumberUpdateDTO modelVillNumberaDto = _mapper.Map<VillaNumberUpdateDTO>(villaNum);


                if (villaNum == null)
                {
                    _Response.IsSucceed = false;
                    _Response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_Response);

                }
                Dtopatch.ApplyTo(modelVillNumberaDto, ModelState);
                // revert it back to villa so we can update the record
                VillaNumber model = _mapper.Map<VillaNumber>(modelVillNumberaDto);

                if (!ModelState.IsValid)
                {
                    _Response.IsSucceed = false;
                    _Response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_Response);
                }
                await VillaNumberRepo.updateAsync(model);



                _Response.Result = model;
                _Response.StatusCode = HttpStatusCode.OK;
                return Ok(_Response);
            }

            catch (Exception ex)
            {
                _Response.IsSucceed = false;
                _Response.ErrorMessage = new List<string> { ex.Message };
            }
            return _Response;

        }





    }


}
