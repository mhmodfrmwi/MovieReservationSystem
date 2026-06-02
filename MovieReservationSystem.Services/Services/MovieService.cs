using MovieReservationSystem.Domain.DTOs.MovieDTOs;
using MovieReservationSystem.Domain.Entities.MovieModule;
using MovieReservationSystem.Domain.Interfaces;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Services.Services
{
    public class MovieService(IUnitOfWork unitOfWork) : IMovieService
    {
        public async Task<MovieDto> AddMovieAsync(CreateMovieDto dto)
        {
            var movie = new Movie
            {
                Title = dto.Title,
                Description = dto.Description,
                DurationInMinutes = dto.DurationInMinutes,
                PosterUrl = dto.PosterUrl,
                Language = dto.Language,
                ReleaseDate = dto.ReleaseDate,
                TrailerUrl = dto.TrailerUrl,
            };
            await unitOfWork.Repository<Movie>().AddAsync(movie);
            await unitOfWork.CompleteAsync();
            return new MovieDto(
                        movie.Id,
                        movie.Title,
                        movie.Description,
                        movie.DurationInMinutes,
                        movie.PosterUrl,
                        movie.Language,
                        movie.ReleaseDate,
                        movie.TrailerUrl
                    );
        }

        public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
        {
            var movies = await unitOfWork.Repository<Movie>().GetAllAsync();

            return movies.Select(m => new MovieDto(
            m.Id,
            m.Title,
            m.Description,
            m.DurationInMinutes,
            m.PosterUrl,
            m.Language,
            m.ReleaseDate,
            m.TrailerUrl
        ));
        }

        public async Task<MovieDto?> GetMovieByIdAsync(int id)
        {
            var movie = await unitOfWork.Repository<Movie>().GetByIdAsync(id);

            if (movie == null) return null;

            return new MovieDto(
            movie.Id,
            movie.Title,
            movie.Description,
            movie.DurationInMinutes,
            movie.PosterUrl,
            movie.Language,
            movie.ReleaseDate,
            movie.TrailerUrl
        );
        }
    }
}
