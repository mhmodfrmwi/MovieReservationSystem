using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Domain.Constants;
using MovieReservationSystem.Domain.DTOs.SeatDTOs;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController(ISeatService seatService) : ControllerBase
    {
        [HttpGet("hall/{hallId}")]
        public async Task<ActionResult<IEnumerable<SeatDto>>> GetSeatsByHall(int hallId)
        {
            var seats = await seatService.GetSeatsByHallIdAsync(hallId);
            return Ok(seats);
        }

        [HttpGet("showtime/{showtimeId}")]
        public async Task<ActionResult<IEnumerable<SeatDto>>> GetSeatsForShowtime(int showtimeId)
        {
            try
            {
                var seats = await seatService.GetSeatsForShowtimeAsync(showtimeId);
                return Ok(seats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateSeats(GenerateSeatsDto dto)
        {
            try
            {
                var result = await seatService.GenerateSeatsForHallAsync(dto);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
