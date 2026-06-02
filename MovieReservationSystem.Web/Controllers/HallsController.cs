using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Domain.DTOs.HallDTOs;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallsController(IHallService hallService) : ControllerBase
    {
        [HttpGet("cinema/{cinemaId}")]
        public async Task<ActionResult<IEnumerable<HallDto>>> GetHallsByCinemaId(int cinemaId)
        {
            var halls = await hallService.GetHallsByCinemaIdAsync(cinemaId);
            return Ok(halls);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HallDto>> GetHallById(int id)
        {
            var hall = await hallService.GetHallByIdAsync(id);
            if (hall == null) return NotFound($"Hall with ID {id} was not found.");

            return Ok(hall);
        }

        [HttpPost]
        public async Task<ActionResult<HallDto>> AddHall(CreateHallDto dto)
        {
            var createdHall = await hallService.AddHallAsync(dto);
            return CreatedAtAction(nameof(GetHallById), new { id = createdHall.Id }, createdHall);
        }
    }
}
