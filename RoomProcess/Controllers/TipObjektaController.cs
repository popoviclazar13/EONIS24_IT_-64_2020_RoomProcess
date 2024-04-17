﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoomProcess.InterfaceRepository;
using RoomProcess.Models.DTO;
using RoomProcess.Models.Entities;
using RoomProcess.Repository;

namespace RoomProcess.Controllers
{
    [Route("api/tipObjekta")]
    [ApiController]
    public class TipObjektaController : ControllerBase
    {
        private readonly ITipObjektaRepository _tipObjektaRepository;
        private readonly IMapper _mapper;

        public TipObjektaController(ITipObjektaRepository tipObjektaRepository, IMapper mapper)
        {
            _tipObjektaRepository = tipObjektaRepository;
            _mapper = mapper;
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        //[Authorize]
        public ActionResult GetTipObjektas()
        {
            var tipObjektas = _mapper.Map<List<TipObjektaDTO>>(_tipObjektaRepository.GetTipObjektas());

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);

            }
            return Ok(tipObjektas);
        }
        [HttpGet("{tipObjektaId}")]
        //[Authorize]
        public ActionResult GetTipObjektaById(int tipObjektaId)
        {
            return Ok(_tipObjektaRepository.GetTipObjektaById(tipObjektaId));
        }
        [HttpPost]
        //[AuthRole("Role", "Admin")]
        public ActionResult<TipObjekta> CreateTipObjekta([FromBody] TipObjektaDTO tipObjektaDTO)
        {
            if (tipObjektaDTO == null)
            {
                return BadRequest(tipObjektaDTO);
            }
            /*if (uloga.UlogaId > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }*/
            var uloga = _tipObjektaRepository.GetTipObjektas().Where(u => u.TipObjektaId == tipObjektaDTO.TipObjektaId).FirstOrDefault();

            if (uloga != null)
            {
                ModelState.AddModelError("", "TipObjekta already exists");
                return StatusCode(422);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }

            var tipObjektaMap = _mapper.Map<TipObjekta>(tipObjektaDTO);

            if (!_tipObjektaRepository.CreateTipObjekta(tipObjektaMap))
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

        public IActionResult UpdateTipObjekta([FromBody] TipObjektaDTO updateTipObjekta)
        {
            if (updateTipObjekta == null)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }
            if (!_tipObjektaRepository.TipObjektaExist(updateTipObjekta.TipObjektaId))
            {
                ModelState.AddModelError("", "Not found");
                return StatusCode(404);
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }
            var tipObjektaMap = _mapper.Map<TipObjekta>(updateTipObjekta);


            if (!_tipObjektaRepository.UpdateTipObjekta(tipObjektaMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating tipObjekta");

            }

            return NoContent();
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{tipObjektaId}")]
        //[AuthRole("Role", "Admin")]

        public IActionResult DeleteTipObjekta(int tipObjektaId)
        {
            var tipObjekta = _tipObjektaRepository.GetTipObjektaById(tipObjektaId);

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }
            if (_tipObjektaRepository.GetTipObjektaById(tipObjektaId) == null)
            {
                ModelState.AddModelError("", "Error 500");
                return StatusCode(500);
            }
            if (!_tipObjektaRepository.DeleteTipObjekta(tipObjekta))
            {
                ModelState.AddModelError("", "Something went wrong while deleting tipObjekta");
            }
            return NoContent();
        }
    }
}
