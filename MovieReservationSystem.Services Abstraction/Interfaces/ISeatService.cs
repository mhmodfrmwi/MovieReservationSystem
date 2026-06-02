using MovieReservationSystem.Domain.DTOs.SeatDTOs;

namespace MovieReservationSystem.Services_Abstraction.Interfaces
{
    public interface ISeatService
    {
        Task<IEnumerable<SeatDto>> GetSeatsByHallIdAsync(int hallId);
        Task<string> GenerateSeatsForHallAsync(GenerateSeatsDto dto);
        Task<IEnumerable<SeatDto>> GetSeatsForShowtimeAsync(int showtimeId);
    }
}
