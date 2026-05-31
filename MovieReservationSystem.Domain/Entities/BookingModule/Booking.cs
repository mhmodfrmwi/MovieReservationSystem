using MovieReservationSystem.Domain.Entities.Enums;
using MovieReservationSystem.Domain.Entities.IdentityModule;
using MovieReservationSystem.Domain.Entities.ShowtimeModule;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieReservationSystem.Domain.Entities.BookingModule
{
    public class Booking : BaseEntity<int>
    {
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public BookingStatus Status { get; set; }

        public string UserId { get; set; }
        public int ShowtimeId { get; set; }

        // Navigation Properties
        public AppUser User { get; set; }
        public Showtime Showtime { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public Payment Payment { get; set; }
    }
}
