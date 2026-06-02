using System;
using System.Collections.Generic;
using System.Text;

namespace MovieReservationSystem.Domain.DTOs.BookingDTOs
{
    public record CreateBookingDto(
        int ShowtimeId,

        List<int> SeatIds
    );
}
