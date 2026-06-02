using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Domain.DTOs.MovieDTOs;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController(IMovieService movieService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetAllMovies()
        {
            var movies = await movieService.GetAllMoviesAsync();
            return Ok(movies);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovieById(int id)
        {
            var movie = await movieService.GetMovieByIdAsync(id);

            if (movie == null)
            {
                return NotFound($"Movie with ID {id} was not found.");
            }

            return Ok(movie);
        }

        [HttpPost]
        public async Task<ActionResult<MovieDto>> AddMovie(CreateMovieDto dto)
        {
            var createdMovie = await movieService.AddMovieAsync(dto);

            return CreatedAtAction(nameof(GetMovieById), new { id = createdMovie.Id }, createdMovie);
        }
    }
}
