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
    public class TrailsController : ControllerBase
    {
        private readonly ITrailsRepository _trailRepo;
        private readonly IMapper _mapper;

        public TrailsController(ITrailsRepository trailRepo, IMapper mapper)
        {
            _trailRepo = trailRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetTrails()
        {
            var objlist = _trailRepo.getTrails();
            var objdto = new List<TrailDto>();
            foreach (var obj in objlist)
            {
                objdto.Add(_mapper.Map<TrailDto>(obj));
            }
            return Ok(objdto);
        }
        
        [Authorize(Roles ="Admin")]
        [HttpGet("{trailid:int}",Name = "GetTrail")]
        public IActionResult GetTrail(int trailid)
        {
            var obj = _trailRepo.getTrail(trailid);
            if (obj == null)
            {
                return NotFound();
            }
            var objdto = _mapper.Map<TrailDto>(obj);
            return Ok(objdto);
        }
        
        [HttpPost]
        public IActionResult CreateTrail([FromBody] CreateDto trailDto)
        {
            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_trailRepo.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("","Trail Already Exists");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var nationalparkobj = _mapper.Map<Trail>(trailDto);
            if (!_trailRepo.createTrail(nationalparkobj))
            {
                ModelState.AddModelError("",$"something went wrong while saving the record {nationalparkobj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetTrail",new { TrailId=nationalparkobj.Id},nationalparkobj);
        }
        
        [HttpPatch("{trailid:int}", Name = "UpdateTrail")]
        public IActionResult UpdateTrail(int nationalParkId, [FromBody] UpdateDto trailDto)
        {
            // if (TrailDto == null || nationalParkId!=TrailDto.Id)
            // {
            //     return BadRequest(ModelState);
            // }
            var nationalparkobj = _mapper.Map<Trail>(trailDto);
            if (!_trailRepo.updateTrail(nationalparkobj))
            {
                ModelState.AddModelError("",$"something went wrong while updating the record {nationalparkobj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        
        [ HttpDelete("{trailid:int}", Name = "DeleteTrail")]
        public IActionResult DeleteTrail(int trailid)
        {
            if (!_trailRepo.TrailExists(trailid))
            {
                return NotFound();
            }
            var nationalparkobj = _trailRepo.getTrail(trailid);
            if (!_trailRepo.deleteTrail(nationalparkobj))
            {
                ModelState.AddModelError("",$"something went wrong while deleting the record {nationalparkobj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
            
        }

        
    }
}
