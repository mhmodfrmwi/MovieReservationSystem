using MovieReservationSystem.Domain.DTOs.ShowtimeDTOs;

namespace MovieReservationSystem.Services.Services
{
    public interface IShowtimeService
    {
        Task<IEnumerable<ShowtimeDto>> GetShowtimesByMovieIdAsync(int movieId);
        Task<ShowtimeDto?> GetShowtimeByIdAsync(int id);
        Task<ShowtimeDto> AddShowtimeAsync(CreateShowtimeDto dto);
    }
}
