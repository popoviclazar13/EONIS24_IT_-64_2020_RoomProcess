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
    [Route("api/rezervacija")]
    [ApiController]
    public class RezervacijaController : ControllerBase
    {
        private readonly IRezervacijaRepository _rezervacijaRepository;
        private readonly IMapper _mapper;
        //Zbog racunanja ukupne cene 
        private readonly IObjekatRepository _objekatRepository;
        private readonly IPopustRepository _opustRepository;

        public RezervacijaController(IRezervacijaRepository rezervacijaRepository, IMapper mapper, IObjekatRepository objekatRepository, IPopustRepository opustRepository)
        {
            _rezervacijaRepository = rezervacijaRepository;
            _mapper = mapper;
            _objekatRepository = objekatRepository;
            _opustRepository = opustRepository;
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        //[AuthRole("Role", "Admin")]
        [AllowAnonymous]
        public IActionResult GetRezervacijas(int pageNumber = 1, int pageSize = 10)
        {
            var rezervacijas = _rezervacijaRepository.GetRezervacijas()
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .ToList();

            var rezervacijasDTO = _mapper.Map<List<RezervacijaDTO>>(rezervacijas);

            if (rezervacijasDTO.Count == 0)
                return NotFound("No rezervacija found");

            return Ok(rezervacijasDTO);
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{rezervacijaId}")]
        [AuthRole("Role", "Admin")]

        public IActionResult GetRezervacijaById(int rezervacijaId)
        {
            if (!_rezervacijaRepository.RezervacijaExist(rezervacijaId))
            {
                ModelState.AddModelError("", "Not found");
                return StatusCode(404);

            }
            var rezervacija = _mapper.Map<RezervacijaDTO>(_rezervacijaRepository.GetRezervacijaById(rezervacijaId));

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);

            }
            return Ok(rezervacija);
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        [AuthRole("Role", "Korisnik")]
        public ActionResult<Rezervacija> CreateRezervacija([FromBody] RezervacijaCreateDTO rezervacijaCreate)
        {
            
            if (rezervacijaCreate == null)
            {
                return BadRequest(rezervacijaCreate);
            }

            // Mapiranje DTO objekta na domenski objekat
            var rezervacijaMap = _mapper.Map<Rezervacija>(rezervacijaCreate);

            //Racunanje broja nocenja
            TimeSpan trajanjeBoravka = rezervacijaMap.DatumOdlaska - rezervacijaMap.DatumDolaska;
            rezervacijaMap.BrojNocenja = (int)trajanjeBoravka.TotalDays;

            //Za racunanje cene
            var objekat = _objekatRepository.GetObjekatById((int)rezervacijaMap.ObjekatId);
            if (objekat == null)
            {
                ModelState.AddModelError("", "Objekat nije pronađen.");
                return NotFound(ModelState);
            }
            //Ovo je za obracunavanje popusta
            if (objekat.Popust != null)
            {
                // Ako postoji popust, primenite ga na cenu rezervacije
                decimal popustIznos = objekat.Popust.PopustIznos;
                decimal popustProcenat = popustIznos / 100;

                decimal cenaBezPopusta = rezervacijaMap.BrojNocenja * objekat.Cena;
                decimal iznosPopusta = cenaBezPopusta * popustProcenat;
                decimal cenaSaPopustom = cenaBezPopusta - iznosPopusta;

                rezervacijaMap.Cena = (int)cenaSaPopustom;
            }
            else
            {
                // Ako ne postoji popust, cena će biti cena bez popusta
                rezervacijaMap.Cena = rezervacijaMap.BrojNocenja * objekat.Cena;
            }
            //

            // Provera preklapanja datuma rezervacije za isti objekat
            var existingReservation = _rezervacijaRepository.GetRezervacijas().FirstOrDefault(r =>
                r.ObjekatId == rezervacijaMap.ObjekatId &&
                r.DatumDolaska < rezervacijaMap.DatumOdlaska &&
                r.DatumOdlaska > rezervacijaMap.DatumDolaska);

            if (existingReservation != null)
            {
                ModelState.AddModelError("", "Datum rezervacije se preklapa sa postojećom rezervacijom za isti objekat.");
                return BadRequest(ModelState);
            }

            // Provera višestrukih rezervacija korisnika za isti vremenski period u istom hotelu
            /*var userReservations = _rezervacijaRepository.GetRezervacijas().Where(r =>
                r.KorisnikId == rezervacijaMap.KorisnikId &&
                r.ObjekatId == rezervacijaMap.ObjekatId &&
                r.DatumDolaska < rezervacijaMap.DatumOdlaska &&
                r.DatumOdlaska > rezervacijaMap.DatumDolaska)
                .ToList();

            if (userReservations.Any())
            {
                ModelState.AddModelError("", "Korisnik već ima rezervaciju za isti vremenski period u istom hotelu.");
                return BadRequest(ModelState);
            }*/

            // Dodavanje rezervacije u repozitorijum
            if (_rezervacijaRepository.CreateRezervacija(rezervacijaMap))
            {
                ModelState.AddModelError("", "Došlo je do greške pri čuvanju rezervacije.");
                return StatusCode(500);
            }

            return Ok("Rezervacija uspešno kreirana.");
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        [AuthRole("Role", "Korisnik")]

        public IActionResult UpdateRezervacija([FromBody] RezervacijaUpdateDTO updateRezervacija)
        {
            if (updateRezervacija == null)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }

            if (!_rezervacijaRepository.RezervacijaExist(updateRezervacija.RezervacijaId))
            {
                ModelState.AddModelError("", "Not found");
                return StatusCode(404);
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }
            var rezervacijaMap = _mapper.Map<Rezervacija>(updateRezervacija);

            //Racunanje broja nocenja
            TimeSpan trajanjeBoravka = rezervacijaMap.DatumOdlaska - rezervacijaMap.DatumDolaska;
            rezervacijaMap.BrojNocenja = (int)trajanjeBoravka.TotalDays;

            //Za racunanje cene
            var objekat = _objekatRepository.GetObjekatById((int)rezervacijaMap.ObjekatId);
            if (objekat == null)
            {
                ModelState.AddModelError("", "Objekat nije pronađen.");
                return NotFound(ModelState);
            }
            //Ovo je za obracunavanje popusta
            if (objekat.Popust != null)
            {
                // Ako postoji popust, primenite ga na cenu rezervacije
                decimal popustIznos = objekat.Popust.PopustIznos;
                decimal popustProcenat = popustIznos / 100;

                decimal cenaBezPopusta = rezervacijaMap.BrojNocenja * objekat.Cena;
                decimal iznosPopusta = cenaBezPopusta * popustProcenat;
                decimal cenaSaPopustom = cenaBezPopusta - iznosPopusta;

                rezervacijaMap.Cena = (int)cenaSaPopustom;
            }
            else
            {
                // Ako ne postoji popust, cena će biti cena bez popusta
                rezervacijaMap.Cena = rezervacijaMap.BrojNocenja * objekat.Cena;
            }
            //

            if (!_rezervacijaRepository.UpdateRezervacija(rezervacijaMap))
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
        [HttpDelete("{rezervacijaId}")]
        [AuthRole("Role", "Admin")]

        public IActionResult DeleteRezervacija(int rezervacijaId)
        {
            var rezervacija = _rezervacijaRepository.GetRezervacijaById(rezervacijaId);

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }
            if (_rezervacijaRepository.GetRezervacijaById(rezervacijaId) == null)
            {
                ModelState.AddModelError("", "There is no Rezervacija with that Id");
                return StatusCode(404, "There is no Rezervacija with that Id");
            }
            if (!_rezervacijaRepository.DeleteRezervacija(rezervacija))
            {
                ModelState.AddModelError("", "Something went wrong while deleting oprema");
            }
            return NoContent();
        }
        //Posebni GET zahtevi
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("byKorisnik/{korisnikId}")]
        [AuthRole("Role", "Admin")]
        //[AuthRole("Role", "Korisnik")]
        public ActionResult GetRezervacijaByIdKorisnik(int korisnikId)
        {
            var rezervacije = _rezervacijaRepository.GetRezervacijaByIdKorisnik(korisnikId);

            if (!rezervacije.Any())
            {
                return NotFound("Korisnik with this ID has not been assigned to any Rezervacija");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Bad request");
            }

            return Ok(rezervacije);

        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("byObjekat/{objekatId}")]
        [AuthRole("Role", "Admin")]
        //[AuthRole("Role", "Vlasnik")]
        public ActionResult GetRezervacijaByIdObjekat(int objekatId)
        {
            var rezervacije = _rezervacijaRepository.GetRezervacijaByIdObjekat(objekatId);

            if (!rezervacije.Any())
            {
                return NotFound("Objekat with this ID has not been assigned to any Rezervacija");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Bad request");
            }

            return Ok(rezervacije);

        }
        //
    }
}
