using MovieReservationSystem.Domain.DTOs.HallDTOs;
using MovieReservationSystem.Domain.Entities.CinemaModule;
using MovieReservationSystem.Domain.Interfaces;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Services.Services
{
    public class HallService(IUnitOfWork unitOfWork) : IHallService
    {
        public async Task<HallDto> AddHallAsync(CreateHallDto dto)
        {
            var hall = new Hall
            {
                Name = dto.Name,
                Capacity = dto.Capacity,
                CinemaId = dto.CinemaId
            };
            await unitOfWork.Repository<Hall>().AddAsync(hall);
            await unitOfWork.CompleteAsync();
            return new HallDto
            (
                hall.Id,
                hall.Name,
                hall.Capacity,
                hall.CinemaId
            );
        }

        public Task<HallDto?> GetHallByIdAsync(int id)
        {
            var hall = unitOfWork.Repository<Hall>().GetByIdAsync(id);
            return hall.ContinueWith(task =>
            {
                var h = task.Result;
                if (h == null) return null;
                return new HallDto
                (
                    h.Id,
                    h.Name,
                    h.Capacity,
                    h.CinemaId
                );
            });
        }

        public async Task<IEnumerable<HallDto>> GetHallsByCinemaIdAsync(int cinemaId)
        {
            var halls = await unitOfWork.Repository<Hall>().GetAllAsync();
            return halls.Where(h => h.CinemaId == cinemaId).Select(h => new HallDto
            (
                h.Id,
                h.Name,
                h.Capacity,
                h.CinemaId
            ));
        }
    }
}
