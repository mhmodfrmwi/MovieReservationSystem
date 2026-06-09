using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Domain.DTOs.GenreDTOs;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(IGenreService genreService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var genres = await genreService.GetAllGenresAsync();
            var categories = genres
                .OrderBy(g => g.Name)
                .Select(g => new CategoryDropdownDto(g.Id, g.Name));
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var genre = await genreService.GetGenreByIdAsync(id);
            return genre is null
                ? NotFound($"Category with ID {id} was not found.")
                : Ok(new CategoryDropdownDto(genre.Id, genre.Name));
        }
    }
}
