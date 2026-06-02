using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Domain.DTOs.ShowtimeDTOs;
using MovieReservationSystem.Services.Services;

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
            if (showtime == null) return NotFound($"Showtime with ID {id} was not found.");

            return Ok(showtime);
        }

        [HttpPost]
        public async Task<ActionResult<ShowtimeDto>> AddShowtime(CreateShowtimeDto dto)
        {
            var createdShowtime = await showtimeService.AddShowtimeAsync(dto);
            return CreatedAtAction(nameof(GetShowtimeById), new { id = createdShowtime.Id }, createdShowtime);
        }
    }
}
