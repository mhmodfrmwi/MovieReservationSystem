using Microsoft.EntityFrameworkCore;
using MovieReservationSystem.Domain.DTOs.ShowtimeDTOs;
using MovieReservationSystem.Domain.Entities.BookingModule;
using MovieReservationSystem.Domain.Entities.CinemaModule;
using MovieReservationSystem.Domain.Entities.MovieModule;
using MovieReservationSystem.Domain.Entities.ShowtimeModule;
using MovieReservationSystem.Domain.Interfaces;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Services.Services
{
    public class ShowtimeService(IUnitOfWork unitOfWork) : IShowtimeService
    {
        public async Task<ShowtimeDto> AddShowtimeAsync(CreateShowtimeDto dto)
        {
            await ValidateReferencesAsync(dto.MovieId, dto.HallId);

            var movie = await unitOfWork.Repository<Movie>().GetByIdAsync(dto.MovieId);
            var showtime = new Showtime
            {
                StartTime = dto.StartTime,
                MovieId = dto.MovieId,
                HallId = dto.HallId,
                EndTime = dto.StartTime.AddMinutes(movie!.DurationInMinutes),
                TicketPrice = dto.TicketPrice
            };
            await unitOfWork.Repository<Showtime>().AddAsync(showtime);
            await unitOfWork.CompleteAsync();
            return MapToDto(showtime);
        }

        public async Task<ShowtimeDto?> GetShowtimeByIdAsync(int id)
        {
            var showTime = await unitOfWork.Repository<Showtime>().GetByIdAsync(id);
            return showTime is null ? null : MapToDto(showTime);
        }

        public async Task<IEnumerable<ShowtimeDto>> GetShowtimesByMovieIdAsync(int movieId)
        {
            var showtimes = await unitOfWork.Repository<Showtime>()
                .GetQueryable()
                .Where(s => s.MovieId == movieId)
                .ToListAsync();

            return showtimes.Select(MapToDto);
        }

        public async Task<ShowtimeDto?> UpdateShowtimeAsync(int id, UpdateShowtimeDto dto)
        {
            var showtime = await unitOfWork.Repository<Showtime>().GetByIdAsync(id);
            if (showtime is null) return null;

            await ValidateReferencesAsync(dto.MovieId, dto.HallId);

            var movie = await unitOfWork.Repository<Movie>().GetByIdAsync(dto.MovieId);
            showtime.StartTime = dto.StartTime;
            showtime.TicketPrice = dto.TicketPrice;
            showtime.MovieId = dto.MovieId;
            showtime.HallId = dto.HallId;
            showtime.EndTime = dto.StartTime.AddMinutes(movie!.DurationInMinutes);

            unitOfWork.Repository<Showtime>().Update(showtime);
            await unitOfWork.CompleteAsync();
            return MapToDto(showtime);
        }

        public async Task<bool> DeleteShowtimeAsync(int id)
        {
            var showtime = await unitOfWork.Repository<Showtime>().GetByIdAsync(id);
            if (showtime is null) return false;

            var hasBookings = await unitOfWork.Repository<Booking>()
                .GetQueryable()
                .AnyAsync(b => b.ShowtimeId == id);

            if (hasBookings)
                throw new Exception("Cannot delete a showtime that has bookings.");

            unitOfWork.Repository<Showtime>().Delete(showtime);
            await unitOfWork.CompleteAsync();
            return true;
        }

        private async Task ValidateReferencesAsync(int movieId, int hallId)
        {
            var movie = await unitOfWork.Repository<Movie>().GetByIdAsync(movieId);
            if (movie is null)
                throw new Exception("Movie not found.");

            var hall = await unitOfWork.Repository<Hall>().GetByIdAsync(hallId);
            if (hall is null)
                throw new Exception("Hall not found.");
        }

        private static ShowtimeDto MapToDto(Showtime showtime) => new(
            showtime.Id,
            showtime.StartTime,
            showtime.TicketPrice,
            showtime.MovieId,
            showtime.HallId
        );
    }
}
