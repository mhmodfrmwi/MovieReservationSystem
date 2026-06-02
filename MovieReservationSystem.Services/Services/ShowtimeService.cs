using MovieReservationSystem.Domain.DTOs.ShowtimeDTOs;
using MovieReservationSystem.Domain.Entities.ShowtimeModule;
using MovieReservationSystem.Domain.Interfaces;

namespace MovieReservationSystem.Services.Services
{
    public class ShowtimeService(IUnitOfWork unitOfWork) : IShowtimeService
    {
        public async Task<ShowtimeDto> AddShowtimeAsync(CreateShowtimeDto dto)
        {
            var showtime = new Showtime
            {
                StartTime = dto.StartTime,
                MovieId = dto.MovieId,
                HallId = dto.HallId,
                EndTime = dto.StartTime.AddHours(2),
                TicketPrice = dto.TicketPrice
            };
            await unitOfWork.Repository<Showtime>().AddAsync(showtime);
            await unitOfWork.CompleteAsync();
            return new ShowtimeDto
            (
                showtime.Id,
                showtime.StartTime,
                showtime.TicketPrice,
                showtime.MovieId,
                showtime.HallId
            );
        }

        public async Task<ShowtimeDto?> GetShowtimeByIdAsync(int id)
        {
            var showTime = await unitOfWork.Repository<Showtime>().GetByIdAsync(id);
            return showTime is null ? null : new ShowtimeDto
            (
                showTime.Id,
                showTime.StartTime,
                showTime.TicketPrice,
                showTime.MovieId,
                showTime.HallId
            );

        }

        public Task<IEnumerable<ShowtimeDto>> GetShowtimesByMovieIdAsync(int movieId)
        {
            var showtimes = unitOfWork.Repository<Showtime>().GetAllAsync();
            return Task.FromResult(showtimes.Result.Where(s => s.MovieId == movieId).Select(s => new ShowtimeDto
            (
                s.Id,
                s.StartTime,
                showtimes.Result.FirstOrDefault(st => st.Id == s.Id)?.TicketPrice ?? 0,
                s.MovieId,
                s.HallId
            )));
        }
    }
}
