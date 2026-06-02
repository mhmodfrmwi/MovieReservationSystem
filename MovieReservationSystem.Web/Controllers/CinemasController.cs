using Microsoft.AspNetCore.Mvc;
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
            if (cinema == null) return NotFound($"Cinema with ID {id} was not found.");

            return Ok(cinema);
        }

        [HttpPost]
        public async Task<ActionResult<CinemaDto>> AddCinema(CreateCinemaDto dto)
        {
            var createdCinema = await cinemaService.AddCinemaAsync(dto);
            return CreatedAtAction(nameof(GetCinemaById), new { id = createdCinema.Id }, createdCinema);
        }
    }
}
