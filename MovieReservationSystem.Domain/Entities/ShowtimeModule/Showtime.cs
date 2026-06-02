using MovieReservationSystem.Domain.Entities.BookingModule;
using MovieReservationSystem.Domain.Entities.CinemaModule;
using MovieReservationSystem.Domain.Entities.MovieModule;

namespace MovieReservationSystem.Domain.Entities.ShowtimeModule
{
    public class Showtime : BaseEntity<int>
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal TicketPrice { get; set; }

        public int MovieId { get; set; }
        public int HallId { get; set; }

        // Navigation Properties
        public Movie Movie { get; set; }
        public Hall Hall { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
