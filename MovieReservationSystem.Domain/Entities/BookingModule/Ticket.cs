using MovieReservationSystem.Domain.Entities.CinemaModule;

namespace MovieReservationSystem.Domain.Entities.BookingModule
{
    public class Ticket : BaseEntity<int>
    {
        public decimal Price { get; set; }
        public int BookingId { get; set; }
        public int SeatId { get; set; }
        // Navigation Properties
        public Booking Booking { get; set; }
        public Seat Seat { get; set; }
    }
}
