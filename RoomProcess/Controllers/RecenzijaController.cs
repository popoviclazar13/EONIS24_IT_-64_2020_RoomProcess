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
    [Route("api/recenzija")]
    [ApiController]
    public class RecenzijaController : ControllerBase
    {
        private readonly IRecenzijaRepository _recenzijaRepository;
        private readonly IMapper _mapper;
        private readonly IRezervacijaRepository _rezervacijaRepository;

        public RecenzijaController(IRecenzijaRepository recenzijaRepository, IMapper mapper, IRezervacijaRepository rezervacijaRepository)
        {
            _recenzijaRepository = recenzijaRepository;
            _mapper = mapper;
            _rezervacijaRepository = rezervacijaRepository;
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        //[AuthRole("Role", "Admin")]
        [AllowAnonymous]
        public ActionResult GetRecenzijas(int pageNumber = 1, int pageSize = 10)
        {
            /* var recenzijas = _mapper.Map<List<RecenzijaDTO>>(_recenzijaRepository.GetRecenzijas());

             if (!ModelState.IsValid)
             {
                 ModelState.AddModelError("", "Bad request");
                 return StatusCode(400);

             }
             return Ok(recenzijas);*/
            var recenzijas = _recenzijaRepository.GetRecenzijas()
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .ToList();

            var recenzijasDTO = _mapper.Map<List<RecenzijaDTO>>(recenzijas);

            if (recenzijasDTO.Count == 0)
                return NotFound("No recenzija found");

            return Ok(recenzijasDTO);
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{recenzijaId}")]
        [AuthRole("Role", "Admin")]

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
        [AuthRole("Role", "Korisnik")]
        public ActionResult<Recenzija> CreateRecenzija([FromBody] RecenzijaCreateDTO recenzijaCreate)
        {

            if (recenzijaCreate == null)
            {
                return BadRequest(recenzijaCreate);
            }

            var rezervacija = _rezervacijaRepository.GetRezervacijaById(recenzijaCreate.RezervacijaId);
            if (rezervacija == null || rezervacija.KorisnikId != recenzijaCreate.KorisnikId)
            {
                ModelState.AddModelError("", "Korisnik nema vezanu rezervaciju.");
                return StatusCode(422, "Korisnik nema vezanu rezervaciju.");
            }

            /*var recenzija = _recenzijaRepository.GetRecenzijas().Where(a => a.RecenzijaId == recenzijaCreate.RecenzijaId).FirstOrDefault();

            if (recenzija != null)
            {
                ModelState.AddModelError("", "Recenzija already exists");
                return StatusCode(422);
            }*/

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
        [AuthRole("Role", "Admin")]

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
        [AuthRole("Role", "Admin")]

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
                ModelState.AddModelError("", "There is no Recenzija with that Id");
                return StatusCode(404, "There is no Recenzija with that Id");
            }
            if (!_recenzijaRepository.DeleteRecenzija(recenzija))
            {
                ModelState.AddModelError("", "Something went wrong while deleting oprema");
            }
            return NoContent();
        }
        //Posebni GET zahtevi
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("byKorisnik/{korisnikId}")] // {} oznacava da trazi parametar unutar URL
        [AuthRole("Role", "Korisnik")]
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
        [HttpGet("byRezervacija/{rezervacijaId}")]
        [AuthRole("Role", "Admin")]
        public ActionResult GetRecenzijaByIdRezervacija(int rezervacijaId)
        {
            var recenzije = _recenzijaRepository.GetRecenzijaByIdRezervacija(rezervacijaId);

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
