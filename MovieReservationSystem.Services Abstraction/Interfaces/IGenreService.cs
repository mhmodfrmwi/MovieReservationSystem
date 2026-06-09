using MovieReservationSystem.Domain.DTOs.GenreDTOs;

namespace MovieReservationSystem.Services_Abstraction.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreDto>> GetAllGenresAsync();
        Task<GenreDto?> GetGenreByIdAsync(int id);
        Task<GenreDto> AddGenreAsync(CreateGenreDto genreDto);
        Task<GenreDto?> UpdateGenreAsync(int id, UpdateGenreDto dto);
        Task<bool> DeleteGenreAsync(int id);
    }
}
