using MovieReservationSystem.Domain.DTOs.HallDTOs;
using MovieReservationSystem.Domain.Entities.CinemaModule;
using MovieReservationSystem.Domain.Entities.ShowtimeModule;
using MovieReservationSystem.Domain.Interfaces;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Services.Services
{
    public class HallService(IUnitOfWork unitOfWork) : IHallService
    {
        public async Task<HallDto> AddHallAsync(CreateHallDto dto)
        {
            var cinema = await unitOfWork.Repository<Cinema>().GetByIdAsync(dto.CinemaId);
            if (cinema is null)
                throw new Exception("Cinema not found.");

            var hall = new Hall
            {
                Name = dto.Name,
                Capacity = dto.Capacity,
                CinemaId = dto.CinemaId
            };
            await unitOfWork.Repository<Hall>().AddAsync(hall);
            await unitOfWork.CompleteAsync();
            return MapToDto(hall);
        }

        public async Task<HallDto?> GetHallByIdAsync(int id)
        {
            var hall = await unitOfWork.Repository<Hall>().GetByIdAsync(id);
            return hall is null ? null : MapToDto(hall);
        }

        public async Task<IEnumerable<HallDto>> GetHallsByCinemaIdAsync(int cinemaId)
        {
            var halls = await unitOfWork.Repository<Hall>().GetAllAsync();
            return halls.Where(h => h.CinemaId == cinemaId).Select(MapToDto);
        }

        public async Task<HallDto?> UpdateHallAsync(int id, UpdateHallDto dto)
        {
            var hall = await unitOfWork.Repository<Hall>().GetByIdAsync(id);
            if (hall is null) return null;

            var cinema = await unitOfWork.Repository<Cinema>().GetByIdAsync(dto.CinemaId);
            if (cinema is null)
                throw new Exception("Cinema not found.");

            hall.Name = dto.Name;
            hall.Capacity = dto.Capacity;
            hall.CinemaId = dto.CinemaId;
            unitOfWork.Repository<Hall>().Update(hall);
            await unitOfWork.CompleteAsync();
            return MapToDto(hall);
        }

        public async Task<bool> DeleteHallAsync(int id)
        {
            var hall = await unitOfWork.Repository<Hall>().GetByIdAsync(id);
            if (hall is null) return false;

            var showtimes = await unitOfWork.Repository<Showtime>().GetAllAsync();
            if (showtimes.Any(s => s.HallId == id))
                throw new Exception("Cannot delete a hall that has showtimes.");

            unitOfWork.Repository<Hall>().Delete(hall);
            await unitOfWork.CompleteAsync();
            return true;
        }

        private static HallDto MapToDto(Hall hall) => new(hall.Id, hall.Name, hall.Capacity, hall.CinemaId);
    }
}
