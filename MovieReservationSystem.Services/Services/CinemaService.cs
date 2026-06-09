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
            var cinema = new Cinema { Name = dto.Name, Location = dto.Location };
            await unitOfWork.Repository<Cinema>().AddAsync(cinema);
            await unitOfWork.CompleteAsync();
            return MapToDto(cinema);
        }

        public async Task<IEnumerable<CinemaDto>> GetAllCinemasAsync()
        {
            var cinemas = await unitOfWork.Repository<Cinema>().GetAllAsync();
            return cinemas.Select(MapToDto);
        }

        public async Task<CinemaDto?> GetCinemaByIdAsync(int id)
        {
            var cinema = await unitOfWork.Repository<Cinema>().GetByIdAsync(id);
            return cinema is null ? null : MapToDto(cinema);
        }

        public async Task<CinemaDto?> UpdateCinemaAsync(int id, UpdateCinemaDto dto)
        {
            var cinema = await unitOfWork.Repository<Cinema>().GetByIdAsync(id);
            if (cinema is null) return null;

            cinema.Name = dto.Name;
            cinema.Location = dto.Location;
            unitOfWork.Repository<Cinema>().Update(cinema);
            await unitOfWork.CompleteAsync();
            return MapToDto(cinema);
        }

        public async Task<bool> DeleteCinemaAsync(int id)
        {
            var cinema = await unitOfWork.Repository<Cinema>().GetByIdAsync(id);
            if (cinema is null) return false;

            var halls = await unitOfWork.Repository<Hall>().GetAllAsync();
            if (halls.Any(h => h.CinemaId == id))
                throw new Exception("Cannot delete a cinema that has halls.");

            unitOfWork.Repository<Cinema>().Delete(cinema);
            await unitOfWork.CompleteAsync();
            return true;
        }

        private static CinemaDto MapToDto(Cinema cinema) => new(cinema.Id, cinema.Name, cinema.Location);
    }
}
