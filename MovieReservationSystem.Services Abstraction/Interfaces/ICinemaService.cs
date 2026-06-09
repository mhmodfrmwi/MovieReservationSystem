using MovieReservationSystem.Domain.DTOs.CinemaDTOs;

namespace MovieReservationSystem.Services_Abstraction.Interfaces
{
    public interface ICinemaService
    {
        Task<IEnumerable<CinemaDto>> GetAllCinemasAsync();
        Task<CinemaDto?> GetCinemaByIdAsync(int id);
        Task<CinemaDto> AddCinemaAsync(CreateCinemaDto dto);
        Task<CinemaDto?> UpdateCinemaAsync(int id, UpdateCinemaDto dto);
        Task<bool> DeleteCinemaAsync(int id);
    }
}
