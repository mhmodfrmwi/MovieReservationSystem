using MovieReservationSystem.Domain.DTOs.MovieDTOs;

namespace MovieReservationSystem.Services_Abstraction.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetAllMoviesAsync();
        Task<MovieDto?> GetMovieByIdAsync(int id);
        Task<MovieDto> AddMovieAsync(CreateMovieDto movieDto);
        Task<Pagination<MovieDto>> GetMoviesAsync(MovieQueryParams queryParams);
        Task<MovieDto?> UpdateMovieAsync(int id, UpdateMovieDto dto);
        Task<bool> DeleteMovieAsync(int id);
    }
}
