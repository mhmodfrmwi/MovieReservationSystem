using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Domain.Constants;
using MovieReservationSystem.Domain.DTOs.ShowtimeDTOs;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowtimesController(IShowtimeService showtimeService) : ControllerBase
    {
        [HttpGet("movie/{movieId}")]
        public async Task<ActionResult<IEnumerable<ShowtimeDto>>> GetShowtimesByMovieId(int movieId)
        {
            var showtimes = await showtimeService.GetShowtimesByMovieIdAsync(movieId);
            return Ok(showtimes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShowtimeDto>> GetShowtimeById(int id)
        {
            var showtime = await showtimeService.GetShowtimeByIdAsync(id);
            return showtime is null ? NotFound($"Showtime with ID {id} was not found.") : Ok(showtime);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<ActionResult<ShowtimeDto>> AddShowtime(CreateShowtimeDto dto)
        {
            try
            {
                var createdShowtime = await showtimeService.AddShowtimeAsync(dto);
                return CreatedAtAction(nameof(GetShowtimeById), new { id = createdShowtime.Id }, createdShowtime);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult<ShowtimeDto>> UpdateShowtime(int id, UpdateShowtimeDto dto)
        {
            try
            {
                var updated = await showtimeService.UpdateShowtimeAsync(id, dto);
                return updated is null ? NotFound($"Showtime with ID {id} was not found.") : Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShowtime(int id)
        {
            try
            {
                var deleted = await showtimeService.DeleteShowtimeAsync(id);
                return deleted ? NoContent() : NotFound($"Showtime with ID {id} was not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
