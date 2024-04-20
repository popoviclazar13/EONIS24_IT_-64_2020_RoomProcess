using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoomProcess.InterfaceRepository;
using RoomProcess.Models.DTO;
using RoomProcess.Models.Entities;
using RoomProcess.Repository;
using System;

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
        public ActionResult GetObjekats(int pageNumber = 1, int pageSize = 10)
        {
            var objekts = _objekatRepository.GetObjekats()
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .ToList();

            var objekatsDTO = _mapper.Map<List<ObjekatDTO>>(objekts);

            if (objekatsDTO.Count == 0)
                return NotFound("No objekat found");

            return Ok(objekatsDTO);

            /*var objekats = _mapper.Map<List<ObjekatDTO>>(_objekatRepository.GetObjekats());

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);

            }
            return Ok(objekats);*/
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

            var existingObjekat = _objekatRepository.GetObjekats().FirstOrDefault(u => u.ObjekatNaziv == objekatCreateDTO.ObjekatNaziv);

            if (existingObjekat != null)
            {
                ModelState.AddModelError("", "Objekat with the same name already exists.");
                return StatusCode(422, "Objekat with the same name already exists.");
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
        //Posebni GET zahtevi
        //Ovaj HttpGet je vezan za pretragu po gradovima
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("byGrad/{grad}")]
        public IActionResult GetObjekatByGrad(string grad)
        {
            var objekat = _objekatRepository.GetObjekatByGrad(grad);
            if (objekat == null)
            {
                return NotFound("There is no Objekat in the requested Grad");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad request");
            }

            return Ok(objekat);
        }
        //Ovaj Httpget je vezan za pretragu po nazivu
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("byNaziv/{naziv}")]
        public IActionResult GetObjekatByNaziv(string naziv)
        {
            var objekat = _objekatRepository.GetObjekatByNaziv(naziv);
            if (objekat == null)
            {
                return NotFound("There is no Objekat with that Naziv");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad request");
            }

            return Ok(objekat);
        }
        //Ovaj Httpget je vezan za pretragu po priceRange
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("priceRange")]
        public IActionResult GetObjekatByPriceRange([FromQuery] int cenaDonja, [FromQuery] int cenaGornja)
        {
            var objekti = _objekatRepository.GetObjekatByPriceRange(cenaDonja, cenaGornja);
            if (objekti == null)
            {
                return NotFound("There is no Objekat with that price Range");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad request");
            }

            return Ok(objekti);
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("byKorisnik/{korisnikId}")]
        //[Authorize]
        public ActionResult GetObjekatByIdKorisnik(int korisnikId)
        {
            var objekti = _objekatRepository.GetObjekatByIdKorisnik(korisnikId);

            if (!objekti.Any())
            {
                return NotFound("Objekat with this ID has not been assigned to any User");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Bad request");
            }

            return Ok(objekti);


        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("byTipObjekta/tipObjektaId")]
        //[Authorize]
        public ActionResult GetObjekatByIdTipObjekta(int tipObjektaId)
        {
            var objekti = _objekatRepository.GetObjekatByIdTipObjekta(tipObjektaId);

            if (!objekti.Any())
            {
                return NotFound("Objekat with this ID has not been assigned to any TipObjekta");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Bad request");
            }

            return Ok(objekti);

        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("byPopust/popustId")]
        //[Authorize]
        public ActionResult GetObjekatByIdPopust(int popustId)
        {
            var objekti = _objekatRepository.GetObjekatByIdPopust(popustId);

            if (!objekti.Any())
            {
                return NotFound("Objekat with this ID has not been assigned to any Popust");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Bad request");
            }

            return Ok(objekti);

        }
        //
    }

}
