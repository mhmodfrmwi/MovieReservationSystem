using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Domain.DTOs.GenreDTOs;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Web.Controllers
{
    [Route("api/Genres")]
    [ApiController]
    public class GenresController(IGenreService genreService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await genreService.GetAllGenresAsync();
            var items = genres.Select(g => new CategoryDropdownDto(g.Id, g.Name));
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenreById(int id)
        {
            var genre = await genreService.GetGenreByIdAsync(id);
            return genre is null
                ? NotFound($"Genre with ID {id} was not found.")
                : Ok(new CategoryDropdownDto(genre.Id, genre.Name));
        }
    }
}
