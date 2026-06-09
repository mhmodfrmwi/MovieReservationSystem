using MovieReservationSystem.Domain.DTOs.ShowtimeDTOs;

namespace MovieReservationSystem.Services_Abstraction.Interfaces
{
    public interface IShowtimeService
    {
        Task<IEnumerable<ShowtimeDto>> GetShowtimesByMovieIdAsync(int movieId);
        Task<ShowtimeDto?> GetShowtimeByIdAsync(int id);
        Task<ShowtimeDto> AddShowtimeAsync(CreateShowtimeDto dto);
        Task<ShowtimeDto?> UpdateShowtimeAsync(int id, UpdateShowtimeDto dto);
        Task<bool> DeleteShowtimeAsync(int id);
    }
}
