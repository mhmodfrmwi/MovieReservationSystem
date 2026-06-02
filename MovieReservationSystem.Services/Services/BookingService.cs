using MovieReservationSystem.Domain.DTOs.BookingDTOs;
using MovieReservationSystem.Domain.Entities.BookingModule;
using MovieReservationSystem.Domain.Entities.Enums;
using MovieReservationSystem.Domain.Entities.ShowtimeModule;
using MovieReservationSystem.Domain.Interfaces;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Services.Services
{
    public class BookingService(IUnitOfWork unitOfWork) : IBookingService
    {
        public async Task<BookingDto> CreateBookingAsync(string userId, CreateBookingDto createBookingDto)
        {
            try
            {
                var showtime = await unitOfWork.Repository<Showtime>().GetByIdAsync(createBookingDto.ShowtimeId);
                if (showtime == null)
                {
                    throw new Exception("Showtime not found");
                }
                decimal totalPrice = showtime.TicketPrice * createBookingDto.SeatIds.Count;
                var booking = new Booking
                {
                    UserId = userId,
                    ShowtimeId = createBookingDto.ShowtimeId,
                    BookingDate = DateTime.UtcNow,
                    TotalAmount = totalPrice,
                    Status = BookingStatus.Confirmed
                };
                await unitOfWork.Repository<Booking>().AddAsync(booking);
                await unitOfWork.CompleteAsync();
                for (var seatId = 0; seatId < createBookingDto.SeatIds.Count; seatId++)
                {
                    var ticket = new Ticket
                    {
                        BookingId = booking.Id,
                        SeatId = createBookingDto.SeatIds[seatId],
                        Price = showtime.TicketPrice
                    };
                    await unitOfWork.Repository<Ticket>().AddAsync(ticket);
                }
                await unitOfWork.CompleteAsync();
                return new BookingDto
                (
                     booking.Id,
                     booking.BookingDate,
                     booking.TotalAmount,
                     booking.Status.ToString(),
                     booking.ShowtimeId,
                     booking.UserId,
                     createBookingDto.SeatIds
                );
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine($"An error occurred while creating the booking: {ex.Message}");
                throw; // Rethrow the exception to be handled by the caller
            }

        }

        public async Task<BookingDto> GetBookingByIdAsync(int bookingId)
        {
            var booking = await unitOfWork.Repository<Booking>().GetByIdAsync(bookingId);
            if (booking == null)
            {
                return null;
            }
            return new BookingDto
            (
                 booking.Id,
                 booking.BookingDate,
                 booking.TotalAmount,
                 booking.Status.ToString(),
                 booking.ShowtimeId,
                 booking.UserId,
                 booking.Tickets.Select(t => t.SeatId).ToList()
            );
        }
    }
}
