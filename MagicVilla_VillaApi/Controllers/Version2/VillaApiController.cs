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
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text.Json;

//using DreamVilla_VillaApi.Models;

namespace DreamVilla_VillaApi.Controllers.Version2
{



    [ApiController]
    //[Route("api/[Controller]")]
    [Route("api/v{Version:apiVersion}/VillaApi")]
    [ApiVersion("2.0")]
    public class VillaApiController : ControllerBase
    {
        //private readonly ILogging _logger;

        //public VillaApiController(ILogging logger)

        //{
        //    _logger = logger;  
        //}
        private readonly IVillaRepository _db;
        private readonly IMapper _mapper;
        protected APIResponse _Response;

        public VillaApiController(IVillaRepository db, IMapper mapp)
        {
            _db = db;
            _mapper = mapp;
            _Response = new();
        }




        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        // [ResponseCache(CacheProfileName = "default30")]
        // [ResponseCache(Location =ResponseCacheLocation.None,NoStore =true)]
        public async Task<ActionResult<APIResponse>> GetVillas([FromQuery(Name = "OccupancyFilter")] int? Occupancy,
            [FromQuery] string? Search, int pageSize = 0, int PageNumber = 1)
        {
            try
            {
                IEnumerable<Villa> VillaList;

                if (Occupancy > 0)
                {
                    VillaList = await _db.GetAllAsync(d => d.Occupancy == Occupancy, PageSize: pageSize, PageNumber: PageNumber);
                    _Response.StatusCode = HttpStatusCode.OK;
                    _Response.Result = VillaList;
                    return Ok(_Response);

                }
                else
                {
                    VillaList = await _db.GetAllAsync(PageSize: pageSize, PageNumber: PageNumber);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    VillaList = VillaList.Where(u => u.Name.ToLower().Contains(Search));
                }

                Pagination pagination = new() { PageSize = pageSize, PageNumber = PageNumber };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _Response.Result = _mapper.Map<List<VillaDTO>>(VillaList);
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

        [HttpGet("{id:int}", Name = "GetVillaa")]
        //[ProducesResponseType(200,Type = typeof(VillaDTO))]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        [ResponseCache(Duration = 30)]

        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {

                if (id == 0)
                {
                    _Response.IsSucceed = false;
                    _Response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_Response);
                }
                var Villa = await _db.GetAsync(u => u.Id == id);
                if (Villa == null)
                {
                    _Response.IsSucceed = false;
                    _Response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_Response);
                }
                else
                {
                    _Response.Result = _mapper.Map<VillaDTO>(Villa);
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromForm] VillaCreateDTO CreateVillaDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _Response.IsSucceed = false;
                    _Response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_Response);
                }

                if (CreateVillaDTO == null)
                {
                    _Response.IsSucceed = false;
                    _Response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_Response);

                }
                if (await _db.GetAsync(n => n.Name.ToLower() == CreateVillaDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessage", $"Villa with this name {CreateVillaDTO.Name} is already Exist");
                    _Response.IsSucceed = false;
                    _Response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_Response);
                }
                Villa Villaa = _mapper.Map<Villa>(CreateVillaDTO);
                await _db.AddAsync(Villaa);

                if (CreateVillaDTO.Image != null)
                {
                    var fileName = Villaa.Id+ Path.GetExtension(CreateVillaDTO.Image.FileName);
                    var filePath = @"wwwroot\ProductImage\" + fileName;

                    var dirLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    FileInfo file = new FileInfo(dirLocation);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    using (var fileStream = new FileStream(dirLocation, FileMode.Create))
                    {
                        CreateVillaDTO.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    Villaa.ImageUrl = baseUrl + "/ProductImage/" + fileName;
                    Villaa.ImageLocalPath = filePath;


                }
                else
                {
                    Villaa.ImageUrl = "https://placehold.co/600x400";
                }

                await _db.updateAsync(Villaa);

                _Response.Result = Villaa;
                _Response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVillaa", new { Villaa.Id }, _Response);
            }

            catch (Exception ex)
            {
                _Response.IsSucceed = false;
                _Response.ErrorMessage = new List<string> { ex.Message };
            }
            return _Response;
        }

        [HttpDelete("{id:int}", Name = "DeleteVillaa")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {

            try
            {
                if (id == 0)
                {

                    _Response.IsSucceed = false;
                    _Response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_Response);
                }

                Villa Villa = await _db.GetAsync(n => n.Id == id);

                if (Villa != null)
                {
                    if (!string.IsNullOrEmpty(Villa.ImageLocalPath))
                    {
                        var oldDirLocalPath = Path.Combine(Directory.GetCurrentDirectory(), Villa.ImageLocalPath);
                        FileInfo file = new FileInfo(oldDirLocalPath);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }

                    await _db.RemoveAsync(Villa);
                    _Response.Result = _mapper.Map<VillaDTO>(Villa);
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
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<APIResponse>> PutVilla(int id, [FromForm] VillaUpdateDTO UpdateVillaDto)
        {
            try
            {
                var villa = await _db.GetAsync(n => n.Id == id, isTracked: false);

                if (id == 0)
                {
                    _Response.IsSucceed = false;
                    _Response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_Response);
                }
                if (villa == null)
                {
                    _Response.IsSucceed = false;
                    _Response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_Response);
                }
                Villa VillaModel = _mapper.Map<Villa>(UpdateVillaDto);

                if (UpdateVillaDto.Image != null)
                {
                    if (!string.IsNullOrEmpty(VillaModel.ImageLocalPath))
                    {
                        var oldDirLocalPath = Path.Combine(Directory.GetCurrentDirectory(), VillaModel.ImageLocalPath);
                        FileInfo file = new FileInfo(oldDirLocalPath);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }  

                    var fileName = UpdateVillaDto.Id + Path.GetExtension(UpdateVillaDto.Image.FileName);
                    var filePath = @"wwwroot\ProductImage\" + fileName;

                    var dirLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    using (var fileStream = new FileStream(dirLocation, FileMode.Create))
                    {
                        UpdateVillaDto.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    VillaModel.ImageUrl = baseUrl + "/ProductImage/" + fileName;
                    VillaModel.ImageLocalPath = filePath;


                }
                else
                {
                    VillaModel.ImageUrl = "https://placehold.co/600x400";
                }

                await _db.updateAsync(VillaModel);
                _Response.Result = VillaModel;
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


        [HttpPatch("id:int", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<ActionResult<APIResponse>> patchVilla(int id, JsonPatchDocument<VillaUpdateDTO> Dtopatch)
        {
            try
            {
                if (id == 0 || Dtopatch == null)
                {
                    _Response.IsSucceed = false;
                    _Response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_Response);
                }
                var villa = await _db.GetAsync(u => u.Id == id, false);
                // we are receiving Dto JasonPatch but we need to fill the dto with the data from villa coming from database

                VillaUpdateDTO modelVillaDto = _mapper.Map<VillaUpdateDTO>(villa);


                if (villa == null)
                {
                    _Response.IsSucceed = false;
                    _Response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_Response);

                }
                Dtopatch.ApplyTo(modelVillaDto, ModelState);
                // revert it back to villa so we can update the record
                Villa model = _mapper.Map<Villa>(modelVillaDto);

                if (!ModelState.IsValid)
                {
                    _Response.IsSucceed = false;
                    _Response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_Response);
                }
                await _db.updateAsync(model);



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
