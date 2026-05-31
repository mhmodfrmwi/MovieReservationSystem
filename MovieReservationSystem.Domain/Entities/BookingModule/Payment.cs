using MovieReservationSystem.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieReservationSystem.Domain.Entities.BookingModule
{
    public class Payment : BaseEntity<int>
    {
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public PaymentMethod Method { get; set; } 
        public PaymentStatus Status { get; set; } 

        public int BookingId { get; set; } 

        // Navigation Properties
        public Booking Booking { get; set; }
    }
}
