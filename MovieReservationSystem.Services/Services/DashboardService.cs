using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieReservationSystem.Domain.DTOs.DashboardDTOs;
using MovieReservationSystem.Domain.Entities.BookingModule;
using MovieReservationSystem.Domain.Entities.CinemaModule;
using MovieReservationSystem.Domain.Entities.Enums;
using MovieReservationSystem.Domain.Entities.IdentityModule;
using MovieReservationSystem.Domain.Entities.MovieModule;
using MovieReservationSystem.Domain.Entities.ShowtimeModule;
using MovieReservationSystem.Domain.Interfaces;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Services.Services
{
    public class DashboardService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager) : IDashboardService
    {
        public async Task<DashboardStatsDto> GetSystemStatsAsync()
        {
            var movies = await unitOfWork.Repository<Movie>()
                .GetQueryable()
                .Include(m => m.Genres)
                .ToListAsync();
            var bookings = await unitOfWork.Repository<Booking>().GetAllAsync();
            var confirmedBookings = bookings.Where(b => b.Status == BookingStatus.Confirmed).ToList();
            var tickets = await unitOfWork.Repository<Ticket>().GetAllAsync();
            var confirmedBookingIds = confirmedBookings.Select(b => b.Id).ToHashSet();
            var showtimes = await unitOfWork.Repository<Showtime>().GetAllAsync();
            var showtimeLookup = showtimes.ToDictionary(s => s.Id);

            var recentBookings = confirmedBookings
                .OrderByDescending(b => b.BookingDate)
                .Take(5)
                .Select(b => new RecentBookingDto
                {
                    BookingId = b.Id,
                    TotalPrice = b.TotalAmount,
                    Date = b.BookingDate
                }).ToList();

            var validTickets = tickets.Where(t => confirmedBookingIds.Contains(t.BookingId)).ToList();

            var topMovies = validTickets
                .Select(t =>
                {
                    var booking = confirmedBookings.First(b => b.Id == t.BookingId);
                    var showtime = showtimeLookup.GetValueOrDefault(booking.ShowtimeId);
                    return showtime?.MovieId;
                })
                .Where(movieId => movieId.HasValue)
                .Join(movies, movieId => movieId, m => m.Id, (_, m) => m.Title)
                .GroupBy(title => title)
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
            var totalCapacity = allSeats.Count;
            var occupancyRate = totalCapacity > 0
                ? Math.Round((double)validTickets.Count / totalCapacity * 100, 2)
                : 0;

            var movieLookup = movies.ToDictionary(m => m.Id);
            var topGenres = validTickets
                .Select(t =>
                {
                    var booking = confirmedBookings.First(b => b.Id == t.BookingId);
                    var showtime = showtimeLookup.GetValueOrDefault(booking.ShowtimeId);
                    return showtime?.MovieId;
                })
                .Where(movieId => movieId.HasValue)
                .Select(movieId => movieLookup.GetValueOrDefault(movieId!.Value))
                .Where(movie => movie is not null)
                .SelectMany(movie => movie!.Genres)
                .GroupBy(g => g.Name)
                .Select(g => new TopGenreDto
                {
                    GenreName = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToList();

            return new DashboardStatsDto
            {
                TotalMovies = movies.Count,
                TotalTicketsSold = validTickets.Count,
                TotalRevenue = confirmedBookings.Sum(b => b.TotalAmount),
                TotalUsers = await userManager.Users.CountAsync(),
                OccupancyRatePercentage = occupancyRate,
                TopMovies = topMovies,
                RecentBookings = recentBookings,
                RevenueLast7Days = dailyRevenues,
                TopGenres = topGenres,
            };
        }
    }
}
