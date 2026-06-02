using MovieReservationSystem.Domain.DTOs.CinemaDTOs;
using MovieReservationSystem.Domain.Entities.CinemaModule;
using MovieReservationSystem.Domain.Interfaces;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Services.Services
{
    public class CinemaService(IUnitOfWork unitOfWork) : ICinemaService
    {
        public async Task<CinemaDto> AddCinemaAsync(CreateCinemaDto dto)
        {
            var cinema = new Cinema
            {
                Name = dto.Name,
                Location = dto.Location
            };
            await unitOfWork.Repository<Cinema>().AddAsync(cinema);
            await unitOfWork.CompleteAsync();
            return new CinemaDto(cinema.Id, cinema.Name, cinema.Location);

        }

        public async Task<IEnumerable<CinemaDto>> GetAllCinemasAsync()
        {
            var cinemas = await unitOfWork.Repository<Cinema>().GetAllAsync();
            return cinemas.Select(c => new CinemaDto(c.Id, c.Name, c.Location));
        }

        public Task<CinemaDto?> GetCinemaByIdAsync(int id)
        {
            var cinema = unitOfWork.Repository<Cinema>().GetByIdAsync(id);
            return cinema.ContinueWith(c => c.Result == null ? null : new CinemaDto(c.Result.Id, c.Result.Name, c.Result.Location));
        }
    }
}
