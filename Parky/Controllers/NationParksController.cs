using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parky.Models;
using Parky.Models.DTO;
using Parky.Repository.IRepopsitory;

namespace Parky.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationParksController : ControllerBase
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationParksController(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var objlist = _npRepo.getNationParks();
            var objdto = new List<NationParkDto>();
            foreach (var obj in objlist)
            {
                objdto.Add(_mapper.Map<NationParkDto>(obj));
            }
            return Ok(objdto);
        }
        
        [Authorize]
        [HttpGet("{nationalParkId:int}",Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var obj = _npRepo.getNationPark(nationalParkId);
            if (obj == null)
            {
                return NotFound();
            }
            var objdto = _mapper.Map<NationParkDto>(obj);
            return Ok(objdto);
        }
        
        
        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationParkDto nationParkDto)
        {
            if (nationParkDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_npRepo.NationParkExists(nationParkDto.Name))
            {
                ModelState.AddModelError("","National Park Exists");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var nationalparkobj = _mapper.Map<NationPark>(nationParkDto);
            if (!_npRepo.createNationPark(nationalparkobj))
            {
                ModelState.AddModelError("",$"something went wrong while saving the record {nationalparkobj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark",new { nationalParkId=nationalparkobj.Id},nationalparkobj);
            
        }
        
        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationParkDto nationParkDto)
        {
            if (nationParkDto == null || nationalParkId!=nationParkDto.Id)
            {
                return BadRequest(ModelState);
            }
            var nationalparkobj = _mapper.Map<NationPark>(nationParkDto);
            if (!_npRepo.updateNationPark(nationalparkobj))
            {
                ModelState.AddModelError("",$"something went wrong while updating the record {nationalparkobj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        
        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_npRepo.NationParkExists(nationalParkId))
            {
                return NotFound();
            }
            var nationalparkobj = _npRepo.getNationPark(nationalParkId);
            if (!_npRepo.deleteNationPark(nationalparkobj))
            {
                ModelState.AddModelError("",$"something went wrong while deleting the record {nationalparkobj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
            
        }

        
    }
}
