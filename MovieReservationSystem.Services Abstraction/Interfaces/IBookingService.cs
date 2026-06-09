using MovieReservationSystem.Domain.DTOs.BookingDTOs;

namespace MovieReservationSystem.Services_Abstraction.Interfaces
{
    public interface IBookingService
    {
        Task<BookingDto> CreateBookingAsync(string userId, CreateBookingDto createBookingDto);
        Task<BookingDto?> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<MyBookingDto>> GetUserBookingsAsync(string userId);
        Task<BookingDto> CancelBookingAsync(int bookingId, string userId);
    }
}
