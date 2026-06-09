using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Domain.Constants;
using MovieReservationSystem.Domain.DTOs.GenreDTOs;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController(IGenreService genreService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllGenres()
        {
            var genres = await genreService.GetAllGenresAsync();
            var categories = genres.Select(g => new CategoryDropdownDto(g.Id, g.Name));
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenreById(int id)
        {
            var genre = await genreService.GetGenreByIdAsync(id);
            return genre is null
                ? NotFound($"Genre with ID {id} was not found.")
                : Ok(new CategoryDropdownDto(genre.Id, genre.Name));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddGenre(CreateGenreDto dto)
        {
            var createdGenre = await genreService.AddGenreAsync(dto);
            return CreatedAtAction(nameof(GetGenreById), new { id = createdGenre.Id }, createdGenre);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id, UpdateGenreDto dto)
        {
            var updated = await genreService.UpdateGenreAsync(id, dto);
            return updated is null ? NotFound($"Genre with ID {id} was not found.") : Ok(updated);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var deleted = await genreService.DeleteGenreAsync(id);
            return deleted ? NoContent() : NotFound($"Genre with ID {id} was not found.");
        }
    }
}
