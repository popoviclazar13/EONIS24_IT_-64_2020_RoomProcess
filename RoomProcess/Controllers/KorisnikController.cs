using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomProcess.Helpers;
using RoomProcess.InterfaceRepository;
using RoomProcess.Models.DTO;
using RoomProcess.Repository;
using RoomProcess.Services.KorisnikService;

namespace RoomProcess.Controllers
{
    [Route("api/korisnik")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        private readonly IKorisnikRepository _korisnikRepository;
        private readonly IMapper _mapper;
        private readonly IKorisnikService _korisnikService;

        public KorisnikController(IKorisnikRepository korisnikRepository, IMapper mapper, IKorisnikService korisnikService)
        {
            _korisnikRepository = korisnikRepository;
            _mapper = mapper;
            _korisnikService = korisnikService;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        //[Authorize]
        public ActionResult GetKorisniks(int pageNumber = 1, int pageSize = 10)
        {
            var korisniks = _korisnikRepository.GetKorisniks()
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .ToList();

            var korisniksDTO = _mapper.Map<List<KorisnikDTO>>(korisniks);

            if (korisniksDTO.Count == 0)
                return NotFound("No korisnik found");

            return Ok(korisniksDTO);
        }

        [HttpGet("korisnikId")]
        //[Authorize]
        public ActionResult GetKorisnikById(int korisnikId)
        {
            if (!_korisnikRepository.KorisnikExist(korisnikId))
            {
                ModelState.AddModelError("", "Korisnik with this ID does not exist");
                return StatusCode(404);

            }
            var korisnik = _mapper.Map<KorisnikDTO>(_korisnikRepository.GetKorisnikById(korisnikId));

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);

            }
            else
            {
                return Ok(korisnik);
            }

        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult LoginKorisnik([FromBody]KorisnikLoginDTO korisnik)
        {
            return Ok(_korisnikService.LoginKorisnik(korisnik));
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public ActionResult CreateKorisnik([FromBody]KorisnikRequestDTO korisnik)
        {
            return Ok(_korisnikService.CreateKorisnik(korisnik));
        }

        [HttpPut("korisnikId")]//mora da se salje 
        [AuthRole("Role", "Admin")]
        public ActionResult UpdateKorisnik(int korisnikId, KorisnikRequestDTO data)
        {
            return Ok(_korisnikService.UpdateKorisnik(korisnikId, data));
        }

        [HttpDelete("{korisnikId}")]
        [AuthRole("Role", "Admin")]
        public ActionResult DeleteKorisnik(int korisnikId)
        {
            return Ok(_korisnikService.DeleteKorisnik(korisnikId));
        }
        //Posebni GET zahtevi
        [HttpGet("byUloga/ulogaId")]
        //[Authorize]
        public ActionResult GetKorisnikByIdUloga(int ulogaId)
        {
            var korisnici = _korisnikRepository.GetKorisnikByIdUloga(ulogaId);

            if (!korisnici.Any())
            {
                return NotFound("Uloga with this ID has not been assigned to any User");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Bad request");
            }

            return Ok(korisnici);

        }
        //
    }
}
