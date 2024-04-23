using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomProcess.Helpers;
using RoomProcess.InterfaceRepository;
using RoomProcess.Models.DTO;
using RoomProcess.Models.Entities;
using RoomProcess.Repository;

namespace RoomProcess.Controllers
{
    [Route("api/oprema")]
    [ApiController]
    public class OpremaController : ControllerBase
    {
        private readonly IOpremaRepository _opremaRepository;
        private readonly IMapper _mapper;

        public OpremaController(IOpremaRepository opremaRepository, IMapper mapper)
        {
            _opremaRepository = opremaRepository;
            _mapper = mapper;
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetOpremas(int pageNumber = 1, int pageSize = 10)
        {
            var opremas = _opremaRepository.GetOpremas()
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .ToList();

            var opremasDTO = _mapper.Map<List<OpremaDTO>>(opremas);

            if (opremasDTO.Count == 0)
                return NotFound("No oprema found");

            return Ok(opremasDTO);

        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{opremaId}")]
        [AllowAnonymous]

        public IActionResult GetOpremaById(int opremaId)
        {
            if (!_opremaRepository.OpremaExist(opremaId))
            {
                ModelState.AddModelError("", "Not found");
                return StatusCode(404);

            }
            var oprema = _mapper.Map<OpremaDTO>(_opremaRepository.GetOpremaById(opremaId));

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);

            }
            return Ok(oprema);
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        [AuthRole("Role", "Admin")]
        public ActionResult<Oprema> CreateOprema([FromBody] OpremaDTO opremaCreate)
        {

            if (opremaCreate == null)
            {
                return BadRequest(opremaCreate);
            }

            var oprema = _opremaRepository.GetOpremas().Where(a => a.OpremaId == opremaCreate.OpremaId).FirstOrDefault();

            if (oprema != null)
            {
                ModelState.AddModelError("", "Oprema already exists");
                return StatusCode(422);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }

            var opremaMap = _mapper.Map<Oprema>(opremaCreate);

            if (!_opremaRepository.CreateOprema(opremaMap))
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
        [AuthRole("Role", "Admin")]
        public IActionResult UpdateOprema([FromBody] OpremaDTO updateOprema)
        {
            if (updateOprema == null)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }
            if (!_opremaRepository.OpremaExist(updateOprema.OpremaId))
            {
                ModelState.AddModelError("", "Not found");
                return StatusCode(404);
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }
            var opremaMap = _mapper.Map<Oprema>(updateOprema);


            if (!_opremaRepository.UpdateOprema(opremaMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating oprema");

            }

            return NoContent();
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{opremaId}")]
        [AuthRole("Role", "Admin")]

        public IActionResult DeleteOprema(int opremaId)
        {
            var oprema = _opremaRepository.GetOpremaById(opremaId);

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }
            if (_opremaRepository.GetOpremaById(opremaId) == null)
            {
                ModelState.AddModelError("", "Not found");
                return StatusCode(404);
            }
            if (!_opremaRepository.DeleteOprema(oprema))
            {
                ModelState.AddModelError("", "Something went wrong while deleting oprema");
            }
            return NoContent();
        }
        //Posebni GET zahtevi
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("byObjekat/objekatId")]
        [AllowAnonymous]
        public ActionResult GetOpremaByIdObjekat(int objekatId)
        {
            var opreme = _opremaRepository.GetOpremaByIdObjekat(objekatId);

            if (!opreme.Any())
            {
                return NotFound("Objekat with this ID has not been assigned to any Oprema");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Bad request");
            }

            return Ok(opreme);

        }
        //
    }
}
