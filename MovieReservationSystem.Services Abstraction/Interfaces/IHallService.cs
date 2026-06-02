using MovieReservationSystem.Domain.DTOs.HallDTOs;

namespace MovieReservationSystem.Services_Abstraction.Interfaces
{
    public interface IHallService
    {
        Task<IEnumerable<HallDto>> GetHallsByCinemaIdAsync(int cinemaId);
        Task<HallDto?> GetHallByIdAsync(int id);
        Task<HallDto> AddHallAsync(CreateHallDto dto);
    }
}
