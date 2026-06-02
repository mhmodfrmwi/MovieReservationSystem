namespace MovieReservationSystem.Domain.DTOs.DashboardDTOs
{
    public class TopMovieDto
    {
        public string MovieName { get; set; }
        public int TicketsSold { get; set; }
    }

    public class RecentBookingDto
    {
        public int BookingId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime Date { get; set; }
    }

    public class DailyRevenueDto
    {
        public string Date { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }

    public class TopGenreDto
    {
        public string GenreName { get; set; } = string.Empty;
        public int Count { get; set; }
    }
    public class DashboardStatsDto
    {
        public int TotalMovies { get; set; }
        public int TotalUsers { get; set; }
        public int TotalTicketsSold { get; set; }
        public decimal TotalRevenue { get; set; }

        public double OccupancyRatePercentage { get; set; }
        public List<TopMovieDto> TopMovies { get; set; } = new();
        public List<RecentBookingDto> RecentBookings { get; set; } = new();

        public List<DailyRevenueDto> RevenueLast7Days { get; set; } = new();
        public List<TopGenreDto> TopGenres { get; set; } = new();
    }
}
