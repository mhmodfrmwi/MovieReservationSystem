using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Domain.Constants;
using MovieReservationSystem.Domain.DTOs.MovieDTOs;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController(IMovieService movieService, IFileService fileService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovieById(int id)
        {
            var movie = await movieService.GetMovieByIdAsync(id);
            return movie is null ? NotFound($"Movie with ID {id} was not found.") : Ok(movie);
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<MovieDto>>> GetMovies([FromQuery] MovieQueryParams queryParams)
        {
            var movies = await movieService.GetMoviesAsync(queryParams);
            return Ok(movies);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<ActionResult<MovieDto>> AddMovie(CreateMovieDto dto)
        {
            var createdMovie = await movieService.AddMovieAsync(dto);
            return CreatedAtAction(nameof(GetMovieById), new { id = createdMovie.Id }, createdMovie);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult<MovieDto>> UpdateMovie(int id, UpdateMovieDto dto)
        {
            try
            {
                var updated = await movieService.UpdateMovieAsync(id, dto);
                return updated is null ? NotFound($"Movie with ID {id} was not found.") : Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            try
            {
                var deleted = await movieService.DeleteMovieAsync(id);
                return deleted ? NoContent() : NotFound($"Movie with ID {id} was not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("upload-poster")]
        public async Task<IActionResult> UploadMoviePoster(IFormFile file)
        {
            try
            {
                var imageUrl = await fileService.UploadFileAsync(file, "movies");
                return Ok(new { PosterUrl = imageUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
