using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomProcess.InterfaceRepository;
using RoomProcess.Models.DTO;
using RoomProcess.Services.KorisnikService;

namespace RoomProcess.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class KorisnikController : Controller
    {
        private readonly IKorisnikRepository _korisnikRepository;
        private readonly Mapper _mapper;
        private readonly IKorisnikService _korisnikService;

        public KorisnikController(IKorisnikRepository korisnikRepository, Mapper mapper, IKorisnikService korisnikService)
        {
            _korisnikRepository = korisnikRepository;
            _mapper = mapper;
            _korisnikService = korisnikService;
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

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        [Authorize]
        public ActionResult GetKorisniks()
        {
            var korisniks = _mapper.Map<List<KorisnikDTO>>(_korisnikRepository.GetKorisniks());

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);

            }
            else { 
                return Ok(korisniks); 
            }
        }

        [HttpGet("{id}")]
        [Authorize]
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

        [HttpPut("{id}")]
        [AuthRole("Role", "Admin")]

    }
}
