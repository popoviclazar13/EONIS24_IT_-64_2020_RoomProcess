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
    [Route("api/popust")]
    [ApiController]
    public class PopustController : ControllerBase
    {
        private readonly IPopustRepository _popustRepository;
        private readonly IMapper _mapper;

        public PopustController(IPopustRepository popustRepository, IMapper mapper)
        {
            _popustRepository = popustRepository;
            _mapper = mapper;
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        [AuthRole("Role", "Admin")]
        public ActionResult GetPopusts(int pageNumber = 1, int pageSize = 10)
        {
            var popusts = _popustRepository.GetPopusts()
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .ToList();

            var popustsDTO = _mapper.Map<List<PopustDTO>>(popusts);

            if (popustsDTO.Count == 0)
                return NotFound("No popust found");

            return Ok(popustsDTO);
        }

        [HttpGet("{popustId}")]
        [AllowAnonymous]
        public ActionResult GetPopustById(int popustId)
        {
            if (!_popustRepository.PopustExist(popustId))
            {
                ModelState.AddModelError("", "Popust with this ID does not exist");
                return StatusCode(404);

            }
            else
            {
                return Ok(_popustRepository.GetPopustById(popustId));
            }
        }
        [HttpPost]
        [AuthRole("Role", "Admin")]
        public ActionResult<Popust> CreatePopust([FromBody] PopustDTO popustDTO)
        {
            if (popustDTO == null)
            {
                return BadRequest(popustDTO);
            }
            var popust = _popustRepository.GetPopusts().Where(u => u.PopustId == popustDTO.PopustId).FirstOrDefault();

            if (popust != null)
            {
                ModelState.AddModelError("", "Popust already exists");
                return StatusCode(422);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }

            var popustMap = _mapper.Map<Popust>(popustDTO);

            if (!_popustRepository.CreatePopust(popustMap))
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

        public IActionResult UpdatePopust([FromBody] PopustDTO updatePopust)
        {
            if (updatePopust == null)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }
            if (!_popustRepository.PopustExist(updatePopust.PopustId))
            {
                ModelState.AddModelError("", "Not found");
                return StatusCode(404);
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }
            var popustMap = _mapper.Map<Popust>(updatePopust);


            if (!_popustRepository.UpdatePopust(popustMap))
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
        [HttpDelete("{popustId}")]
        [AuthRole("Role", "Admin")]
        public IActionResult DeletePopust(int popustId)
        {
            var popust = _popustRepository.GetPopustById(popustId);

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Bad request");
                return StatusCode(400);
            }
            if (_popustRepository.GetPopustById(popustId) == null)
            {
                ModelState.AddModelError("", "Popust with this ID does not exist");
                return StatusCode(404);
            }
            if (!_popustRepository.DeletePopust(popust))
            {
                ModelState.AddModelError("", "Something went wrong while deleting popust");
            }
            return NoContent();
        }
    }
}
