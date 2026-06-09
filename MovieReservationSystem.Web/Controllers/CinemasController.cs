using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Domain.Constants;
using MovieReservationSystem.Domain.DTOs.CinemaDTOs;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemasController(ICinemaService cinemaService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CinemaDto>>> GetAllCinemas()
        {
            var cinemas = await cinemaService.GetAllCinemasAsync();
            return Ok(cinemas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CinemaDto>> GetCinemaById(int id)
        {
            var cinema = await cinemaService.GetCinemaByIdAsync(id);
            return cinema is null ? NotFound($"Cinema with ID {id} was not found.") : Ok(cinema);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<ActionResult<CinemaDto>> AddCinema(CreateCinemaDto dto)
        {
            var createdCinema = await cinemaService.AddCinemaAsync(dto);
            return CreatedAtAction(nameof(GetCinemaById), new { id = createdCinema.Id }, createdCinema);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult<CinemaDto>> UpdateCinema(int id, UpdateCinemaDto dto)
        {
            var updated = await cinemaService.UpdateCinemaAsync(id, dto);
            return updated is null ? NotFound($"Cinema with ID {id} was not found.") : Ok(updated);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCinema(int id)
        {
            try
            {
                var deleted = await cinemaService.DeleteCinemaAsync(id);
                return deleted ? NoContent() : NotFound($"Cinema with ID {id} was not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
