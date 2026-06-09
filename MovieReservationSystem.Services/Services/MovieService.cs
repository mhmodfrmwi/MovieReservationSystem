using Microsoft.EntityFrameworkCore;
using MovieReservationSystem.Domain.DTOs.MovieDTOs;
using MovieReservationSystem.Domain.Entities.MovieModule;
using MovieReservationSystem.Domain.Entities.ShowtimeModule;
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

            await AssignGenresAsync(movie, dto.GenreIds);
            await unitOfWork.Repository<Movie>().AddAsync(movie);
            await unitOfWork.CompleteAsync();
            return MapToDto(movie);
        }

        public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
        {
            var movies = await unitOfWork.Repository<Movie>()
                .GetQueryable()
                .Include(m => m.Genres)
                .ToListAsync();

            return movies.Select(MapToDto);
        }

        public async Task<MovieDto?> GetMovieByIdAsync(int id)
        {
            var movie = await unitOfWork.Repository<Movie>()
                .GetQueryable()
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.Id == id);

            return movie is null ? null : MapToDto(movie);
        }

        public async Task<Pagination<MovieDto>> GetMoviesAsync(MovieQueryParams queryParams)
        {
            var query = unitOfWork.Repository<Movie>()
                .GetQueryable()
                .Include(m => m.Genres)
                .AsQueryable();

            if (!string.IsNullOrEmpty(queryParams.Search))
                query = query.Where(m => m.Title.Contains(queryParams.Search));

            if (queryParams.GenreId.HasValue)
            {
                var genreId = queryParams.GenreId.Value;
                query = query.Where(m => m.Genres.Any(g => g.Id == genreId));
            }
            else if (!string.IsNullOrWhiteSpace(queryParams.Genre))
            {
                var genreName = queryParams.Genre.Trim().ToLower();
                query = query.Where(m => m.Genres.Any(g => g.Name.ToLower() == genreName));
            }

            var totalItems = await query.CountAsync();

            var movies = await query
                .Skip((queryParams.PageIndex - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            var movieDtos = movies.Select(MapToDto);
            return new Pagination<MovieDto>(queryParams.PageIndex, queryParams.PageSize, totalItems, movieDtos);
        }

        public async Task<MovieDto?> UpdateMovieAsync(int id, UpdateMovieDto dto)
        {
            var movie = await unitOfWork.Repository<Movie>()
                .GetQueryable()
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie is null) return null;

            movie.Title = dto.Title;
            movie.Description = dto.Description;
            movie.DurationInMinutes = dto.DurationInMinutes;
            movie.PosterUrl = dto.PosterUrl;
            movie.Language = dto.Language;
            movie.ReleaseDate = dto.ReleaseDate;
            movie.TrailerUrl = dto.TrailerUrl;

            movie.Genres.Clear();
            await AssignGenresAsync(movie, dto.GenreIds);

            unitOfWork.Repository<Movie>().Update(movie);
            await unitOfWork.CompleteAsync();
            return MapToDto(movie);
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movie = await unitOfWork.Repository<Movie>().GetByIdAsync(id);
            if (movie is null) return false;

            var hasShowtimes = await unitOfWork.Repository<Showtime>()
                .GetQueryable()
                .AnyAsync(s => s.MovieId == id);

            if (hasShowtimes)
                throw new Exception("Cannot delete a movie that has showtimes.");

            unitOfWork.Repository<Movie>().Delete(movie);
            await unitOfWork.CompleteAsync();
            return true;
        }

        private async Task AssignGenresAsync(Movie movie, List<int>? genreIds)
        {
            if (genreIds is null || genreIds.Count == 0) return;

            var genres = await unitOfWork.Repository<Genre>().GetAllAsync();
            foreach (var genreId in genreIds)
            {
                var genre = genres.FirstOrDefault(g => g.Id == genreId);
                if (genre is not null)
                    movie.Genres.Add(genre);
            }
        }

        private static MovieDto MapToDto(Movie movie) => new(
            movie.Id,
            movie.Title,
            movie.Description,
            movie.DurationInMinutes,
            movie.PosterUrl,
            movie.Language,
            movie.ReleaseDate,
            movie.TrailerUrl,
            movie.Genres.Select(g => g.Name).ToList()
        );
    }
}
