using DreamVilla_VillaApi.Data;
using DreamVilla_VillaApi.Logging;
using DreamVilla_VillaApi.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
//using DreamVilla_VillaApi.Models;

namespace DreamVilla_VillaApi.Controllers
{



    [ApiController]
    //[Route("api/[Controller]")]
    [Route("api/VillaApi")]
    public class VillaApiController : ControllerBase
    {
        private readonly ILogging _logger;

        public VillaApiController(ILogging logger)

        {
            _logger = logger;  
        }

        [HttpGet]

        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {

            List<VillaDTO> Villas = VillaStore.VillaList;
            if (Villas == null)
            return NotFound();

            _logger.Logging("We are getting all Villas","");
            return Ok(Villas);
            
        }

        [HttpGet("{id:int}", Name = "GetVillaa")]
        //[ProducesResponseType(200,Type = typeof(VillaDTO))]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.Logging($"Error getting Villa number {id}","error");
                return BadRequest();
            }
            var Villa = VillaStore.VillaList.FirstOrDefault(u => u.id == id);
            if (Villa == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Villa);
            }


        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO Villa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Villa == null)
                return BadRequest(Villa);
            if (Villa.id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if (VillaStore.VillaList.FirstOrDefault(n => n.name.ToLower() == Villa.name.ToLower()) != null)
            {
                ModelState.AddModelError("", $"Villa with this name {Villa.name} is already Exist");
                return BadRequest(ModelState);
            }

            Villa.id = VillaStore.VillaList.OrderByDescending(n => n.id).FirstOrDefault().id + 1;
            VillaStore.VillaList.Add(Villa);
            return CreatedAtRoute("GetVillaa", new { id = Villa.id }, Villa);
        }

        [HttpDelete("{id:int}",Name = "DeleteVillaa")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
               
                return BadRequest();
            }

            VillaDTO Villa = VillaStore.VillaList.FirstOrDefault(n => n.id == id);

            if (Villa != null)
            {
                VillaStore.VillaList.Remove(Villa);
                return NoContent();

            }
            return NotFound();

        }


        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public ActionResult<VillaDTO> putVilla(int id ,[FromBody]VillaDTO Villa)
        {
            if(id == null || id != Villa.id)
            {
                return BadRequest();
            }
            var villa = VillaStore.VillaList.FirstOrDefault(n => n.id == id);

            if (id == 0)
            {
                return BadRequest();
            }
            if (villa == null)
            {
                return NotFound();
            }
            villa.id = Villa.id;
            villa.name = Villa.name;
            villa.sqft = Villa.sqft;
            villa.occupancy = Villa.occupancy;
            return NoContent();
            
        }


        [HttpPatch("id:int",Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult patchVilla(int id , JsonPatchDocument<VillaDTO> Dtopatch) { 

            if(id == 0 || Dtopatch == null)
            {
                return BadRequest();
            }
            var DtoVilla = VillaStore.VillaList.FirstOrDefault(u => u.id == id);
            if(DtoVilla == null)
            {
                return NotFound();

            }
            Dtopatch.ApplyTo(DtoVilla,ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return NoContent();

        }
    }

    
}
