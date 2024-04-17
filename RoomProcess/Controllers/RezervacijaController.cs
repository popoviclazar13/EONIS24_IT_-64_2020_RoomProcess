﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public RezervacijaController(IRezervacijaRepository rezervacijaRepository, IMapper mapper)
        {
            _rezervacijaRepository = rezervacijaRepository;
            _mapper = mapper;
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetRezervacijas()
        {
            var rezervacijas = _mapper.Map<List<RezervacijaDTO>>(_rezervacijaRepository.GetRezervacijas());

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);

            }

            return Ok(rezervacijas);
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{rezervacijaId}")]
        //[Authorize]

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
        //[AuthRole("Role", "Admin")]
        public ActionResult<Rezervacija> CreateRezervacija([FromBody] RezervacijaCreateDTO rezervacijaCreate)
        {

            if (rezervacijaCreate == null)
            {
                return BadRequest(rezervacijaCreate);
            }
            /*if (aranzmanCreate.AranzmanID > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }*/
            var rezervacija = _rezervacijaRepository.GetRezervacijas().Where(a => a.RezervacijaId == rezervacijaCreate.RezervacijaId).FirstOrDefault();

            if (rezervacija != null)
            {
                ModelState.AddModelError("", "Rezervacija already exists");
                return StatusCode(422);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }

            var rezervacijaMap = _mapper.Map<Rezervacija>(rezervacijaCreate);

            if (!_rezervacijaRepository.CreateRezervacija(rezervacijaMap))
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

        public IActionResult UpdateRezervacija([FromBody] RezervacijaUpdateDTO updateRezervacija)
        {
            if (updateRezervacija == null)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }

            /*if (id != updateAranzman.AranzmanID)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }*/
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
        //[AuthRole("Role", "Admin")]

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
                ModelState.AddModelError("", "Not found");
                return StatusCode(404);
            }
            if (!_rezervacijaRepository.DeleteRezervacija(rezervacija))
            {
                ModelState.AddModelError("", "Something went wrong while deleting oprema");
            }
            return NoContent();
        }
    }
}