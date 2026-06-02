using MovieReservationSystem.Domain.DTOs.DashboardDTOs;
using MovieReservationSystem.Domain.Entities.BookingModule;
using MovieReservationSystem.Domain.Entities.CinemaModule;
using MovieReservationSystem.Domain.Entities.Enums;
using MovieReservationSystem.Domain.Entities.MovieModule;
using MovieReservationSystem.Domain.Entities.ShowtimeModule;
using MovieReservationSystem.Domain.Interfaces;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Services.Services
{
    public class DashboardService(IUnitOfWork unitOfWork) : IDashboardService
    {
        public async Task<DashboardStatsDto> GetSystemStatsAsync()
        {


            var movies = await unitOfWork.Repository<Movie>().GetAllAsync();
            var bookings = await unitOfWork.Repository<Booking>().GetAllAsync();
            var confirmedBookings = bookings.Where(b => b.Status == BookingStatus.Confirmed).ToList();
            var tickets = await unitOfWork.Repository<Ticket>().GetAllAsync();
            var confirmedBookingIds = confirmedBookings.Select(b => b.Id).ToList();

            var recentBookings = confirmedBookings
                .OrderByDescending(b => b.BookingDate)
                .Take(5)
                .Select(b => new RecentBookingDto
                {
                    BookingId = b.Id,
                    TotalPrice = b.TotalAmount,
                    Date = b.BookingDate
                }).ToList();


            var showtimes = await unitOfWork.Repository<Showtime>().GetAllAsync();

            var validTickets = tickets.Where(t => confirmedBookingIds.Contains(t.BookingId)).ToList();

            var topMovies = validTickets
                .Join(showtimes, t => t.BookingId, s => s.Id, (t, s) => new { t, s })
                .Join(movies, ts => ts.s.MovieId, m => m.Id, (ts, m) => new { m.Title })
                .GroupBy(x => x.Title)
                .Select(g => new TopMovieDto
                {
                    MovieName = g.Key,
                    TicketsSold = g.Count()
                })
                .OrderByDescending(m => m.TicketsSold)
                .Take(5)
                .ToList();
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
            var dailyRevenues = confirmedBookings
                .Where(b => b.BookingDate >= sevenDaysAgo)
                .GroupBy(b => b.BookingDate.Date)
                .Select(g => new DailyRevenueDto
                {
                    Date = g.Key.ToString("dd MMM"),
                    Revenue = g.Sum(b => b.TotalAmount)
                })
                .OrderBy(r => r.Date)
                .ToList();

            var allSeats = await unitOfWork.Repository<Seat>().GetAllAsync();
            int totalCapacity = allSeats.Count;
            double occupancyRate = 0;

            if (totalCapacity > 0)
            {
                occupancyRate = Math.Round(((double)validTickets.Count / totalCapacity) * 100, 2);
            }

            var topGenres = validTickets
                .Join(showtimes, t => t.BookingId, s => s.Id, (t, s) => new { t, s })
                .Join(movies, ts => ts.s.MovieId, m => m.Id, (ts, m) => new { m.Genres })
                .SelectMany(x => x.Genres)
                .GroupBy(g => g.Name)
                .Select(g => new TopGenreDto
                {
                    GenreName = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToList();
            var stats = new DashboardStatsDto
            {
                TotalMovies = movies.Count,
                TotalTicketsSold = validTickets.Count,
                TotalRevenue = confirmedBookings.Sum(b => b.TotalAmount),
                TotalUsers = 0,
                OccupancyRatePercentage = occupancyRate,
                TopMovies = topMovies,
                RecentBookings = recentBookings,
                RevenueLast7Days = dailyRevenues,
                TopGenres= topGenres,
            };
            return stats;
        }
    }
}
