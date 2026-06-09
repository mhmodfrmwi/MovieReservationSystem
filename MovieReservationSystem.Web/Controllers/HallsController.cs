using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Domain.Constants;
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
            return hall is null ? NotFound($"Hall with ID {id} was not found.") : Ok(hall);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<ActionResult<HallDto>> AddHall(CreateHallDto dto)
        {
            try
            {
                var createdHall = await hallService.AddHallAsync(dto);
                return CreatedAtAction(nameof(GetHallById), new { id = createdHall.Id }, createdHall);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult<HallDto>> UpdateHall(int id, UpdateHallDto dto)
        {
            try
            {
                var updated = await hallService.UpdateHallAsync(id, dto);
                return updated is null ? NotFound($"Hall with ID {id} was not found.") : Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHall(int id)
        {
            try
            {
                var deleted = await hallService.DeleteHallAsync(id);
                return deleted ? NoContent() : NotFound($"Hall with ID {id} was not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
