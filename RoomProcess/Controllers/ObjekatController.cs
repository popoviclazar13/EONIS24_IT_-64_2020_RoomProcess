using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoomProcess.InterfaceRepository;
using RoomProcess.Models.DTO;
using RoomProcess.Models.Entities;
using RoomProcess.Repository;

namespace RoomProcess.Controllers
{
    [Route("api/objekat")]
    [ApiController]
    public class ObjekatController : ControllerBase
    {
        private readonly IObjekatRepository _objekatRepository;
        private readonly IMapper _mapper;

        public ObjekatController(IObjekatRepository objekatRepository, IMapper mapper)
        {
            _objekatRepository = objekatRepository;
            _mapper = mapper;
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public ActionResult GetObjekats()
        {
            var objekats = _mapper.Map<List<ObjekatDTO>>(_objekatRepository.GetObjekats());

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);

            }
            return Ok(objekats);
        }
        [HttpGet("{objekatId}")]
        //[Authorize]
        public ActionResult GetObjekatById(int objekatId)
        {
            if (!_objekatRepository.ObjekatExist(objekatId))
            {
                ModelState.AddModelError("", "Objekat with this ID does not exist");
                return StatusCode(404);

            }
            else
            {
                return Ok(_objekatRepository.GetObjekatById(objekatId));
            }
        }
        [HttpPost]
        //[AuthRole("Role", "Admin")]
        public ActionResult<Popust> CreateObjekat([FromBody] ObjekatCreateDTO objekatCreateDTO)
        {
            if (objekatCreateDTO == null)
            {
                return BadRequest(objekatCreateDTO);
            }
            var objekat = _objekatRepository.GetObjekats().Where(u => u.ObjekatId == objekatCreateDTO.ObjekatId).FirstOrDefault();

            if (objekat != null)
            {
                ModelState.AddModelError("", "Objekat already exists");
                return StatusCode(422);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }

            var objekatMap = _mapper.Map<Objekat>(objekatCreateDTO);

            if (!_objekatRepository.CreateObjekat(objekatMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
            }

            return Ok("Successfully created");
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        //[AuthRole("Role", "Admin")]

        public IActionResult UpdateObjekat([FromBody] ObjekatUpdateDTO updateObjekat)
        {
            if (updateObjekat == null)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }
            if (!_objekatRepository.ObjekatExist(updateObjekat.ObjekatId))
            {
                ModelState.AddModelError("", "Not found");
                return StatusCode(404);
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }
            var objekatMap = _mapper.Map<Objekat>(updateObjekat);


            if (!_objekatRepository.UpdateObjekat(objekatMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating popust");

            }

            return NoContent();
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{objekatId}")]
        //[AuthRole("Role", "Admin")]
        public IActionResult DeleteObjekat(int objekatId)
        {
            var objekat = _objekatRepository.GetObjekatById(objekatId);

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }
            if (_objekatRepository.GetObjekatById(objekatId) == null)
            {
                ModelState.AddModelError("", "Objekat with this ID does not exist");
                return StatusCode(404);
            }
            if (!_objekatRepository.DeleteObjekat(objekat))
            {
                ModelState.AddModelError("", "Something went wrong while deleting popust");
            }
            return NoContent();
        }
    }

}
