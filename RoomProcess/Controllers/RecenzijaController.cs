using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoomProcess.InterfaceRepository;
using RoomProcess.Models.DTO;
using RoomProcess.Models.Entities;
using RoomProcess.Repository;

namespace RoomProcess.Controllers
{
    [Route("api/recenzija")]
    [ApiController]
    public class RecenzijaController : ControllerBase
    {
        private readonly IRecenzijaRepository _recenzijaRepository;
        private readonly IMapper _mapper;

        public RecenzijaController(IRecenzijaRepository recenzijaRepository, IMapper mapper)
        {
            _recenzijaRepository = recenzijaRepository;
            _mapper = mapper;
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        //[Authorize]
        public ActionResult GetRecenzijas()
        {
            var recenzijas = _mapper.Map<List<RecenzijaDTO>>(_recenzijaRepository.GetRecenzijas());

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);

            }
            return Ok(recenzijas);
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{recenzijaId}")]
        //[Authorize]

        public IActionResult GetRecenzijaById(int recenzijaId)
        {
            if (!_recenzijaRepository.RecenzijaExist(recenzijaId))
            {
                ModelState.AddModelError("", "Not found");
                return StatusCode(404);

            }
            var recenzija = _mapper.Map<RecenzijaDTO>(_recenzijaRepository.GetRecenzijaById(recenzijaId));

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);

            }
            return Ok(recenzija);
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        //[AuthRole("Role", "Admin")]
        public ActionResult<Recenzija> CreateRecenzija([FromBody] RecenzijaCreateDTO recenzijaCreate)
        {

            if (recenzijaCreate == null)
            {
                return BadRequest(recenzijaCreate);
            }
            /*if (aranzmanCreate.AranzmanID > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }*/
            var recenzija = _recenzijaRepository.GetRecenzijas().Where(a => a.RecenzijaId == recenzijaCreate.RecenzijaId).FirstOrDefault();

            if (recenzija != null)
            {
                ModelState.AddModelError("", "Recenzija already exists");
                return StatusCode(422);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }

            var recenzijaMap = _mapper.Map<Recenzija>(recenzijaCreate);

            if (!_recenzijaRepository.CreateRecenzija(recenzijaMap))
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

        public IActionResult UpdateRecenzija([FromBody] RecenzijaUpdateDTO updateRecenzija)
        {
            if (updateRecenzija == null)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }

            /*if (id != updateAranzman.AranzmanID)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }*/
            if (!_recenzijaRepository.RecenzijaExist(updateRecenzija.RecenzijaId))
            {
                ModelState.AddModelError("", "Not found");
                return StatusCode(404);
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }
            var recenzijaMap = _mapper.Map<Recenzija>(updateRecenzija);


            if (!_recenzijaRepository.UpdateRecenzija(recenzijaMap))
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
        [HttpDelete("{recenzijaId}")]
        //[AuthRole("Role", "Admin")]

        public IActionResult DeleteRecenzija(int recenzijaId)
        {
            var recenzija = _recenzijaRepository.GetRecenzijaById(recenzijaId);

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }
            if (_recenzijaRepository.GetRecenzijaById(recenzijaId) == null)
            {
                ModelState.AddModelError("", "Error 500");
                return StatusCode(500);
            }
            if (!_recenzijaRepository.DeleteRecenzija(recenzija))
            {
                ModelState.AddModelError("", "Something went wrong while deleting oprema");
            }
            return NoContent();
        }
        //Posebni GET zahtevi
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("byKorisnik/korisnikId")]
        //[Authorize]
        public ActionResult GetRecenzijaByIdKorisnik(int korisnikId)
        {
            var recenzije = _recenzijaRepository.GetRecenzijaByIdKorisnik(korisnikId);

            if (!recenzije.Any())
            {
                return NotFound("Korisnik with this ID has not been assigned to any Recenzija");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Bad request");
            }

            return Ok(recenzije);

        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("byRezervacija/rezervacijaId")]
        //[Authorize]
        public ActionResult GetRecenzijaByIdRezervacija(int rezrvacijaId)
        {
            var recenzije = _recenzijaRepository.GetRecenzijaByIdRezervacija(rezrvacijaId);

            if (!recenzije.Any())
            {
                return NotFound("Rezervacija with this ID has not been assigned to any Recenzija");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Bad request");
            }

            return Ok(recenzije);

        }
        //
    }
}
