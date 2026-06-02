using MovieReservationSystem.Domain.DTOs.BookingDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieReservationSystem.Services_Abstraction.Interfaces
{
    public interface IBookingService
    {
        Task<BookingDto> CreateBookingAsync(string userId, CreateBookingDto createBookingDto);
        Task<BookingDto> GetBookingByIdAsync(int bookingId);
    }
}
