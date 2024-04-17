﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomProcess.Helpers;
using RoomProcess.InterfaceRepository;
using RoomProcess.Models.DTO;
using RoomProcess.Models.Entities;
using RoomProcess.Repository;
using RoomProcess.Services.KorisnikService;
using System;

namespace RoomProcess.Controllers
{
    [Route("api/uloga")]
    [ApiController]
    public class UlogaController : ControllerBase
    {
        private readonly IUlogaRepository _ulogaRepository;
        private readonly IMapper _mapper;

        public UlogaController(IUlogaRepository ulogaRepository, IMapper mapper)
        {
            _ulogaRepository = ulogaRepository;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        //[Authorize]
        public ActionResult GetUlogas()
        {
            var ulogas = _mapper.Map<List<UlogaDTO>>(_ulogaRepository.GetUlogas());

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);

            }
            return Ok(ulogas);
        }

        [HttpGet("{ulogaId}")]
        //[Authorize]
        public ActionResult GetUlogaByID(int ulogaId)
        {
            return Ok(_ulogaRepository.GetUlogaById(ulogaId));
        }

        [HttpPost]
        //[AuthRole("Role", "Admin")]
        public ActionResult<Uloga> CreateUloga([FromBody]UlogaDTO ulogaDTO)
        {
            if (ulogaDTO == null)
            {
                return BadRequest(ulogaDTO);
            }
            /*if (uloga.UlogaId > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }*/
            var uloga = _ulogaRepository.GetUlogas().Where(u => u.UlogaId == ulogaDTO.UlogaId).FirstOrDefault();

            if (uloga != null)
            {
                ModelState.AddModelError("", "Uloga already exists");
                return StatusCode(422);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }

            var ulogaMap = _mapper.Map<Uloga>(ulogaDTO);

            if (!_ulogaRepository.CreateUloga(ulogaMap))
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
        [HttpPut("{ulogaId}")]
        //[AuthRole("Role", "Admin")]
        
        public IActionResult UpdateUloga(int ulogaID, [FromBody] UlogaDTO updateUloga)
        {
            if (updateUloga == null)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }
            if (!_ulogaRepository.UlogaExist(ulogaID))
            {
                ModelState.AddModelError("", "Not found");
                return StatusCode(404);
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }
            var ulogaMap = _mapper.Map<Uloga>(updateUloga);


            if (!_ulogaRepository.UpdateUloga(ulogaMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating uloga");

            }

            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{ulogaId}")]
        //[AuthRole("Role", "Admin")]

        public IActionResult DeleteUloga(int ulogaId)
        {
            var uloga = _ulogaRepository.GetUlogaById(ulogaId);

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }
            if (_ulogaRepository.GetUlogaById(ulogaId) == null)
            {
                ModelState.AddModelError("", "Error 500");
                return StatusCode(500);
            }
            if (!_ulogaRepository.DeleteUloga(uloga))
            {
                ModelState.AddModelError("", "Something went wrong while deleting uloga");
            }
            return NoContent();
        }
        
    }
}